"""Phase3의 별도 Discord 연결. 기존 봇/실제 AI 실행기는 변경하지 않습니다."""
import asyncio
import json
import logging
import os
import sys
import hashlib
import subprocess
import uuid
from pathlib import Path
import discord
from .core import Store,Identity,Denied
from .art_delivery import Deliveries
from .delivery_watch import FolderWatcher
from .intake import safe_root,MAX_ZIP

ROOT=Path('data/phase3')


def identity_config():
    return [int(x) for x in os.environ['LIMITLESS_OWNER_IDS'].split(',')],int(os.environ['LIMITLESS_GUILD_ID'])


def engine(config):
    owners,guild=identity_config();channel=int(config['channel_id'])
    allowed={int(x) for x in os.environ['LIMITLESS_CHANNEL_IDS'].split(',')}|{channel}
    store=Store('data/phase2/relay.sqlite',set(owners),guild,allowed)
    catalog=json.loads((ROOT/'asset-catalog.json').read_text(encoding='utf8'))
    deliveries=Deliveries(store,ROOT,config['base_sha'],catalog)
    return deliveries,Identity(owners[0],guild,channel)


async def inspect():
    logging.getLogger('discord').disabled=True
    guild=int(os.environ['LIMITLESS_GUILD_ID'])
    async with discord.Client(intents=discord.Intents.none()) as client:
        await client.login(os.environ['DISCORD_BOT_TOKEN'])
        app=await client.application_info()
        channels=await client.fetch_guild(guild)
        text=[c for c in await channels.fetch_channels() if isinstance(c,discord.TextChannel)]
        result={'channels':[{'id':str(c.id),'name':c.name} for c in text],
                'message_content_enabled':bool(app.flags.gateway_message_content or app.flags.gateway_message_content_limited),
                'configured_channels':os.environ['LIMITLESS_CHANNEL_IDS'].split(',')}
        Path('data/phase3').mkdir(parents=True,exist_ok=True)
        Path('data/phase3/discord-inspection.json').write_text(json.dumps(result,ensure_ascii=False,indent=2),encoding='utf8')
        print(json.dumps(result,ensure_ascii=True))


