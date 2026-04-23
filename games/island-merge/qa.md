# QA Test Planı — Mini Kaşifler: Kayıp Adanın Sırrı

**ID**: `island-merge`
**Kapı**: qa
**Durum**: Pre-build plan — execution build teslimi sonrası
**Tarih**: 2026-04-23

---

## 1. Cihaz Matrisi

| Öncelik | Cihaz Profili | RAM / API | Durum |
|---------|--------------|-----------|-------|
| P0 | Android low-end (örn. Samsung Galaxy A03s) | 3 GB / API 24 | Beklemede |
| P0 | Android mid-range (örn. Pixel 6a) | 6 GB / API 33+ | Beklemede |
| P1 | Android tablet (örn. Samsung Galaxy Tab A8) | 3-4 GB / API 30+ | Beklemede |
| P1 | iOS iPhone SE 2nd gen | 3 GB / iOS 15 | SKIPPED — Mac erişimi yok (backlog) |
| P2 | iOS iPhone 14 | 6 GB / iOS 16+ | SKIPPED — Mac erişimi yok (backlog) |

> iOS cihazlar Mac build ortamı hazır olduğunda yeniden atanır. Android-first ship stratejisi.

---

## 2. Smoke Checklist

| # | Test | Beklenen | Sonuç |
|---|------|----------|-------|
| S01 | Cold start (3 ölçüm ortalaması, low-end cihaz, Release build) | ≤ 2.0 s | NOT-RUN |
| S02 | Ana menü → core loop → geri | Akış hatasız, FPS sabit | NOT-RUN |
| S03 | 1 full run (L1): merge → quest teslim → fog açılma | Skor ve XP kaydedildi, fog animasyonu tetiklendi | NOT-RUN |
| S04 | Ses kapatma / titreşim toggle | Ayar anında geçerli, uygulama yeniden başlatıldıktan sonra korundu | NOT-RUN |
| S05 | Uçak modu aktifken oyun | Crash yok; rewarded ad offline fallback mesajı görünüyor | NOT-RUN |
| S06 | Arka plana al (2 dk) → geri dön | Board durumu korundu, enerji sayacı doğru hesaplandı | NOT-RUN |
| S07 | Ekran kilidi → aç | State kayıpsız; sayaç background süresi dahil doğru | NOT-RUN |
| S08 | Çağrı interrupt sırasında oyun | Uygulama pause/resume; board ve sayaç senkronize | NOT-RUN |

---

## 3. Test Senaryoları

### 3.1 Merge Engine

| ID | Senaryo | Beklenen | Sonuç |
|----|---------|----------|-------|
| ME-01 | Aynı tip + aynı tier 2 item drag-and-drop | Tier+1 item üretildi; kaynak item'lar silindi | NOT-RUN |
| ME-02 | Aynı tier farklı tip merge denemesi (Taş Kırığı + Dal) | Merge reddedildi; item'lar original konumunda kaldı | NOT-RUN |
| ME-03 | Tier 5 (tavan) item ile merge denemesi (örn. 2x Mistik Kristal) | Merge reddedildi veya zincir tavan uyarısı gösterildi | NOT-RUN |
| ME-04 | Dolu board'da merge (63 hücre dolu) | Merge sonrası 1 hücre boşalır; crash yok | NOT-RUN |
| ME-05 | Hızlı ardışık merge (3 merge < 2 sn) | Race condition yok; her merge ayrı ayrı kaydedildi | NOT-RUN |

### 3.2 Energy System

| ID | Senaryo | Beklenen | Sonuç |
|----|---------|----------|-------|
| EN-01 | L1-15 arası enerji sıfırlanma | Cooldown = 0; anında dolar; bariyer ekranı gösterilmez | NOT-RUN |
| EN-02 | L16+ enerji = 0 | Opt-in rewarded buton gösterildi; IAP shop erişimi var | NOT-RUN |
| EN-03 | Rewarded ad izle → +50 enerji | Enerji +50 eklendi; placement 30 s cooldown başladı | NOT-RUN |
| EN-04 | IAP energy_100 satın al (sandbox) | +100 enerji eklendi; SQLite kaydı doğrulandı | NOT-RUN |
| EN-05 | Uygulama arka plandayken enerji sayacı | Foreground'a dönünce geçen süre hesaplanmış; cheat-tolerant (SQLite Players.lastEnergyRefill) | NOT-RUN |

### 3.3 Fog Reveal

