# POSTMORTEM — island-merge (Mini Kaşifler: Kayıp Adanın Sırrı)

**Tarih**: 2026-04-24
**Durum**: BAŞARISIZ. Sistem tarafından tamamlanmış olarak işaretlendi (tag `game/island-merge-v1.0.0`, gate `shipped`) ama gerçekte oyun kabul edilemez kalitede — sahibin vizyonunu karşılamıyor.

## Sahibin vizyonu (brief'teki ekran görüntüsü)
- 2.5D renkli, illustrated karakter sprite'ları (Kaşif, Lila, Momo, Papağan — full body)
- Zengin, painterly 5 ortam (Tropik Orman, Sahil Mağarası, Antik Tapınak, Volkan, Buz)
- Oynanış: harita keşfi, match-3 style game board, tapınak bulmaca odası
- Karakter kostüm varyasyonları
- "Eğlenceli, canlı, profesyonel mobil oyun"

## Teslim edilen
- SkiaSharp ile basit 7×9 grid — renkli dikdörtgen + harf overlay ("T1", "O1", "K1")
- SD-Turbo ile üretilmiş sprite'lar — orta kalite, transparent bg ile
- Statik Idle breath + hover bounce animation
- Text-heavy menü, MAUI default kontrol stil
- Juice, screen-shake, particle, VFX, shader efekt = **yok**

## Kök sebepler

### 1. Oyun motoru seçimi yanlış
**.NET MAUI + SkiaSharp** — enterprise UI framework'ü oyun için değil. Unity, Godot, Cocos2d-x profesyonel mobil oyun için standart. MAUI'da juice ekleyemezsin, shader yazamazsın, GPU-heavy efektler zor. CLAUDE.md'de "Teknoloji: .NET 10 + .NET MAUI" pazarlık konusu değil yazılı — ama bu AAA mobile için yanlış bir kararmış.

### 2. Asset pipeline amatör
- Referans görselden crop → placeholder
- SD-Turbo 4-step CPU → düşük kalite sprite
- Sprite sheet / skeletal rig / frame-by-frame animation yok
- Background parallax yok, UI shader yok

### 3. Game feel eksik
- Tap feedback minimum (tek pop scale)
- Screen shake yok
- Particle burst yok (merge, quest complete, unlock)
- Juicy transitions yok (page change = default Shell slide)
- Sound = placeholder NullAudio, music yok, ambient yok

### 4. UI/UX amatör
- XAML default stil; custom theme yok
- HUD minimum (text label'lar)
- Menu animasyonsuz
- Empty state, loading state, error state = yok veya zayıf
- Dialog, toast, modal sistem yok

### 5. Agent sistemi eksik roller
10 agent şu rollerle sınırlı: PM, Market, Design, MAUI Dev, QA, Monetization, Store, Infra, Asset Designer, Animator. EKSİK:
- **Art Director** (karakter concept, environment concept, art bible)
- **UX/UI Designer** (HUD, menu flow, onboarding, micro-interactions)
- **Sound Designer** (SFX, music, ambient, diegetic feedback)
- **Narrative Designer** (story, dialog, character voice, quest writing)
- **Game Feel / Juice Specialist** (screen shake, particles, VFX, timing)
- **LiveOps Manager** (events, seasons, push notifications)
- **Growth / UA Specialist** (ASO, creative testing, LTV modeling)
- **Data Analyst** (funnel, retention, ARPU, cohort)
- **Localization** (TR + EN + MENA + LATAM)
- **Community Manager** (Discord, reviews, bug reports)

### 6. Rule dosyaları eksik
Mevcut: coding, maui, performance, monetization, testing, logging. EKSİK:
- art-direction (style guide, palette, iconography)
- game-feel (juice checklist, tap feedback, screen shake)
- ux-principles (onboarding flow, micro-interactions, loading states)
- narrative (character voice, dialog style, quest writing)
- sound (SFX palette, music structure, diegetic layering)
- growth (UA creative guidelines, ASO keywords)
- live-ops (season cadence, event templates)
- analytics (event taxonomy, funnel definitions)
- localization (terminology, cultural adaptation)
- platform-specific (Android/iOS store requirements)

### 7. Skill'ler eksik
Mevcut 6 skill brief üretimine odaklı. Profesyonel production'da gerekli:
- character-design-sprint
- environment-design-sprint
- art-bible
- ui-wireframe
- animation-spec
- sound-design-brief
- narrative-beat
- aso-sprint
- creative-ad-spec
- live-ops-plan
- post-launch-optimization
- tutorial-design

### 8. MCP / plugin boşluğu
Sadece factory MCP var. Profesyonel pipeline için gerekli:
- Asset library entegrasyonu (Freepik, OpenGameArt, Figma Community)
- Analytics mock
- Store Console API (metadata automation)
- A/B test planner

### 9. Test zayıf
QA uçtan uca test yapmadı ilk turda; kod review bazlı GO verdi. Runtime'da ise karakter seçimi, Oyna butonu navigation, merge drag-drop hepsi kırıktı. Windows target'ta touch event dönüştürmesi test edilmedi. Real device test yok.

### 10. Çok hızlı ship
Owner 2 gün sürdü ama "ship" kararı verildi. Oysa:
- Görsel QA yok (sprite kalite, animasyon akıcı mı)
- UX QA yok (onboarding anlaşılır mı, flow doğal mı)
- Performance QA yok (frame time, memory — sadece kod review)
- Beta test yok (10+ kullanıcı flow gözlemi)

## Alınan dersler (v2 sistem için)

1. **Motor seçimi oyun türüne göre**: Casual puzzle + illustrated → Unity veya Godot. MAUI sadece ultra-basit text/puzzle için.
2. **Asset pipeline profesyonel**: Concept → sketch → finalized sprite → rig → sheet. SD sadece concept/placeholder, final sprite için artist veya premium AI (Midjourney v6 + ControlNet).
3. **Game feel = ürün**: juice, timing, feedback oyunun kendisidir — son dakika eklenemez, başından itibaren tasarlanır.
4. **UX ayrı disiplin**: game design ≠ UI design. Ayrı agent, ayrı skill.
5. **QA runtime-first**: kod review ikincil; uçtan uca runtime test birincil. Tester agent gerçek cihazda oynar.
6. **Ship kriteri sert**: demo video, 10 kullanıcı beta, store listing önizleme olmadan ship yok.
7. **Cross-platform ≠ otomatik**: "Windows'ta da çalışır" demek Windows-specific UX test edilecek demek.

## Proje durumu

- `game/island-merge-v1.0.0` tag (tutulur, tarihsel kayıt)
- Kod/asset/doc `games/_failed/island-merge/` altında arşivlendi
- MCP'de game_meta_patch ile "failed" bayrağı konulacak
- Reform sonrası v2 sistemde **ilk oyun**: yeni brief geldiğinde sıfırdan, yeni pipeline ile üretilecek

## Aksiyon öğeleri (sistem tarafı)

- [ ] Agent listesi yeni rollerle genişletilecek
- [ ] Rule dosyaları 10+ yeni MD ile genişletilecek
- [ ] Skill'ler 12+ yeni playbook ile genişletilecek
- [ ] CLAUDE.md anayasası baştan yazılacak
- [ ] Template'ler profesyonel game dev pipeline'ı için yenilenecek
- [ ] MCP + plugin + library araştırması
- [ ] Motor tavsiyesi: Unity-first veya Godot-first, MAUI ultra-casual için opsiyon
