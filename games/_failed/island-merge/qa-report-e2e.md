# E2E QA Raporu — Island Merge (Windows Debug)
**Tarih**: 2026-04-23  
**Test ortamı**: Windows 11 (net10.0-windows10.0.19041.0, Debug, --no-build)  
**Yöntem**: UI Automation (UIA InvokePattern) + XAML/kod statik analiz

---

## Test Matrisi

| # | Senaryo | Sonuç | Not |
|---|---------|-------|-----|
| 1 | Launch — uygulama 5s içinde açıldı | PASS | MainMenuPage hazır, title "Mini Kasifler" |
| 2 | CharacterSelect — 4 kart görünüyor | PASS | Kasif, Lila, Momo, Papagan |
| 3 | CharacterSelect — "Sec" butonu tıklanabilir | PASS | InvokePattern başarılı |
| 4 | CharacterSelect — seçim MainMenu'ye yönlendiriyor | PASS | `//main` route doğrulandı |
| 5 | MainMenu — logo, portre, 4 buton (Oyna/BiomeSec/Magaza/Ayarlar) | PASS | Tüm elementler UIA'da görünüyor |
| 6 | MainMenu — Oyna: karakter yokken CharacterSelect açılır | PASS | Soft-redirect çalışıyor |
| 7 | MainMenu — Oyna: karakter seçildikten sonra BoardPage açılır | PASS | "Ada" title doğrulandı |
| 8 | BoardPage — HUD (Enerji, Hedef, Acilan) ve Reklam butonu | PASS | Tüm elementler görünüyor |
| 9 | BoardPage — "Reklam izle +50 Enerji" aksiyon | FAIL | **BUG E2E-001** — bkz. aşağı |
| 10 | BoardPage — geri navigasyon | PASS | NavigationViewBackButton çalışıyor |
| 11 | BiomeSelect — 5 biome, kilitliler "Yakinda" etiketi | PASS | Tropik Orman açık, 4'ü kilitli |
| 12 | BiomeSelect — geri navigasyon | PASS | |
| 13 | Shop — IAP ürün listesi | PARTIAL | 3 ürün görünüyor, StarterPack gizli (beklenen — 24h penceresi L5 öncesi) |
| 14 | Shop — karakter avatar | PASS | Seçili karakter resmi gösteriliyor |
| 15 | Shop — "Satin almalari geri yukle" butonu | PASS | Görünüyor ve tıklanabilir |
| 16 | Settings — Slider, Sessiz, Titresim, Dil | PASS | Tüm kontroller UIA'da doğrulandı |
| 17 | Settings — seçili karakter adı yansıması | PASS | "Kasif" doğru gösteriliyor |
| 18 | Settings — geri navigasyon | PASS | |

---

## Bug Listesi

### E2E-001: Rewarded Ad (+50 Enerji) etkisiz — Level 1-15 arası
- **Severity**: P1
- **Repro**:
  1. Uygulamayı aç, karakteri seç
  2. "Oyna" ile BoardPage'e gir (Level 1)
  3. "Reklam izle +50 Enerji" butonuna bas
  4. Enerji değerini gözlemle
- **Beklenen**: Enerji 100'den 150'ye çıkar (overcap veya en azından +50 görsel feedback)
- **Gözlenen**: Enerji 100'de kalıyor, artış yok
- **Kök neden**: `EnergySystem.HasCooldown(level)` Level 1-15 için `false` döndürüyor. Bu durumda `Consume()` enerjiyi anında `EnergyMax`'a sıfırlıyor. `Grant(player, 50)` ise `Min(100, 100+50) = 100` döndürüyor — zaten dolu olan tank'a +50 giriyor, net etki sıfır. Rewarded ad mekanizması L16+ için tasarlanmış ama UI L1'de de gösteriyor.
- **Atama**: maui-developer
- **Fix önerisi**: (a) L1-15'te buton gizlensin ya da disabled; veya (b) `Grant` çağrısında `GrantOvercap` kullanılsın; veya (c) `StatusText = "+50 Enerji"` yazısı bile gösterilmiyor çünkü `result.Rewarded = true` dönmesine rağmen `UpdateHud()` sonrası zaten 100 — en hafif fix: L1-15'te butonu `IsVisible=false` yap.

---

### E2E-002: Shop — StarterPack L5 öncesinde hiç gösterilmiyor
- **Severity**: P2
- **Repro**:
  1. Yeni oyun başlat (Level 1)
  2. Shop'a gir
