using System.Security.Cryptography;
using System.Text.Json;

// CLI는 승인 수단이 아닙니다. 정확한 형식만 분기하고 각 경로의 보호된 읽기 검증으로 넘깁니다.
// Main과 모의 시험이 같은 분기를 사용하며 강제/환경변수/전역 운영 우회는 없습니다.
internal static class TrialEntryGate
{
    internal static int Dispatch(string[] args, AdmissionBinding binding, Func<int> admin, Func<int> supervisor)
    {
        AdmissionPolicy.Require(args.Length >= 5 && args[1] == "--contract" && args[3] == "--contract-sha"
            && args[2] == Path.Combine(binding.CodeRoot,"TrialContract.json") && AdmissionPolicy.Sha(args[4]),"ENTRY_ARGUMENTS_HOLD");
        string mode=args[0];
        bool valid=mode switch {
            "--register-only"=>args.Length==6 && args[5]=="--password-console",
            "--stop-owned-instance"=>args.Length==7 && args[5]=="--instance" && Guid.TryParseExact(args[6],"D",out var id) && id!=Guid.Empty,
            "--start-once" or "--remove-owned-task" or "--supervise"=>args.Length==5,
            _=>false };
        AdmissionPolicy.Require(valid,"ENTRY_MODE_HOLD");
        return mode=="--supervise"?supervisor():admin();
    }

    // 모든 인자는 내부 어댑터에서만 공급합니다. verifyReadOnly는 이미지/보호경로/실계정 조회이며 쓰기하지 않습니다.
    // 고비용 읽기 뒤 시각과 신뢰를 다시 검사한 후에만 호출자가 최초 변경/영구 소비를 할 수 있습니다.
    internal static void Validate(byte[] raw,string sha,string mode,byte[] operationRaw,AdmissionBinding binding,
        Func<DateTimeOffset> now,Func<string,int,byte[]> trust,Action<TrialContract> verifyReadOnly,
        Func<AccountObservation> account,CancellationToken cancellation=default, CurrentFakeOnlyContract.ExecutionWindow? window=null)
    {
        cancellation.ThrowIfCancellationRequested();
        AdmissionPolicy.Require(AdmissionPolicy.Sha(sha) && Convert.ToHexString(SHA256.HashData(raw)).ToLowerInvariant()==sha,"ENTRY_CONTRACT_SHA");
        AdmissionPolicy.Contract(raw,now(),binding,window:window);
        var c=JsonSerializer.Deserialize(raw,ContractJson.Default.TrialContract)!;
        NativeTrialPolicy.Shape(c,mode,now(),window);
        string operation=mode=="--supervise"?"--start-once":mode;
        var a=OperationApproval.Validate(operationRaw,c.TrialId,sha,operation,now(),false,window);
        FakeOnlyPolicy.Evidence(c,now(),trust,window:window);
        verifyReadOnly(c);
        var observed=account();
        FakeOnlyPolicy.Evidence(c,now(),trust,window:window);
        NativeTrialPolicy.Authorize(c,sha,mode,a,observed,now(),binding,window);
        cancellation.ThrowIfCancellationRequested();
    }
}
