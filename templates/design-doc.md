# Design Doc — {{title}}

**ID**: `{{id}}`
**Kapı**: design
**Designer**: game-designer

## High concept (2 cümle)
{{oyunu iki cümlede anlat}}

## Core loop
{{4-6 adımlı liste ya da sözel diyagram}}
1. {{adım}}
2. {{adım}}

## Oyuncu girdisi
- Kontrol: {{1-tap / swipe / tilt / 2-parmak}}
- Neden: {{seçim gerekçesi}}

## Tur yapısı
- Süre: {{60–180 s aralığında}}
- Başlama: {{hemen / kısa hazırlık / tutorial-first-run}}
- Bitiş: {{skor / ölüm / hedef}}
- Restart: {{1-tap / 2-tap}}

## Progression
- **Unlock ekseni**: {{karakter / tema / skill / level}}
- **Currency**: {{soft: nasıl kazanılır, hard: nasıl alınır}}
- **Pacing**:
  - D1: {{oyuncu nerede olmalı}}
  - D3: {{}}
  - D7: {{}}

## Difficulty modeli
- Tip: {{DDA / sabit eğri / karışık}}
- Formül veya örnek: {{}}
- Örnek sayılar: Level 1 / 10 / 50

## Monetization noktaları (iskelet)
| Nokta | Tip | Tetikleyici | Değer teklifi |
|---|---|---|---|
| {{nokta}} | RA / IA / IAP | {{ne zaman}} | {{ne veriyor}} |

Detay: monetization agent kapanışında `games/<id>/monetization.md`.

## Retention hook
- **D1**: {{}}
- **D3**: {{}}
- **D7**: {{}}

## Ses / görsel yön
- Palet: {{}}
- Müzik tonu: {{}}
- SFX teması: {{}}

## Teknik uyarılar (MAUI)
- Render: {{SkiaSharp / GraphicsView / XAML}}
- SQLite tablolar: {{}}
- Platform API: {{vibration / haptic / bildirim}}

## Açık sorular (maks 2)
1. {{soru}}
   - **Önerim**: {{karar}}

## Başarı kriteri (build + QA için)
- [ ] Cold start ≤ 2 s.
- [ ] 60 FPS idle.
- [ ] Core loop 5 sn içinde anlaşılır (yeni oyuncu testi).
- [ ] 1 session'da ≥ 3 tur tamamlanabilir (fazla sürükleyici değilse sorun).
