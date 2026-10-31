using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

// 정적 검토용 API 초안. 실제 Windows 호출은 제공하지 않습니다.
// 기존 WindowsBootstrapHost.CreateProtected와 동일한 초기 SD 전달 구조를 검토합니다.
// 부모 HANDLE pin, 토큰, 최종 경로/owner/DACL readback은 기존 구현 재사용 대상이며
// 아래 선언 자체가 그 조건을 증명하지 않습니다. 실제 backend 연결 전에는 HOLD입니다.
internal static class StageSecurityApi
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct SecurityAttributes
    {
        internal uint Length;
        internal IntPtr Descriptor;
        internal int InheritHandle;
    }

    [DllImport("kernel32.dll", CharSet=CharSet.Unicode, SetLastError=true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool CreateDirectoryW(string path, ref SecurityAttributes security);

    // 생성 후 SetNamedSecurityInfo/Set-Acl을 호출하는 경로는 두지 않습니다.
    // 검증 실패 시 실제 호출 0회를 유지하기 위해 초안에는 P/Invoke 호출부가 없습니다.
}
