# Örnek Brief: "Neon Bird 15s"

Bu dosya sahibin PM'e nasıl brief vereceğinin somut örneğidir. `/new-game` komutunun argümanı bundan çok daha kısadır; bu dosya PM'in brief'i nasıl yapısalladığını gösterir.

## Sahibin orijinal girdisi
```
/new-game "Flappy tarzı bir kuş oyunu ama 15 saniyelik turlar, neon retro estetik, hızlı combo sistemi"
```

## PM'in ürettiği brief (docs/games/neon-bird-15s/brief.md)

```markdown
# Game Brief — Neon Bird 15s

**ID**: `neon-bird-15s`
**Tarih**: 2026-04-23
**Kapı**: intake

## Ham fikir (sahipten)
> Flappy tarzı bir kuş oyunu ama 15 saniyelik turlar, neon retro estetik, hızlı combo sistemi

## Yorumlanmış niyet
Tek parmak tap-kontrollü, 15 saniyelik kısa turlarda yüksek skor kovalama oyunu. Görsel dil: 80'ler neon retro (mor/mavi/pembe palet + grid zemin). Combo mekaniği skor kaybını riske atacak karar anlarını yaratacak.

## Hedef oyuncu (tahmin)
- Yaş aralığı: 16-40
- Oyun deneyimi: arcade / hyper-casual kesişimi
- Motivasyon: yüksek skor, kısa mola oyunu

## Beklenen core loop (tek cümle)
Tap ile kuşu zıplat → engellerin arasından geç → combo sayacını yüksek tut → 15 sn sonunda skor kaydet.

## Referans görseller / linkler
yok (sahibin tarif ettiği estetik: neon retro sentetik palet)

## Açık sorular (PM kendi cevaplar; maks 2)
1. Combo mekaniği nasıl tanımlanır?
   - **PM cevabı**: Ardışık 3+ engel hatasız geçişi = combo. Her combo skoru 1.5x çarpanla ekler. Hata combo'yu sıfırlar. Design kapısı somutlaştıracak.
2. 15 saniye sonunda ne olur?
   - **PM cevabı**: Run biter; skor kaydedilir; 2 saniyelik replay highlight + restart CTA. Design kapısı UI'ı çizer.

## Sıradaki kapı
**research** — market-analyst agent'a devredildi.
```

## Notlar
- Sahip PM'e **tek satırlık fikir** verir.
- PM brief'i yapısal formata çevirir, niyeti yeniden anlatır, açık soruları **kendi cevaplar**.
- Görsel referans varsa sahip yapıştırır; PM `brief.md`'ye embed eder.
- Brief kayıt sonrası PM hemen research kapısına geçer; sahibe tek paragraflık özet verir, onay beklemez.

## Sahibin beklenen yanıtı (PM'den özet sonrası)
- "Devam et." → PM iş akışını sürdürür.
- Yön değişikliği varsa: "Mavi yerine yeşil palet olsun" → PM `game_meta_patch` ile kayıt + ilgili agent'a güncelleme mesajı.
- İptal: "Bu oyunu bırak." → `game_meta_patch(id, '{"status":"cancelled"}')` + log. DB satırı silinmez; audit için kalır.