| ID | Senaryo | Beklenen | Sonuç |
|----|---------|----------|-------|
| FG-01 | Her 3 merge sonrası 1 hücre açılması | Tam 3. merge'de fog kaldırma animasyonu tetiklendi | NOT-RUN |
| FG-02 | Quest tamamlanınca ek 3 hücre patlaması | Quest teslim ekranında +3 hücre açılma animasyonu | NOT-RUN |
| FG-03 | Fog hücresi açılınca hazine/üretici reveal | Yeni item veya üretici hücrede görünür; SQLite fogRevealedMask güncellendi | NOT-RUN |
| FG-04 | Tüm hücreler açık durumdayken merge | Fog reveal tetiklenmiyor; hata yok | NOT-RUN |

### 3.4 Quest Completion + Ödül

| ID | Senaryo | Beklenen | Sonuç |
|----|---------|----------|-------|
| QS-01 | Aktif quest hedef item teslimi | XP + Altın Sikke eklendi; quest tamamlandı; yeni quest atandı | NOT-RUN |
| QS-02 | 2x Loot rewarded (görev teslim ekranı opt-in) | Ad izlendi; XP + coin 2x ile güncellendi | NOT-RUN |
| QS-03 | Görev atlama (item yok) | Görev beklemede; kullanıcı merge'e yönlendirildi; crash yok | NOT-RUN |

### 3.5 SQLite Persistence

