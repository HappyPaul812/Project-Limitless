using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using ProjectLimitless.Audio;
using ProjectLimitless.UI;
using ProjectLimitless.World;
using UnityEditor;
using UnityEngine;

namespace ProjectLimitless.EditorTools
{
    /// <summary>
    /// 현재 Story 원문과 음성 metadata를 읽어 사람이 먼저 들어볼 순서를 만든다.
    /// WAV/Catalog/Scene/Save를 수정하거나 음성의 뜻이 맞다고 자동 판정하지 않는다.
    /// Tools/TTS/prepare_story_semantic_risk.py로 현재 Source 입력을 준비한 뒤 Edit Mode에서 Run한다.
    /// </summary>
    public static class StoryVoiceSemanticRiskAudit
    {
        [Serializable] public sealed class Row
        {
            public int main, source_line, source_pos, sequence_rank, page, text_length, sentence_count, line_count, score, sample_rate, channels, bits;
            public long sample_count;
            public string source, branch, raw_speaker_id, raw_name, raw_text, dialogue_id, audit_key, sequence, route, quest, scene;
            public string semantic_state, evidence, speaker, runtime_name, runtime_text, manifest_text, manifest_path, batch, production_batch, voice_id, priority, recommended_action;
            public string wav_path, wav_filename, guid, catalog_path, catalog_guid, catalog_speaker, sha256, pcm_hash, risk, reasons, playback_locations;
            public bool user_pass, runtime_pass, protected_pass, known_regen, voice_target, player_silent, direction_silent;
            public bool missing, duplicate_id, duplicate_pcm, cross_main_duplicate, cross_speaker_duplicate, low_volume, short_duration, multi_sentence_short, neighbor_outlier, manifest_drift, catalog_mismatch;
            public double duration, rms_db, peak_db, seconds_per_char, speaker_ratio_median;
        }
        [Serializable] public sealed class Manifest { public string dialogue_id, text, speaker, path, batch, voice_id; }
        [Serializable] public sealed class SourceHash { public string path, sha256; }
        [Serializable] public sealed class Input { public Row[] rows; public Manifest[] manifests; public SourceHash[] source_hashes; }
        [Serializable] public sealed class Report { public Row[] rows; public string[] checks; public string[] unused_manifests; public int dialogue, voice, player, protected_count, known_regen, pending, high, medium, low, queue_count; public double median_rms, minimum_rms, median_seconds_per_char; }
        sealed class Wave { public int rate, channels, bits; public long count; public double seconds, rms, peak; public string pcm; }
        const BindingFlags Hidden = BindingFlags.Instance | BindingFlags.NonPublic;
        const BindingFlags Static = BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public;
        static readonly CultureInfo Inv = CultureInfo.InvariantCulture;
        static string Root => Path.GetFullPath(Path.Combine(Application.dataPath, "../../.."));
        static string Output => Path.Combine(Root, "문서/00_프로젝트");

        static string Hash(byte[] bytes)
        {
            using (var sha = SHA256.Create()) return string.Concat(sha.ComputeHash(bytes).Select(b => b.ToString("x2")));
        }
        static string Norm(string value) => Regex.Replace(value ?? "", @"\s+", "");
        static double Db(double value) => value > 0 ? 20 * Math.Log10(value) : -120;
        static double Median(IEnumerable<double> input)
        {
            var a = input.OrderBy(v => v).ToArray();
            if (a.Length == 0) return 0;
            return (a[(a.Length - 1) / 2] + a[a.Length / 2]) / 2;
        }

