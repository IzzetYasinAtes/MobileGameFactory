---
name: polish-or-kill-audit
description: Build + Juice + Animation sonrası PM'in yürüttüğü kritik gate. 60 saniye oyun + 5 soru → GO/KILL karar. Kaynak israfını önler (Supercell/Voodoo disiplini).
---

# Skill: polish-or-kill-audit

## Ne zaman
Build ✓ + Juice ✓ + Animation ✓ tamamlandığında, QA'dan **ÖNCE**. PM ve Game Designer ortak yürütür.

## Amaç
**"Oyun eğlenceli mi?"** sorusunun net cevabı. Kod green + test green yeterli değil. Oyunun ürün-pazar fit'i gerçekten var mı kontrol edilir.

## Adımlar

### 1. Oyunu başlat
```bash
cd games/<id>/src/<id>
dotnet run -f net10.0-windows10.0.19041.0 -c Debug --no-build
```
Veya Android emulator / cihaz.

### 2. 60 saniye oyna
Hiçbir konsol çıktısı okumadan, sadece **oyuncu olarak**:
- Main Menu → Oyna
- İlk stage'i geç
- İlk merge/match yap
- İlk quest complete
- İlk biome transition (veya level up)
- Shop'a bir bak
- Settings'e bir bak

### 3. 5 sorunun net cevabı
1. **"Yine oynar mıyım?"** — kendine dürüst cevap
2. **"Juice Budget matrisi %100 implement mi?"** — her event min 3 kanal?
3. **"HUD + primary CTA doğru mu?"** — ≤5 öğe, 1 CTA, anlaşılır?
4. **"D1 retention muhtemelen ≥40% mi?"** — rakip teardown ile karşılaştır
5. **"Oyun polish'lenmiş mi yoksa prototype mi?"** — görsel + animasyon + ses entegre?

### 4. Karar matrisi

| Soru sayısı EVET | Karar |
|---|---|
| 5/5 | **GO** (full confidence) |
| 4/5 | **GO** (tek zayıf alan işaretle, QA'ya bildir) |
| 3/5 | **POLISH** (eksik 2 alan için hedef fix round, sonra tekrar audit) |
| ≤2/5 | **KILL** (ship'e layık değil, kaynak yakmaya değmez) |

### 5. KILL kararı çıktısı
- `game_meta_patch(id, {"status":"killed"})`
- `games/<id>/POSTMORTEM.md` yaz (aşağıda template)
- `log_append(decision="POLISH_OR_KILL=kill", why="<5 soru sonucu>")`
- Sahibe bildir: "Oyun X öldürüldü çünkü Y. Z öğrendik."

### 6. POLISH kararı çıktısı
- Eksik alanları listele (örn: "juice eksik tap feedback; UX FTUE 60s değil 120s")
- İlgili agent'lara fix dispatch (Game Feel Engineer, UX/UI Designer)
- Polish round sonunda **tekrar audit** — 5/5 veya 4/5 hedef

### 7. GO kararı çıktısı
- `gate_advance(id, "qa")`
- QA Tester dispatch
- `log_append(decision="POLISH_OR_KILL=go", why="<5 soru + confidence>")`

## POSTMORTEM template (kill durumunda)

```markdown
# POSTMORTEM — <game-id>

## Vizyon
<ne hedeflendi>

## Teslim edilen
<ne yapıldı>

## Kill kararı
**Tarih**: YYYY-MM-DD
**5 soru sonucu**: 2/5 EVET
**Net neden**: <juice eksik / fun değil / D1 beklenti yok>

## Öğrendiğimiz
- <insight 1>
- <insight 2>
- <insight 3>

## Sistem feedback
<agent / rule / skill güncellenmeli mi?>

## Aksiyon öğeleri
- [ ] <öğrendiğimizi v1 sistemine yansıt>
```

## Yasaklar
- Sevgi körlüğü ("ben yaptım, iyi olmuş") — dürüst ol
- "Birkaç fix sonra iyi olur" çoklu tekrarı (2 polish round max, sonra kill)
- Metric numbers olmadan karar ("güzel görünüyor" yeterli değil)
- Build/test green → otomatik GO sanma (v1 hatası)

## Done
- 60s oyun tam
- 5 soru karar
- GO/POLISH/KILL log
- Kill ise POSTMORTEM.md yazıldı
