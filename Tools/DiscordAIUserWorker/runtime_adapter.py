"""기존 4역할 검증기를 바꾸지 않고 자기 계정의 원본 실패 로그를 연결합니다."""
import os
import subprocess
import uuid
from collab.private_execution_log import prepare_archive, save_execution_log


def recorded_run_once(store, identity, plan, workspace, executable, context, verified_runner,
                      process_runner=subprocess.run):
    request = uuid.uuid4().hex
    archive = prepare_archive(context['profile'], request)
    private = {}
    secrets = [v for k,v in os.environ.items() if any(word in k.upper() for word in ('TOKEN','SECRET','PASSWORD','API_KEY'))]

    def invoke(command, **kwargs):
        if len(command) < 2 or command[1] != 'exec':
            # 버전·로그인 상태·features 원문은 기록하지 않습니다.
            return process_runner(command, **kwargs)
        options = dict(kwargs)
        for key in ('text','encoding','errors'):
            options.pop(key,None)
        if isinstance(options.get('input'),str):
            options['input'] = options['input'].encode('utf8')
        metadata = {'request_id':request,'plan_sha256':plan['sha256'],'account':context['name'],
                    'session_id':context['session_id'],'codex_home':context['codex_home'],
                    'argv':list(map(str,command)),'timeout_seconds':90}
        try:
            completed = process_runner(command, **options)
        except subprocess.TimeoutExpired as exc:
            private.update(save_execution_log(archive,exc.output or b'',exc.stderr or b'',
                                              {**metadata,'exit_code':None,'termination':'TIMEOUT'},secrets))
            raise
        private.update(save_execution_log(archive,completed.stdout,completed.stderr,
                                          {**metadata,'exit_code':completed.returncode},secrets))
        return subprocess.CompletedProcess(completed.args,completed.returncode,
                                            completed.stdout.decode('utf8',errors='replace'),
                                            completed.stderr.decode('utf8',errors='replace'))

    result = verified_runner(store,identity,plan,workspace,executable,invoke=invoke)
    if private:
        result['private_log'] = private
    return result