        /// <summary>RIFF의 fmt/data를 찾는다. 추가 chunk와 홀수 padding을 건너뛰어 header 차이를 PCM 중복으로 오인하지 않는다.</summary>
        static Wave ReadWave(byte[] bytes)
        {
            using (var stream = new MemoryStream(bytes))
            using (var reader = new BinaryReader(stream))
            {
                if (Encoding.ASCII.GetString(reader.ReadBytes(4)) != "RIFF") throw new InvalidDataException("RIFF required");
                reader.ReadUInt32();
                if (Encoding.ASCII.GetString(reader.ReadBytes(4)) != "WAVE") throw new InvalidDataException("WAVE required");
                int format = 0, channels = 0, rate = 0, bits = 0, align = 0;
                var pcm = new List<byte>();
                while (stream.Position + 8 <= stream.Length)
                {
                    string id = Encoding.ASCII.GetString(reader.ReadBytes(4));
                    int size = checked((int)reader.ReadUInt32());
                    long end = stream.Position + size;
                    if (end > stream.Length) throw new InvalidDataException("Truncated chunk");
                    if (id == "fmt ")
                    {
                        format = reader.ReadUInt16(); channels = reader.ReadUInt16(); rate = reader.ReadInt32();
                        reader.ReadInt32(); align = reader.ReadUInt16(); bits = reader.ReadUInt16();
                    }
                    else if (id == "data") pcm.AddRange(reader.ReadBytes(size));
                    stream.Position = end + (size % 2);
                }
                if ((format != 1 && format != 3) || channels < 1 || rate < 1 || align != channels * bits / 8 || pcm.Count == 0 || pcm.Count % align != 0)
                    throw new InvalidDataException("Unsupported/invalid PCM layout");
                if (!(format == 1 && new[] { 8, 16, 24, 32 }.Contains(bits)) && !(format == 3 && bits == 32))
                    throw new InvalidDataException("Unsupported PCM encoding");
                byte[] data = pcm.ToArray(); int width = bits / 8; double sum = 0, peak = 0;
                for (int i = 0; i < data.Length; i += width)
                {
                    double v;
                    if (format == 3) v = BitConverter.ToSingle(data, i);
                    else if (bits == 8) v = (data[i] - 128) / 128.0;
                    else if (bits == 16) v = BitConverter.ToInt16(data, i) / 32768.0;
                    else if (bits == 24) { int n = data[i] | data[i+1] << 8 | data[i+2] << 16; if ((n & 0x800000) != 0) n |= unchecked((int)0xff000000); v = n / 8388608.0; }
                    else v = BitConverter.ToInt32(data, i) / 2147483648.0;
                    if (double.IsNaN(v) || double.IsInfinity(v)) throw new InvalidDataException("Nonfinite PCM");
                    sum += v * v; peak = Math.Max(peak, Math.Abs(v));
                }
                // 동일 sample bytes라도 sampling/encoding이 다른 파일은 같은 실제 PCM으로 세지 않는다.
                var prefix = Encoding.ASCII.GetBytes(format + ":" + channels + ":" + rate + ":" + bits + ":");
                return new Wave { rate=rate, channels=channels, bits=bits, count=data.Length/width,
                    seconds=(double)data.Length/align/rate, rms=Db(Math.Sqrt(sum/(data.Length/width))), peak=Db(peak), pcm=Hash(prefix.Concat(data).ToArray()) };
            }
        }

        static void TextStats(Row r)
        {
            r.text_length = r.runtime_text.Count(char.IsLetterOrDigit);
            r.line_count = r.runtime_text.Split('\n').Count(l => !string.IsNullOrWhiteSpace(l));
            // 연속 ...와 Unicode …는 문장 경계 하나다. 줄바꿈만 있는 이어지는 문장은 중복해서 세지 않는다.
            string[] parts = Regex.Split(r.runtime_text, @"[.!?。！？…]+").Where(s => s.Any(char.IsLetterOrDigit)).ToArray();
            r.sentence_count = parts.Length;
            r.seconds_per_char = r.text_length > 0 ? r.duration/r.text_length : 0;
        }