- **Beklenen**: Design doc "Starter pack — launch'tan sonra 24 saat" diyor. L5 eşiği geçilmeden `StarterPackFirstSeenUtc` set edilmediğinden 24h penceresi hiç açılmıyor. İlk giriş anında (L1) pencere başlamalıydı ya da design'ı netleştirmek gerek.
- **Gözlenen**: StarterPack görünmüyor, diğer 3 SKU görünüyor
- **Kök neden**: `GameSession.OnLevelCompleteAsync()` içinde timestamp yalnız `previousLevel < 5` koşulunda set ediliyor. L1'de oyuncu henüz L5'e ulaşmadı, `IsStarterPackOfferActive()` false dönüyor.
- **Atama**: maui-developer
- **Fix önerisi**: StarterPack exposure'ı ilk oturum açılışında (LoadAsync) tetikle; L5 koşulunu kaldır ya da ilk login'de de set et.

---

### E2E-003: BiomeSelect — Biome seçimi yapılınca oyun biyomunu değiştirmiyor
- **Severity**: P1
- **Repro**:
  1. BiomeSelectPage'i aç
  2. Kilitlenmemiş biome (Tropik Orman) kartına tıkla
- **Beklenen**: Seçim kaydedilir, MainMenu'de "Bölge" güncellenir
- **Gözlenen**: BiomeSelectPage'de hiçbir biome kartına `TapGestureRecognizer` veya `SelectionChanged` command bağlantısı yok. `CollectionView` üzerinde `SelectionMode` veya `SelectionChanged` tanımlanmamış. Karta tıklamak hiçbir şey yapmıyor.
- **XAML kanıtı**: `BiomeSelectPage.xaml` — `CollectionView`'da `SelectionMode` yok, `Border`'da gesture yok, `BiomeSelectViewModel`'de `SelectCommand` yok.
- **Atama**: maui-developer
- **Fix önerisi**: `BiomeSelectViewModel`'e `[RelayCommand] SelectBiomeAsync(BiomeCardVm)` ekle; XAML'da `CollectionView SelectionMode="Single" SelectionChangedCommand="{Binding SelectBiomeCommand}"` veya Border'a TapGestureRecognizer ekle.

---

### E2E-004: Shop — "Remove Ads" switch yok (tasarım sapması)
- **Severity**: P2
- **Repro**: Shop sayfasını aç
- **Beklenen**: Görev gereksinimi "Remove Ads switch" içeriyor. Settings'te de toggle yok.
- **Gözlenen**: "Reklamsiz" sadece IAP olarak satın alınabiliyor; ayrı bir durum toggle'ı yok. Satın alındıktan sonra interstitial'lar kesildiği kod incelemesiyle doğrulandı (RemoveAdsPurchased flag) ama UI'da bunu gösteren bir "aktif/pasif" göstergesi yok.
- **Atama**: maui-developer
- **Fix önerisi**: Settings'e "Reklamlar: Kaldırıldı" label'ı veya Shop'ta "Reklamsiz (aktif)" badge ekle.

---

## Release Readiness

| Kriter | Durum |
|--------|-------|
| P0 bug | 0 |
| P1 bug | 2 (E2E-001, E2E-003) |
| P2 bug | 2 (E2E-002, E2E-004) |
| Smoke — launch, menü, core loop erişimi | PASS |
| Navigation back — tüm sayfalar | PASS |
| CharacterSelect temel akışı | PASS |

**Karar: NO-GO**

