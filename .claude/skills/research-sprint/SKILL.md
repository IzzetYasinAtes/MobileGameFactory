---
name: research-sprint
description: Market Analyst agent'ın research kapısında izlediği adım-adım playbook. Tek sprint içinde 3 rakip teardown + fit önerisi üretir.
---

# Skill: research-sprint

## Ön koşul
- `game_get(gameId)` ile brief okundu.
- `artifact_list(gameId)` ile brief.md path'i alındı.

## Adımlar

### 1. Rakip belirleme
WebSearch sorguları:
- "<genre> mobile game 2025 top charts"
- "<core-loop-kelimeler> mobile game popular"
- "<tema> mobile game successful indie"

3 rakip seç. Kriterler:
- Benzer core loop veya genre.
- En az 100K indirme veya üst %10 sıralama.
- Son 12 ayda aktif güncelleme.

### 2. Teardown (her rakip için)
WebFetch ile:
- Store sayfası (description, rating, indirme kaba aralığı).
- 2-3 kullanıcı review'ı (pozitif + negatif).
- Mümkünse oynanış videosu kaynağı (yorum linki).

Teardown formatı (≤120 kelime):
- **Ad** + platform link.
- **Core loop** (1 cümle).
- **Güçlü**: 2 madde.
- **Zayıf**: 2 madde.
- **Monetization**: reklam tipi + IAP yaklaşımı.

### 3. Pattern çıkarımı
- **Başarı kalıpları** (3-5 madde): ortak retention hook'u, reklam yerleşimi, onboarding uzunluğu.
- **Başarısızlık kalıpları** (3 madde): yorumlarda tekrar eden şikayetler.

### 4. Fit önerisi
Bizim fikrimiz bu pazarın neresinde? 1 paragraf:
- Ana diferansiyasyon (1 cümle).
- Niçin çalışır (market kanıtı).
- Hangi risk yönetilmeli.

### 5. Diferansiyasyon önerileri
3 somut öneri:
- Her biri: ne, nasıl uygulanır, hangi rakibe karşı avantaj.

### 6. market.md yaz ve kaydet
Path: `docs/games/<id>/market.md`. Uzunluk 400–600 kelime.
```
artifact_register(gameId, gate="research", kind="market", path="docs/games/<id>/market.md")
```

### 7. Kapı kapanış
```
message_send(to="project-manager", type="handoff", subject="market.md hazır", body="<3 satır: diferansiyasyon + risk>")
log_append(agent="market-analyst", gate="research", gameId=<id>, decision="<fit tek satır>", why="<kanıt>")
```

## Yasaklar
- Kaynaksız iddia.
- 3'ten fazla rakip (token bütçesi).
- Kendinden çıkarım yapmayan analiz ("oyun güzel" değil, "retention D7 şu rakamda çünkü...").

## Done
market.md kayıtlı + PM handoff + 1 log.