        /// <summary>임계값은 듣기 우선순위다. protected PASS가 모든 heuristic보다 우선한다.</summary>
        static void Classify(Row r, bool recentBoundary)
        {
            var reasons = new List<string>(); int score = 0;
            Action<string,int> add = (why, points) => { reasons.Add(why); score += points; };
            r.low_volume = r.rms_db < -45;
            r.short_duration = r.text_length >= 12 && r.seconds_per_char < Math.Min(.07, r.speaker_ratio_median * .45);
            r.multi_sentence_short = r.sentence_count >= 2 && (r.duration < 2.2 || r.short_duration);
            if (r.known_regen) add("EXISTING_TTS_REGEN_REQUIRED_NO_NEW_VERDICT",100);
            if (r.missing) add("MISSING_AUDIO",100);
            if (r.manifest_drift || r.catalog_mismatch || r.duplicate_id) add("TEXT_OR_MAPPING_INTEGRITY_CANDIDATE",80);
            if (r.low_volume) add("LOW_RMS_BELOW_MINUS45_DBFS",80);
            if (r.short_duration) add("EXTREME_SHORT_TEXT_DURATION_RATIO",70);
            if (r.multi_sentence_short) add("MULTI_SENTENCE_SHORT_AUDIO",70);
            if (r.duplicate_pcm) add("DUPLICATE_PCM_DIFFERENT_ID",80);
            if (r.cross_main_duplicate) add("CROSS_MAIN_DUPLICATE_PCM",20);
            if (r.cross_speaker_duplicate) add("CROSS_SPEAKER_DUPLICATE_PCM",20);
            if (r.neighbor_outlier) add("SAME_SPEAKER_NEARBY_OUTLIER",70);
            if (r.dialogue_id.Contains("_supp_") && !r.protected_pass && (r.main == 3 || r.main == 4)) add("PREVIOUS_PROBLEM_SUPPLEMENT_BATCH",70);
            if (r.sentence_count >= 3 || r.line_count >= 3 || (r.sentence_count >= 2 && r.text_length >= 35)) add("LONG_MULTI_SENTENCE_OR_3_LINES",25);
            if (r.duration > Math.Max(16,r.text_length*r.speaker_ratio_median*2.5)) add("EXCESSIVE_DURATION_RATIO",25);
            if (recentBoundary) add("ADJACENT_PRODUCTION_BATCH_BOUNDARY",20);
            if (r.dialogue_id.Contains("_supp_") && !r.protected_pass) add("SUPPLEMENT_PACK_UNLISTENED",20);
            if (!r.user_pass && ((r.sequence??"").Contains("First") || (r.sequence??"").Contains("AfterBattle") || r.main >= 12 || r.main == 0)) add("IMPORTANT_STORY_UNLISTENED",20);
            if (reasons.Count == 0) reasons.Add("NORMAL_METADATA_SEMANTICS_UNCONFIRMED");
            r.score = score; r.risk = score >= 70 ? "HIGH" : score >= 20 ? "MEDIUM" : "LOW";
            if (r.protected_pass)
            {
                // 측정값 자체는 보존하지만 정상 Voice를 문제 후보/청취 대기로 다시 올리지 않는다.
                r.score=0; r.risk="LOW"; reasons=new List<string>{"PROTECTED_PASS_LATEST_HUMAN_RUNTIME_EVIDENCE"};
                r.low_volume=r.short_duration=r.multi_sentence_short=r.neighbor_outlier=false;
            }
            r.reasons=string.Join(";",reasons);
            r.priority="SEMANTIC_LISTENING_PRIORITY_"+r.risk;
            r.recommended_action=r.protected_pass?"KEEP_PROTECTED_PASS":r.known_regen?"USE_EXISTING_REGEN_HANDOFF_NO_NEW_VERDICT":"LISTEN_COMPARE_FULL_RUNTIME_TEXT";
        }

