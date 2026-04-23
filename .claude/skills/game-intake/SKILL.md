---
name: game-intake
description: Sahibin 1-2 cümlelik oyun fikrini yapısal bir brief'e çevirir ve intake kapısını açar. PM agent tarafından çağrılır. Otomatik slug üretir, game_create çağırır, brief.md dosyasını templates'ten doldurur.
---

# Skill: game-intake

## Ne zaman kullanılır
Kullanıcı `/new-game "<fikir>"` yazdığında veya PM yeni bir brief teslim aldığında.

## Adımlar

### 1. Slug üret
- Fikrin ana iki kelimesi + kısa niteleyici.
- Tüm küçük harf, tire ile ayır, ≤30 karakter.
- Örnek: "Suçlu kuş, 15s turlar" → `neon-bird-15s`.
- Benzersizlik: `game_list()` ile çakışma kontrolü; çakışırsa sayı ekle.

### 2. game_create çağır
```
game_create(id=<slug>, title=<okunabilir başlık>, brief=<1-2 cümle ham fikir>)
```

### 3. brief.md yaz
`templates/game-brief.md` şablonunu kopyala → `games/<id>/brief.md`:
- Başlık
- Ham fikir (değişmeden)
- Yorumlanmış niyet (PM'in 2-3 cümle özetlemesi)
- Hedef oyuncu (tahmin; market-analyst doğrulayacak)
- Beklenen core loop (tek cümle)
- Referans görseller (yapıştırılmışsa)
- Açık sorular (varsa; PM kendi cevaplar)

### 4. artifact_register
```
artifact_register(gameId, gate="intake", kind="brief", path="games/<id>/brief.md")
```

### 5. Research kapısına geç
```
gate_advance(gameId, "research")
log_append(agent="project-manager", gate="intake", decision="brief kabul edildi → research", why="<ana niyet tek satır>")
message_send(to="market-analyst", type="handoff", subject="research başlat", body="<3 satır özet + brief path>")
```

### 6. Sahibe özet
Tek paragraf:
- Oyun adı (title).
- Niyet (1 cümle).
- Sıradaki adım: research (market analizi).
- Tahmini süre: kısa.

## Done
- game_create ✓
- brief.md dosya ✓
- artifact_register ✓
- gate_advance ✓
- log_append ✓
- market-analyst inbox'ına handoff ✓
- Sahibe tek paragraf ✓

## Hata yönetimi
- Slug çakışırsa: `-2`, `-3` ekle, tekrar dene.
- `game_create` 400 dönerse: title/brief sanitize et, tekrar dene (1 kere).
- Başarısızlık ısrarı: `message_send(to="mcp-infrastructure", type="escalation", subject="game_create fail", body=<hata>)`.
