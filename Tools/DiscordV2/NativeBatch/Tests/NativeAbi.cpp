// 설치된 Microsoft SDK 헤더와 관리 선언의 x64 layout/vtable 슬롯을 컴파일로 대조합니다.
// /Zs 구문 검사만 사용하며 실행 파일을 만들거나 Windows API를 호출하지 않습니다.
#define CINTERFACE
#include <windows.h>
#include <taskschd.h>
#include <lm.h>
#include <ntsecapi.h>
#include <tlhelp32.h>
#include <stddef.h>
static_assert(sizeof(void*)==8);
static_assert(PROCESS_ALL_ACCESS==0x1fffff && THREAD_ALL_ACCESS==0x1fffff && TOKEN_ALL_ACCESS==0xf01ff && JOB_OBJECT_ALL_ACCESS==0x1f003f);
static_assert(sizeof(SECURITY_ATTRIBUTES)==24 && sizeof(VARIANT)==24);
static_assert(sizeof(FILE_ATTRIBUTE_TAG_INFO)==8 && sizeof(LUID_AND_ATTRIBUTES)==12 && sizeof(SID_AND_ATTRIBUTES)==16 && offsetof(TOKEN_GROUPS,Groups)==8);
static_assert(sizeof(GENERIC_MAPPING)==16 && sizeof(LSA_OBJECT_ATTRIBUTES)==48 && sizeof(LSA_UNICODE_STRING)==16);
static_assert(sizeof(PROCESSENTRY32W)==568 && offsetof(PROCESSENTRY32W,szExeFile)==44);
static_assert(sizeof(STARTUPINFOW)==104 && sizeof(STARTUPINFOEXW)==112 && sizeof(PROCESS_INFORMATION)==24);
static_assert(sizeof(JOBOBJECT_BASIC_LIMIT_INFORMATION)==64 && sizeof(JOBOBJECT_EXTENDED_LIMIT_INFORMATION)==144 && sizeof(JOBOBJECT_BASIC_ACCOUNTING_INFORMATION)==48);
static_assert(sizeof(TOKEN_STATISTICS)==56 && offsetof(SECURITY_LOGON_SESSION_DATA,LogonTime)==80);
static_assert(sizeof(USER_INFO_4)==192 && offsetof(USER_INFO_4,usri4_user_sid)==152);
static_assert(offsetof(ITaskServiceVtbl,GetFolder)==7*8 && offsetof(ITaskServiceVtbl,NewTask)==9*8 && offsetof(ITaskServiceVtbl,Connect)==10*8);
static_assert(offsetof(ITaskFolderVtbl,GetTask)==13*8 && offsetof(ITaskFolderVtbl,DeleteTask)==15*8 && offsetof(ITaskFolderVtbl,RegisterTaskDefinition)==17*8);
static_assert(offsetof(ITaskDefinitionVtbl,put_XmlText)==20*8);
static_assert(offsetof(IRegisteredTaskVtbl,get_Enabled)==10*8 && offsetof(IRegisteredTaskVtbl,put_Enabled)==11*8 && offsetof(IRegisteredTaskVtbl,Run)==12*8);
static_assert(sizeof(TASK_STATE)==4 && offsetof(IRegisteredTaskVtbl,get_State)==9*8);
static_assert(offsetof(IRegisteredTaskVtbl,GetInstances)==14*8 && offsetof(IRegisteredTaskVtbl,get_Xml)==20*8 && offsetof(IRegisteredTaskVtbl,GetSecurityDescriptor)==21*8);
static_assert(offsetof(IRunningTaskCollectionVtbl,get_Count)==7*8 && offsetof(IRunningTaskCollectionVtbl,get_Item)==8*8);
static_assert(offsetof(IRunningTaskVtbl,get_InstanceGuid)==8*8 && offsetof(IRunningTaskVtbl,get_Path)==9*8 && offsetof(IRunningTaskVtbl,Stop)==12*8);
