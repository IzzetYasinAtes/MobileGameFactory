# Monetization İçgörüler

Küçük, solo üretilmiş mobil oyunlar için para kazanma yaklaşımları — sahadan gözlemler.

## Temel ayarlar

### Ücretsiz + ads + IAP (F2P)
Küçük oyunlar için **varsayılan model**. En geniş kitle.

### Premium (paid upfront)
- 0.99–2.99 USD indirilebilir.
- Kitle çok küçük; yalnızca güçlü marka veya niş bir deneyimse mantıklı.
- Küçük oyun için önerilmez; dönüşüm zorluğu yüksek.

### Premium lite (ücretsiz + tek IAP "remove ads")
- F2P'nin özel bir varyantı.
- Kaliteli bir küçük oyun için **en sağlıklı** model.
- Default: ads açık; 2.99 USD tek sefer ödeme = ads kapalı + 2x coin.

## Rewarded ad (RA) - en değerli reklam tipi

### Neden değerli
- Oyuncu **izlemeyi seçer**; negatif his yok.
- eCPM yüksek (Android'de $5–15 orta pazarlarda, $15–30 üst pazarlarda).
- D7 retention'a **pozitif** katkı — oyuncu kazandığı şeye bağlanır.

### Yerleşim noktaları (güçten zayıfa)
1. **Revive / continue** (ölünce "izle ve devam et").
2. **2x reward** (run sonunda "izle, kazancı ikiye katla").
3. **Skip timer** (cooldown / energy sistemi varsa).
4. **Unlock preview** (cosmetic'i denemek için izle).
5. **Daily gift** (1/gün izle).

### Tuzaklar
- Reward değersizse kimse izlemez; eCPM sıfır.
- Reward çok değerliyse oyun ekonomisi bozulur (grind manasız olur).
- Offline'de rewarded çalışmaz; alternatif yol sun.

## Interstitial ad (IA)

### Frekans kuralı
- **Maks 1 / 4 dk**.
- İlk 3 run boyunca hiç.
- Session start'ta asla.
- Core loop ortasında asla (sadece run bitiminde, menüye dönüşte).
- Skip butonu ≥ 5 s.

### Zarar eşiği
İnterstitial her artırıldığında:
- D1-D7 retention düşer.
- Yorum ratings düşer.
- Uninstall rate artar.

Sabit fayda noktası: günde 2-3 interstitial ortalama oyuncu başına. Fazlası LTV'yi düşürür.

## Banner ad

Küçük oyunlar için **genellikle zarar**:
- Çok düşük eCPM ($0.3–1).
- Gameplay ekranında dikkat dağıtıcı, performance etkiliyor.
- Ekran real estate kaybı.

Kabul edilebilir tek yer: shop/menu ekranları. Varsayılan **kapalı**.

## App-open ad

- Session başlangıcında açılış ekranı.
- Yüksek eCPM ama **D1 retention'a zarar** (kullanıcı ilk izlenimde bariyer görür).
- Küçük oyun için **önerilmez**.

## IAP stratejileri

### 1. Remove ads (tek sefer)
- 1.99–3.99 USD.
- En yüksek conversion rate tier (küçük oyunlarda %2-4).
- Ads ile paralel çalışır (kullanıcıya seçenek).

### 2. Cosmetic (skin, tema, efekt)
- 0.99–4.99 USD aralığında.
- Visual progression desteklerse çalışır.
- Pay-to-win değil — leaderboard/skill'i etkilemez.

### 3. Soft currency paketi
- 0.99 / 4.99 / 9.99 tier.
- Oran: 100 / 600 / 1500 (örnek — bonus ile sıralama).
- Kazanma hızı korunmalı, IAP sadece hızlandırır.

### 4. Permanent boost
- 2x XP, 2x coin, extra slot — tek ödeme, kalıcı.
- Engaged oyuncunun en çok ilgi gösterdiği SKU.

### 5. Starter pack
- İlk 24-72 saat sınırlı, tek sefer.
- Yüksek conversion (%5-10 casual oyunlarda).
- Sadece giriş sonrası gösterilmeli.

## Yasaklı/riskli

- **Loot box**: rastgelelik + ödeme. Yasal kısıtlar (Hollanda, Belçika), topluluk tepkisi.
- **Subscription**: tek oyun için retention yetersiz.
- **Pay-to-win**: leaderboard veya rekabette ödemeli avantaj.
- **Dark pattern**: sahte geri sayım, "sadece bugün!" aldatmacası.

## Gelir tahmini (kaba, küçük oyun)

Varsayım: DAU (daily active user) 1.000, %40 ad-view, %2 IAP dönüşüm.
- Ad geliri (Android orta pazar): $15–40 / gün.
- IAP: 20 kullanıcı × ortalama $2 = $40 / gün.
- **Toplam**: ~$60 / gün = $1.800 / ay, 1K DAU için.

Küçük bir oyunun DAU'u ship sonrası ilk ay 500-5.000 arası gezinir. Büyümek için: ASO, cross-promotion (studio'nuzun diğer oyunları), organik paylaşım hook'u.

## Uyumluluk kontrolü (her ship öncesi)

- [ ] ATT (iOS 14.5+): IDFA izni dialogu. Reddetme durumu test edildi.
- [ ] GDPR (Avrupa): consent banner + data safety declaration.
- [ ] CCPA (California): "Do not sell my info" linki.
- [ ] COPPA (13 yaş altı): ads SDK'da children-mode flag.
- [ ] Data safety formu (Android): SDK başına topladığı veri işaretlendi.
- [ ] Privacy nutrition (iOS): aynı.

## Ölçüm sonrası aksiyon

Ship sonrası ilk 7 günde gör:
- RA opt-in rate < %20 → reward değerini artır.
- Interstitial frequency feedback → yorumlarda şikayet varsa cooldown'u gevşet.
- IAP conversion < %1 → fiyatlandırma + SKU seçimi gözden geçir.
- D1 retention < %25 → onboarding veya ilk ad yerleşimi hatalı.

## Not
Tüm bu rakamlar **yaklaşık**; bölgeye, genre'a, oyuncu kitlesine göre değişir. Kendi verin konuştuğu anda bu doküman fon müziği haline gelir; data öncelikli.