        static string Csv(object value)
        {
            string s = value is double ? ((double)value).ToString("0.########",Inv) : Convert.ToString(value,Inv) ?? "";
            return "\"" + s.Replace("\"","\"\"") + "\"";
        }
        static void WriteCsv(string name, Row[] rows, bool queue=false)
        {
            var columns = new[] { "main","quest","scene","sequence","sequence_rank","page","route","playback_locations","dialogue_id","speaker","runtime_name","runtime_text","manifest_text","manifest_path","batch","production_batch","voice_id","wav_filename","wav_path","duration","rms_db","peak_db","sample_count","sample_rate","channels","bits","sha256","pcm_hash","catalog_path","catalog_guid","catalog_speaker","guid","sentence_count","line_count","text_length","seconds_per_char","speaker_ratio_median","semantic_state","user_pass","runtime_pass","protected_pass","known_regen","risk","priority","score","reasons","recommended_action","missing","duplicate_id","duplicate_pcm","cross_main_duplicate","cross_speaker_duplicate","low_volume","short_duration","multi_sentence_short","neighbor_outlier","manifest_drift","catalog_mismatch","evidence" };
            var sb=new StringBuilder(); sb.AppendLine((queue ? "queue_order,gameplay_order,action," : "")+string.Join(",",columns));
            var fields=columns.Select(c=>typeof(Row).GetField(c)).ToArray();
            var gameplay=rows.OrderBy(r=>r.main).ThenBy(r=>r.sequence_rank).ThenBy(r=>r.main==16&&r.route=="default"?1:0).ThenBy(r=>r.page).ThenBy(r=>r.route).Select((r,i)=>new {r.audit_key,index=i+1}).ToDictionary(x=>x.audit_key,x=>x.index);
            for(int i=0;i<rows.Length;i++)
            {
                var r=rows[i];
                if(queue) sb.Append((i+1)+","+gameplay[r.audit_key]+","+Csv("LISTEN_IN_GAMEPLAY_SEQUENCE_COMPARE_FULL_TEXT")+",");
                sb.AppendLine(string.Join(",",fields.Select(f=>Csv(f.GetValue(r)))));
            }
            File.WriteAllText(Path.Combine(Output,name),sb.ToString(),new UTF8Encoding(true));
        }

        /// <summary>실제 factory 배열에서 위치를 얻는다. Main16 공유 ID는 양쪽 경로의 위치를 기록하고 한 건으로 센다.</summary>
        static void Factory(Row[] rows, Type type, string method, int main, string sequence, int rank, List<string> checks, params object[] args)
        {
            var lines=(DialogueLine[])type.GetMethod(method,Static).Invoke(null,args);
            for(int i=0;i<lines.Length;i++)
            {
                var line=lines[i];
                // Edit Mode에는 PlayerName이 비어 있다. 동적 Player factory만 입력의 Player 의미와 대조한다.
                // 사용자 Session 이름을 임시로 바꾸지 않으며 NPC의 stable ID는 그대로 비교한다.
                var matches=rows.Where(r=>r.main==main && (string.IsNullOrEmpty(line.DialogueId) ? r.runtime_text==line.Message&&(r.speaker==line.SpeakerId||(r.player_silent&&string.IsNullOrEmpty(line.SpeakerId))) : r.dialogue_id==line.DialogueId)).ToArray();
                if(matches.Length==0) throw new InvalidDataException("Factory row not extracted: "+main+"/"+sequence+"/"+line.Message);
                foreach(var r in matches)
                {
                    if(r.runtime_text!=line.Message||(r.speaker!=line.SpeakerId&&!(r.player_silent&&string.IsNullOrEmpty(line.SpeakerId))))throw new InvalidDataException("Factory drift: "+r.dialogue_id);
                    r.playback_locations=(r.playback_locations??"")+sequence+":"+(i+1)+";";
                    if(main==16 && sequence.EndsWith("/default") && (r.playback_locations??"").Contains("/hearing:")) continue;
                    r.page=i+1; r.sequence_rank=rank;
                    if(main!=16)r.sequence=sequence;
                }
                checks.Add("Factory "+main+"/"+sequence+"/"+(i+1));
            }
        }

