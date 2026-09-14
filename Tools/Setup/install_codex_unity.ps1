$ErrorActionPreference = 'Stop'

Write-Host '=== Project-Limitless Codex + Unity setup ==='

function Test-Command($name) {
    return [bool](Get-Command $name -ErrorAction SilentlyContinue)
}

if (-not (Test-Command 'codex')) {
    throw 'Codex CLI를 찾을 수 없습니다. Codex가 설치된 터미널에서 다시 실행하세요.'
}

Write-Host '[1/3] Unity 공식 Codex 플러그인 marketplace 등록'
codex plugin marketplace add Unity-Technologies/unity-agent-plugin

Write-Host '[2/3] Unity 공식 Codex 플러그인 설치'
codex plugin add unity@unity-agent-plugin

Write-Host '[3/3] 설치 상태 확인'
codex plugin list

Write-Host ''
Write-Host 'Codex 쪽 Unity 공식 플러그인 설치가 끝났습니다.'
Write-Host 'Unity 프로젝트를 열면 Packages/manifest.json에 고정된 MCP for Unity 패키지가 설치됩니다.'
Write-Host '그 다음 Unity에서 Window > MCP for Unity를 열고 setup wizard를 완료한 뒤 Codex를 선택해 Configure Selected를 누르세요.'
Write-Host 'Python/uv가 없으면 MCP for Unity wizard의 안내에 따라 설치하세요.'
Write-Host '상태가 Connected로 표시되면 Codex가 Unity Editor를 직접 읽고 조작할 수 있습니다.'
