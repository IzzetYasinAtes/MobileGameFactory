# Growth & ASO Kuralları

## Sert kurallar
1. **D7 ROAS ≥ 8%** (payback ≤90 gün) — aksi durumda UA durdurulur
2. **CPI ≤ $0.25 casual** / ≤ $1.50 hybrid (ülke ve kanala göre ayarlanır)
3. **Creative refresh**: haftalık 5 varyant test, winner scale
4. **Misleading creative YASAK** (gameplay'de olmayan özellik göstermek)
5. **ASO keyword density**: long description'da ana keyword **1-2%** (stuffing yasak)

## ASO (App Store Optimization)

### Title (brand + 1-2 keyword)
- **Play Store**: 30 char, 50 char tam (ilk 30 görünür)
- **App Store**: 30 char
- Örnek: "Mini Kaşifler: Ada Macerası" (27 char) — brand + "macera" keyword

### Short description (Play Store, 80 char)
- Oyun tipi + value prop + CTA
- Örnek: "Merge, keşfet, çöz! Gizemli adada eğlenceli macera seni bekliyor. Oyna!"

### Long description (Play Store, 4000 char)
Yapı:
1. Hook paragraf (3 satır, emoji opsiyonel)
2. Özellik listesi (bullet, 6–10 madde)
3. Sosyal kanıt (star rating, download, press quote)
4. What's new (son sürüm highlight)
5. Gizlilik + support link

### Keyword strategy
- **Primary 5** (genre + core mechanic): merge, match, puzzle, adventure, casual
- **Secondary 10** (brand + theme + feature): island, explorer, quest, relaxing, offline, free, colorful, family, kids-friendly, cute
- **Long-tail 15** (niche + localized): "merge oyunu türkçe", "offline puzzle oyunu", "reklamsız merge", "ada macera oyunu"

### Her pazar için ASO ayrı optimize
TR, EN, DE, FR, ES, AR, JA, ZH-CN — her biri için native keyword research (Sensor Tower, AppMagic).

### Icon
- **512×512** Play, **1024×1024** App Store
- **A/B test 2 varyant** (hero character closeup vs emblem)
- **Silhouette testi**: 48px thumbnail'de okunur
- **Palette**: brand ana renk %60+ alan
- **Style**: art-bible.md ile tutarlı

### Screenshots
- **5–8 screenshot** her dil
- **Mobile portrait** (1080×1920 Play, iPhone 6.5" 1242×2688 App Store)
- **A/B test 2 set**: "gameplay focus" vs "feature focus"
- İlk 3 screenshot kritik (user scroll'da ilk 3'ü görür)
- Text overlay: kısa, 3-5 kelime, localized

### Video preview
- **15–30 saniye**, ilk 3 sn hook
- Landscape (Play) + Portrait (App Store)
- Gameplay %70+ göster, UI + story %30
- Sub-caption (video mute edilir, metin şart)
- Call-to-action sonunda: "Şimdi Oyna"

## UA (User Acquisition)

### Channel mix
| Channel | Share | Strength | Weakness |
|---|---|---|---|
| Facebook Ads | 40% | Targeting + lookalike | CPI artış |
| TikTok | 30% | Viral potential + young audience | Creative burn fast |
| Unity Ads | 15% | Gameplay network (in-game) | Limited targeting |
| AppLovin MAX | 10% | Hybrid casual reach | Quality variable |
| IronSource | 5% | Rewarded network | Limited CPI control |

### Creative formats
- **Playable** (50%): ad içinde interactive demo, CVR en yüksek
- **Video** (30%): 15–30s, hook + gameplay + CTA
- **Static** (20%): banner + single image, low-budget test

### Testing protocol
- **Haftalık 5 creative varyant**
- **3 gün test**: CPI + CTR ölçüm
- **Winner** (CPI alt %30 altında): scale up
- **Loser**: kill + rationale log
- **Creative burn**: 2 hafta sonra refresh (%30 yeni, %70 winner pool)

### Bid strategy
- **Target CPI**: casual TR $0.08, US $0.35; hybrid TR $0.50, US $2.00
- **Target CPA** (install + D7 active): target CPI × 1.5
- **Budget**: $50/creative/day başlangıç, scale factor 2×/gün performansa göre

### Lookalike audiences
- **Installer LAL 1-3%**: broad reach
- **Payer LAL 0.5-1%**: quality optimization
- **Whale LAL** (top 5% spender): premium quality
- **Retention LAL** (D30 active): engagement optimization

### Geo-targeting
- **Tier 1** (birincil): TR (TR market), US/UK/DE
- **Tier 2** (value): LATAM (BR, MX, AR), SEA (PH, ID, TH, VN)
- **Tier 3** (test/soft launch): NZ, AU, CA

## Metric targets

| Metric | Ship blocker | Target | Stretch |
|---|---|---|---|
| D1 retention | <30% | ≥40% | ≥50% |
| D7 retention | <10% | ≥15% | ≥25% |
| D30 retention | <5% | ≥8% | ≥15% |
| ARPDAU | <$0.05 | ≥$0.15 | ≥$0.50 |
| Payer conversion | <1% | ≥2.5% | ≥5% |
| D7 ROAS | <4% | ≥8% | ≥15% |
| CPI (casual TR) | >$0.25 | ≤$0.15 | ≤$0.08 |

## Soft launch plan (opsiyonel)

### Ülke seçimi
- **Primary test**: TR (birincil pazar olduğu için feedback kalitesi yüksek)
- **Alt test**: PH / ID / VN (düşük CPI, metric temeli)
- **Tertiary**: CA / NZ / AU (EN speaker ama US değil — US UA bulaşmaz)

### Süre
- 4–8 hafta
- Hedef: metric doğrulama + crash-free + qualitative feedback

### Kill kriter
- D1 < 30% veya D7 < 10% → kill veya major rework
- Crash-free < 99% → tech blocker, kill veya ertele

## Launch checklist (T-minus 4 hafta)
- [ ] Store metadata TR + EN + 2 ek dil
- [ ] 5+ screenshot her dil
- [ ] Feature graphic + icon 2 varyant A/B
- [ ] Video preview 15-30s TR/EN
- [ ] Press kit (1 sayfa PDF)
- [ ] Launch trailer YouTube (60-90s)
- [ ] 5+ creative varyant hazır (UA)
- [ ] Analytics tracking (Firebase + Appsflyer)
- [ ] Privacy policy URL aktif
- [ ] Data Safety Google Play dolduruldu
- [ ] Content rating IARC
- [ ] Keystore production signed
- [ ] 20 tester closed testing (Google 2024 kuralı — 12 gün)

## Yasaklar
- **Misleading creative** (oyunda olmayan gameplay göster) → Play/App Store ban
- **Fake reviews** (Google detect + ban)
- **Incentivized install farm** (policy ihlali)
- **Cross-promo kid-directed** (Families Policy)
- **Targeting <13 olmadan COPPA flag** (child-directed reklam SDK)
- **CPI cost <-> LTV mismatch** (ROAS negative sürdürme)
