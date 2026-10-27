# 실제 디렉터리 생성 기능은 없습니다. 미래 native readback 근거의 거부 조건만 합성 검증합니다.
function Test-V2StagingProof($Proof){
 $reasons=[Collections.Generic.List[string]]::new()
 $expected='C:\Program Files\LimitlessV2Staging20261027\R9-01f8a36c9bfe4241b24b7eaa6caaddc2'
 if($Proof.path -cne $expected){$reasons.Add('PATH')}
 if($Proof.created_new -ne $true -or $Proof.source_preserved -ne $true){$reasons.Add('FRESH_AND_PRESERVE')}
 if($Proof.parent_owner_verified -ne $true -or $Proof.parent_write_safe -ne $true -or $Proof.handle_ancestors_pinned -ne $true){$reasons.Add('PARENT')}
 if($Proof.creation_sddl -cne 'O:BAG:SYD:P(A;OICI;FA;;;SY)(A;OICI;FA;;;BA)'){ $reasons.Add('CREATION_DACL') }
 if($Proof.zip_sha -cne 'a4a652bdf73c9a1007bd1e761ae539d3459896b46d8a021fa1459012a1d15c41' -or $Proof.manifest_sha -cne '250bab2d46010841353a319fbcf34581784366ac3042e6bc0fdc97aadd17c09e'){ $reasons.Add('PINS') }
 if($Proof.payload_count -ne 766 -or $Proof.entry_count -ne 767 -or $Proof.exact_set -ne $true -or $Proof.all_payload_sha -ne $true){$reasons.Add('FILESET_SHA')}
 if($Proof.all_owners_ba_sy -ne $true -or $Proof.all_dacl_exact -ne $true -or $Proof.readback_verified -ne $true){$reasons.Add('OWNER_DACL')}
 if($Proof.reparse_count -ne 0 -or $Proof.hardlink_count_max -ne 1 -or $Proof.ads_count -ne 0 -or $Proof.unexpected_count -ne 0 -or $Proof.case_alias_count -ne 0){$reasons.Add('LINK_OR_EXTRA')}
 if($Proof.exclusive_source_bytes -ne $true -or $Proof.atomic_create_new -ne $true -or $Proof.install_revalidation_required -ne $true){$reasons.Add('TOCTOU')}
 @{status=if($reasons.Count -eq 0){'OFFLINE_PLAN_PASS_NOT_CREATED'}else{'HOLD'};reasons=@($reasons);native_verified=$false;created=$false}
}