async def setup():
    owners,guild=identity_config();safe_root(ROOT)
    async with discord.Client(intents=discord.Intents.none()) as client:
        await client.login(os.environ['DISCORD_BOT_TOKEN']);g=await client.fetch_guild(guild)
        channels=await g.fetch_channels();channel=next((c for c in channels if isinstance(c,discord.TextChannel) and c.name=='아트-납품'),None)
        channel_status='EXISTING_ART_CHANNEL'
        if channel is None:
            try:channel=await g.create_text_channel('아트-납품',topic='LIMITLESS PNG/ZIP 납품·격리 검역. 수령은 미술 승인이나 게임 적용이 아닙니다.',reason='바울 대표 Phase3 아트 자동 납품 구축 지시')
            except discord.Forbidden:
                channel=await client.fetch_channel(int(os.environ['LIMITLESS_CHANNEL_IDS'].split(',')[0]));channel_status='TEMP_CONFIGURED_CHANNEL_NEEDS_ART_CHANNEL'
            else:channel_status='CREATED_ART_CHANNEL'
        app=await client.application_info();enabled=bool(app.flags.gateway_message_content or app.flags.gateway_message_content_limited)
        # 실제 원본 루트는 고정된 초기 보호 지문이 있는 위치만 사용합니다.
        original=Path('F:/study/codex/Project-Limitless')
        baseline=json.loads((original/'Temp/DiscordAICollab20261023/SecureSetup/before.json').read_text(encoding='utf-8-sig'))
        protected={Path(p).name.casefold():h for p,h in baseline.items() if p.lower().endswith('.png')}
        catalog={'source':'원본7356파일 보호 지문의 PNG 상위집합; 모든 항목이 미술 승인됐다는 뜻은 아님',
                 'names':sorted(protected),'sha256':sorted({h for p,h in baseline.items() if p.lower().endswith('.png')})}
        (ROOT/'asset-catalog.json').write_text(json.dumps(catalog,ensure_ascii=False,indent=2),encoding='utf8')
        inbox=safe_root(original/'Temp/ArtDeliveryInbox')
        config={'channel_id':str(channel.id),'channel_name':channel.name,'channel_status':channel_status,
                'message_content_enabled':enabled,'inbox':str(inbox),'base_sha':subprocess.check_output(['git','rev-parse','main'],cwd=original,text=True).strip(),
                'poll_seconds':3,'quiet_seconds':3,'ai_worker_enabled':False,'unity_worker_enabled':False}
        (ROOT/'config.json').write_text(json.dumps(config,ensure_ascii=False,indent=2),encoding='utf8')
        deliveries,owner=engine(config)
        last=[m async for m in channel.history(limit=1)]
        with deliveries.store.tx() as db:db.execute('INSERT OR IGNORE INTO art_delivery_cursors VALUES(?,?)',(str(channel.id),str(last[0].id if last else 0)))
        sent=await channel.send('[ART DELIVERY / Phase3 준비] PNG·ZIP은 이 채널에 첨부해 주세요. PNG는 같은 이름+.manifest.json을 함께 올려 주세요. 계약·권리 불명은 HOLD이며 Codex 대기 업무만 등록합니다. AI/Unity 자동실행·미술승인·게임적용 없음. '+('일반 첨부 감지 설정 확인.' if enabled else 'Message Content 설정 전에는 봇 멘션 첨부만 지원합니다.'),allowed_mentions=discord.AllowedMentions.none())
        config['setup_message_id']=str(sent.id);(ROOT/'config.json').write_text(json.dumps(config,ensure_ascii=False,indent=2),encoding='utf8')
        print(json.dumps(config,ensure_ascii=True))


async def ingest_message(message,client,deliveries,config,probe_id=None):
    owners,guild=identity_config();channel=int(config['channel_id'])
    if not message.guild or message.guild.id!=guild or message.channel.id!=channel:return []
    probe=probe_id is not None and message.id==probe_id and message.author.id==client.user.id
    if not probe and (message.author.bot or message.author.id not in owners):return []
    attachments=message.attachments
    if not attachments:return []
    metadata={a.filename:a for a in attachments if a.filename.endswith('.manifest.json') and a.size<=1024*1024}
    result=[]
    for attachment in attachments:
        if attachment.filename.endswith('.manifest.json'):continue
        key='discord:'+str(message.id)+':'+str(attachment.id)
        if deliveries.seen(key):continue
        if not attachment.filename.lower().endswith(('.png','.zip')) or not 0<attachment.size<=MAX_ZIP:
            owner=Identity(owners[0] if probe else message.author.id,guild,channel)
            result.append(deliveries.reject_transport(owner,key,'ATTACHMENT_TYPE_OR_SIZE_LIMIT'));continue
        content=await attachment.read()
        side=metadata.get(attachment.filename+'.manifest.json');meta=await side.read() if side else None
        owner=Identity(owners[0] if probe else message.author.id,guild,channel)
        source='DISCORD_SELF_TRANSPORT_TEST' if probe else 'DISCORD_ATTACHMENT'
        result.append(await asyncio.to_thread(deliveries.receive,owner,key,attachment.filename,content,meta,source))
    return result


async def publish_outbox(deliveries,channel):
    async def send(text):
        sent=await channel.send(text,allowed_mentions=discord.AllowedMentions.none());return sent.id
    await deliveries.flush(send)