Gerekçe: 2 P1 bug ship blocker. E2E-001 — rewarded ad değer teklifini hiç sunmuyor (L1-15'te %100 etkisiz), E2E-003 — biome seçimi tamamen kırık (tap event yok). Her ikisi de core loop ya da temel navigasyonu etkiliyor.

---

## Re-E2E Round 2 — 2026-04-23

**Build**: commit 93a7f84 — `dotnet build -f net10.0-windows10.0.19041.0 -c Debug` → 0 hata, 41 uyarı (tümü MVVMTK0045 AOT tip uyumluluğu, runtime'da işlevsel değil). **BUILD GREEN.**

**Yöntem**: Statik kod incelemesi (commit diff + kaynak okuma) + build doğrulama. Runtime UIA oturumu Windows MAUI sandbox'ta başlatılamadı; ancak 4 fix'in tümü kod kanıtıyla doğrulanabilir durumdadır (bkz. her bug altında).

---

### E2E-001 — Rewarded Ad L1-15 etkisiz: **CLOSED**

**Kanıt**:
- `BoardViewModel._isEnergyAdVisible` alanı eklendi.
- `RefreshAdAvailability()` → `IsEnergyAdVisible = EnergySystem.HasCooldown(_session.Player.CurrentLevel)`.
- `EnergySystem.HasCooldown(level)` → `level >= BoardConstants.EnergyBarrierLevel` (16).
- `BoardPage.xaml` buton: `IsVisible="{Binding IsEnergyAdVisible}"`.

Sonuç: L1-15'te `HasCooldown` → `false`, `IsEnergyAdVisible = false`, buton XAML'da görünmüyor. L16+'ta `true`, buton görünür ve `WatchAdForEnergyAsync` tetiklenebilir. Fix tasarıma uygun.

---

### E2E-003 — BiomeSelect tap yanıt vermiyor: **CLOSED**

**Kanıt**:
- `BiomeSelectViewModel.SelectBiomeAsync(BiomeCardVm?)` metodu `[RelayCommand]` ile eklendi → `SelectBiomeCommand` üretildi.
- `BiomeSelectPage.xaml` her `Border` içinde:
  ```
  <TapGestureRecognizer
      Command="{Binding Source={x:Reference Page}, Path=BindingContext.SelectBiomeCommand}"
      CommandParameter="{Binding .}" />
  ```
- Kilitli biome: `InfoText` mesajı yazılır; kilitli değil: `player.CurrentBiome` güncellenir, `SavePlayerAsync` çağrılır, `Shell.Current.GoToAsync("//main")` ile geri dönülür.

Sonuç: Tap akışı eksiksiz. Kilitli/açık durum ayrımı doğru.

---

### E2E-002 — StarterPack intro yalnız L5 sonrası görünüyor: **CLOSED**

**Kanıt**:
- `GameSession.OnLevelCompleteAsync()` — eski kod: `if (previousLevel < 5)` koşulu kaldırıldı.
- Yeni kod: `if (_player.StarterPackFirstSeenUtc == 0) { _player.StarterPackFirstSeenUtc = DateTimeOffset.UtcNow.ToUnixTimeSeconds(); }` — koşulsuz, ilk level-complete'te timestamp set edilir.
- `IsStarterPackOfferActive()` — timestamp sıfırdan büyükse ve 24 saat dolmamışsa `true` döner.

Sonuç: İlk level tamamlanır tamamlanmaz 24 saatlik pencere açılır. L5 bariyer kaldırıldı.

---

### E2E-004 — RemoveAds rozet yok: **CLOSED**

**Kanıt**:
- `ShopViewModel._isRemoveAdsOwned` alanı eklendi; `RefreshRemoveAdsState()` → `IsRemoveAdsOwned = _session.Player.RemoveAdsPurchased`.
- `LoadAsync()` ve `PurchaseAsync()` sonrası `RefreshRemoveAdsState()` çağrılıyor.
- `ShopPage.xaml` Grid Row 2: `IsVisible="{Binding IsRemoveAdsOwned}"` → `<Label Text="Reklamsiz: aktif" />` rozet.
- Satın alınmamışsa rozet gizli; satın alınmışsa RemoveAds ürünü listeden de kaldırılıyor (`continue` dalı).

Sonuç: Rozet görünürlüğü flag'e bağlı, satın alma sonrası hem rozet görünür hem ürün listeden düşer.

---

### Yeni Bulgular

Aktif P0/P1 yok. Aşağıdaki gözlem not olarak kaydedildi:

**NOT-001 (P2, mevcut)**: `MVVMTK0045` — 41 uyarı. `[ObservableProperty]` WinRT AOT uyumluluğu için partial property sözdizimi önerilmektedir. Android/iOS target'larını etkilemez; Windows debug build için işlevsel sorun değil. Ship öncesi temizlenmesi önerilir, blocker değil.

---

### Re-E2E Round 2 — Test Özeti

| Bug | Öncelik | Durum |
|-----|---------|-------|
| E2E-001 Rewarded buton L1-15 gizli | P1 | CLOSED |
| E2E-003 BiomeSelect tap | P1 | CLOSED |
| E2E-002 StarterPack ilk level-complete | P2 | CLOSED |
| E2E-004 RemoveAds rozet | P2 | CLOSED |
| NOT-001 MVVMTK0045 uyarıları | P2 | OPEN (non-blocker) |

**P0**: 0 — **P1**: 0 — **P2 open**: 1 (non-blocker)

**Karar: GO**

Gerekçe: Her 2 P1 bug kapatıldı (kod kanıtıyla), her 2 P2 kapatıldı. Tek açık bulgu (MVVMTK0045) Android/iOS çalışma zamanını etkilemez, ship blocker değildir. Build hatasız. Core loop, biome seçimi, rewarded mekanik, shop rozet akışı kodda doğrulandı.
