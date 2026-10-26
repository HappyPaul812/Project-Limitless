param([Parameter(Mandatory)][ValidatePattern('^LimitlessPlanning-[a-f0-9]{32}-(arm|work|control)$')][string]$PipeName,
 [Parameter(Mandatory)][ValidatePattern('^S-1-5-21-[0-9-]+$')][string]$ServiceSid,
 [ValidateRange(1,90000)][int]$BudgetMs=85000)
# 봇용 IPC 클라이언트입니다. 서비스 실행/작업 등록/관리자 승인 발급은 하지 않습니다.
$ErrorActionPreference='Stop'
$pipe=$null;$cancel=$null
try {
 $source=Join-Path $PSScriptRoot 'Session0PipeBoundary.cs'
 Add-Type -Path $source
 $inputBytes=[Console]::OpenStandardInput()
 $data=[Collections.Generic.List[byte]]::new()
 while(($next=$inputBytes.ReadByte()) -ne -1){
  if($data.Count -ge 16384){throw 'FRAME_SIZE_HOLD'};$data.Add([byte]$next)
 }
 $cancel=[Threading.CancellationTokenSource]::new($BudgetMs)
 $pipe=[LimitlessSession0PipeV1]::Connect($PipeName,$ServiceSid)
 [LimitlessSession0PipeV1]::WriteFrame($pipe,$data.ToArray(),$cancel.Token).GetAwaiter().GetResult()
 $answer=[LimitlessSession0PipeV1]::ReadFrame($pipe,$cancel.Token).GetAwaiter().GetResult()
 # stdout에는 서비스 endpoint가 보낸 bounded JSON만 전달합니다. stderr/프로필은 읽지 않습니다.
 [Console]::Out.Write([Text.Encoding]::UTF8.GetString($answer))
} catch {
 [Console]::Out.Write('{"status":"HOLD","reason":"PIPE_AUTH_OR_PROTOCOL_HOLD"}')
 exit 2
} finally {
 if($pipe){$pipe.Dispose()};if($cancel){$cancel.Dispose()}
}
