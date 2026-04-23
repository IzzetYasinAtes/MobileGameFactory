# Design Doc — Mini Kaşifler: Kayıp Adanın Sırrı

**ID**: `island-merge`
**Kapı**: design
**Designer**: game-designer

---

## High concept (2 cümle)

Gizemli bir adaya düşen kaşif çocuk, fog-of-war'la örtülü 5 bölgeyi merge mekaniğiyle açar; her birleştirme hareketi hem kaynak üretir hem haritanın bir parçasını keşfe açar. Kısa seanslar (2-5 dk), tatmin edici "ilerledim" hissi ve sömürücü olmayan rewarded reklamla casual-cozy rafında farklılaşır.

---

## Core loop

1. **Üret** — Board'daki üretici nesne (ağaç, taş yatağı, kristal kaynağı) hafif enerji harcarek kaynak çıkarır.
2. **Merge** — Aynı tier'dan 2 aynı item'ı drag-and-drop ile birleştirir; bir üst tier item oluşur.
3. **Quest tamamla** — Aktif görev belirli item tier'larını ister; teslim ederek XP + soft currency (Altın Sikke) kazan.
4. **Fog aç** — Yeterli XP birikmesi veya belirli merge zinciri tamamlanması fog karesini kaldırır; yeni üretici ya da hazine açılır.
5. **Bölge ilerle** — 20 levelde bir bölge (5 bölge toplamda) tamamlanır; yeni ortam teması + özel item zinciri açılır.

**Seans sonu tetikleyicisi**: Aktif görev tamamlandı veya enerji tükendi (L16+). Restart: tek tap "Devam Et".

---

## Oyuncu girdisi

- **Kontrol**: Tek parmak drag-and-drop (tap-to-select + tap-to-drop alternatif toggle, erişilebilirlik için).
- **Neden**: Merge türünün standart girdisi; öğrenme eğrisi sıfır. Fog kareleri tapa açılır (ayrı gesture yok). Yeni gesture eklemek ilk-run karmaşıklığını artırır — hayır.

---

## Tur yapısı

- **Süre**: 90-180 s (hedef: 1 görev = 1 tur; L1-15 arası ~90 s, L30+ ~150 s).
- **Başlama**: Anında — board açık, ilk item zaten üretilmiş, "merge et" ok göstergesi L1'de 3 sn gösterilir.
- **Bitiş**: Görev teslim ekranı (skor + XP + fog açılma animasyonu).
- **Restart**: 1-tap. Board durumu SQLite'a her merge sonrası otomatik kaydedilir.

---

## Board kararı: Tek ekran, 7×9 grid

**Karar**: Travel Town modeli — tek ekran, kaydırma yok, 7×9 = 63 hücre.

**Gerekçe**: Market analizi, "kaydırma gerektirmeyen board" retention metriklerini doğrudan iyileştiriyor diyor (Travel Town kanıtı). Ayrı "dünya haritası + merge board" split-screen kompleksliği cold-start süresi ve ilk-run anlaşılırlığını bozar. Fog kareler board içindeki hücrelerde görünür — kaydırma gerektirmeden hem merge hem keşif tek ekranda olur.

---

## Merge zinciri (her kaynak için 5 tier)

| Tier | Taş | Odun | Kristal |
|------|-----|------|---------|
| 1 | Taş Kırığı | Dal | Ham Kristal |
| 2 | Taş Yığını | Tomruk | Kristal Parçası |
| 3 | Sütun | Kiriş | Kristal Küp |
| 4 | Duvar | Ahşap Platform | Kristal Heykel |
| 5 | Anıt Taşı | İskele | Mistik Kristal |

Her bölgede o ortama özgü zincir eklenir (örn. B2 Sahil: Kabuk → Mercan → Deniz Feneri zinciri). Yeni zincirler kilitsiz gösterilir — "???" hayır, "gri + kilit ikonu" evet. Belirsizlik churn yaratır (market analizi, EverMerge dersi).

---

## Progression sistemi

- **Yapı**: 5 bölge × 20 level = 100 level (v1.0).
- **Bölgeler**: B1 Tropik Orman → B2 Sahil/Mağara → B3 Antik Tapınak → B4 Volkan (Ateş Dağı) → B5 Buz Diyarı.
- **Currency**:
  - *Altın Sikke* (soft): merge zinciri tamamlama + görev teslim. Board genişletme ve üretici yükseltme için harcanır.
  - *Keşif Mücevheri* (hard): günlük görev + D7 event + IAP. Enerji doldurma, kozmetik, pet kostümü için.
- **Pet seçimi (L1)**: Kaşif/Lila, Maymun Momo veya Papağan Rüzgar seçer (kozmetik; aynı mekanik).
- **Pacing**:
  - D1: B1 level 3-5; fog'un ilk 8 karesi açık; merge zincirine alışmış.
  - D3: B1 tamamlanmış veya L12-15; ilk rewarded ad ile tanışmış.
  - D7: B2 ortasında (L25-30); streak bonusu aktif; haftalık event ilk kez tetiklenmiş.

---

## Difficulty modeli

**Tip**: Adaptif eğri — hedef merge sayısı ve süre toleransı player hızına göre ölçeklenir.

**Formül**:
```
MergeHedefi(L) = 3 + floor(L / 5)          -- bazal merge adedi
SüreToleransı(L) = 180 - (L * 0.8) s       -- minimum 90 s'de sabitlenir (L>=112 hypothetically)
EnerjiMaliyeti(merge) = 1 + floor(tier / 2) -- tier 1-2: 1 enerji, tier 3-4: 2, tier 5: 3
```

