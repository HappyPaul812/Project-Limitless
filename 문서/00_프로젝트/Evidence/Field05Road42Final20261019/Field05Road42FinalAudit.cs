using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using ProjectLimitless.Core;
using ProjectLimitless.World;
using ProjectLimitless.Player;
using ProjectLimitless.Monster;
using ProjectLimitless.NPC;
using ProjectLimitless.UI;
using ProjectLimitless.Battle;
using Object=UnityEngine.Object;
namespace ProjectLimitless.EditorTools {
// 원본 프로젝트를 건드리지 않고 Batch 격리 복사본에서만 실행하는 일회성 검수 도구입니다.
[InitializeOnLoad] public static class Field05Road42FinalAudit {
const string Key="Field05Road42Final20261019";
const string Field="Field_05_WesternOutskirts";
static string Root=>Path.GetFullPath(Path.Combine(Application.dataPath,"../../../Field05Road42Final20261019/Evidence",SessionState.GetString(Key+"Mode","Comparison")+(SessionState.GetBool(Key+"Access",false)?"_Access":"")));
static BindingFlags Flags=BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance|BindingFlags.Static;
static List<string> Checks=new List<string>(),Errors=new List<string>();
static IEnumerator routine;static double deadline;
static Dictionary<string,string> mapping=new Dictionary<string,string>{
{"Grass_-10_7","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_-9_7","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_-8_7","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_-7_7","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_-6_7","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_-5_7","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_-4_7","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_-3_7","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_-2_7","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_-1_7","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_0_7","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_1_7","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_2_7","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_3_7","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_4_7","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_5_7","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_6_7","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_7_7","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_8_7","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_9_7","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_10_7","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_-10_6","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_-9_6","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_-8_6","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_-7_6","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_-6_6","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_-5_6","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_-4_6","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_-3_6","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_-2_6","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_-1_6","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_0_6","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_1_6","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_2_6","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_3_6","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_4_6","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_5_6","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_6_6","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_7_6","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_8_6","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_9_6","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_10_6","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_-10_5","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_-9_5","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_-8_5","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_-7_5","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_-6_5","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_-5_5","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_-4_5","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_-3_5","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_-2_5","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_-1_5","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_0_5","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_1_5","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_2_5","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_3_5","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_4_5","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_5_5","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_6_5","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_7_5","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_8_5","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_9_5","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_10_5","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_-10_4","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_-9_4","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_-8_4","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_-7_4","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_-6_4","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_-5_4","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_-4_4","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_-3_4","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_-2_4","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_-1_4","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_0_4","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_1_4","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_2_4","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_3_4","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_4_4","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_5_4","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_6_4","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_7_4","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_8_4","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_9_4","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_10_4","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_-10_3","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_-9_3","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_-8_3","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_-7_3","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_-6_3","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_-5_3","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_-4_3","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_-3_3","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_-2_3","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_-1_3","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_0_3","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_1_3","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_2_3","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_3_3","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_4_3","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_5_3","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_6_3","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_7_3","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_8_3","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_9_3","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_10_3","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_-10_2","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_-9_2","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_-8_2","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_-7_2","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_-6_2","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_-5_2","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_-4_2","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_-3_2","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_-2_2","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_-1_2","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_0_2","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_1_2","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_2_2","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_3_2","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_4_2","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_5_2","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_6_2","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_7_2","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_8_2","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_9_2","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_10_2","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_-10_1","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_-9_1","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_-8_1","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_-7_1","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_-6_1","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_-5_1","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_-4_1","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_-3_1","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_-2_1","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_-1_1","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_0_1","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_1_1","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_2_1","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_3_1","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_4_1","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_5_1","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_6_1","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_7_1","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_8_1","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_9_1","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_10_1","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_-10_0","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_-9_0","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_-8_0","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_-7_0","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_-6_0","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_-5_0","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_-4_0","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_-3_0","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_-2_0","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_-1_0","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_0_0","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_1_0","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_2_0","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_3_0","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_4_0","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_5_0","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_6_0","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_7_0","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_8_0","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_9_0","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_10_0","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_-10_-1","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_-9_-1","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_-8_-1","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_-7_-1","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_-6_-1","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_-5_-1","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_-4_-1","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_-3_-1","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_-2_-1","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_-1_-1","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_0_-1","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_1_-1","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_2_-1","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_3_-1","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_4_-1","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_5_-1","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_6_-1","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_7_-1","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_8_-1","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_9_-1","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_10_-1","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_-10_-2","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_-9_-2","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_-8_-2","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_-7_-2","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_-6_-2","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_-5_-2","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_-4_-2","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_-3_-2","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_-2_-2","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_-1_-2","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_0_-2","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_1_-2","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_2_-2","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_3_-2","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_4_-2","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_5_-2","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_6_-2","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_7_-2","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_8_-2","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_9_-2","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_10_-2","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_-10_-3","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_-9_-3","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_-8_-3","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_-7_-3","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_-6_-3","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_-5_-3","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_-4_-3","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_-3_-3","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_-2_-3","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_-1_-3","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_0_-3","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_1_-3","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_2_-3","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_3_-3","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_4_-3","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_5_-3","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_6_-3","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_7_-3","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_8_-3","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_9_-3","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_10_-3","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_-10_-4","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_-9_-4","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_-8_-4","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_-7_-4","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_-6_-4","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_-5_-4","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_-4_-4","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_-3_-4","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_-2_-4","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_-1_-4","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_0_-4","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_1_-4","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_2_-4","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_3_-4","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_4_-4","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_5_-4","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_6_-4","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_7_-4","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_8_-4","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_9_-4","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_10_-4","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_-10_-5","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_-9_-5","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_-8_-5","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_-7_-5","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_-6_-5","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_-5_-5","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_-4_-5","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_-3_-5","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_-2_-5","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_-1_-5","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_0_-5","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_1_-5","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_2_-5","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_3_-5","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_4_-5","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_5_-5","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_6_-5","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_7_-5","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_8_-5","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_9_-5","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_10_-5","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_-10_-6","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_-9_-6","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_-8_-6","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_-7_-6","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_-6_-6","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_-5_-6","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_-4_-6","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_-3_-6","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_-2_-6","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_-1_-6","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_0_-6","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_1_-6","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_2_-6","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_3_-6","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_4_-6","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_5_-6","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_6_-6","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_7_-6","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_8_-6","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_9_-6","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_10_-6","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_-10_-7","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_-9_-7","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_-8_-7","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_-7_-7","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_-6_-7","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_-5_-7","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_-4_-7","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_-3_-7","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_-2_-7","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_-1_-7","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_0_-7","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_1_-7","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_2_-7","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_3_-7","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_4_-7","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_5_-7","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_6_-7","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_7_-7","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"Grass_8_-7","Ground/LL_C2_F05_DryLoam_B_128_v1.png"},
{"Grass_9_-7","Ground/LL_C2_F05_DryLoam_A_128_v1.png"},
{"Grass_10_-7","Ground/LL_C2_F05_DryLoam_C_128_v1.png"},
{"RoadLeft_-7","Road/LL_C2_F05_WornRoad_Left_128_v1.png"},
{"RoadRight_-7","Road/LL_C2_F05_WornRoad_Right_128_v1.png"},
{"RoadLeft_-6","Road/LL_C2_F05_WornRoad_Left_128_v1.png"},
{"RoadRight_-6","Road/LL_C2_F05_WornRoad_Right_128_v1.png"},
{"RoadLeft_-5","Road/LL_C2_F05_WornRoad_Left_128_v1.png"},
{"RoadRight_-5","Road/LL_C2_F05_WornRoad_Right_128_v1.png"},
{"RoadLeft_-4","Road/LL_C2_F05_WornRoad_Left_128_v1.png"},
{"RoadRight_-4","Road/LL_C2_F05_WornRoad_Right_128_v1.png"},
{"RoadLeft_-3","Road/LL_C2_F05_WornRoad_Left_128_v1.png"},
{"RoadRight_-3","Road/LL_C2_F05_WornRoad_Right_128_v1.png"},
{"RoadLeft_-2","Road/LL_C2_F05_WornRoad_Left_128_v1.png"},
{"RoadRight_-2","Road/LL_C2_F05_WornRoad_Right_128_v1.png"},
{"RoadLeft_-1","Road/LL_C2_F05_WornRoad_Left_128_v1.png"},
{"RoadRight_-1","Road/LL_C2_F05_WornRoad_Right_128_v1.png"},
{"RoadLeft_0","Road/LL_C2_F05_WornRoad_Left_128_v1.png"},
{"RoadRight_0","Road/LL_C2_F05_WornRoad_Right_128_v1.png"},
{"RoadLeft_1","Road/LL_C2_F05_WornRoad_Left_128_v1.png"},
{"RoadRight_1","Road/LL_C2_F05_WornRoad_Right_128_v1.png"},
{"RoadLeft_2","Road/LL_C2_F05_WornRoad_Left_128_v1.png"},
{"RoadRight_2","Road/LL_C2_F05_WornRoad_Right_128_v1.png"},
{"RoadLeft_3","Road/LL_C2_F05_WornRoad_Left_128_v1.png"},
{"RoadRight_3","Road/LL_C2_F05_WornRoad_Right_128_v1.png"},
{"RoadLeft_4","Road/LL_C2_F05_WornRoad_Left_128_v1.png"},
{"RoadRight_4","Road/LL_C2_F05_WornRoad_Right_128_v1.png"},
{"RoadLeft_5","Road/LL_C2_F05_WornRoad_Left_128_v1.png"},
{"RoadRight_5","Road/LL_C2_F05_WornRoad_Right_128_v1.png"},
{"RoadLeft_6","Road/LL_C2_F05_WornRoad_Left_128_v1.png"},
{"RoadRight_6","Road/LL_C2_F05_WornRoad_Right_128_v1.png"},
{"RoadLeft_7","Road/LL_C2_F05_WornRoad_Left_128_v1.png"},
{"RoadRight_7","Road/LL_C2_F05_WornRoad_Right_128_v1.png"},
{"RockWest","Props/LL_C2_F05_RockWest_128_v1.png"},
{"BushWest","Props/LL_C2_F05_Bush_Olive_128_v1.png"},
{"BushEast","Props/LL_C2_F05_Bush_Dry_128_v1.png"},
{"Fence_-4_5.7","Props/LL_C2_F05_Fence_128_v1.png"},
{"Fence_3_5.7","Props/LL_C2_F05_Fence_128_v1.png"},
{"Fence_-3_5.7","Props/LL_C2_F05_Fence_128_v1.png"},
{"TreeWest","Props/LL_C2_F05_Tree_Medium_128x156_v1.png"},
{"Fence_-6_-5.8","Props/LL_C2_F05_Fence_128_v1.png"},
{"TreeNorthWest","Props/LL_C2_F05_Tree_Big_255x256_v1.png"},
{"TreeNorthEast","Props/LL_C2_F05_Tree_Medium_128x156_v1.png"},
{"Fence_-5_-5.8","Props/LL_C2_F05_Fence_128_v1.png"},
{"Fence_6_-5.8","Props/LL_C2_F05_Fence_128_v1.png"},
{"BushSouth","Props/LL_C2_F05_Bush_Olive_128_v1.png"},
{"Fence_4_5.7","Props/LL_C2_F05_Fence_128_v1.png"},
{"Fence_5_-5.8","Props/LL_C2_F05_Fence_128_v1.png"},
{"RockNorth","Props/LL_C2_F05_RockNorth_85x54_v1.png"},
{"TreeEast","Props/LL_C2_F05_Tree_Big_255x256_v1.png"},
{"TreeSouthWest","Props/LL_C2_F05_Tree_Medium_128x156_v1.png"},};
static Field05Road42FinalAudit(){EditorApplication.playModeStateChanged+=s=>{if(!SessionState.GetBool(Key,false)||s!=PlayModeStateChange.EnteredPlayMode)return;
GameSaveService.AuditSaveDirectory=Path.Combine(Root,"Saves");UserSettingsService.BeginAudit(Path.Combine(Root,"Settings"));UserSettingsService.SetMuteAll(true);
Application.runInBackground=true;Application.logMessageReceived+=Log;routine=Flatten(Run());deadline=EditorApplication.timeSinceStartup+1200;EditorApplication.update+=Tick;};}


public static void RunAccessBatch(){SessionState.SetBool(Key+"Access",true);RunBatch();}
public static void RunBatch(){SessionState.SetString(Key+"Mode","Comparison");
if(!Application.isBatchMode||!Application.dataPath.Replace('\\','/').Contains("/Temp/Field05Review20261019/Client/Assets"))throw new Exception("격리 경로만 허용합니다.");
Directory.CreateDirectory(Root);File.WriteAllText(Path.Combine(Root,"status.txt"),"RUNNING");File.WriteAllText(Path.Combine(Root,"console_errors.txt"),"");GameSaveService.AuditSaveDirectory=Path.Combine(Root,"Saves");UserSettingsService.BeginAudit(Path.Combine(Root,"Settings"));UserSettingsService.SetMuteAll(true);
Check(Path.GetFullPath(GameSaveService.GetSaveFilePath(1)).StartsWith(Path.Combine(Root,"Saves")),"Save.Isolation");
foreach(var path in Directory.GetFiles("Assets/_Project/Art/Field05Renewal","*.png",SearchOption.AllDirectories)){
var ti=(TextureImporter)AssetImporter.GetAtPath(path);ti.textureType=TextureImporterType.Sprite;ti.spriteImportMode=SpriteImportMode.Single;ti.spritePixelsPerUnit=path.Contains("RockNorth")?100:128;ti.spritePivot=new Vector2(.5f,.5f);ti.filterMode=FilterMode.Point;ti.mipmapEnabled=false;ti.textureCompression=TextureImporterCompression.Uncompressed;ti.isReadable=true;ti.alphaIsTransparency=true;ti.SaveAndReimport();}
var candidate=EditorSceneManager.OpenScene("Assets/_Project/Scenes/Field_05_WesternOutskirts.unity");ApplyRoad("Final");
EditorSceneManager.SaveScene(candidate,"Assets/_Project/QA_Field05_FinalCandidate.unity",true);
EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);SessionState.SetBool(Key,true);EditorApplication.EnterPlaymode();}
static void Check(bool b,string label){Checks.Add((b?"PASS|":"FAIL|")+label);File.WriteAllLines(Path.Combine(Root,"checks.txt"),Checks);if(!b)throw new Exception(label);}
static void Note(string text){File.AppendAllText(Path.Combine(Root,"observations.txt"),text+"\n");}
static void Log(string text,string stack,LogType type){if(type==LogType.Error||type==LogType.Exception||type==LogType.Assert){Errors.Add(text+"\n"+stack);File.WriteAllLines(Path.Combine(Root,"console_errors.txt"),Errors);}}
        static PlayerController Player=>Object.FindAnyObjectByType<PlayerController>();
        static object Call(object o,string n,params object[] a)=>o.GetType().GetMethod(n,Flags).Invoke(o,a);
        static object Get(object o,string n)=>o.GetType().GetField(n,Flags).GetValue(o);
        static IEnumerator Wait(float s=.15f){float until=Time.realtimeSinceStartup+s;while(Time.realtimeSinceStartup<until)yield return null;}
        static IEnumerator Settled(string name){float until=Time.realtimeSinceStartup+40;while(SceneManager.GetActiveScene().name!=name&&Time.realtimeSinceStartup<until)yield return null;Check(SceneManager.GetActiveScene().name==name,"Scene."+name);MonsterEncounterService.SuppressForSeconds(3600);yield return Wait(.6f);if(Player!=null)Player.enabled=false;AudioListener.volume=0;}
        static IEnumerator Load(string name){if(name!="Bootstrap"&&name!="Battle")GameSessionData.RecordLocation(name,name==Field?"Spawn_From_Arbel":name=="Arbel"?"Spawn_Arbel_Center":"");SceneManager.LoadScene(name);yield return Settled(name);}
        static void At(Vector2 p){Player.enabled=false;var body=Player.GetComponent<Rigidbody2D>();body.linearVelocity=Vector2.zero;body.position=p;Player.transform.position=p;Physics2D.SyncTransforms();}
        static IEnumerator Close(){for(int n=0;n<50&&DialoguePresenter.Instance!=null&&DialoguePresenter.Instance.IsOpen;n++){DialoguePresenter.Instance.Advance();yield return Wait(.04f);}Check(DialoguePresenter.Instance==null||!DialoguePresenter.Instance.IsOpen,"Dialogue.Close");}
        static SpriteRenderer[] Env()=>GameObject.Find("FieldEnvironment").GetComponentsInChildren<SpriteRenderer>(true);
        static MonsterFieldController[] Monsters()=>Object.FindObjectsByType<MonsterFieldController>(FindObjectsSortMode.None).Where(x=>x.gameObject.scene.name==Field).ToArray();
        static void Seed(){GameSessionData.Reset();GameSaveService.SelectSlot(1);GameSessionData.ConfigurePlayer(PlayerVisualType.Male,"격리 아트 검수");GameSessionData.SelectJob("fighter");GameSessionData.SelectPlayerPath("path.mobility");GameSessionData.ConfigureProgress(30,0);PlayerController.SetMovementLocked(typeof(Field05Road42FinalAudit),true);}
        static void Capture(string name,Vector3 position,float size,int width=1280,int height=720){
            Check(SystemInfo.graphicsDeviceType!=UnityEngine.Rendering.GraphicsDeviceType.Null,"GPU.Available");var c=Camera.main;var rt=RenderTexture.GetTemporary(width,height,24,RenderTextureFormat.ARGB32);var oldTarget=c.targetTexture;var pos=c.transform.position;float os=c.orthographicSize,aspect=c.aspect;var active=RenderTexture.active;
            try{c.transform.position=position;c.orthographicSize=size;c.aspect=(float)width/height;c.targetTexture=rt;c.Render();RenderTexture.active=rt;var tex=new Texture2D(width,height,TextureFormat.RGBA32,false);tex.ReadPixels(new Rect(0,0,width,height),0,0);tex.Apply();File.WriteAllBytes(Path.Combine(Root,name),tex.EncodeToPNG());Object.DestroyImmediate(tex);}
            finally{c.transform.position=pos;c.orthographicSize=os;c.aspect=aspect;c.targetTexture=oldTarget;RenderTexture.active=active;RenderTexture.ReleaseTemporary(rt);}
        }
        static IEnumerator NormalVictory(string label,string returnScene){yield return Settled("Battle");var c=Object.FindAnyObjectByType<BattleSceneController>();var foes=(Formation)Get(c,"enemies");int attacks=0;double until=EditorApplication.timeSinceStartup+180;Time.timeScale=8;
            while(!(bool)Get(c,"battleEnded")&&EditorApplication.timeSinceStartup<until){var btn=(UnityEngine.UI.Button)Get(c,"attackButton");if(!(bool)Get(c,"actionPlaying")&&btn.IsActive()&&btn.interactable){btn.onClick.Invoke();var targets=(IReadOnlyList<Combatant>)Get(c,"selectableTargets");if((bool)Get(c,"choosingTarget")&&targets.Count>0){typeof(SecondRegressionAudit).GetMethod("Hit",Flags).Invoke(null,new object[]{c,targets[0]});attacks++;}}yield return null;}
            Time.timeScale=1;Check(foes.IsDefeated&&attacks>0,"Battle.ActualUIVictory."+label+".Attacks"+attacks);Object.FindObjectsByType<UnityEngine.UI.Button>(FindObjectsSortMode.None).Single(x=>x.name=="VictoryReturn").onClick.Invoke();yield return Settled(returnScene);
        }
        static IEnumerator MonsterBattles(){foreach(var spawn in Resources.LoadAll<FieldMonsterSpawnDefinition>("MonsterSpawns").Where(x=>x.SceneName==Field).OrderBy(x=>x.SpawnId)){
            PartyResourceService.Reset();var monster=Monsters().Single(x=>x.SpawnDefinition==spawn);At(new Vector2(0,-4));typeof(MonsterEncounterService).GetField("suppressedUntil",Flags).SetValue(null,0f);Call(monster,"BeginEncounter");yield return NormalVictory(spawn.SpawnId,Field);Apply();At(new Vector2(0,-4));
            var times=(Dictionary<string,float>)typeof(MonsterEncounterService).GetField("defeatedUntilBySpawn",Flags).GetValue(null);float until=times[Field+":"+spawn.SpawnId];bool checked30=false;while(Time.realtimeSinceStartup<until){if(!checked30&&Time.realtimeSinceStartup>=until-5){Check(!Monsters().Any(x=>x.SpawnDefinition==spawn),"Respawn.AbsentAt30."+spawn.SpawnId);checked30=true;}yield return null;}yield return Wait(.2f);Check(Monsters().Count(x=>x.SpawnDefinition==spawn)==1,"Respawn.OneAt35."+spawn.SpawnId);
        }Check(Monsters().Length==2,"Monster.FinalTwoNoDuplicate");}

static string Snapshot()=>string.Join("\n",Object.FindObjectsByType<Transform>(FindObjectsSortMode.None).Where(x=>x.gameObject.scene.name==Field).OrderBy(x=>x.GetEntityId()).Select(x=>x.GetEntityId()+"|"+x.position+"|"+x.localScale+"|"+string.Join(";",x.GetComponents<Collider2D>().Select(c=>c.GetType().Name+"|"+c.offset+"|"+c.bounds+"|"+c.enabled+"|"+c.isTrigger+(c is BoxCollider2D box?"|"+box.size:"")))));
static void Apply(){if(SceneManager.GetActiveScene().name!=Field)return;ApplyRoad(SessionState.GetString(Key+"Mode","V1"));}
static void Views(string prefix){
foreach(int w in new[]{1280,960,1680}){string a=w==1280?"16x9":"4x3";Capture(prefix+"_overview_"+a+".png",new Vector3(0,0,-10),7.5f,w,720);
Capture(prefix+"_east_"+a+".png",new Vector3(8,0,-10),2.5f,w,720);
Capture(prefix+"_road_"+a+".png",new Vector3(0,0,-10),3,w,720);
Capture(prefix+"_irrigation_"+a+".png",new Vector3(5,2,-10),2,w,720);
Capture(prefix+"_watchpost_"+a+".png",new Vector3(-2,2,-10),2,w,720);
Capture(prefix+"_pulse_"+a+".png",new Vector3(-6,-2,-10),2,w,720);}
foreach(var r in Env().Where(x=>mapping.ContainsKey(x.name)&&mapping[x.name].StartsWith("Props")))Capture(prefix+"_prop_"+r.name.Replace('.','_')+".png",r.transform.position+new Vector3(0,0,-10),1.3f,768,768);
}
// 픽셀 접지는 Sprite rect 안의 alpha 하단과 현재 Collider 하단을 같은 월드 단위로 기록합니다.
static void Grounding(string phase){var lines=new List<string>{"phase,name,rect,ppu,pivot,alphaFootWorld,colliderMinY,delta"};foreach(var r in Env().Where(x=>mapping[x.name].StartsWith("Props"))){var s=r.sprite;var path=AssetDatabase.GetAssetPath(s.texture);var tex=new Texture2D(2,2);tex.LoadImage(File.ReadAllBytes(path));var rect=s.rect;int bottom=-1;
for(int y=(int)rect.y;y<(int)rect.yMax&&bottom<0;y++)for(int x=(int)rect.x;x<(int)rect.xMax;x++)if(tex.GetPixel(x,y).a>0){bottom=y;break;}
float local=(bottom-rect.y-s.pivot.y)/s.pixelsPerUnit;float foot=r.transform.TransformPoint(new Vector3(0,local,0)).y;var col=r.GetComponent<Collider2D>();lines.Add(phase+","+r.name+","+rect.ToString().Replace(',',';')+","+s.pixelsPerUnit+","+s.pivot.ToString().Replace(',',';')+","+foot+","+(col?col.bounds.min.y:float.NaN)+","+(col?foot-col.bounds.min.y:float.NaN));Object.Destroy(tex);
Check(s!=null,"Sprite."+phase+"."+r.name);if(r.name=="RockNorth")Check(rect.width==85&&rect.height==54&&s.pixelsPerUnit==100,"RockNorth.85x54.PPU100."+phase);}
File.WriteAllLines(Path.Combine(Root,"Grounding_"+phase+".csv"),lines);}
static IEnumerator Continue(){At(new Vector2(6,-2));var pos=(Vector2)Player.transform.position;string quest=JsonUtility.ToJson(QuestService.ExportSaveData());Check(GameSaveService.SaveCurrentWorldPosition(pos,Field),"Save.Isolated");yield return Load("Bootstrap");GameSessionData.Reset();var btn=GameObject.Find("StartMenuCanvas/Slot01/Action")?.GetComponent<UnityEngine.UI.Button>();Check(btn!=null,"Continue.ActualButton");btn.onClick.Invoke();yield return Settled(Field);Apply();Check(Vector2.Distance(Player.transform.position,pos)<.1f&&JsonUtility.ToJson(QuestService.ExportSaveData())==quest,"Continue.PositionQuest");}
static void ColliderViews(string phase){foreach(var r in Env().Where(x=>mapping[x.name].StartsWith("Props")&&x.GetComponent<Collider2D>()!=null)){var c=r.GetComponent<Collider2D>();var b=c.bounds;var go=new GameObject("QA_ColliderOverlay");var line=go.AddComponent<LineRenderer>();line.material=new Material(Shader.Find("Sprites/Default"));line.startColor=line.endColor=Color.magenta;line.startWidth=line.endWidth=.012f;line.sortingOrder=1000;line.positionCount=5;line.SetPositions(new[]{new Vector3(b.min.x,b.min.y,-1),new Vector3(b.max.x,b.min.y,-1),new Vector3(b.max.x,b.max.y,-1),new Vector3(b.min.x,b.max.y,-1),new Vector3(b.min.x,b.min.y,-1)});Capture(phase+"_collider_"+r.name.Replace('.','_')+".png",r.transform.position+new Vector3(0,0,-10),1.3f,768,768);Object.DestroyImmediate(line.material);Object.DestroyImmediate(go);}}
static IEnumerator Story(){
var irrigation=GameObject.Find("field05_main14_irrigation").GetComponent<Chapter2ObjectiveSite>();At(irrigation.transform.position);Call(irrigation,"TryInteract");yield return Close();Check(QuestService.ActiveMainQuest.CurrentObjectiveIndex==3,"Main14.Irrigation");
var serin=GameObject.Find("field05-serin");Check(Vector2.Distance(serin.transform.position,new Vector2(1,0))<.01f,"Serin.Position");At(new Vector2(1,0));Check(Chapter2IntroFlow.TryHandleNpc("field05-serin",serin.GetComponent<NpcController>()),"Serin.Interaction");yield return Close();Check(QuestService.ActiveMainQuest.CurrentObjectiveIndex==4,"Main14.Serin");
var watch=GameObject.Find("field05_main14_watchpost").GetComponent<Chapter2ObjectiveSite>();At(watch.transform.position);Call(watch,"TryInteract");yield return Close();Check(QuestService.ActiveMainQuest.CurrentObjectiveIndex==5,"Main14.Watchpost");
var battle=GameObject.Find(Chapter2IntroFlow.Main14Battle).GetComponent<Chapter2ObjectiveSite>();At(battle.transform.position);Call(battle,"TryInteract");yield return NormalVictory("Main14Story",Field);Apply();Check(QuestService.ActiveMainQuest.CurrentObjectiveIndex==6,"Main14.StoryBattleReturn");
At(new Vector2(-6,-2));yield return Wait(.5f);yield return Close();Check(QuestService.ActiveMainQuest.CurrentObjectiveIndex==7,"Main14.StrongPulse");yield return Continue();
}
static IEnumerator Exits(){foreach(var pair in new[]{new[]{"east_to_arbel","Arbel"},new[]{"west_to_field05",Field}}){var tr=Object.FindObjectsByType<SceneTransitionTrigger>(FindObjectsSortMode.None).Single(x=>x.name=="Transition_"+pair[0]);tr.SendMessage("OnTriggerEnter2D",Player.GetComponent<Collider2D>(),SendMessageOptions.RequireReceiver);yield return Settled(pair[1]);Check(Vector2.Distance(Player.transform.position,new Vector2(pair[1]==Field?8:-8,0))<.2f,"Exit.Spawn."+pair[0]);Apply();}}
static IEnumerator Cameras(){var cam=Camera.main;foreach(float aspect in new[]{16f/9,4f/3,21f/9}){cam.aspect=aspect;foreach(var p in new[]{new Vector2(9.5f,0),new Vector2(-9.5f,0),new Vector2(0,6.5f),new Vector2(0,-6.5f)}){At(p);yield return Wait(.3f);var b=Object.FindAnyObjectByType<WorldBounds2D>().Bounds;float half=cam.orthographicSize;Check(cam.transform.position.x-half*aspect>=b.min.x-.001f&&cam.transform.position.x+half*aspect<=b.max.x+.001f&&cam.transform.position.y-half>=b.min.y-.001f&&cam.transform.position.y+half<=b.max.y+.001f,"Camera.Bounds."+aspect+"."+p);}}}
        static bool ClearPoint(Vector2 p,float rad)=>!Physics2D.OverlapCircleAll(p,rad).Any(x=>!x.isTrigger&&x.GetComponentInParent<PlayerController>()==null&&x.GetComponentInParent<MonsterFieldController>()==null);
        static IEnumerator Wheelchair(){
            Check(GameSessionData.SelectedPlayerPathId=="path.mobility","Wheelchair.PathMobility");var monsters=Monsters().SelectMany(x=>x.GetComponents<Collider2D>()).ToArray();foreach(var x in monsters)x.enabled=false;
            foreach(var target in AccessTargets()){
                float radius=Player.GetComponent<CircleCollider2D>().radius*Player.transform.lossyScale.x+.03f;const float step=.25f;var start=new Vector2Int(Mathf.RoundToInt(Player.transform.position.x/step),Mathf.RoundToInt(Player.transform.position.y/step));var goal=new Vector2Int(Mathf.RoundToInt(target.x/step),Mathf.RoundToInt(target.y/step));var queue=new Queue<Vector2Int>();var prev=new Dictionary<Vector2Int,Vector2Int>();queue.Enqueue(start);prev[start]=start;
                while(queue.Count>0&&!prev.ContainsKey(goal)){var a=queue.Dequeue();foreach(var dir in new[]{Vector2Int.up,Vector2Int.down,Vector2Int.left,Vector2Int.right}){var n=a+dir;Vector2 p=(Vector2)n*step;if(Mathf.Abs(p.x)>9.8f||Mathf.Abs(p.y)>6.8f||prev.ContainsKey(n)||!ClearPoint(p,radius))continue;prev[n]=a;queue.Enqueue(n);}}
                Check(prev.ContainsKey(goal),"Wheelchair.RouteRadius."+target);var path=new List<Vector2>();var node=goal;while(node!=start){path.Add((Vector2)node*step);node=prev[node];}path.Reverse();var body=Player.GetComponent<Rigidbody2D>();
                foreach(var point in path){double end=EditorApplication.timeSinceStartup+2;while(Vector2.Distance(body.position,point)>.08f&&EditorApplication.timeSinceStartup<end){body.MovePosition(Vector2.MoveTowards(body.position,point,.08f));yield return Wait(.025f);}Check(Vector2.Distance(body.position,point)<.1f,"Wheelchair.PhysicsWaypoint."+point);}
                Note("Wheelchair target="+target+";waypoints="+path.Count);
            }foreach(var x in monsters)if(x!=null)x.enabled=true;
        }

        static IEnumerator WheelchairWalking(){
            Check(GameSessionData.SelectedPlayerPathId=="path.vision","Walking.PathMobility");var monsters=Monsters().SelectMany(x=>x.GetComponents<Collider2D>()).ToArray();foreach(var x in monsters)x.enabled=false;
            foreach(var target in AccessTargets()){
                float radius=Player.GetComponent<CircleCollider2D>().radius*Player.transform.lossyScale.x+.03f;const float step=.25f;var start=new Vector2Int(Mathf.RoundToInt(Player.transform.position.x/step),Mathf.RoundToInt(Player.transform.position.y/step));var goal=new Vector2Int(Mathf.RoundToInt(target.x/step),Mathf.RoundToInt(target.y/step));var queue=new Queue<Vector2Int>();var prev=new Dictionary<Vector2Int,Vector2Int>();queue.Enqueue(start);prev[start]=start;
                while(queue.Count>0&&!prev.ContainsKey(goal)){var a=queue.Dequeue();foreach(var dir in new[]{Vector2Int.up,Vector2Int.down,Vector2Int.left,Vector2Int.right}){var n=a+dir;Vector2 p=(Vector2)n*step;if(Mathf.Abs(p.x)>9.8f||Mathf.Abs(p.y)>6.8f||prev.ContainsKey(n)||!ClearPoint(p,radius))continue;prev[n]=a;queue.Enqueue(n);}}
                Check(prev.ContainsKey(goal),"Walking.RouteRadius."+target);var path=new List<Vector2>();var node=goal;while(node!=start){path.Add((Vector2)node*step);node=prev[node];}path.Reverse();var body=Player.GetComponent<Rigidbody2D>();
                foreach(var point in path){double end=EditorApplication.timeSinceStartup+2;while(Vector2.Distance(body.position,point)>.08f&&EditorApplication.timeSinceStartup<end){body.MovePosition(Vector2.MoveTowards(body.position,point,.08f));yield return Wait(.025f);}Check(Vector2.Distance(body.position,point)<.1f,"Walking.PhysicsWaypoint."+point);}
                Note("Wheelchair target="+target+";waypoints="+path.Count);
            }foreach(var x in monsters)if(x!=null)x.enabled=true;
        }

        static IEnumerator PhysicalExit(string source,string destination,Vector2 start,float targetX){
            Check(SceneManager.GetActiveScene().name==source,"PhysicalExit.Source."+source);At(start);yield return Wait(.1f);var body=Player.GetComponent<Rigidbody2D>();double end=EditorApplication.timeSinceStartup+15;
            while(SceneManager.GetActiveScene().name==source&&EditorApplication.timeSinceStartup<end){if(body!=null)body.MovePosition(Vector2.MoveTowards(body.position,new Vector2(targetX,start.y),.06f));yield return Wait(.025f);}
            yield return Settled(destination);Check(SceneManager.GetActiveScene().name==destination,"PhysicalExit.ActualTrigger."+source+".To."+destination);
        }


// 원본 Scene은 저장하지 않습니다. 30개 기존 도로만 이동/회전하며 B의 추가12개는 Collider 없는 임시 Renderer입니다.
static void ApplyRoad(string mode){
foreach(var t in GameObject.Find("FieldEnvironment").GetComponentsInChildren<Transform>(true).Where(x=>x.name.StartsWith("RoadV2Extra_")).ToArray())Object.DestroyImmediate(t.gameObject);
foreach(var r in Env())if(mapping.ContainsKey(r.name))r.sprite=AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_Project/QA_Field05/"+mapping[r.name]);
if(mode=="V1"||mode=="Comparison")return;
var env=Env();var parent=GameObject.Find("FieldEnvironment").transform;
for(int i=0;i<15;i++)foreach(bool upper in new[]{true,false}){
var names=env.Where(x=>x.name.StartsWith(upper?"RoadLeft_":"RoadRight_")).OrderBy(x=>float.Parse(x.name.Substring(x.name.IndexOf('_')+1),System.Globalization.CultureInfo.InvariantCulture)).ToArray();var r=names[i];r.transform.position=new Vector3(i-4,upper?.5f:-.5f,0);r.transform.rotation=Quaternion.Euler(0,0,-90);r.flipX=false;r.flipY=false;
}
if(mode=="B"||mode=="Final")for(int x=-10;x<=-5;x++)foreach(bool upper in new[]{true,false}){var template=env.First(r=>r.name.StartsWith(upper?"RoadLeft_":"RoadRight_"));var go=new GameObject("RoadV2Extra_"+x+"_"+(upper?"Upper":"Lower"));go.transform.SetParent(parent,false);go.transform.position=new Vector3(x,upper?.5f:-.5f,0);go.transform.rotation=Quaternion.Euler(0,0,-90);var sr=go.AddComponent<SpriteRenderer>();sr.sprite=template.sprite;sr.color=template.color;sr.sharedMaterial=template.sharedMaterial;sr.sortingLayerID=template.sortingLayerID;sr.sortingOrder=template.sortingOrder;}
if(mode=="Final"){
 foreach(var r in Env()){
  if(!r.name.StartsWith("Road")){r.sprite=AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_Project/Art/Field05Renewal/"+mapping[r.name]);continue;}
  bool upper=r.transform.position.y>0,west=Mathf.Abs(r.transform.position.x+10)<.01f;
  r.sprite=AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_Project/Art/Field05Renewal/Road/LL_C2_F05_Road_EW_"+(west?"WestEnd_":"")+(upper?"Upper":"Lower")+"_128_v2.png");
  r.transform.rotation=Quaternion.identity;r.flipX=false;r.flipY=false;
 }
 Check(Env().Select(x=>AssetDatabase.GetAssetPath(x.sprite)).Distinct().Count()==14,"Final.ActivePNG14");
 Check(Env().Where(x=>x.name.StartsWith("Road")).All(x=>x.transform.eulerAngles.z==0&&!x.flipX&&!x.flipY),"Final.Rotation0FlipFalse");
}
Check(Env().Length==((mode=="B"||mode=="Final")?375:363),mode+".RendererCount");Check(Env().Where(x=>x.name.StartsWith("Road")).All(x=>x.GetComponents<Collider2D>().Length==0),mode+".Road.NoCollider");}
static string SpriteProtection(Transform x){var sr=x.GetComponent<SpriteRenderer>();return sr!=null?sr.sprite+"|"+sr.color+"|"+sr.sortingOrder:"";}
static string ProtectedSnapshot()=>string.Join("\n",Object.FindObjectsByType<Transform>(FindObjectsSortMode.None).Where(x=>x.gameObject.scene.name==Field&&!x.name.StartsWith("Road")).OrderBy(x=>x.GetEntityId()).Select(x=>x.GetEntityId()+"|"+x.position+"|"+x.localScale+"|"+string.Join(";",x.GetComponents<Collider2D>().Select(c=>c.offset+"|"+c.bounds+"|"+c.enabled+"|"+c.isTrigger))+"|"+SpriteProtection(x)));
static void ReportRoad(string mode){var lines=new List<string>{"mode,name,entityId,x,y,rotationZ,scale,flipX,flipY,spriteGUID,spriteFileID,sorting,colliderCount"};foreach(var r in Env().Where(x=>x.name.StartsWith("Road")).OrderBy(x=>x.name)){string guid;long fileid;AssetDatabase.TryGetGUIDAndLocalFileIdentifier(r.sprite,out guid,out fileid);lines.Add(mode+","+r.name+","+r.GetEntityId()+","+r.transform.position.x+","+r.transform.position.y+","+r.transform.eulerAngles.z+","+r.transform.localScale.ToString().Replace(',', ';')+","+r.flipX+","+r.flipY+","+guid+","+fileid+","+r.sortingOrder+","+r.GetComponents<Collider2D>().Length);}File.WriteAllLines(Path.Combine(Root,mode+"_RoadMapping.csv"),lines);}
static void RoadViews(string mode){foreach(int width in new[]{1280,960,1680}){string a=width==1280?"16x9":width==960?"4x3":"21x9";
Capture(mode+"_overview_"+a+".png",new Vector3(0,0,-10),7.5f,width,720);
Capture(mode+"_east_"+a+".png",new Vector3(8,0,-10),2.5f,width,720);
Capture(mode+"_west_"+a+".png",new Vector3(-7,0,-10),3,width,720);
Capture(mode+"_center_"+a+".png",new Vector3(0,0,-10),3,width,720);
Capture(mode+"_irrigation_"+a+".png",new Vector3(5,2,-10),2,width,720);
Capture(mode+"_watchpost_"+a+".png",new Vector3(-2,2,-10),2,width,720);
Capture(mode+"_pulse_"+a+".png",new Vector3(-6,-2,-10),2,width,720);
} }
static void QuestFixture(){var def=Resources.LoadAll<QuestDefinition>("QuestDefinitions").Single(x=>x.QuestId==Chapter2IntroFlow.Main14);var completed=Resources.LoadAll<QuestDefinition>("QuestDefinitions").Where(x=>x.QuestId.StartsWith("main_")&&string.CompareOrdinal(x.QuestId,Chapter2IntroFlow.Main14)<0).Select(x=>x.QuestId).ToArray();QuestService.ImportSaveData(new QuestProgressSaveData{CompletedQuestIds=completed,ActiveQuests=new[]{new ActiveQuestSaveData{QuestId=Chapter2IntroFlow.Main14,Objectives=def.Objectives.Select((x,i)=>new QuestObjectiveProgressData{ObjectiveId=x.ObjectiveId,CurrentCount=i<2?1:0}).ToArray()}},TrackedQuestId=Chapter2IntroFlow.Main14});}
static Vector2[] AccessTargets()=>SessionState.GetBool(Key+"Access",false)?new[]{new Vector2(3,-2),new Vector2(-4,2),new Vector2(-9.5f,.25f),new Vector2(9.5f,.25f)}:new[]{new Vector2(7,.25f),new Vector2(5,1),new Vector2(1,-1),new Vector2(-2,1),new Vector2(-6,-1),new Vector2(9.5f,.25f)};
static IEnumerator AccessOnly(){foreach(var mode in new[]{"Final"}){SessionState.SetString(Key+"Mode",mode);Directory.CreateDirectory(Root);Checks.Clear();Errors.Clear();GameSaveService.AuditSaveDirectory=Path.Combine(Root,"Saves");UserSettingsService.BeginAudit(Path.Combine(Root,"Settings"));File.WriteAllText(Path.Combine(Root,"status.txt"),"RUNNING");Seed();QuestFixture();GameSessionData.SetPendingSpawnPoint("Spawn_From_Arbel");yield return Load(Field);Apply();At(new Vector2(8,0));yield return Wheelchair();GameSessionData.SelectPlayerPath("path.vision");yield return Load(Field);Apply();At(new Vector2(8,0));yield return WheelchairWalking();File.WriteAllText(Path.Combine(Root,"status.txt"),"ACCESS_PASS");}}
static IEnumerator Run(){if(SessionState.GetBool(Key+"Access",false)){yield return AccessOnly();yield break;}AudioListener.volume=0;yield return Load("Bootstrap");Seed();QuestFixture();GameSessionData.SetPendingSpawnPoint("Spawn_From_Arbel");yield return Load(Field);At(new Vector2(8,0));yield return Wait(.3f);
// 같은 프레임에서 카메라와 캐릭터 상태를 유지한 채 세 후보를 순차 RenderTexture로 비교합니다.
ApplyRoad("V1");var protectedState=ProtectedSnapshot();foreach(var mode in new[]{"V1","B","Final"}){ApplyRoad(mode);Check(ProtectedSnapshot()==protectedState,mode+".ProtectedNonRoadUnchanged");RoadViews(mode);ReportRoad(mode);Check(!Env().Any(x=>AssetDatabase.GetAssetPath(x.sprite).Contains("Optional_Story")),mode+".OptionalZero");}
File.WriteAllLines(Path.Combine(Root,"comparison_console.txt"),Errors);
foreach(var mode in new[]{"Final"}){
SessionState.SetString(Key+"Mode",mode);Directory.CreateDirectory(Root);Checks.Clear();Errors.Clear();File.WriteAllText(Path.Combine(Root,"status.txt"),"RUNNING");File.WriteAllText(Path.Combine(Root,"console_errors.txt"),"");GameSaveService.AuditSaveDirectory=Path.Combine(Root,"Saves");UserSettingsService.BeginAudit(Path.Combine(Root,"Settings"));
Seed();QuestFixture();GameSessionData.SetPendingSpawnPoint("Spawn_From_Arbel");yield return Load(Field);Apply();
Check(Env().All(x=>x.sprite!=null),mode+".NoMissingSprite");Check(Object.FindObjectsByType<Transform>(FindObjectsSortMode.None).All(x=>GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(x.gameObject)==0),mode+".NoMissingScript");
foreach(var m in Monsters()){var d=m.SpawnDefinition;Check(d.Position==(d.SpawnId.Contains("soot")?new Vector2(3,-3):new Vector2(-4,3))&&d.ActivityRadius==1&&d.RespawnSeconds==35,mode+".MonsterData."+d.SpawnId);}
Check(Object.FindObjectsByType<SceneTransitionTrigger>(FindObjectsSortMode.None).Length==1,mode+".OnlyEastExit");
At(new Vector2(8,0));yield return Wheelchair();GameSessionData.SelectPlayerPath("path.vision");yield return Load(Field);Apply();At(new Vector2(8,0));yield return WheelchairWalking();GameSessionData.SelectPlayerPath("path.mobility");yield return Load(Field);Apply();yield return Cameras();yield return Continue();yield return Story();
yield return PhysicalExit(Field,"Arbel",new Vector2(9.5f,.25f),10.9f);Check(Vector2.Distance(Player.transform.position,new Vector2(-8,0))<.2f,mode+".PhysicalArbelSpawn");yield return PhysicalExit("Arbel",Field,new Vector2(-9.5f,.25f),-10.9f);Apply();Check(Vector2.Distance(Player.transform.position,new Vector2(8,0))<.2f,mode+".PhysicalField05Spawn");yield return MonsterBattles();
var unexpected=Errors.Where(x=>!x.Contains("UnityEditor.Search")&&!x.Contains("Host type is not matching any asset type at Path Packages/com.unity.render-pipelines.universal/Shaders/Nature/SpeedTree8_PBRLit.shadergraph.")).ToArray();File.WriteAllLines(Path.Combine(Root,"unexpected_console.txt"),unexpected);Check(unexpected.Length==0,mode+".NoAdditionalRuntimeError");File.WriteAllText(Path.Combine(Root,"status.txt"),"FUNCTION_PASS_ENVIRONMENT_PARTIAL_USER_REVIEW_PENDING");}
}
        static IEnumerator Flatten(IEnumerator r){var stack=new Stack<IEnumerator>();stack.Push(r);while(stack.Count>0){var top=stack.Peek();if(!top.MoveNext()){stack.Pop();continue;}if(top.Current is IEnumerator next){stack.Push(next);continue;}yield return top.Current;}}
        static void Tick(){try{if(EditorApplication.timeSinceStartup>deadline)throw new TimeoutException("Art QA timeout");if(routine.MoveNext())return;EditorApplication.update-=Tick;SessionState.SetBool(Key,false);EditorApplication.Exit(0);}catch(Exception e){Checks.Add("FAIL|"+e);File.WriteAllLines(Path.Combine(Root,"checks.txt"),Checks);File.WriteAllText(Path.Combine(Root,"status.txt"),"FAIL");Debug.LogException(e);EditorApplication.Exit(1);}}
    }
}