        public static string Run()
        {
            if(EditorApplication.isPlaying||EditorApplication.isCompiling)throw new InvalidOperationException("Idle Edit Mode required");
            var input=JsonUtility.FromJson<Input>(File.ReadAllText(Path.Combine(Root,"Temp/StorySemanticRisk20261006/input.json")));
            var checks=new List<string>();
            foreach(var s in input.source_hashes)
            {
                if(Hash(File.ReadAllBytes(Path.Combine(Root,s.path)))!=s.sha256)throw new InvalidDataException("Input Source changed: "+s.path);
                checks.Add("Fresh Source hash "+s.path);
            }
            var story=Resources.Load<VoiceClipCatalog>("Audio/Voice/Story/StoryVoiceCatalog");
            var opening=Resources.Load<VoiceClipCatalog>("Audio/Voice/Opening/OpeningNarrationCatalog");
            if(story==null||opening==null)throw new InvalidDataException("Catalog missing");
            var catalogIds=new List<string>();
            foreach(var catalog in new[]{story,opening})
            {
                var array=(Array)catalog.GetType().GetField("entries",Hidden).GetValue(catalog);
                foreach(var entry in array)catalogIds.Add((string)entry.GetType().GetField("id",Hidden).GetValue(entry));
            }
            var rows=input.rows;
            foreach(var r in rows)
            {
                var line=new DialogueLine(r.raw_speaker_id,r.raw_name=="<PlayerName>"?"플레이어":r.raw_name,r.raw_text,r.dialogue_id);
                r.speaker=r.main==0?"narrator":line.SpeakerId; r.runtime_name=line.SpeakerName; r.runtime_text=line.Message;
                r.player_silent=line.IsPlayer; r.direction_silent=line.IsDirection||string.IsNullOrEmpty(r.speaker);
                r.voice_target=!r.player_silent&&!r.direction_silent&&!string.IsNullOrEmpty(r.dialogue_id);
                if(!r.voice_target) { r.risk="NOT_VOICE_TARGET"; continue; }
                var ms=input.manifests.Where(m=>m.dialogue_id==r.dialogue_id).ToArray();
                r.duplicate_id=ms.Length>1||catalogIds.Count(id=>id==r.dialogue_id)>1||rows.Count(x=>x.dialogue_id==r.dialogue_id)>1;
                var m=ms.FirstOrDefault();
                r.manifest_text=m==null?"":m.text; r.manifest_path=m==null?"":m.path; r.batch=m==null?"":m.batch; r.voice_id=m==null?"":m.voice_id;
                if(string.IsNullOrEmpty(r.production_batch))r.production_batch=r.batch;
                r.manifest_drift=m==null||Norm(r.runtime_text)!=Norm(r.manifest_text);
                var catalog=r.main==0?opening:story; var clip=catalog.Find(r.dialogue_id,r.main==0?null:r.speaker);
                r.catalog_path=AssetDatabase.GetAssetPath(clip); r.catalog_guid=AssetDatabase.AssetPathToGUID(r.catalog_path);
                r.catalog_speaker=r.main==0?"":r.speaker;
                // Catalog의 실제 등록 화자도 읽어 Find 성공 여부만으로 연결을 추정하지 않는다.
                foreach(var entry in (Array)catalog.GetType().GetField("entries",Hidden).GetValue(catalog))
                    if((string)entry.GetType().GetField("id",Hidden).GetValue(entry)==r.dialogue_id)
                        r.catalog_speaker=(string)entry.GetType().GetField("speakerId",Hidden).GetValue(entry);
                r.wav_path=string.IsNullOrEmpty(r.catalog_path)?"":Path.Combine("Unity/Client",r.catalog_path).Replace('\\','/');
                r.wav_filename=Path.GetFileName(r.wav_path); r.guid=r.catalog_guid;
                r.missing=clip==null||!File.Exists(Path.Combine(Root,r.wav_path));
                r.catalog_mismatch=clip!=null&&r.main!=0&&r.catalog_speaker!=r.speaker;
                if(!r.missing)
                {
                    byte[] bytes=File.ReadAllBytes(Path.Combine(Root,r.wav_path)); var w=ReadWave(bytes);
                    r.sha256=Hash(bytes);r.pcm_hash=w.pcm;r.duration=w.seconds;r.sample_count=w.count;r.sample_rate=w.rate;r.channels=w.channels;r.bits=w.bits;r.rms_db=w.rms;r.peak_db=w.peak;
                    if(clip.samples!=w.count/w.channels||clip.frequency!=w.rate||clip.channels!=w.channels)throw new InvalidDataException("Editor Clip metadata drift: "+r.dialogue_id);
                    checks.Add("WAV/Catalog sample layout "+r.dialogue_id);
                }
                TextStats(r);
            }
            foreach(var spec in new[]{new[]{"FirstConversation","1"},new[]{"AfterBattleConversation","3"}})
                Factory(rows,typeof(MainQuest03TaeonActor),spec[0],3,spec[0],int.Parse(spec[1]),checks);
            foreach(var spec in new[]{new[]{"FirstConversation","1"},new[]{"EncounterConversation","2"},new[]{"AfterBattleConversation","3"}})
                Factory(rows,typeof(MainQuest04MielActor),spec[0],4,spec[0],int.Parse(spec[1]),checks);
            Factory(rows,typeof(MainQuest05ReturnFlow),"GuardReport",5,"GuardReport",1,checks);
            Factory(rows,typeof(MainQuest05ReturnFlow),"RepresentativeReport",5,"RepresentativeReport",2,checks);
            Factory(rows,typeof(MainQuest07Interactable),"PaulFirst",7,"PaulFirst",3,checks);
            Factory(rows,typeof(MainQuest07Interactable),"MielMeeting",7,"MielMeeting",5,checks);
            string[] main08Ids={MainQuest08FieldFlow.Trace01,MainQuest08FieldFlow.Trace02,MainQuest08FieldFlow.Trace03,MainQuest08FieldFlow.WheelTracks,MainQuest08FieldFlow.Investigation,MainQuest08FieldFlow.DeepZone};
            string[] main08Names={"Trace01","Trace02","Trace03","WheelTracks","Investigation","DeepArea"};
            for(int i=0;i<main08Ids.Length;i++)Factory(rows,typeof(MainQuest08FieldFlow),"Lines",8,main08Names[i],i+1,checks,main08Ids[i]);
            string[] main15Ids={"field06_main15_soot","field06_main15_tracks","field06_main15_pulse","field06_main15_crack","field06_main15_heat"};
            for(int i=0;i<main15Ids.Length;i++)Factory(rows,typeof(Main15Site),"Lines",15,"QuestSequence"+(i+2).ToString("00"),i+2,checks,main15Ids[i]);
            for(int i=0;i<12;i++)
            {
                if(i<=5)Factory(rows,typeof(MainQuest09Dialogue),"Lines",9,"Lines/step"+i,i+1,checks,i);
                if(i<=6)Factory(rows,typeof(MainQuest10Dialogue),"Lines",10,"Lines/step"+i,i+1,checks,i);
                Factory(rows,typeof(MainQuest11Dialogue),"Lines",11,"Lines/step"+i,i+1,checks,i);
            }
            string[] scenes={"start","ash","vibration","tracks","witness_ground","witness_emerge","retry","afterimage","canyon","report"};
            for(int i=0;i<scenes.Length;i++)foreach(bool hearing in new[]{true,false})
                Factory(rows,typeof(Main16DialogueCatalog),"Get",16,scenes[i]+(hearing?"/hearing":"/default"),i+1,checks,scenes[i],hearing);
            var voices=rows.Where(r=>r.voice_target).ToArray();
            foreach(var group in voices.Where(r=>!r.missing).GroupBy(r=>r.pcm_hash))
            {
                bool dup=group.Select(r=>r.dialogue_id).Distinct().Count()>1;
                foreach(var r in group) {r.duplicate_pcm=dup;r.cross_main_duplicate=dup&&group.Select(x=>x.main).Distinct().Count()>1;r.cross_speaker_duplicate=dup&&group.Select(x=>x.speaker).Distinct().Count()>1;}
            }
            double allMedian=Median(voices.Where(r=>!r.missing).Select(r=>r.seconds_per_char));
            foreach(var r in voices)
            {
                var peers=voices.Where(x=>!x.missing&&x.speaker==r.speaker&&x.text_length>=12).ToArray();
                r.speaker_ratio_median=peers.Length>=5?Median(peers.Select(x=>x.seconds_per_char)):allMedian;
                var near=voices.Where(x=>x.main==r.main&&x.speaker==r.speaker&&x.dialogue_id!=r.dialogue_id&&Math.Abs(x.sequence_rank-r.sequence_rank)<=1&&!x.missing).ToArray();
                r.neighbor_outlier=near.Length>=3&&(r.rms_db<Median(near.Select(x=>x.rms_db))-18||(r.text_length>=12&&r.seconds_per_char<Median(near.Select(x=>x.seconds_per_char))*.35));
                bool boundary=voices.Any(x=>x.main==r.main&&x.sequence_rank==r.sequence_rank&&Math.Abs(x.page-r.page)==1&&x.production_batch!=r.production_batch);
                Classify(r,boundary);
            }
            var pending=voices.Where(r=>!r.user_pass&&!r.known_regen).ToArray();
            // Main 안에서는 페이지 순서를 유지한다. Main 착수 순서만 HIGH 수/최대 점수/점수 합으로 정한다.
            var mainRanks=pending.GroupBy(r=>r.main).OrderByDescending(g=>g.Count(r=>r.risk=="HIGH")).ThenByDescending(g=>g.Max(r=>r.score)).ThenByDescending(g=>g.Sum(r=>r.score)).ThenBy(g=>g.Key).Select((g,i)=>new{main=g.Key,rank=i}).ToDictionary(x=>x.main,x=>x.rank);
            var queue=pending.OrderBy(r=>mainRanks[r.main]).ThenBy(r=>r.sequence_rank).ThenBy(r=>r.main==16&&r.route=="default"?1:0).ThenBy(r=>r.page).ThenBy(r=>r.route).ToArray();
            if(voices.Any(r=>r.protected_pass&&(r.risk!="LOW"||r.known_regen))||queue.Any(r=>r.protected_pass))throw new InvalidDataException("Protected PASS override violated");
            checks.Add("Protected PASS/known regen excluded from pending Queue");
            var report=new Report {rows=rows,checks=checks.ToArray(),unused_manifests=input.manifests.Where(m=>!voices.Any(r=>r.dialogue_id==m.dialogue_id)).Select(m=>m.dialogue_id).ToArray(),dialogue=rows.Length,voice=voices.Length,player=rows.Count(r=>r.player_silent),protected_count=voices.Count(r=>r.protected_pass),known_regen=voices.Count(r=>r.known_regen),pending=pending.Length,high=voices.Count(r=>r.risk=="HIGH"),medium=voices.Count(r=>r.risk=="MEDIUM"),low=voices.Count(r=>r.risk=="LOW"),queue_count=queue.Length,median_rms=Median(voices.Select(r=>r.rms_db)),minimum_rms=voices.Min(r=>r.rms_db),median_seconds_per_char=allMedian};
            WriteCsv("StoryVoice_SemanticRisk_Audit.csv",voices.OrderBy(r=>r.main).ThenBy(r=>r.sequence_rank).ThenBy(r=>r.main==16&&r.route=="default"?1:0).ThenBy(r=>r.page).ThenBy(r=>r.route).ToArray());
            var gameplayQueue=queue.OrderBy(r=>r.main).ThenBy(r=>r.sequence_rank).ThenBy(r=>r.main==16&&r.route=="default"?1:0).ThenBy(r=>r.page).ThenBy(r=>r.route).ToArray();
            WriteCsv("StoryVoice_ListeningQueue.csv",gameplayQueue,true);
            WriteCsv("StoryVoice_PriorityListeningQueue.csv",queue,true);
            WriteCsv("StoryVoice_Protected_PASS.csv",voices.Where(r=>r.protected_pass).ToArray());
            File.WriteAllText(Path.Combine(Output,"StoryVoice_SemanticRisk_Audit.json"),JsonUtility.ToJson(report,true),new UTF8Encoding(false));
            return "PASS dialogue="+report.dialogue+" voice="+report.voice+" protected="+report.protected_count+" pending="+report.pending+" H/M/L="+report.high+"/"+report.medium+"/"+report.low+" checks="+checks.Count;
        }