async def run():
    import msvcrt
    config=json.loads((ROOT/'config.json').read_text(encoding='utf8'))
    lock=(ROOT/'service.lock').open('a+b');lock.seek(0);lock.write(b'1');lock.flush();lock.seek(0)
    try:msvcrt.locking(lock.fileno(),msvcrt.LK_NBLCK,1)
    except OSError:raise Denied('PHASE3_SERVICE_ALREADY_RUNNING') from None
    (ROOT/'stop-request').unlink(missing_ok=True)
    deliveries,owner=engine(config);deliveries.recover_outbox()
    watcher=FolderWatcher(deliveries,owner,config['inbox'],config['quiet_seconds'])
    async with discord.Client(intents=discord.Intents.none()) as client:
        await client.login(os.environ['DISCORD_BOT_TOKEN']);channel=await client.fetch_channel(int(config['channel_id']))
        if channel.guild.id!=owner.guild_id:raise Denied('CHANNEL_SCOPE_INVALID')
        print('PHASE3_RUNNING: 파일 감시/첨부 수령만. AI/Unity worker 비활성.',flush=True)
        while True:
            if (ROOT/'stop-request').exists():
                (ROOT/'service-status.json').write_text(json.dumps({'status':'STOPPED','ai_calls':0,'unity_runs':0}),encoding='utf8')
                lock.close();return
            try:
                await asyncio.to_thread(watcher.tick)
                with deliveries.store.tx() as db:cursor=int(db.execute('SELECT message_id FROM art_delivery_cursors WHERE channel=?',(str(channel.id),)).fetchone()[0])
                async for message in channel.history(limit=50,after=discord.Object(id=cursor),oldest_first=True):
                    await ingest_message(message,client,deliveries,config)
                    with deliveries.store.tx() as db:db.execute('UPDATE art_delivery_cursors SET message_id=? WHERE channel=?',(str(message.id),str(channel.id)))
                await publish_outbox(deliveries,channel)
                (ROOT/'service-status.json').write_text(json.dumps({'status':'RUNNING','last_tick':__import__('time').time(),'ai_calls':0,'unity_runs':0}),encoding='utf8')
            except Exception:
                (ROOT/'service-status.json').write_text(json.dumps({'status':'POLL_OR_REPORT_NOT_VERIFIED','ai_calls':0,'unity_runs':0}),encoding='utf8')
            await asyncio.sleep(config['poll_seconds'])


