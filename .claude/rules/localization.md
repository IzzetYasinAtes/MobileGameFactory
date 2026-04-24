# Localization Kuralları

## Sert kurallar
1. **Tier 1 zorunlu**: TR + EN her oyunda
2. **Native speaker review** zorunlu Tier 2+ (Google Translate tek başına yasak)
3. **Hardcoded string yasak** — tüm UI text l10n key'li
4. **Text container %30 genişleme toleransı** (TR → DE, AR uzar)
5. **RTL (Arabic, Hebrew) UI flip zorunlu** — FlowDirection `RightToLeft`

## Dil tier öncelik (MGF default)

| Tier | Diller | Launch timing |
|---|---|---|
| 1 | TR (birincil pazar), EN (global) | v1.0 zorunlu |
| 2 | DE, FR, ES, PT-BR | v1.0+30 gün |
| 3 | AR, RU | v1.0+60 gün |
| 4 | JA, KO, ZH-CN, ZH-TW | v1.0+90 gün |
| 5 | ID, TH, VI, FIL | soft launch isteğe bağlı |

## Translation workflow

### 1. String extract
MAUI: `AppResources.resx` + `.csv` export → l10n/strings.en.json

Key naming convention:
- `ui.btn.<name>` — button metinleri
- `ui.label.<name>` — label
- `ui.popup.<id>.title/body/cta` — popup
- `gameplay.quest.<stage_id>.intro/outro/hint1/hint2` — quest metinleri
- `char.<name>.bark.<type>_<num>` — karakter dialog
- `error.<type>` — hata mesajları
- `store.<sku>.name/desc` — IAP

### 2. Çeviri
- **1. tur**: GPT-4 / Claude ile bulk translate (ekonomik + hızlı)
- **2. tur**: native speaker review (ticari agency veya freelance)
  - Tier 2: $0.08–0.12/word
  - Tier 3: $0.10–0.15/word
  - Tier 4: $0.15–0.25/word
- **3. tur**: in-context QA (build'de görsel kontrol)

### 3. Integration
```xml
<!-- MAUI XAML -->
<Button Text="{x:Static loc:AppResources.ui_btn_play}" />
```

```csharp
// IStringLocalizer<T>
public partial class MainMenuViewModel : ObservableObject
{
    [ObservableProperty] string _playText;

    public MainMenuViewModel(IStringLocalizer<MainMenuViewModel> localizer)
    {
        _playText = localizer["ui.btn.play"];
    }
}
```

## Glossary (terminology kilitli)

Her oyunda `l10n/glossary.md` — terminology sabitler.

Örnek Mini Kaşifler:
```
Stone (EN) = Taş (TR)
Wood (EN) = Odun (TR)
Crystal (EN) = Kristal (TR)
Merge (EN) = Birleştir (TR)
Quest (EN) = Görev (TR)
Boost (EN) = Güçlendirici (TR)
```

Bu terim bağlantıları **tüm build boyunca kilitli** — değişirse `glossary.md` güncellenir ve tüm dosyalar re-translate edilir.

## Kültürel adaptasyon

### Date/time
- TR: DD.MM.YYYY, HH:mm (24h)
- EN (US): MM/DD/YYYY, h:mm am/pm
- EN (UK): DD/MM/YYYY, HH:mm
- JA: YYYY/MM/DD
- Stelle **system culture** takip eder (CurrentCulture)

### Currency
- Store otomatik (manuel override **yasak**)
- `Preferences.Get<double>("iap_price")` cache edilmez (her kuruş değişir)

### Numbers
- TR: decimal `,` thousand `.` → 1.234,56
- EN: decimal `.` thousand `,` → 1,234.56
- DE: decimal `,` thousand `.` → 1.234,56

### Gender
- TR: gender-neutral (default OK)
- AR: male/female inflection — dialog duplicate (karakter dişi/erkek)
- ES: "tú" (casual) vs "usted" (formal) — MGF casual = tú
- DE: "du" (casual) vs "Sie" (formal) — MGF casual = du
- JA: keigo dikkatli (casual honorifics)

### Direction
- RTL: AR, HE (Hebrew), FA (Farsi)
- **FlowDirection**: `RightToLeft` — page-level
- Icon arrow/swipe flip
- Progression map: right-to-left flow

### Holidays
Seasonal events region-specific:
- TR: Ramazan, Kurban, Cumhuriyet Bayramı
- US: Halloween, Thanksgiving, 4 July
- JP: Sakura, Obon, Tanabata
- ZH: Spring Festival, Mid-Autumn
- IN: Diwali, Holi
- AR/ME: Ramadan, Eid

### Sensitive
- **Religious references** — neutralize veya skip
- **Political** — yasak (hiçbir pazarda)
- **Alcohol** — child-directed'da yasak
- **Gore/violence** — PEGI 7 limitinde
- **Stereotype** — yasak

## Pseudo-locale testing

`xx-pseudo.json` — string %30 genişletilmiş, accent ile:

```json
{
  "ui.btn.play": "[Plåååy]",
  "ui.btn.settings": "[Sëttíngsssss]"
}
```

UI container overflow test edilir. TR "Ayarlar" (7 char) → AR "الإعدادات" (8 char + font daha geniş) → DE "Einstellungen" (13 char).

## Font / glyph fallback

### Latin (TR, EN, DE, FR, ES, PT)
- Primary: Lilita One (headline) + Inter (body)
- Fallback: System default

### CJK (JA, KO, ZH)
- Noto Sans CJK (JP/KR/TC/SC varyantları)
- Fallback system

### Arabic (AR)
- Noto Naskh Arabic veya Cairo
- RTL support built-in

### Cyrillic (RU)
- Noto Sans Cyrillic veya Google default

### Emoji
- System emoji (iOS/Android farklı — kabul)
- Game icon olarak emoji kullanma (emoji set değişir)

## Text-in-image yasağı
- UI icon'da yazı YASAK (localization değiştiremez)
- Button içi text l10n key ile
- Poster/splash background text'siz olmalı (veya ayrı l10n overlay)

## Store metadata localization
Her pazar için:
- Title (30 char)
- Short description (80 char)
- Long description (4000 char)
- Screenshots with l10n overlay
- Video preview (l10n subtitle)
- Keyword pool

**Tier 1** (TR + EN): v1.0'da
**Tier 2** (DE, FR, ES): v1.0 + 30 gün
Diğer: launch'a göre

## QA checklist
- [ ] TR + EN string bundle complete
- [ ] Glossary terminology uyumlu
- [ ] Pseudo-loc test geçti (overflow yok)
- [ ] RTL (AR) UI flip test
- [ ] Font fallback CJK/AR doğru
- [ ] Culture-specific format (date/number)
- [ ] Hardcoded string taraması (grep "hardcoded TR/EN word" → 0 sonuç)
- [ ] Store metadata TR + EN hazır

## Yasaklar
- Google Translate tek kaynak Tier 2+
- Hardcoded string
- Text-in-image
- Cultural insensitivity
- RTL skip (Arabic için)
- Gender stereotyping
