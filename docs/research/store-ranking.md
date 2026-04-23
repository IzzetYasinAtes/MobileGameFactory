# Store Ranking / ASO Faktörleri

Play Store ve App Store'da keşfedilebilirlik için ölçülebilir faktörler.

## Ortak faktörler (iki store)

### 1. Title
- Keyword taşır + marka çağrışımı.
- Android ≤30 char, iOS ≤30 char.
- Kısa, tek kelimelik marka + niteleyici: "Neon Bird — Retro Arcade".
- Title içine 1-2 yüksek hacimli keyword sıkıştır (spam'lamadan).

### 2. Icon
- 1024×1024 PNG.
- Rakip gridinde ayrışmalı.
- Tek merkezi öğe + güçlü kontrast.
- Metin yok ya da 1-2 kelime (okunabilir boyutta).
- A/B test: 2-3 varyant Play Console'da deneme.

### 3. Screenshots
- İlk 2 screenshot kritik; preview alanında görünür.
- Gerçek gameplay + ekran üstü metin ("EASY TO PLAY", "HARD TO MASTER").
- Yatay/dikey: oyunun orientation'ına uyumlu.
- 4-8 screenshot; tablet için 2 ekstra.

### 4. Promo video (opsiyonel)
- 15-30 s.
- İlk 3 saniyede core loop göster.
- Gameplay tercih, CGI/trailer değil (store algoritması gameplay'i ödüllendirir).

### 5. Rating
- **> 4.3** yüksek rank için eşik.
- İlk 100 rating kritik (küçük sample'da ortalama hızlı düşer).
- In-app rating prompt: `Microsoft.Maui.StoreReview` veya native API; oyuncu iyi bir andayken (yüksek skor sonrası).

### 6. Retention
- **D1, D7 retention** store algoritmasına sinyal.
- Oyuncu kısa süre içinde siliyorsa rank düşer.

### 7. Crash rate
- **< %1** hedef; %2 üstü cezalandırılır.
- Play Console Android Vitals, App Store Connect Crashes.

## Android'e özel

### Play Console sinyalleri
- **Install conversion rate** (store görüntüleme → kurulum).
- **Short description** (80 char) — ilk dikkat çekme.
- **Data safety**: doldurulmuş ve doğru.
- **Target SDK**: en son Android sürümüne yakın.
- **App size**: < 40 MB tercih edilir (install conversion artar).
- **Keyword yoğunluğu**: description'da doğal dilde 2-3 keyword tekrarı.

### ASO araçları (opsiyonel)
- AppTweak, Sensor Tower, data.ai — yüksek hacimli keyword bulma.
- Ücretsiz: Google Play keyword tool, AppFollow free tier.

## iOS'a özel

### App Store sinyalleri
- **Subtitle** (30 char): title altında ek bilgi + keyword.
- **Keywords field** (100 char): virgülle; hiçbiri description'da tekrar edilmez (algoritma zaten okur).
- **Promotional text** (170 char): launch sonrası değiştirilebilir (ilk 7 gün boost için).
- **In-App Events** (iOS 15+): challenge/etkinlik için event bildirimi.
- **Price**: ücretsizse free, IAP miktarı önden belirt.
- **Category + secondary category**: doğru seçim; Games → Arcade/Puzzle/vb.

### Apple Search Ads (opsiyonel)
- Ücretli boost; launch haftasında düşünülebilir.
- CPT $0.10-2.00 aralığında.

## Launch stratejisi

### Soft launch (opsiyonel)
- 1-2 hafta küçük pazarlarda (Türkiye, Polonya, Vietnam, Filipinler).
- Retention/monetization ölçümü.
- Düzeltme, sonra global.

### Staged rollout (Android)
- %10 → %25 → %50 → %100 (3-7 gün aralıklarla).
- Crash sinyali varsa rollout durdur.

### Apple review
- 1-3 gün.
- Expedited review nadiren gerekli (launch tarihi planla).

## Hard kurallar (iki store)

- **Metadata spam** yasak: title'da keyword dizmek, emoji abuse.
- **Screenshot metin yoğunluğu** abartı: gameplay görünürlüğü kayıp.
- **Fake reviews**: Apple hesap ban eder.
- **Yanıltıcı icon**: gameplay'i yansıtmıyorsa ret.
- **Privacy policy** linki çalışır olmak zorunda.

## Ölçüm matrisi (ship sonrası ilk ay)

| Metrik | Hedef | Eylem |
|---|---|---|
| Install conversion rate | > %25 | Düşükse screenshot/icon A/B |
| D1 retention | > %30 | Düşükse onboarding/first-ad noktası |
| D7 retention | > %10 | Düşükse progression/retention hook |
| Rating | > 4.3 | 4.0 altı → review prompt timing |
| Crash rate | < %1 | 2% aşılırsa P0 hotfix |
| Keyword rank (top-3 keyword'te) | < 50 | Düşükse title/desc iyileştir |

## Uzun vadeli taktikler

- Her 6-8 haftada "update" atılması (yeni seviye, skin, challenge) — store algoritması aktif oyunları ödüllendirir.
- Mevsimsel event (Yılbaşı, yaz) — store'da "featured" seçimi için metadata.
- Cross-promo: kendi diğer oyunlarınız arasında house ads — ücretsiz dağıtım.

## Kaynak
Store dokümanları (Google Play, App Store Connect resmi rehberler), ASO pratisyenleri (AppTweak, Phiture, Adam Ali blog'ları) ve topluluk gözlemleri (r/androiddev, Apple Developer forums).