**Örnek sayılar**:
| Level | Merge hedefi | Süre toleransı | Enerji/eylem |
|-------|-------------|----------------|-------------|
| L1 | 3 merge | 180 s | 1 |
| L10 | 5 merge | 172 s | 1-2 |
| L50 | 13 merge | 140 s | 1-3 |

**L1-15 enerji istisnası**: CoolDown = 0. Enerji tükenirse anında dolar. Oyuncu enerji bariyer hissetmez; bağlanma önce gelir.

---

## Monetization noktaları (iskelet)

| Nokta | Tip | Tetikleyici | Değer teklifi |
|-------|-----|-------------|---------------|
| +50 Enerji | Rewarded | L16+ enerji tükenince, opt-in buton | "Devam et, 1 reklam izle" — anında doldurma |
| 2x Loot | Rewarded | Görev teslim ekranı, her zaman opt-in | Ödülü ikiye katla (Altın Sikke + XP) |
| Üretici Hızlandır | Rewarded | Üretici sayacı görünürken tap | "5 dk bekleme → şimdi al" |
| Seviye Dönüş | Interstitial | Her 3 level tamamlanınca DÖNÜŞ ekranı | Pasif, skip ≥5 s; ilk 3 run'da hiç |
| Enerji 100 | IAP 0.99 | Shop veya enerji tükenmesi sonrası | Anlık enerji paketi |
| Enerji 500 | IAP 2.99 | Shop | Büyük paket, %150 değer farkı |
| Starter Pack | IAP 4.99 | L5 sonrası, 24 s pencere, tek sefer | 500 Enerji + 200 Mücevher + özel pet kostümü |
| Remove Ads | IAP 3.99 | Post-interstitial veya shop | Tüm interstitial kaldırır; rewarded opt-in kalır |

---

## Retention hook

- **D1**: Günlük görev sistemi (3 küçük görev: "5 merge yap", "1 fog aç", "1 görev teslim et") → tamamlayınca +20 Mücevher. Görev sıfırlanması sabah 06:00 TR saati.
- **D3**: Streak bonusu — 3 gün üst üste oynayan oyuncuya pet kostümü (Momo: Kaşif şapkası / Rüzgar: Denizci bandanası) + 2x enerji doldurma bonusu o gün.
- **D7**: Haftalık LiveOps event — TR kültürel içerikli (ör. Nevruz: "Bahar Kristalleri" zinciri; 23 Nisan: özel bayrak item). Event board'a özel 5 hücrelik ek alan açar, 7 günlük ödül takvimi.

---

## Ses / görsel yön

- **Palet**: Sıcak tropik — mercan, turkuaz, amber, yeşil yaprak; her bölge kendi accent rengi (B4 Volkan: kırmızı-turuncu, B5 Buz: mor-beyaz).
- **Müzik tonu**: Lo-fi ambient + steel drum melodisi; bölge geçişinde enstrüman değişir (B3 Tapınak: ud/ney). BPM ~75, rahatlatıcı ama sıkıcı değil.
- **SFX teması**: Merge = yumuşak "pop" + kristal "chime"; fog açılma = "whoosh" + merak uyandıran arpej; görev teslim = tatmin edici "ding" combo.

---

## Teknik uyarılar (MAUI)

- **Render**: `GraphicsView` + `SkiaSharp`. 63 hücreli board sürekli invalidate gerektirmez; sadece değişen hücreler dirty-flag ile yeniden çizilir. Hedef 60 FPS — `SKCanvas.DrawBitmap` atlas sprite ile (tek PNG atlas, ayrı dosya yükleme yok).
- **SQLite şema**:
  - `Players` (id, petChoice, totalXP, softCurrency, hardCurrency, streakDays, lastLoginDate)
  - `BoardState` (id, playerId, levelId, hucreIndex, itemId, itemTier)
  - `Items` (id, chainType, tier, name, spriteKey)
  - `Quests` (id, levelId, targetItemId, targetTier, quantity, rewardXP, rewardCoin)
  - `Progress` (id, playerId, currentLevel, currentZone, fogRevealedMask)
  - `Inventory` (id, playerId, itemId, tier, quantity)
- **Platform API**: Hafif titreşim (haptic) merge anında (`HapticFeedback.Perform(HapticFeedbackType.Click)`); Android bildirim kanalı üretici hazır olunca (L10+).
- **Enerji sayacı**: `Preferences` değil SQLite `Players.energy + lastEnergyRefill`; uygulama arka planda iken geçen süre hesaplanır, cheat-tolerant.

---

## Açık sorular (maks 2)

1. **Fog açma birimi**: Fog, her merge'de mi açılmalı (küçük adımlar) yoksa quest tamamlanınca mı (büyük tatmin)?
   - **Önerim**: Her 3 merge'de 1 hücre açılır (sürekli mikro-tatmin), quest tamamlanınca ek 3 hücre patlaması (büyük tatmin). İkisi birlikte — micro + macro loop.

2. **B1-B2 arası bölge geçiş animasyonu**: Uzun cutscene mi (15 s) yoksa hızlı reveal mi (3 s)?
   - **Önerim**: 3 saniyelik animasyon — fog sisi dağılır, yeni ortam açılır, karakter sevinç gösterisi. 15 s cutscene session süresini bozar ve skip oranı yüksek olur; maliyet/etki dengesizdir.

---

## Başarı kriteri (build + QA için)

- [ ] Cold start ≤ 2 s.
- [ ] 60 FPS idle ve merge animasyonu sırasında.
- [ ] Core loop 5 sn içinde anlaşılır (yeni oyuncu testi, L1 ilk merge).
- [ ] 1 session'da ≥ 2 görev tamamlanabilir.
- [ ] L1-15 arası hiçbir enerji bariyer ekranı gösterilmez.
- [ ] Rewarded ad opt-in oranı ≥ %30 (değer teklifi yeterli mi ölçüsü).
