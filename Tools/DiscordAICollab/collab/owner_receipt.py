"""이미 자동 수령된 대표의 일반 첨부 1건을 재대조합니다. 수동 업무 생성·AI 실행은 없습니다."""
import asyncio
import hashlib
import json
import logging
import os
import time
from pathlib import Path

import discord
from .core import Denied, sha256, valid_sha
from .art_inspection import png
from .delivery_discord import ROOT, engine, identity_config, publish_outbox
from .intake import safe_root, MAX_ZIP


def mark_test_only(deliveries, identity, expected, data, transport):
    """Discord 검증 후 로컬 증거를 결합합니다. 기존 파일 바이트·기존 3업무는 변경하지 않습니다."""
    store=deliveries.store
    store.authorize(identity)
    digest=valid_sha(expected['sha256'])
    if (transport.get('owner_verified') is not True or transport.get('normal_attachment') is not True
            or transport.get('original_report_verified') is not True):
        raise Denied('OWNER_NORMAL_RECEIPT_NOT_VERIFIED')
    if not isinstance(data,bytes) or not 0<len(data)<=MAX_ZIP or hashlib.sha256(data).hexdigest()!=digest:
        raise Denied('OWNER_ATTACHMENT_SHA_MISMATCH')
    image=png(data)
    key=expected['event_key'];task_id=expected['task_id']
    with store.tx() as db:
        row=db.execute('SELECT * FROM art_deliveries WHERE sha=?',(digest,)).fetchone()
        event=db.execute('SELECT * FROM art_delivery_events WHERE event_key=?',(key,)).fetchone()
        if row is None or event is None or row['task_id']!=task_id or event['sha']!=digest:
            raise Denied('OWNER_RECEIPT_MAPPING_MISMATCH')
        report=json.loads(row['report']);task=store.task(db,task_id)
        evidence=json.loads(task['evidence'] or '{}');payload=json.loads(task['payload'])
        if (report.get('source')!='DISCORD_ATTACHMENT' or task['state']!='BLOCKED'
                or task['tech']!='NOT_VERIFIED' or task['art']!='USER_REVIEW_PENDING'
                or payload.get('artifact_sha')!=digest):
            raise Denied('OWNER_RECEIPT_NOT_BLOCKED_NORMAL_ATTACHMENT')
        artifact=Path(row['artifact'])
        if artifact.is_symlink() or not artifact.is_file():
            raise Denied('OWNER_QUARANTINE_MISSING_OR_LINK')
        safe_root(artifact.parent)
        if artifact.resolve().parent!=deliveries.vault.resolve()/digest or sha256(artifact)!=digest or artifact.stat().st_size!=len(data):
            raise Denied('OWNER_QUARANTINE_TAMPERED')
        verification={'mode':'TEST_ONLY','owner_verified':True,'source':'DISCORD_ATTACHMENT',
                      'event_key':key,'attachment_sha256':digest,'stored_sha256':digest,
                      'bytes':len(data),'format':'PNG','png':image,'verified_at':store.clock(),
                      'task_created_by_existing_watcher':True,'manual_task_creation':False,
                      'ai_calls':0,'unity_runs':0,'art_approval':'TEST_ONLY_NOT_APPROVABLE'}
        report.update(test_only=True,test_verification=verification)
        payload.update(test_only=True,approval_eligible=False)
        evidence.update(test_only=True,owner_attachment_verification=verification)
        title=task['title'] if task['title'].startswith('TEST_ONLY / ') else 'TEST_ONLY / '+task['title']
        db.execute('UPDATE tasks SET title=?,payload=?,evidence=? WHERE id=?',
                   (title,json.dumps(payload,ensure_ascii=False),json.dumps(evidence,ensure_ascii=False),task_id))
        db.execute('UPDATE art_deliveries SET report=? WHERE sha=?',(json.dumps(report,ensure_ascii=False),digest))
        # 원래 수령 이벤트 결과는 과거 사실로 유지하고 별도 재검증 영수증만 추가합니다.
        report_key='owner-receipt-test:'+key
        text=('[ART DELIVERY / TEST_ONLY / OWNER VERIFIED]\n일반 PNG 자동 수령 확인: SHA256 '+digest+
              '\n크기 '+str(len(data))+' bytes / PNG / Codex 검수 대기 '+task_id+
              '\n기존 감시기가 자동 등록한 별도 업무입니다. 시험 전용·실제 미술 승인 불가. AI/Unity/게임 적용0.')
        db.execute('INSERT OR IGNORE INTO art_delivery_outbox VALUES(?,?,?,?,?)',
                   (report_key,text,'PENDING',None,store.clock()))
        store.audit(db,'OWNER_ATTACHMENT_TEST_ONLY_VERIFIED',task_id)
    return {'status':'PASS','task_id':task_id,'sha256':digest,'artifact':str(artifact),
            'report_event_key':report_key,'verification':verification}


