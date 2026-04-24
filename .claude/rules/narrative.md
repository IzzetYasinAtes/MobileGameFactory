# Narrative Design Kuralları

## Sert kurallar
1. **Dialog line ≤ 80 karakter** (UI overflow önler)
2. **Karakter voice kilitli** — brief'ten sonra değişmez
3. **No exposition dump** — tek monolog max 3 satır
4. **Age-appropriate** — PEGI 7 default hedef
5. **Localization-friendly** — regionalism minimize

## Ne zaman narrative skip
- Hyper-casual (tap-only, reflex)
- Pure puzzle (sudoku, 2048)
- Board / card (abstract)

Bu durumda gate skip, `log_append(decision="narrative skipped", why="genre=X")`.

## Dialog style guide

### Kısa
- UI'de tek line max **80 karakter**
- 3 satır monolog = 240 char TAVAN
- Skip button her zaman açık

### Karakter voice matrix
Her karakter için:
- **Tone**: cheerful / wise / sassy / shy / stoic
- **Vocabulary level**: simple / moderate / sophisticated
- **Catchphrase** (opsiyonel): 1 kısa ifade (kullanımı selective)
- **Motivation**: ne istiyor, ne engel oluyor

Örnek:
```
Karakter: Kaşif
Tone: cheerful + brave
Vocabulary: simple (child-friendly)
Catchphrase: "Haydi macera!"
Motivation: kayıp medeniyeti keşfetmek
```

### Show don't tell
- ❌ "Kaşif mutludur çünkü quest'i bitirdi"
- ✅ Kaşif: "Sandığı açalım, hadi!" + happy animation

### Age-appropriate (PEGI 7)
- Şiddet: yok (tokat, yumruk, silah yok)
- Korku: çok düşük (atmosferik gerginlik OK, jump scare yok)
- Cinsellik: yok
- Küfür: yok
- Alkol/uyuşturucu: yok
- Ayrımcılık: yok

## Story structure (3-5 beat)
1. **Opening hook** — stage 1-5 giriş (inciting incident)
2. **Rising action** — world 2-3 (komplikasyon)
3. **Midpoint twist** — world 3-4 boss (revelation)
4. **Climax** — son world boss (max tension)
5. **Resolution** — outro (reward + world state)

Minimum: 3 beat (hyper-casual). Max 5 (casual adventure).

## Quest metin bankası

### Intro (her stage)
- ≤80 karakter
- Objective + hint (opsiyonel) + character voice
- Örnek: "İki taş birleştir, ilk sandığı aç! 🗝️"

### Outro (her stage)
- ≤80 karakter
- Reward mention + reaction + tease
- Örnek: "Harika! Yeni macera bekler."

### Hint (2 per stage)
- Hint 1: light tip
- Hint 2: strong (ad-gated opsiyonel)
- Örnek: "İpucu: Yan yana olanları birleştir."

## Pet dialog bank

### Idle barks (random, rare)
- ≤60 char, 4-6 farklı line
- Örnek: "Merhaba!", "Ne yapıyoruz?", "Sandık nerede?"

### Celebration (quest complete)
- ≤60 char, 3-4 line
- Örnek: "Harika!", "Muhteşem!", "Bir tane daha!"

### Failure comfort (stage fail)
- ≤60 char, 3-4 line
- Örnek: "Denemek güzel!", "Tekrar deneyelim.", "Yakındın!"

## Boss taunt + defeat
Her boss için:
- **2 taunt** (intro + mid-fight)
- **1 defeat line**
- Örnek boss Volkan Devi:
  - Taunt 1: "Ateşim sönmez!"
  - Taunt 2: "Daha ne bekliyorsun?"
  - Defeat: "Ahh... yetenekliymişsin..."

## Outro / credits
Oyun sonu (son world completed):
- Son dialog (3-5 line)
- Credit scroll (kısa)
- "Tekrar başla?" veya "Yeni macera?" teaser v1.1 için

## Localization notes

### Culturally specific
Bu öğeleri işaretle (localization agent'ı tarafından adapt edilir):
- Ramazan teması TR için
- Sakura teması JP için
- Diwali IN
- Hanukkah ISR/LATAM Yahudi toplulukları
- Thanksgiving US/CA

### Regionalism minimize
- Slang (argo) — global çeviri zor
- Idiom (deyim) — direkt çeviri saçma çıkar
- Political/religious — yasak
- Alcohol references — yasak (child-targeted)

### Gender
- TR: gender-neutral (default)
- AR: male/female inflection
- LATAM ES: friendly (tú vs usted — tú)
- DE: informal (du vs sie — du)

## Narrative QA
- [ ] Tüm dialog ≤80 char
- [ ] Karakter voice tutarlı (tone drift yok)
- [ ] PEGI 7 uyumlu
- [ ] Hint quest-level'a uygun
- [ ] Grammar TR + EN proofread
- [ ] Localization notes işaretli
- [ ] Skip button her cutscene'de

## Yasaklar
- Uzun cutscene (≥5 saniye, skip edilemez)
- Text-wall popup (>150 char)
- Political referans
- Religious mock
- Gore / violence / sexual content
- Grammar hataları
- Gender stereotype
