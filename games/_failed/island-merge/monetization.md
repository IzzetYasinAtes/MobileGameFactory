# Monetization Planı — Mini Kaşifler: Kayıp Adanın Sırrı

**ID**: `island-merge`
**Kapı**: build
**Agent**: monetization
**Tarih**: 2026-04-23

---

## Reklam Envanteri

| Placement | Tip | Tetikleyici | Koşul | Frekans Tavanı | Değer Teklifi | Tahmini eCPM Katkısı |
|-----------|-----|-------------|-------|----------------|---------------|----------------------|
| Enerji Dolumu | Rewarded | L16+ enerji sıfırlandığında opt-in buton | L16+; enerji = 0 | 4 kez/session (30 s cooldown aynı placement) | +50 Enerji — anında devam | Yüksek (intent yüksek, %30+ opt-in hedefi) |
| 2x Loot | Rewarded | Görev teslim ekranı; her teslimde opt-in buton | L5+ | Teslim başına 1 kez | 2x Altın Sikke + 2x XP | Orta-yüksek (tatmin anına bağlı) |
| Üretici Hızlandır | Rewarded | Üretici sayacı görünürken tap (L10+) | Sayaç > 60 s kaldıysa | 2 kez/session | Üretici bekleme süresi anında tamamlanır | Orta (soft utility, opt-in kolaylaştırılmış) |
| Seviye Dönüş | Interstitial | Her 3. level tamamlanınca dönüş ekranı | L4+; ilk 3 run asla; session ≥ 4 dk aktif | Maks 1 / 4 dakika aktif süre | Pasif — skip ≥ 5. saniye | Düşük-orta (frekans kısıtlı; tolere edilir) |

**Banner**: KAPALI. Shop ekranında bile footer banner açılmayacak — marka kalitesi ve küçük ekran kullanılabilirliği öncelikli.

**App-open**: KAPALI. Retention düşürücü; merge türü ilk-açılış anının kesilmesine özellikle duyarlı.

---

## IAP Kataloğu

| SKU | Fiyat (USD) | Yerel TR (Otomatik) | Ne Sağlıyor | Stratejik Rol | Hedef Segment |
|-----|-------------|---------------------|-------------|---------------|---------------|
| `energy_100` | 0.99 | ~32 TL | 100 Enerji (anında) | Impulse; düşük bariyer, yüksek dönüşüm | Casual, ilk satın alma kapısı |
| `energy_500` | 2.99 | ~96 TL | 500 Enerji (anında) | Value; birim fiyat %55 daha iyi | Core kullanıcı, retention yüksek |
| `starter_pack` | 4.99 | ~160 TL | 500 Enerji + 200 Keşif Mücevheri + özel pet kostümü (Kaşif şapkası / Denizci bandanası) | 24 saat pencere, tek sefer; sosyal ispat + value | L5'te ilk kez tetiklenen yeni oyuncu |
| `remove_ads` | 3.99 | ~128 TL | Tüm interstitial kaldırılır; rewarded ve starter pack korunur | Retention koruyucu; review kalitesi artırır | Reklamdan rahatsız olan orta-üst segment |

Fiyatlar Google Play / App Store bölgesel otomatiğine bırakılır; manuel TL override yok.

---

## Oyuncu-Dostu Anayasa

1. **Rewarded her zaman opt-in.** Hiçbir reklam zorla oynatılmaz; değer teklifi ekranda net görünür ("50 Enerji kazan"), butonun yanında "Hayır, teşekkürler" seçeneği daima mevcuttur.
2. **L1-L15 dokunulmaz.** Bu seviyelerde enerji bariyer gösterimi, interstitial ve IAP popup tamamen kapalıdır. Bağlanma önce gelir; monetizasyon sonra.
3. **1 session'da en fazla 2 interstitial.** Session sayacı SQLite `Sessions` tablosunda tutulur; sayaç her 4 dakikada bir sıfırlanır, session genelinde hard cap 2'dir.
4. **Remove Ads satın alındıktan sonra interstitial sonsuza kadar kapalıdır.** Güncelleme veya yeniden yükleme sonrası restore purchase akışı zorunludur (StoreKit 2 / Play Billing).

---

## Baskı ve Denge Kuralları

