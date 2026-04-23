---
name: qa-pass
description: QA Tester agent'ın qa kapısında izlediği playbook. Test matrisi doldurur, edge case'leri kovalar, GO/NO-GO kararı verir.
---

# Skill: qa-pass

## Ön koşul
- `artifact_list(gameId)` → design.md + `games/<id>/src/` hazır.
- `.claude/rules/testing.md` okundu.
- Android Release build eldede (APK/AAB).

## Adımlar

### 1. Smoke (ilk 10 dk)
`.claude/rules/testing.md` smoke checklist'i. Tek fail → stop, dev'e döndür.

### 2. Functional test
Her UI akışını aç:
- Menu → Play → Core loop → Game over → Menu.
- Shop / IAP (sandbox).
- Settings (ses, titreşim, dil).
- Rewarded ad akışı (test unit).
- Interstitial gösterim koşulu (uygun mu?).

### 3. Edge case panel
testing.md'deki edge case kütüphanesini koş:
- Uçak modu.
- Dolu disk (disk doldurma: `fallocate` veya büyük dummy dosya).
- Background → foreground (5 sn, 2 dk, 30 dk).
- Ekran dönüşü.
- Çağrı/SMS interrupt.
- Düşük memory (`adb shell am send-trim-memory`).
- Cihaz tarih manipülasyonu.

### 4. Performance ölçüm
En az 1 low-end cihazda:
- Cold start (stopwatch, 3 ölçüm ortalaması) ≤ 2 s.
- 10 dk oyun sonu memory snapshot ≤ 250 MB.
- Frame drop sayısı (Profiler) kabul edilebilir mi?

### 5. Bug formatı
Her bug `games/<id>/qa.md` içinde testing.md formatına uygun. P0/P1/P2 öncelik.

### 6. GO/NO-GO kararı
`.claude/rules/testing.md` kriterleri:
- P0 = NO-GO.
- 2+ P1 = NO-GO.
- Perf hedefi aşıldı = NO-GO.

### 7. Kapı kapanış
```
artifact_register(gameId, gate="qa", kind="qa", path="games/<id>/qa.md")
message_send(to="maui-developer", type="handoff", subject="bug listesi", body="<P0/P1 sayısı>")   # sadece bug varsa
message_send(to="project-manager", type="handoff", subject="qa: GO|NO-GO", body="<sayılar>")
log_append(agent="qa-tester", gate="qa", gameId=<id>, decision="GO|NO-GO", why="<rakamsal kanıt>")
```

## Yasaklar
- Reproduction adımı yazılmamış bug.
- Emulator-only test (en az 1 fiziksel cihaz şart low-end için).
- Pass demek için tek deneme (3 ölçüm minimum).

## Done
qa.md kayıtlı + varsa dev handoff + PM'e GO/NO-GO.
