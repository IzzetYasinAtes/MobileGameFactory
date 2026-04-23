---
name: release-prep
description: Store/Release agent'ın release kapısında izlediği playbook. Metadata, asset, submission adımları, Windows→iOS gerçeğini yönetir.
---

# Skill: release-prep

## Ön koşul
- qa.md GO.
- `.claude/rules/monetization.md` + `docs/store/android-publish.md` + `docs/store/ios-publish.md` okundu.

## Adımlar

### 1. ASO blok
- **Title** (Android 30 char / iOS 30 char): keyword + marka çağrışımı.
- **Short description** (Android 80 char).
- **Long description** (4000 char): 3 paragraf; ilk 200 char kritik (preview alanında görünür).
- **iOS keywords** (100 char, virgülle).
- **Kategori**: Games → Subcategory.
- **Tags**: 5 adet.

Keyword araştırması: benzer rakip (market.md'den) + Google Play keyword tool notu.

### 2. Asset checklist
- App icon 1024×1024 PNG (adaptive için Android foreground/background).
- Feature graphic 1024×500 (Android).
- Screenshots:
  - Android phone: 4 min, 16:9 ya da 9:16.
  - Android tablet: 2 min.
  - iOS 6.5": 4 min.
  - iOS 5.5": 4 min (backward compat, opsiyonel).
- Promo video 30 s (opsiyonel, önerilir).

Her asset için `games/<id>/release.md` içinde path sütunu.

### 3. Legal / policy
- Privacy policy: `games/<id>/privacy.md` → host edilecek (GitHub Pages, Netlify free, vb).
- Data safety (Android) / Privacy nutrition (iOS) formu — SDK başına topladığı veri işaretle.
- Age rating questionnaire — tek seferlik.
- Terms of service (opsiyonel, küçük oyun için privacy yeterli).

### 4. Submission adımları (platform ayrık)
**Android**:
1. `keytool` ile upload keystore.
2. `dotnet publish -f net10.0-android -c Release -p:AndroidPackageFormats=aab`.
3. Play Console → Yeni uygulama → Internal testing → Production.
4. Metadata yükle, asset yükle.
5. Rollout stratejisi: Internal → Closed beta (opsiyonel) → %10 staged rollout → %100.

**iOS** (Mac varsa):
1. `dotnet publish -f net10.0-ios -c Release` (Mac üzerinde).
2. `xcodebuild archive` + Xcode Organizer upload.
3. App Store Connect → TestFlight → Submit for Review.
4. Review süresi 1–3 gün.

### 5. Windows gerçeği
Mac erişimi yoksa `release.md` içinde:
- Android: ship-ready.
- iOS: blocked; çözüm önerileri (cloud Mac, vb).
- `game_meta_patch(gameId, '{"iosStatus":"blocked_mac_needed"}')`.

### 6. Launch notları
- Tarih hedefi.
- Hangi bölgeler (önerim: EN + TR öncelik, diğer dilleri store auto-translate).
- Fiyatlandırma (free + IAP).
- İlk 7 gün monitoring planı.

### 7. release.md yaz
Path: `games/<id>/release.md` — `templates/release-checklist.md`'den.
Uzunluk 500–800 kelime.

### 8. Kapı kapanış
```
artifact_register(gameId, gate="release", kind="release", path="games/<id>/release.md")
message_send(to="project-manager", type="handoff", subject="release hazır", body="<android ready + ios status + eksik asset sayısı>")
log_append(agent="store-release", gate="release", gameId=<id>, decision="android ship-ready", why="<eksik varsa belirt>")
```

## Yasaklar
- Privacy policy'siz submit.
- "Asset sonra eklenecek" ile ship-ready demek.
- Mac olmadan iOS ship iddiası.

## Done
release.md kayıtlı + PM'e tek handoff + net durum (Android ready / iOS status).