async def live_test():
    config=json.loads((ROOT/'config.json').read_text(encoding='utf8'));deliveries,owner=engine(config)
    fixture=ROOT/'fixtures';fixture.mkdir(exist_ok=True)
    original_zip=Path('F:/Downloads/Limitless_Chapter1_VenomBee_OriginalKeyArt_Pilot_v1_20261023.zip')
    import zipfile
    archive_bytes=original_zip.read_bytes()
    if hashlib.sha256(archive_bytes).hexdigest()!='2e65662d68c77cfb90d7e39d9d40c2cd2816ee33882debe8a8e93ace782bd478':raise Denied('ORIGINAL_FIXTURE_SHA_MISMATCH')
    with zipfile.ZipFile(original_zip) as z:data=z.read('VenomBee_OriginalKeyArt_Pilot_v1_512.png')
    name='Phase3_Bee_PILOT_COPY.png';manifest={'artist_role':'RenewalArtAgent','issue':63,'base_sha':config['base_sha'],
        'approval_status':'USER_REVIEW_PENDING','source':'기존Issue63 ZIP의 바이트동일 격리 사본; 신규 제작 아님',
        'commercial_rights':False,'files':[{'path':name,'sha256':hashlib.sha256(data).hexdigest(),'dimensions':[512,512],'frame_size':[512,512],'frames':1}]}
    (fixture/name).write_bytes(data);meta=json.dumps(manifest,ensure_ascii=False).encode();(fixture/(name+'.manifest.json')).write_bytes(meta)
    result={'mode':'REAL_FILE_TRANSPORT_TEST_NO_AI','original_zip_sha256':hashlib.sha256(archive_bytes).hexdigest(),'png_sha256':hashlib.sha256(data).hexdigest()}
    async with discord.Client(intents=discord.Intents.none()) as client:
        await client.login(os.environ['DISCORD_BOT_TOKEN']);channel=await client.fetch_channel(int(config['channel_id']))
        sent=await channel.send('[PHASE3 / 실제 파일 전송 시험] 봇 자체 업로드→서버 원본 재수신입니다. 대표 사용자 업로드를 사칭하지 않습니다. 기존 후보 사본·미술승인 아님.',
            files=[discord.File(fixture/name),discord.File(fixture/(name+'.manifest.json'))],allowed_mentions=discord.AllowedMentions.none())
        received=await channel.fetch_message(sent.id)
        result['upload_message_id']=str(sent.id);result['discord_received']=await ingest_message(received,client,deliveries,config,probe_id=sent.id)
        # 실제 공유 폴더에 byte동일 ZIP/PNG를 완료 rename으로 납품합니다.
        inbox=Path(config['inbox']);token=uuid.uuid4().hex[:8]
        zipname='Phase3_Bee_OriginalZip_COPY_'+token+'.zip';pngname='Phase3_Bee_Duplicate_COPY_'+token+'.png'
        for file,content,context in ((zipname,archive_bytes,{'artist_role':'RenewalArtAgent','issue':63,'base_sha':config['base_sha']}),(pngname,data,{**manifest,'files':[{**manifest['files'][0],'path':pngname}]})):
            partial=inbox/(file+'.part');partial.write_bytes(content);partial.rename(inbox/file)
            (inbox/(file+'.manifest.json')).write_text(json.dumps(context,ensure_ascii=False),encoding='utf8')
        # 실행 코드가 없는 경로 이탈 시험 ZIP도 실제 폴더 감시를 통과시켜 차단을 확인합니다.
        import io
        unsafe=io.BytesIO();escape_name='PHASE3_ESCAPE_MUST_NOT_EXIST_'+token+'.txt'
        with zipfile.ZipFile(unsafe,'w') as z:z.writestr('../'+escape_name,b'TEST_MARKER_ONLY_NO_EXECUTABLE')
        (inbox/('Phase3_UnsafePath_TEST_'+token+'.zip')).write_bytes(unsafe.getvalue())
        watcher=FolderWatcher(deliveries,owner,inbox,3);observed=[]
        for _ in range(5):observed.extend(await asyncio.to_thread(watcher.tick));await asyncio.sleep(1)
        result['folder_received']=observed
        result['unsafe_zip_did_not_escape']=not (inbox.parent/escape_name).exists()
        result['discord_repeat']=await ingest_message(received,client,deliveries,config,probe_id=sent.id)
        await publish_outbox(deliveries,channel)
        with deliveries.store.tx() as db:result['outbox']=[dict(r) for r in db.execute('SELECT status,message_id FROM art_delivery_outbox')]
    result['original_zip_unchanged']=hashlib.sha256(original_zip.read_bytes()).hexdigest()==result['original_zip_sha256']
    (ROOT/'live-test.json').write_text(json.dumps(result,ensure_ascii=False,indent=2),encoding='utf8')
    print(json.dumps({k:v for k,v in result.items() if k not in ('discord_received','folder_received')},ensure_ascii=True,indent=2))


async def main():
    logging.getLogger('discord').disabled=True
    action=sys.argv[1] if len(sys.argv)>1 else 'inspect'
    if action=='inspect':await inspect()
    elif action=='setup':await setup()
    elif action=='run':await run()
    elif action=='live-test':await live_test()
    elif action=='stop':
        safe_root(ROOT);(ROOT/'stop-request').write_text('STOP',encoding='ascii');print('PHASE3_STOP_REQUESTED; 기존 봇/AI 영향 없음')
    else:raise Denied('UNKNOWN_PHASE3_ACTION')


if __name__=='__main__':
    try:asyncio.run(main())
    except Exception:
        print('PHASE3_DISCORD_NOT_VERIFIED: 비밀 원문 출력 없음');raise SystemExit(2)
