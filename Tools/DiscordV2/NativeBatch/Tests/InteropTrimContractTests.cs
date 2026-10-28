using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.InteropServices;

// 변경한 제네릭 보존 계약과 그 반환 형식만 검사합니다. Windows API·기존 시험 묶음은 호출하지 않습니다.
internal static partial class AdmissionTests
{
    private static int InteropTrimContract()
    {
        Test("interop-trim-constructor-contract-not-suppression",()=>
        {
            var method=typeof(Native).GetMethod("Info",BindingFlags.Static|BindingFlags.NonPublic)!;
            var parameter=method.GetGenericArguments().Single();
            var requirement=parameter.GetCustomAttribute<DynamicallyAccessedMembersAttribute>();
            Check(requirement?.MemberTypes==(DynamicallyAccessedMemberTypes.PublicConstructors|DynamicallyAccessedMemberTypes.NonPublicConstructors));
            Check((parameter.GenericParameterAttributes&GenericParameterAttributes.NotNullableValueTypeConstraint)!=0);
            Check(method.GetCustomAttribute<RequiresUnreferencedCodeAttribute>() is null);
        });
        Test("interop-trim-int-layout-roundtrip",()=>
        {
            Check(nint.Size==8&&Marshal.SizeOf<int>()==4);nint p=Marshal.AllocHGlobal(4);
            try{Marshal.WriteInt32(p,unchecked((int)0x87654321));Check(Marshal.PtrToStructure<int>(p)==unchecked((int)0x87654321));}
            finally{Marshal.FreeHGlobal(p);}
        });
        Test("interop-trim-tokenstats-x64-field-roundtrip",()=>
        {
            string[] names=["TokenId","AuthenticationId","Expiration","Type","Impersonation","Charged","Available","Groups","Privileges","ModifiedId"];
            int[] offsets=[0,8,16,24,28,32,36,40,44,48];
            Check(nint.Size==8&&Marshal.SizeOf<Native.TokenStats>()==56&&Marshal.SizeOf<Native.Luid>()==8);
            Check(Marshal.OffsetOf<Native.Luid>("Low")==0&&Marshal.OffsetOf<Native.Luid>("High")==4);
            for(int i=0;i<names.Length;i++)Check(Marshal.OffsetOf<Native.TokenStats>(names[i])==offsets[i]);
            nint p=Marshal.AllocHGlobal(56);
            try
            {
                // 모든 필드에 서로 다른 바이트를 넣어 참조·생성자 보존 선언이 배치를 바꾸지 않는지 확인합니다.
                for(int i=0;i<56;i++)Marshal.WriteByte(p,i,(byte)(i+1));
                var value=Marshal.PtrToStructure<Native.TokenStats>(p);
                Check(value.TokenId.Low==0x04030201&&value.TokenId.High==0x08070605);
                Check(value.AuthenticationId.Low==0x0c0b0a09&&value.AuthenticationId.High==0x100f0e0d);
                Check(value.Expiration==0x1817161514131211&&value.Type==0x1c1b1a19&&value.Impersonation==0x201f1e1d);
                Check(value.Charged==0x24232221&&value.Available==0x28272625&&value.Groups==0x2c2b2a29&&value.Privileges==0x302f2e2d);
                Check(value.ModifiedId.Low==0x34333231&&value.ModifiedId.High==0x38373635);
            }
            finally{Marshal.FreeHGlobal(p);}
        });
        Console.WriteLine($"INTEROP_TRIM_CONTRACT: {passed} PASS / {failed} FAIL; WINDOWS_SECURITY_API_CALLS=0; NATIVE_AOT_NOT_VERIFIED");
        return failed==0?0:1;
    }
}
