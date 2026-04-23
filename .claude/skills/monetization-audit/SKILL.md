---
name: monetization-audit
description: Monetization agent'ın iş akışı. Build çıktısındaki para kazanma noktalarını denetler, frekans/denge problemlerini bulur, oyuncuyu rahatsız etmeden gelir akışını maksimize eder.
---

# Skill: monetization-audit

## Ön koşul
- `artifact_list(gameId)` → design.md + `games/<id>/src/` path'leri.
- `.claude/rules/monetization.md` okundu.

## Adımlar

### 1. Envanter çıkar
design.md'deki her monetization yerleşimini listele:
- Tip (RA/IA/Banner/IAP).
- Nerede (UI ekranı / gameplay anı).
- Frekans/tetikleyici.
- Değer teklifi.

### 2. Kurallara karşı denetim
Her madde için kontrol:
- Interstitial: 4 dk cooldown + ilk 3 run mute?
- Rewarded: opt-in + cooldown 30 s?
- Banner: varsa sadece non-gameplay ekran?
- IAP: pay-to-win değil?

İhlaller → değişiklik önerisi.

### 3. Denge senaryosu
Ortalama oyuncu 2-3 session:
- Toplam interstitial sayısı ≤ 2? ✓
- Rewarded fırsatı ≥ 3 (opt-in)? ✓
- IAP gösterimi: organic (store, shop sayfası) mi yoksa pop-up mı?

### 4. eCPM/conversion tahmini
Her placement için kaba tahmin:
- Rewarded eCPM: $5–15 (Android orta).
- Interstitial eCPM: $3–10.
- IAP conversion: %1–3 orta seviye.

Bu sadece tahmin; canlı ölçüm ship sonrası.

### 5. Developer notu (gerekirse)
Entegrasyon değişikliği gerekiyorsa:
```
message_send(to="maui-developer", type="handoff", subject="monetization entegrasyon", body="<3 madde: ne/nerede/neden>")
```

### 6. monetization.md yaz
Path: `games/<id>/monetization.md`. Uzunluk 300–500 kelime.
Bölümler: Reklam envanteri tablosu, IAP kataloğu, oyuncu-dostu anayasa, risk uyarıları.

### 7. Kapı kapanış
```
artifact_register(gameId, gate=<mevcut>, kind="monetization", path=...)
message_send(to="project-manager", type="handoff", subject="monetization plan", body="<özet + 1 risk>")
log_append(agent="monetization", gate=<>, gameId=<id>, decision="<plan tek satır>", why="<ana prensip>")
```

## Yasaklar
- Tahmin rakamlarını "kesin" sunmak.
- Kurallara aykırı yerleşimi "sahibi onayladı" varsayımıyla geçirmek.
- Ek dosya (sadece monetization.md).

## Done
monetization.md + opsiyonel dev notu + PM handoff.
