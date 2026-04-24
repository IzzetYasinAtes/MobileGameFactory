# Profesyonel Mobil Oyun Üretim Pipeline'ı — Research

**Tarih:** 2026-04-23
**Kapsam:** Supercell, Playrix, King, Voodoo, Metacore pipeline'ları; takım rolleri; milestone'lar; engine karşılaştırması; art/sound pipeline; ship kalite çıtası.
**Amaç:** MobileGameFactory sisteminin (tek sahip + 8 agent + .NET MAUI) endüstri standartlarıyla hizalanması.

---

## 1. Stüdyo Pipeline Modelleri

### 1.1 Supercell — "Kill early, kill often"
Supercell 5 hit oyun için **30+ projeyi öldürmüş**; son 10 oyunun 7'si prototipte, 2'si soft launch'ta kesildi, sadece Clash Royale global çıktı. Kill kararı üst yönetimde değil, **oyunu yapan takımda**. 2025'te Mo.co için **invite-only soft launch** denendi — motive oyuncu tabanı toplamak için.
- Kaynak: [Quality is worth killing for — Game Developer](https://www.gamedeveloper.com/business/quality-is-worth-killing-for-supercell-s-ruthless-approach-to-production)
- Kaynak: [Mo.co and Supercell's New Launch Philosophy — Naavik](https://naavik.co/digest/mo-co-and-supercells-new-launch-philosophy/)

### 1.2 Playrix — Modular live ops makinesi
Gardenscapes/Homescapes gibi live title'larda **modüler asset sistemi + versiyonlu content pipeline**. Sürekli canlı güncelleme mimarisi; match-3 türünü narrative meta-layer ile yeniledi.
- Kaynak: [Playrix — Wikipedia](https://en.wikipedia.org/wiki/Playrix)
- Kaynak: [Kwalee's Playrix-style plan — MobileGamer.biz](https://mobilegamer.biz/kwalee-gets-serious-about-casual-games-with-playrix-style-plan-to-rejuvenate-stale-genres/)

### 1.3 King — 14 yıllık live ops
Candy Crush **65 level ile çıktı, bugün 21,000+ level**. Live event'ler retention motoru. Head of Live Ops Eva Ryott: data + qualitative feedback denge.
- Kaynak: [Inside Live Ops at King — Gamesforum](https://www.globalgamesforum.com/media/inside-live-ops-at-king-how-candy-crush-stays-fresh-after-14-years)
- Kaynak: [How King runs live events — PocketGamer.biz](https://www.pocketgamer.biz/interview/66696/how-king-runs-live-in-game-events-in-candy-crush-saga/)

### 1.4 Voodoo — Prototip fabrikası
**Yılda ~1,000 prototip test**, %0.4'ü (≈4 oyun) full launch, 1 tane hit olur. KPI eşiği: **D1 retention, D7 retention, CPI ~$0.20-0.25**. Go/no-go karar **günler içinde** verilir.
- Kaynak: [Voodoo's Secret Sauce — Voodoo](https://voodoo.io/news/voodoo-s-secret-sauce-from-0-to-250m-hybridcasual-revenue-in-3-years)
- Kaynak: [Why Most Games Deserve to Die — Deconstructor of Fun](https://www.deconstructoroffun.com/blog/2025/06/02/how-voodoo-builds-hits-four-product-lessons-from-a-2000-prototypes-a-year-machine)

### 1.5 Metacore — Faz bazlı takım genişlemesi
Merge Mansion **$700M+ revenue, 5 yıl**. Model: **Faz 1** 2-3 kişilik ekip market fit arar; doğrulanırsa **Faz 2** scale takımı kurulur (6 ay content). Helsinki + Berlin.
- Kaynak: [Metacore's Merge Mansion $700M — GamesMarket](https://www.gamesmarket.global/fifth-anniversary-metacores-merge-mansion-with-more-than-dollar700-million-in-revenue-after-five-years-d637f659709480d20a95acfaaf3e5b0b/)
- Kaynak: [Metacore scaled Merge Mansion — Liftoff](https://liftoff.ai/blog/metacore-merge-mansion-live-event-strategy/)

---

## 2. Takım Rolleri

| Rol | Ne yapar | MGF karşılığı |
|---|---|---|
| **Producer** | Program, bütçe, cross-team koordinasyon, ship sorumluluğu | Project Manager |
| **Game Designer** | Core loop, progression, difficulty, economy | Game Designer |
| **Art Director** | Visual identity, style guide, asset kalite onayı | (yok — Designer + Dev) |
| **Tech Artist** | Art ↔ engine köprüsü, shader, optimize, rig | (yok — MAUI Dev üstlenir) |
| **UI/UX Designer** | Ekran akışı, onboarding, friction çıkarma | Game Designer alt görevi |
| **Engineer** | Gameplay + tools + platform entegrasyonu | MAUI Developer |
| **QA** | Test matrisi, bug, device lab, regression | QA Tester |
| **Data Analyst** | KPI dashboard, A/B, retention cohort | Infrastructure (sınırlı) |
| **UA Manager** | Paid acquisition, CPI optimize, creative test | Store/Release (sınırlı) |
| **ASO Specialist** | Store listing, keyword, screenshot A/B | Store/Release |
| **Community Manager** | Discord/Reddit, patch notu, kullanıcı feedback | (yok) |
| **Narrative Designer** | Story, dialog, world-building | (küçük oyun gereksiz) |
| **Monetization Designer** | Ad placement, IAP economy, LTV | Monetization |

Kaynak: [Key roles in a game development team — Pingle Studio](https://pinglestudio.com/blog/key-roles-in-a-game-development-team-2024-edition) · [ASO Specialist — GameBiz Consulting](https://gamebizconsulting.com/aso-specialist)

---

## 3. Milestone'lar

| Milestone | Tanım | MGF kapısı |
|---|---|---|
| **Concept** | 1-sayfa pitch, referans, hedef kitle | Intake |
| **Prototype** | Core mechanic çalışıyor, placeholder art | Design çıktısı |
| **Vertical Slice** | Tek level, final kalitede art+sound+mechanic. Ship'in küçük önizlemesi | Build alt-gate |
| **Alpha** | Feature-complete, content eksik olabilir. Başından sonuna oynanabilir | Build sonu |
| **Beta** | Content-complete, optimize + bug fix fazı, external test | QA |
| **Soft Launch** | 2-5 ülkede canlı; D1/D7/CPI ölç; kill/scale karar | Release hazırlık |
| **Global Launch** | Tüm marketlerde açık, UA kampanyası full | Ship |
| **Live Ops** | Event, update, season, live balance | (v1.0 sonrası) |

Kaynak: [9 stages of game production — Game World Observer (Tim Cain)](https://gameworldobserver.com/2023/11/22/game-production-stages-prototype-alpha-beta-ship-tim-cain) · [Prototypes & Vertical Slice — Rami Ismail](https://ltpf.ramiismail.com/prototypes-and-vertical-slice/)

---

## 4. Engine Tercihleri — ve .NET MAUI Gerçeği

### 4.1 Endüstri standartları (2025-2026)

| Engine | Sweet spot | Build boyutu | Notlar |
|---|---|---|---|
| **Unity** | Casual → mid-core her şey, en geniş ecosystem, ad SDK'larla yerleşik | 25-60 MB | Pazar %60+ pay. Royalty drama sonrası güven sarsıldı ama hâlâ default. |
| **Godot 4** | 2D casual, puzzle, indie. Royalty-free, açık kaynak | 120-160 MB baseline | 2D render güçlü; C# veya GDScript. |
| **Cocos2d-x / Cocos Creator** | Çok hafif 2D casual, özellikle Asya pazarı | 40-70 MB | Native C++/TypeScript; küçük binary kralı. |
| **Defold** | Hyper-casual, puzzle; Foundation destekli, royalty-free | 35-60 MB | Lua-based; analytics/IAP yerleşik. |

Kaynak: [Best Mobile Game Engines 2025 — SunStrike](https://sunstrikestudios.com/en/the_best_mobile_game_engines_in_2025) · [Best 2D Game Engines 2025 — Polydin](https://polydin.com/best-2d-game-engines/) · [Unity Alternatives 2026 — Udonis](https://www.blog.udonis.co/mobile-marketing/mobile-games/unity-alternatives)

### 4.2 .NET MAUI + SkiaSharp — Gerçekçi değerlendirme

**Ne iyi:**
- SkiaSharp GPU-accelerated 2D rendering (SKGLView) — 60 FPS için yeterli teorik tavan.
- C# + .NET 10 ecosystem; xUnit, CommunityToolkit.Mvvm gibi olgun tooling.
- Tek codebase Android + iOS + Windows + macOS.
- Kaynak: [SkiaSharp in .NET MAUI — skiasharp.com](https://skiasharp.com/how-to-use-skiasharp-in-net-maui-for-stunning-cross-platform-graphics/)

**Ne tehlikeli (ship blocker olabilecek):**
1. **Ad SDK eksikliği**: AdMob Android'de native binding üzerinden çalışıyor ama test case'ler Unity kadar oturmuş değil. IAP tarafı Play Billing + StoreKit 2 için **manuel binding yazma** ihtimali yüksek.
2. **Cold start şişmesi**: MAUI runtime + .NET bootstrap ~200-400 ms ek yük; `<2s cold start` hedefi (bkz. performance.md) agresif, AOT + trim şart.
3. **Game dev ekosistemi yok**: Unity Asset Store muadili yok; particle, tween, rig tooling sıfırdan kurulur.
4. **Animation/rig pipeline**: Spine/DragonBones runtime'ları MAUI/Skia için resmi değil; topluluk portları sınırlı.
5. **Profiler zayıf**: Unity Profiler yerine `dotnet-trace` + Android Studio Profiler parça parça birleştiriliyor.

**Verdict:** .NET MAUI **sadece 2D, küçük ölçekli, custom-rendered, oyun-loop-light** projeler için uygun — puzzle, word, idle-clicker, turn-based card gibi. Reflex-heavy action, particle-yoğun, rig'li karakter, AR/3D oyunlar için **Unity veya Godot** gerekir. MGF anayasası "küçük, polished, 60-180s session" dediği için **marjinal uygun** ama ekosistem friction pahalıya patlar. Her oyun fikri engine-fit açısından filtrelenmeli.

Kaynak: [SkiaSharp performance — skiasharp.com](https://skiasharp.com/can-skiasharp-improve-mobile-app-ui-performance-on-android-and-ios/) · [SkiaSharp in 2025 — skiasharp.com](https://skiasharp.com/what-is-skiasharp-and-why-should-net-developers-use-it-in-2025/)

---

## 5. Art Pipeline

1. **Concept art** — style exploration, mood board, referans. Karakter/çevre design explore.
2. **Sprite / 3D asset** — 2D'de final sprite sheet veya modüler parçalar; 3D'de model + UV + texture.
3. **Rig** — dijital iskelet; skin weighting bone etki dağılımını belirler.
4. **Animation** — keyframe + timing; idle, action, transition.
5. **VFX** — particle, flash, screen-shake, hit feedback.
6. **Shader** — material tanımı (metal, glass, holographic); stylized look.
7. **Integration** — engine'e import, material atama, collision, lighting tuning.

Kaynak: [Game Art Pipeline — Pixune](https://pixune.com/blog/game-art-pipeline/) · [Navigating Game Art Production Pipeline — Ixie Gaming](https://www.ixiegaming.com/blog/navigating-the-game-art-production-pipeline/)

**MGF uygulaması:** Küçük oyun için concept → sprite atlas → minimal rig → basit tween animation → Skia particle → shader yerine SKPaint efektleri. Rig + VFX adımları çoğu oyunda atlanabilir.

---

## 6. Sound Pipeline

- **SFX library**: Sonniss GameAudioGDC (ücretsiz, royalty-free), BOOM Library, A Sound Effect — mobil oyun kategorileri hazır paketler (pop, whoosh, coin, victory, UI click).
- **Music composition**: 30-60s loop, mono veya stereo 96kbps; menu + gameplay + tension varyasyonu.
- **Ambient**: arka plan doku; gameplay ekranında düşük volume.
- **Diegetic feedback**: her tap/combo/damage karşılığı net SFX — *juice* kaynağı.
- **Middleware**: Wwise/FMOD büyük oyunda; küçük MAUI oyununda **doğrudan `IAudio` service + MediaElement/Plugin.Maui.Audio** yeterli.
- **Pipeline hatası**: ses tasarımını son haftaya bırakmak — pre-production'da audio identity tanımı şart.

Kaynak: [Game audio pipeline — A Sound Effect](https://www.asoundeffect.com/game-audio-pipeline/) · [9 Sound Design Tips — GameAnalytics](https://www.gameanalytics.com/blog/9-sound-design-tips-to-improve-your-games-audio) · [GameAudioGDC — Sonniss](https://sonniss.com/gameaudiogdc/)

---

## 7. Ship Kalite Çıtası — Metrikler

### Teknik
- **Cold start ≤ 2.0s** mid-range Android release (MGF performance.md ile uyumlu).
- **60 FPS sabit**, 95p ≥ 55 FPS.
- **Memory ≤ 250 MB peak**, idle ≤ 180 MB.
- **APK/AAB ≤ 40 MB**.
- **Battery drain < 5% / 10dk** orta cihaz.

### Juice & Feel (Jonasson/Purho, GDC 2012)
- Her input → görsel + ses feedback (wobble, squirt, bounce, cute noise).
- Maksimum output, minimum input.
- Particle + screen-shake + tween, boring breakout'u bile satılır hale getirir.
- Kaynak: [Juice It or Lose It — GDC Vault](https://www.gdcvault.com/play/1016487/Juice-It-or-Lose) · [Video — YouTube](https://www.youtube.com/watch?v=Fy0aCDmgnxg)

### Oyuncu KPI (Voodoo benchmark)
- **D1 retention ≥ 35%** casual; **≥ 40%** hyper-casual tavan.
- **D7 retention ≥ 10-15%**.
- **CPI ≤ $0.25** hyper-casual; ≤ $1.50 casual.
- Soft launch'ta bu eşikler tutmuyorsa → kill (Supercell disiplini).

### Polish checklist
- Tüm smoke test maddeleri geçer (bkz. testing.md).
- P0 bug yok, ≤1 P1.
- Onboarding ≤ 30s kullanıcıyı core loop'a sokar.
- Store screenshot'lar A/B test edilmiş 2+ varyant.

---

## 8. MGF Sistemine Çıkarımlar

1. **Supercell kill disiplinini taklit et**: her gate'te explicit kill kriteri olsun, PM "sunk cost" etkisine girmesin. Soft launch olmadığı için prototype + QA kapılarında sert filtre.
2. **Voodoo hız**: prototip → go/no-go günler, ay değil. MGF'de "Design" kapısı 1 oturumda biter; bu hız korunsun.
3. **Metacore faz modeli**: küçük oyun = küçük agent yükü; hit sinyali gelirse live ops'a adanmış takım (yeni agent) oluşturmayı düşün.
4. **Playrix modülerliği**: her oyunun `games/<id>/` klasörü modüler asset sistemi zaten içeriyor; content versiyonlama için git tag yeterli.
5. **Engine gerçeği**: .NET MAUI + SkiaSharp puzzle/idle/card için çalışır; action/3D fikri gelirse **Unity çıkış rampası** (farklı repo, farklı pipeline) planı olmalı — CLAUDE.md'deki "yalnız MAUI" kısıtı bazı oyun fikirlerini peşinen eler.
6. **Eksik rol uyarısı**: Data Analyst ve Community Manager yok; bu local-only + backend-yok mimaride kasıtlı. UA ve Live Ops de sınırlı — küçük oyun ölçeğinde kabul edilebilir trade-off.
7. **Juice bütçesi**: her MAUI Developer build'i, ship öncesi "Jonasson-Purho juice pass" — particle + sound + screen-shake eklensin; polish kapısı ayrı olmasa da QA alt-kriteri yapılabilir.

---

## Kaynaklar (toplu)

- [Quality is worth killing for — Game Developer](https://www.gamedeveloper.com/business/quality-is-worth-killing-for-supercell-s-ruthless-approach-to-production)
- [Mo.co launch philosophy — Naavik](https://naavik.co/digest/mo-co-and-supercells-new-launch-philosophy/)
- [Voodoo's Secret Sauce — Voodoo](https://voodoo.io/news/voodoo-s-secret-sauce-from-0-to-250m-hybridcasual-revenue-in-3-years)
- [How Voodoo Builds Hits — Deconstructor of Fun](https://www.deconstructoroffun.com/blog/2025/06/02/how-voodoo-builds-hits-four-product-lessons-from-a-2000-prototypes-a-year-machine)
- [Voodoo Purple Diver prototype — GameAnalytics](https://www.gameanalytics.com/blog/prototype-phases-for-hit-casual-game-purple-diver-voodoo)
- [Playrix — Wikipedia](https://en.wikipedia.org/wiki/Playrix)
- [Inside Live Ops at King — Gamesforum](https://www.globalgamesforum.com/media/inside-live-ops-at-king-how-candy-crush-stays-fresh-after-14-years)
- [How King runs live events — PocketGamer.biz](https://www.pocketgamer.biz/interview/66696/how-king-runs-live-in-game-events-in-candy-crush-saga/)
- [Metacore Merge Mansion $700M — GamesMarket](https://www.gamesmarket.global/fifth-anniversary-metacores-merge-mansion-with-more-than-dollar700-million-in-revenue-after-five-years-d637f659709480d20a95acfaaf3e5b0b/)
- [Metacore live event strategy — Liftoff](https://liftoff.ai/blog/metacore-merge-mansion-live-event-strategy/)
- [Key roles — Pingle Studio](https://pinglestudio.com/blog/key-roles-in-a-game-development-team-2024-edition)
- [ASO Specialist — GameBiz Consulting](https://gamebizconsulting.com/aso-specialist)
- [Tim Cain 9 stages — Game World Observer](https://gameworldobserver.com/2023/11/22/game-production-stages-prototype-alpha-beta-ship-tim-cain)
- [Prototypes & Vertical Slice — Rami Ismail](https://ltpf.ramiismail.com/prototypes-and-vertical-slice/)
- [Best Mobile Game Engines 2025 — SunStrike](https://sunstrikestudios.com/en/the_best_mobile_game_engines_in_2025)
- [Unity Alternatives 2026 — Udonis](https://www.blog.udonis.co/mobile-marketing/mobile-games/unity-alternatives)
- [SkiaSharp in .NET MAUI — skiasharp.com](https://skiasharp.com/how-to-use-skiasharp-in-net-maui-for-stunning-cross-platform-graphics/)
- [SkiaSharp performance — skiasharp.com](https://skiasharp.com/can-skiasharp-improve-mobile-app-ui-performance-on-android-and-ios/)
- [Game Art Pipeline — Pixune](https://pixune.com/blog/game-art-pipeline/)
- [Game art production pipeline — Ixie Gaming](https://www.ixiegaming.com/blog/navigating-the-game-art-production-pipeline/)
- [Game audio pipeline — A Sound Effect](https://www.asoundeffect.com/game-audio-pipeline/)
- [Sonniss GameAudioGDC](https://sonniss.com/gameaudiogdc/)
- [Juice It or Lose It — GDC Vault](https://www.gdcvault.com/play/1016487/Juice-It-or-Lose)
- [Juice It or Lose It video — YouTube](https://www.youtube.com/watch?v=Fy0aCDmgnxg)