        /// <summary>실제 파일을 건드리지 않는 합성 입력으로 분류 경계와 보호 우선 규칙을 확인한다.</summary>
        public static string SelfTest()
        {
            var r=new Row {dialogue_id="qa_001",runtime_text="첫 문장입니다. 두 번째 문장입니다.",duration=1,rms_db=-20,speaker_ratio_median=.16};
            TextStats(r);Classify(r,false);
            if(!r.multi_sentence_short||r.risk!="HIGH")throw new Exception("Short multi sentence rule");
            r.protected_pass=true;Classify(r,false);
            if(r.risk!="LOW"||r.score!=0||r.multi_sentence_short)throw new Exception("Protected override");
            r=new Row {dialogue_id="qa_002",runtime_text="정상 길이입니다.",duration=4,rms_db=-46,speaker_ratio_median=.16};TextStats(r);Classify(r,false);
            if(!r.low_volume||r.risk!="HIGH")throw new Exception("Low volume rule");
            if(Csv("가,\"나\"\n다")!="\"가,\"\"나\"\"\n다\"")throw new Exception("CSV escape");
            r=new Row {dialogue_id="qa_003",main=6,runtime_text="정상 발화입니다.",duration=3,rms_db=-20,speaker_ratio_median=.2,semantic_state="NEEDS_LISTENING"};TextStats(r);Classify(r,false);
            if(r.risk!="LOW"||r.semantic_state!="NEEDS_LISTENING")throw new Exception("Normal metadata is not semantic PASS");
            r.duplicate_pcm=r.cross_main_duplicate=r.cross_speaker_duplicate=true;Classify(r,false);
            if(r.risk!="HIGH"||!r.reasons.Contains("CROSS_SPEAKER"))throw new Exception("Duplicate priority");
            r=new Row {dialogue_id="qa_004",main=6,runtime_text="짧은 문장입니다.",duration=20,rms_db=-20,speaker_ratio_median=.2};TextStats(r);Classify(r,false);
            if(r.risk!="MEDIUM")throw new Exception("Excessive duration rule");
            // 알려진 값의16bit WAV를 메모리에만 만들어 decoder/RMS/PCM hash의 header 독립성을 확인한다.
            byte[] bytes;
            using(var s=new MemoryStream())using(var w=new BinaryWriter(s))
            {w.Write(Encoding.ASCII.GetBytes("RIFF"));w.Write(40);w.Write(Encoding.ASCII.GetBytes("WAVEfmt "));w.Write(16);w.Write((ushort)1);w.Write((ushort)1);w.Write(24000);w.Write(48000);w.Write((ushort)2);w.Write((ushort)16);w.Write(Encoding.ASCII.GetBytes("data"));w.Write(4);w.Write((short)16384);w.Write((short)-16384);bytes=s.ToArray();}
            var a=ReadWave(bytes);bytes[4]=99;var b=ReadWave(bytes);
            if(a.count!=2||Math.Abs(a.rms-Db(.5))>1e-10||a.pcm!=b.pcm)throw new Exception("PCM decode/RMS/header independent hash");
            return "PASS 10 synthetic cases: short-multi/protected/low-volume/CSV/normal-nonsemantic/duplicate/excessive/PCM-count/RMS/header-hash";
        }
    }
}
