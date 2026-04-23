# Mobil Oyun Desenleri (İşe Yarayanlar)

Küçük, tek geliştiriciyle üretilebilir mobil oyunlarda tekrar eden başarı desenleri ve tuzaklar.

## Başarı desenleri

### 1. Hemen anlaşılan core loop
- İlk 5 saniyede ne yaptığın belli olmalı.
- Tutorial sade: "yapıp öğren", ekran dolusu metin değil.
- Örnek: Flappy Bird, Crossy Road, Stack.

### 2. Kısa session, yüksek tekrar
- 30 s – 3 dk tur süresi.
- "Bir tur daha" hissi: skor, kombo, yeni unlock eşiği.
- Kaybetme hızlı + restart 1 tap uzakta.

### 3. Yumuşak ilerleme
- Her run'dan küçük bir kazanç (coin, XP, rastgele cosmetic drop).
- Plateaux olmamalı — her 5-10 run'da yeni bir şey açılmalı.

### 4. Görsel kimlik
- Tek ayırt edici palet + 1 sinyal efekt (parçacık, glow, gölgelendirme).
- Icon + screenshot'ta hemen tanınır.
- Örnek: Monument Valley (izometrik + pastel), Alto's Adventure (silüet + gradient).

### 5. Dokunsal geri bildirim
- Her aksiyonda ses + titreşim + görsel (3-katmanlı feedback).
- "Hiç bir şey olmadı" hissi öldürür.

### 6. Paylaşılabilir an
- Yüksek skor anında paylaş butonu.
- Günlük challenge meydan okuma formatında.
- Gerektiğinde leaderboard (arkadaşlar listesi opsiyonel).

### 7. Dinamik zorluk
- Oyuncu skill'i ölç, zorluğu kaydırılabilir şekilde ayarla.
- "Zor" ve "Kolay" modu ayıran oyunlar yerine organik adaptasyon.

## Başarısızlık desenleri

### 1. Onboarding uzunluğu
- 3 dakika tutorial = oyuncu D0'da çıkar.
- Wall of text, skip yok, forced tap-through.

### 2. Ad spam
- Run arası interstitial + app-open + banner.
- 1-star yorumlar başlıca bu sebepten.

### 3. Paywall'lar
- Level 5'te "devam etmek için IAP al".
- Leaderboard'da para = üst sıra.

### 4. Session kesici
- Çok uzun tur (10+ dk).
- Kaydedilemeyen progress.
- Arka plana alınca state kaybı.

### 5. Grind duvarı
- "X saat oyna 1 item aç" yerine D1'de yeni şey.
- Ekonomik dengesizlik (soft currency enflasyonu).

### 6. Genel görsel dil
- Jenerik stok asset, kimliksiz tipografi.
- Store icon'da rekabetten ayrışmıyor.

## Retention hook'ları
- **D1**: yeni cosmetic drop / daily challenge başlangıcı.
- **D3**: streak bonus eşiği.
- **D7**: yeni mod unlock veya leaderboard cycle.
- **D30**: sezonluk etkinlik (opsiyonel; tek oyun için overhead).

## Session uzunluğu önerileri
| Oyun türü | Önerilen tur | Toplam session |
|---|---|---|
| Hyper-casual | 15-60 s | 5-10 dk |
| Casual | 1-5 dk | 15-30 dk |
| Puzzle | 2-8 dk | 20-45 dk |
| Arcade | 1-3 dk | 10-20 dk |

## Kontrol şeması
- **1 parmak / tap**: en geniş kitle.
- **Swipe**: daha fazla ifade.
- **Tilt**: yalnızca bir mekanik önemliyse (yoksa accessibility problem).
- **Çok parmak / gesture**: küçük oyuna karmaşık, kaçın.

## Örnek çalışma
Stack (Ketchapp): core loop 5 saniyede belli (kule üst üste kondur), tur ortalaması 45 s, restart 1 tap, her run küçük skor ilerleme, monetization tek interstitial arada + opsiyonel remove-ads. Basit + polished + sürdürülebilir.

## Referanslar
- Mobil oyun tasarım pratiği (indie studio blog'ları, Game Developer Conference mobile track, Reddit r/gamedev, r/IndieDev).
- Store analitik gözlemleri (Sensor Tower, data.ai kamu raporları).
- Playtest deneyim notları (topluluk + kendi test oturumları).
