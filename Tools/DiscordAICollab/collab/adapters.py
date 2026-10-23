"""네트워크·추론은 명시적인 별도 단계입니다. 모의 결과는 실제 QA로 승격하지 않습니다."""
from __future__ import annotations

import json
import re
import urllib.request
from pathlib import Path

from .core import Denied

REPOSITORY = 'HappyPaul812/Project-Limitless'


class GitHubReader:
    def __init__(self, token=None, transport=urllib.request.urlopen):
        self.token,self.transport = token,transport

    def read(self, kind, number=None):
        # 경로·호스트·HTTP method를 호출자가 조립할 수 없게 합니다.
        if kind == 'base':
            endpoint = 'commits/main'
        elif kind=='canonical_ref' and number in ('main','docs/planning','docs/art-new','docs/art-renewal'):
            from urllib.parse import quote
            endpoint='commits/'+quote(number,safe='')
        elif kind in ('issues','pulls','comments') and type(number) is int and number>0:
            endpoint = f'issues/{number}/comments?per_page=100' if kind=='comments' else f'{kind}/{number}'
        else:
            raise Denied('GitHub 읽기 대상 오류')
        req = urllib.request.Request(f'https://api.github.com/repos/{REPOSITORY}/{endpoint}',
                                     headers={'Accept':'application/vnd.github+json','User-Agent':'limitless-collab-pilot','X-GitHub-Api-Version':'2022-11-28'})
        if self.token:
            req.add_header('Authorization','Bearer '+self.token)
        # urllib 기본 리다이렉트는 인증 헤더를 타 호스트로 보낼 수 있으므로 거부합니다.
        class NoRedirect(urllib.request.HTTPRedirectHandler):
            def redirect_request(self,*args,**kwargs):
                raise Denied('GitHub 리다이렉트 거부')
        transport = self.transport
        if transport is urllib.request.urlopen:
            transport = urllib.request.build_opener(NoRedirect()).open
        try:
            with transport(req,timeout=15) as response:
                content = response.read(2*1024*1024+1)
                if len(content)>2*1024*1024:
                    raise Denied('GitHub 응답 크기 초과')
                return json.loads(content)
        except Denied:
            raise
        except Exception as exc:
            # 토큰이 포함될 수 있는 HTTP/네트워크 원문은 로그에 노출하지 않습니다.
            raise Denied('GitHub 읽기 실패; 자동 재시도 없음') from exc

    def write(self,*_args,**_kwargs):
        raise Denied('파일럿 GitHub 쓰기·Push·merge 비활성')


class CodexRunner:
    """현재는 명령 미리보기까지만 제공합니다. 기존 로그인/대화를 읽거나 복사하지 않습니다."""
    def command_preview(self, executable, workspace):
        path = Path(workspace).resolve()
        if not (path/'.git').is_file() or any(p.lower() in ('unity','userdata','saves') for p in path.parts):
            raise Denied('별도 Git worktree가 필요합니다')
        return [str(executable),'exec','--json','--ephemeral','--ignore-user-config',
                '--sandbox','read-only','-C',str(path),'-']

    def run(self,*_args,**_kwargs):
        # .env의 값 하나로 유료 호출·외부 MCP 권한을 우회할 수 없도록 실행기를 차단합니다.
        raise Denied('실제 Codex 호출은 승인·격리·한도 실증 후 별도 단계입니다')


class MockWorker:
    def __init__(self,store):
        self.store = store

    def tick(self):
        self.store.recover()
        for task in self.store.snapshot()['tasks']:
            if task['state']=='NEW':
                self.store.triage(task['id'])
            elif task['state']=='REVIEW_PENDING':
                self.store.auto_handoff(task['id'])
        task = self.store.claim()
        if task is None:
            return None
        # TECH_PASS는 모의 파이프라인 자체의 계약 통과입니다. Unity·미술 PASS가 아닙니다.
        evidence = {'mode':'MOCK','role':task['role'],'unity':'NOT_VERIFIED',
                    'external_ai_called':False,'product_applied':False,
                    'finding':'복제 이벤트 전달 계약만 확인; 원래 이슈의 QA는 재실행하지 않음'}
        self.store.finish(task['id'],task['lease'],'TECH_PASS',evidence)
        return self.store.auto_handoff(task['id'])


def format_message(task):
    # Discord mention·코드블록·제어 문자를 제거하고 본문 길이를 제한합니다.
    title = re.sub(r'[\x00-\x1f@`<>]',' ',task['title'])[:180]
    return f"[{task['role']}] {task['id']} | {task['state']} | {title}"
