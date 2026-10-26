// 테스트 전용: 서비스 Host가 쓰는 익명 stdin Job 경로를 가짜 실행기 두 개로 검증합니다.
using System;
using System.Threading;
using System.Threading.Tasks;
public static class JoinNativeTest {
 public static object[] Parallel(string exe,string[] args,string cwd,byte[] input) {
  var a=Task.Run(()=>LimitlessBrokerJob90V1.RunInput(exe,args,cwd,6000,CancellationToken.None,input));
  var b=Task.Run(()=>LimitlessBrokerJob90V1.RunInput(exe,args,cwd,6000,CancellationToken.None,input));
  Task.WaitAll(a,b);return new object[]{a.Result,b.Result};
 }
}