- **Interstitial tetik zinciri**: Level tamamlama → dönüş ekranı → interstitial (eğer: L4+, run sayısı ≥ 4, son interstitial'dan ≥ 4 dk geçmiş). Koşullardan biri sağlanmazsa reklam atlanır, sonraki fırsata taşınır.
- **Rewarded opt-in-rate hedefi**: Enerji Dolumu placement %30+; 2x Loot %25+. Bu oranın altında kalınırsa (A/B test sonrası) değer teklifi miktarı artırılır, reklam zorunlu hale getirilmez.
- **ARPDAU hedefi**: 0.20 USD (Türkiye için ~6.5 TL). Rewarded impression başına 0.10–0.15 USD eCPM varsayımıyla günde 1.5–2 rewarded impression ortalama → interstitial katkısı ile birlikte hedefe ulaşılabilir.
- **Çift currency dengesi**: Soft (Altın Sikke) kazanma hızı L50'ye kadar IAP gerektirmeden normal ilerlemeye yetecek şekilde kalibre edilir. Hard (Keşif Mücevheri) sert cap IAP'a bağlanmış; günlük görevlerle küçük miktarlarda ücretsiz kazanılır.

---

## A/B Test Önerisi (Launch +2 Hafta)

| Varyant | Rewarded Placement Sayısı | Beklenti |
|---------|--------------------------|----------|
| A (kontrol) | 3 (Enerji Dolumu + 2x Loot + Üretici Hızlandır) | Baseline opt-in-rate ve ARPDAU |
| B (test) | 5 (+Fog Açma Bonusu: +3 hücre; +Günlük Bonus: 2x streak coin) | Daha yüksek impression, opt-in-rate düşer mi? |

Karar kriteri: 14 gün sonra ARPDAU ve D7 retention. Varyant B, ARPDAU +%15 sağlıyor ama D7 retention -%5 veya daha fazla düşüyorsa kontrol seçilir.

---

## Uyumluluk

### ATT (iOS 14.5+)
- İlk açılışta native ATT dialog gösterilir. Kullanıcı reddederse rewarded ad çalışmaya devam eder; yalnızca hedefleme değişir (contextual ads). İzin durumu SQLite `Players.attConsent` alanında saklanır.

### GDPR / CCPA
- AB ve Kaliforniya IP'lerinde ilk açılışta consent banner: "Kişiselleştirilmiş reklamlara izin ver / Sadece gerekli". Seçim `Players.gdprConsent` alanında saklanır; değiştirme seçeneği ayarlar ekranında daima erişilebilir.

### Google Families Policy / COPPA
- **Kritik Risk**: Brief'te ikincil hedef kitle 10-17 (bir kısmı 7-14) belirtilmiştir. Google Families Programı için 13 yaş altı kullanıcıya hedeflenmiş reklam yasaktır.
- **Karar**: Oyun "mixed audience" olarak sınıflandırılacak; reklam SDK'ya `setTagForChildDirectedTreatment(false)` ve `setTagForUnderAgeOfConsent(true)` (AB) ile uygun işaret gönderilecek. Bunun yerine Google Families Programı'na katılım (reklam kaldırılır, IAP kısıtlanır) v1.0 için reddedilir — gelir etkisi kabul edilemez. Yaş doğrulama mekanizması eklenmez (friksiyon + maliyet yüksek); store metadata "12+" olarak işaretlenir.

---

## IAnalytics Event Listesi (Stub v1.0)

```csharp
// Rewarded
analytics.Track("ad_request",       new { placement, level });
analytics.Track("ad_impression",    new { placement, level, adNetwork });
analytics.Track("ad_reward_granted",new { placement, rewardType, rewardAmount, level });
analytics.Track("ad_skipped",       new { placement, level, watchedSeconds });

// Interstitial
analytics.Track("interstitial_shown",   new { trigger, level, sessionMinutes });
analytics.Track("interstitial_skipped", new { level, skippedAtSecond });

// IAP
analytics.Track("iap_initiated",  new { sku, price, trigger });
analytics.Track("iap_completed",  new { sku, price, currency });
analytics.Track("iap_failed",     new { sku, reason });
analytics.Track("iap_restored",   new { sku });
```

Tüm event'ler `IAnalytics` interface arkasında; v1.0 stub implementasyonu local SQLite `AnalyticsEvents` tablosuna yazar (backend yok, local-only kural).

---

## Risk Uyarıları

| Risk | Seviye | Eylem |
|------|--------|-------|
| Google Families / COPPA uyumsuzluğu | Yüksek | Store metadata 12+ olarak işaretle; reklam SDK'ya yaş işareti gönder; hukuki onay alınmadan çocuk-odaklı pazarlama yapma |
| Rewarded opt-in-rate %25 altında kalırsa ARPDAU hedefi tutmaz | Orta | A/B test sonrası değer teklifi miktarını artır (ör. +50 → +80 Enerji) |
| Remove Ads satın alındıktan sonra restore purchase eksikliği | Orta | StoreKit 2 ve Play Billing restore akışı build kapısında developer'a zorunlu not olarak iletildi |
| TR kur oynaklığı — TL fiyatlar store otomatiği dışına çıkarsa algı bozulur | Düşük | Manuel override yok; store otomatiğine güven |