| ID | Senaryo | Beklenen | Sonuç |
|----|---------|----------|-------|
| DB-01 | App restart sonrası board durumu | Tüm hücre konumları, XP, enerji, fog mask korundu | NOT-RUN |
| DB-02 | Merge sonrası force-kill → restart | Son merge kaydedildi (her merge'de otomatik commit) | NOT-RUN |
| DB-03 | Progress tablosu currentLevel + currentZone | Seviye ve bölge bilgisi doğru yüklendi | NOT-RUN |

### 3.6 Biome Transition

| ID | Senaryo | Beklenen | Sonuç |
|----|---------|----------|-------|
| BT-01 | B1 L20 tamamlanınca B2 açılışı | 3 sn reveal animasyonu tetiklendi; yeni zincir kilidi açıldı; crash yok | NOT-RUN |
| BT-02 | Yeni biome item zincirleri "gri + kilit ikonu" gösterimi | Kilit ikonu görünüyor; "???" yok | NOT-RUN |
| BT-03 | B1 → B2 geçişinde ses/müzik değişimi | Müzik enstrümanı değişti; geçiş kesintisi yok | NOT-RUN |

### 3.7 IAP Stub (Sandbox)

| ID | Senaryo | Beklenen | Sonuç |
|----|---------|----------|-------|
| IAP-01 | energy_100 (0.99 USD) sandbox satın alma | +100 enerji eklendi; SQLite doğrulandı; analytics `iap_completed` kayıt | NOT-RUN |
| IAP-02 | energy_500 (2.99 USD) sandbox satın alma | +500 enerji eklendi | NOT-RUN |
| IAP-03 | starter_pack (4.99 USD) — L5 sonrası 24 saat pencere | Tek sefer gösterildi; 500 enerji + 200 mücevher + kostüm eklendi | NOT-RUN |
| IAP-04 | remove_ads (3.99 USD) satın alma + doğrulama | Interstitial sonraki levelde gösterilmedi; rewarded opt-in çalışıyor | NOT-RUN |
| IAP-05 | remove_ads restore purchase | Yeniden yükleme sonrası interstitial hâlâ kapalı | NOT-RUN |

### 3.8 Rewarded Ad

| ID | Senaryo | Beklenen | Sonuç |
|----|---------|----------|-------|
| RW-01 | L16+ enerji = 0 opt-in butonu → izle → +50 enerji | +50 enerji eklendi; analytics `ad_reward_granted` kayıt | NOT-RUN |
| RW-02 | Aynı placement 30 s cooldown | 30 s geçmeden buton disabled/gizli | NOT-RUN |
| RW-03 | "Hayır, teşekkürler" seçeneği | Reklam gösterilmedi; kullanıcı önceki ekrana döndü | NOT-RUN |

### 3.9 Interstitial Frekans

| ID | Senaryo | Beklenen | Sonuç |
|----|---------|----------|-------|
| INT-01 | İlk 3 run — interstitial gösterimi | Hiçbir run'da interstitial tetiklenmedi | NOT-RUN |
| INT-02 | L4+ her 3 level tamamlanınca dönüş ekranı | İlk uygun koşulda interstitial gösterildi; skip ≥ 5. saniye | NOT-RUN |
| INT-03 | Session'da 2. interstitial (hard cap) | 2. interstitial gösterildi; 3. hiç tetiklenmedi (session boyunca) | NOT-RUN |
| INT-04 | 4 dk kısıtı: son interstitial'dan < 4 dk sonra level tamamlandı | Reklam atlandı; sonraki fırsata taşındı | NOT-RUN |

---

## 4. Edge Case Kütüphanesi

| ID | Senaryo | Repro Özeti | Beklenen |
|----|---------|-------------|----------|
| EC-01 | Dolu disk | Dummy 4 GB dosya + save tetikle | Save fail; kullanıcıya mesaj; crash yok |
| EC-02 | SQLite corruption | DB dosyasını elle boz / sil → restart | Otomatik re-init; veri sıfırlandı mesajı; crash yok |
| EC-03 | Rewarded cache yok (offline) | Uçak modu → enerji tüket → opt-in butona bas | Offline fallback mesajı; enerji eklenmedi; crash yok |
| EC-04 | IAP receipt fail | Sandbox'ta receipt doğrulama kesilmesi simüle et | IAP başarısız mesajı; enerji eklenmedi; retry seçeneği |
| EC-05 | Low memory (`onTrimMemory`) | `adb shell am send-trim-memory <pkg> RUNNING_CRITICAL` | State korundu; board restart'ta aynı; crash yok |
| EC-06 | Cihaz tarihi geri alındı | Sistem saatini 1 gün geri al → uygulamaya gir | Streak hesabı bozulmadı; negatif gün streak'e eklenmedi |
| EC-07 | Uçak modu + rewarded opt-in | Uçak modunda enerji tüket → rewarded butona bas | Offline fallback mesajı; crash yok; butonu tekrar aktif |

---

## 5. Performans Bench Hedefleri

| Metrik | Hedef | Ölçüm Yöntemi |
|--------|-------|---------------|
| Cold start | ≤ 2.0 s (low-end cihaz, Release) | Stopwatch, 3 ölçüm ortalaması |
| Frame rate | 60 FPS min; 55 FPS 95. yüzdelik | Android Studio Profiler / Choreographer callback |
| Memory peak | ≤ 250 MB | Android Studio Memory Profiler, 10 dk oyun sonu |
| Memory idle | ≤ 180 MB | Profiler, ana menüde beklerken |
| AAB boyutu | ≤ 40 MB | `dotnet build -c Release` çıktısı |
| Battery drain | ≤ %5 (10 dk sürekli oyun) | Android Studio Energy Profiler, mid-range cihaz |

---

## 6. GO/NO-GO Kriterleri

| Koşul | Etki |
|-------|------|
| P0 bug açık | NO-GO |
| 2+ P1 bug açık | NO-GO (tek P1 negotiable) |
| Herhangi bir performans hedefi aşıldı | NO-GO |
| Smoke checklist'te herhangi bir FAIL | NO-GO |
| Tüm P0 kapalı, ≤1 P1, tüm perf hedefleri geçti | GO |

---

## 7. Bug Template (Örnek)

```
## P0-001: Tier 5 merge crash — board doluyken Mistik Kristal merge denemesi
- **Repro**:
  1) Board'u 63 hücreye kadar doldur.
  2) 2 adet Mistik Kristal (tier 5, tavan) üret.
  3) İkisini drag-and-drop ile birleştirmeye çalış.
- **Beklenen**: Merge reddedildi; tavan uyarı mesajı; crash yok.
- **Gözlenen**: Uygulama `NullReferenceException` ile kapandı.
- **Cihaz**: Samsung Galaxy A03s / Android 11 (API 30)
- **Atama**: maui-developer
- **Durum**: open
```

---

## 8. Özel Notlar

### COPPA / Google Families Policy
Oyun "mixed audience" (12+) sınıflandırması. Reklam SDK'ya `setTagForChildDirectedTreatment(false)` + `setTagForUnderAgeOfConsent(true)` (AB bölgesi) doğru iletilmeli. QA'da AB VPN ile test yapılmalı: consent banner görünüyor mu?

### ATT Dialog (iOS — backlog)
Mac build hazır olduğunda: ilk açılışta native ATT dialog gösterilmeli; reddedilince rewarded ad çalışmaya devam etmeli; `Players.attConsent` SQLite'a yazıldı mı doğrulanmalı.

### GDPR Consent Banner
TR IP'de banner zorunlu değil; ancak AB IP'den test yapılmalı (VPN yeterli). "Sadece gerekli" seçilince kişiselleştirilmiş reklam kapalı, rewarded çalışmaya devam etmeli. Ayarlar ekranında consent değişikliği seçeneği her zaman erişilebilir olmalı.

### L1-15 Monetization Kilidi
L1-15 arası enerji bariyer ekranı, interstitial ve IAP popup hiçbir koşulda açılmamalı. Ayrı test geçişi zorunlu.

---

## Execution Durumu

Build teslimi bekleniyor. Plan hazır — APK/AAB elime geçince senaryo sırası: Smoke → Functional → Edge Case → Perf → GO/NO-GO kararı.