async def verify():
    logger=logging.getLogger('discord');logger.disabled=True
    logger.addHandler(logging.NullHandler());logger.propagate=False
    config=json.loads((ROOT/'config.json').read_text(encoding='utf8'))
    expected=json.loads((ROOT/'owner-receipt-plan.json').read_text(encoding='utf8'))
    valid_sha(expected['sha256'])
    deliveries,owner=engine(config)
    parts=expected['event_key'].split(':')
    if len(parts)!=3 or parts[0]!='discord' or not all(p.isdigit() for p in parts[1:]):
        raise Denied('OWNER_EVENT_INVALID')
    owners,guild=identity_config()
    async with discord.Client(intents=discord.Intents.none()) as client:
        await client.login(os.environ['DISCORD_BOT_TOKEN'])
        g=await client.fetch_guild(guild)
        channel=next((c for c in await g.fetch_channels() if c.id==int(config['channel_id'])),None)
        if channel is None or channel.name!='아트-납품':
            raise Denied('ART_CHANNEL_MISMATCH')
        member=await g.fetch_member(client.user.id)
        perms=channel.permissions_for(member)
        permissions={k:getattr(perms,k) for k in ('view_channel','read_message_history','send_messages','attach_files')}
        app=await client.application_info()
        enabled=bool(app.flags.gateway_message_content or app.flags.gateway_message_content_limited)
        if not enabled or not all(permissions.values()):
            raise Denied('ART_CHANNEL_PERMISSION_OR_INTENT_MISSING')
        message=await channel.fetch_message(int(parts[1]))
        normal=(message.author.id in owners and not message.author.bot and message.author.id!=client.user.id
                and message.channel.id==owner.channel_id and message.guild.id==guild)
        if not normal:
            raise Denied('NOT_OWNER_NORMAL_MESSAGE')
        # 해당 대표 작성자로 다시 인증합니다. 첫 owner라고 추측하지 않습니다.
        from .core import Identity
        identity=Identity(message.author.id,guild,channel.id)
        attachment=next((a for a in message.attachments if a.id==int(parts[2])),None)
        if attachment is None or not attachment.filename.lower().endswith('.png') or not 0<attachment.size<=MAX_ZIP:
            raise Denied('NORMAL_PNG_ATTACHMENT_MISSING')
        with deliveries.store.tx() as db:
            original_report=db.execute('SELECT * FROM art_delivery_outbox WHERE event_key=?',(expected['event_key'],)).fetchone()
        if original_report is None or original_report['status']!='SENT':
            raise Denied('ORIGINAL_AUTO_REPORT_NOT_SENT')
        posted=await channel.fetch_message(int(original_report['message_id']))
        original_report_verified=(posted.author.id==client.user.id and expected['sha256'] in posted.content
                                  and expected['task_id'] in posted.content)
        data=await attachment.read()
        if len(data)!=attachment.size:
            raise Denied('ATTACHMENT_SIZE_MISMATCH')
        result=await asyncio.to_thread(mark_test_only,deliveries,identity,expected,data,
            {'owner_verified':normal,'normal_attachment':normal,'original_report_verified':original_report_verified})
        await publish_outbox(deliveries,channel)
        with deliveries.store.tx() as db:
            receipt=db.execute('SELECT status,message_id FROM art_delivery_outbox WHERE event_key=?',
                               (result['report_event_key'],)).fetchone()
        result.update(channel_id=str(channel.id),message_content_intent=enabled,permissions=permissions,
                      original_auto_report_message_id=original_report['message_id'],
                      verification_report_status=receipt['status'],verification_report_message_id=receipt['message_id'],
                      original_uploaded_at=message.created_at.timestamp(),observed_at=time.time(),
                      new_upload_requested=False,new_manual_task_created=False)
        if receipt['status']!='SENT':
            raise Denied('TEST_REPORT_NOT_VERIFIED_NO_AUTO_RETRY')
        final_post=await channel.fetch_message(int(receipt['message_id']))
        result['final_report_on_discord_verified']=(final_post.author.id==client.user.id and 'TEST_ONLY' in final_post.content
                                                    and result['sha256'] in final_post.content)
        if not result['final_report_on_discord_verified']:
            raise Denied('TEST_REPORT_READBACK_FAILED')
    (ROOT/'owner-receipt-verified.json').write_text(json.dumps(result,ensure_ascii=False,indent=2),encoding='utf8')
    print(json.dumps(result,ensure_ascii=True))


if __name__=='__main__':
    try:
        asyncio.run(verify())
    except Exception:
        print('OWNER_RECEIPT_NOT_VERIFIED: 비밀 원문을 출력하지 않습니다. 자동 재시도 없음.')
        raise SystemExit(2)
