"""보호 설치의 C# Host가 만든 익명 stdin만 받습니다. 직접 실행도 실제 서비스 토큰을 요구합니다."""
import argparse
import json
from pathlib import Path
import sys

INSTALL=Path(__file__).resolve().parents[3]
sys.path.insert(0,str(INSTALL/'Code/Tools/DiscordAICollab'))
from collab.broker_host_runtime import main

if __name__=='__main__':
    parser=argparse.ArgumentParser();parser.add_argument('--action',choices=['initialize','dispatch'],required=True)
    args=parser.parse_args()
    try:
        raw=sys.stdin.buffer.read(32769)
        if len(raw)>32768:raise ValueError('FRAME_HOLD')
        result=main(INSTALL,args.action,raw)
        print(json.dumps(result,ensure_ascii=False,allow_nan=False))
    except Exception:
        print('{"status":"HOLD","reason":"HOST_ENTRY_HOLD_NO_RETRY"}')
        raise SystemExit(2)
