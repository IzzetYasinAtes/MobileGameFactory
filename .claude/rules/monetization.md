# Monetization Kuralları

## Temel ilke
**Oyuncu izletir, zorlanmaz.** Rewarded > Interstitial > IAP sert satış. Oyuncu şikayet ederse LTV sıfırlanır.

## Reklam tipleri ve limitleri
### Rewarded ad
- Her zaman **opt-in** (kullanıcı butona basar).
- Değer teklifi net: "2x coin", "revive", "skip 1 dk cooldown", "unlock skin".
- Cooldown: 30 saniye (aynı placement için); farklı placement sınırsız.
- Offline durumda: rewarded gösterilemiyorsa alternatif yol (IAP veya bekleme) sun.

### Interstitial ad
- **Maks 1 / 4 dakika** aktif oyun süresi.
- İlk **3 run** içinde hiçbir zaman.
- Session start'ta asla.
- Core loop ortasında asla (round bittikten sonra dönüş sayfasında, loading değil).
- Skip butonu ≥ 5. saniyede.

### Banner ad
- Varsayılan **kapalı**. Eklenecekse sadece menu/shop ekranlarında; gameplay ekranında yasak.
- Footer sabit, popup değil.

### App-open ad
- Varsayılan **kapalı**. Çok agresif, retention düşürür.

## IAP
### Kabul edilen kategoriler
- **Remove ads** (tek sefer, 1.99–3.99 USD).
- **Cosmetic** (skin, tema, efekt).
- **Permanent QoL** (double XP, 2x coin, extra slot).
- **Soft currency paketi** (dengeli oran; kazanma hızı korunur).
- **Starter pack** (launch'tan sonra 24 saat, tek sefer).

### Yasak
- Pay-to-win (leaderboard'da avantaj, rekabetçi güç farkı).
- Loot box rastgeleliği (yasal kısıtlamalar + topluluk tepkisi).
- Abonelik modeli (küçük oyun için uygun değil).
- Dark pattern: "sadece bugün!", sahte geri sayım.

## Fiyat mimarisi
- Tier'lar: 0.99 / 2.99 / 4.99 / 9.99 (yerel store currency'e göre).
- En yüksek tier asla 19.99 üstü (küçük oyun için).
- Bölgesel fiyatlama store otomatiği; manuel override yok.

## Ölçüm
- Her reklam placement'i için: impression, ad-opt-in-rate, eCPM.
- Her IAP SKU için: conversion, ARPPU.
- `IAnalytics` event'leri: `ad_request`, `ad_impression`, `ad_reward_granted`, `iap_initiated`, `iap_completed`, `iap_failed`.

## Uyumluluk
- ATT (iOS 14.5+): IDFA için izin dialogu; reddedince rewarded ad çalışmaya devam etsin (sadece hedefleme değişir).
- GDPR/CCPA: consent flow — ilk açılışta sade, "kabul et / sadece gerekli".
- Data safety / Privacy nutrition: sadece toplanan veri (ad SDK kaynaklı) işaretlenir.

## Denge testi
- Sample run: ortalama oyuncu 1 session'da en fazla 2 interstitial görsün.
- Rewarded ad kullanıcı tarafından en az 1 kez ARTURDAĞI için değer teklifi yeterli mi? "Kim bunu izler?" sorusunu geçebiliyor mu?
