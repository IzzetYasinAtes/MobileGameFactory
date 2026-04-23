# Release Checklist — {{title}}

**ID**: `{{id}}`
**Kapı**: release
**Hedef sürüm**: v1.0.0

## ASO

### Title + short description
- **Title (≤30 char)**: {{}}
- **Short description (≤80 char, Android)**: {{}}
- **Subtitle (≤30 char, iOS)**: {{}}

### Long description (≤4000 char)
```
{{3 paragraf; ilk 200 karakter kritik}}
```

### Keywords
- **Android**: description'da doğal dilde 2-3 keyword tekrarı.
- **iOS keywords (100 char, virgülle)**: {{kw1,kw2,kw3}}

### Kategori
- **Android**: Games → {{subcategory}}
- **iOS**: Games → {{primary}}, {{secondary}}

## Asset checklist

| Asset | Boyut | Path | Durum |
|---|---|---|---|
| App icon | 1024×1024 PNG | | ☐ |
| Feature graphic (Android) | 1024×500 | | ☐ |
| Phone screenshot 1 (Android) | 16:9 veya 9:16 | | ☐ |
| Phone screenshot 2 | | | ☐ |
| Phone screenshot 3 | | | ☐ |
| Phone screenshot 4 | | | ☐ |
| Tablet screenshot (Android) | 7"/10" | | ☐ |
| iOS 6.5" screenshot | 1284×2778 | | ☐ |
| iOS 5.5" screenshot (legacy) | 1242×2208 | | ☐ |
| Promo video (opsiyonel) | 30 s YouTube | | ☐ |

## Legal / policy

- [ ] Privacy policy URL: {{https://... }}
- [ ] Data safety formu doldu (Android).
- [ ] Privacy nutrition labels (iOS).
- [ ] Age rating questionnaire (her iki store).
- [ ] Terms of service (opsiyonel).
- [ ] Ads declaration: Yes.
- [ ] IAP declaration: Yes (varsa).

## Build hazırlığı

### Android
- [ ] versionCode artırıldı.
- [ ] AAB build yeşil (`dotnet publish -f net10.0-android -c Release -p:AndroidPackageFormats=aab`).
- [ ] Keystore güvenli yerde.
- [ ] Play App Signing etkin.

### iOS (Mac gerekli)
- [ ] ApplicationVersion + DisplayVersion artırıldı.
- [ ] Dist certificate + provisioning profile güncel.
- [ ] Archive + IPA yeşil.
- [ ] TestFlight 7 günlük tamam.

## Windows → iOS durumu
- **Mac erişimi**: {{var / yok / cloud}}
- **iOS ship planı**: {{ready / backlog}}
- **Meta kaydı**: `game_meta_patch(gameId, '{"iosStatus":"ready"|"blocked_mac_needed"}')`

## Submission adımları

### Android (Play Console)
1. Yeni release → Internal testing.
2. AAB yükle.
3. Release notes (TR + EN, ≤500 char).
4. Rollout: Internal → Production (%10 → %25 → %100).

### iOS (App Store Connect) — Mac varsa
1. New Version → 1.0.0.
2. Build seç (archive yüklü).
3. Metadata + screenshot + keywords.
4. Submit for Review.

## Launch plan
- **Hedef tarih**: {{YYYY-MM-DD}}
- **Bölgeler**: {{EN + TR öncelik / global}}
- **Fiyat**: Free.
- **İlk 7 gün monitoring**:
  - Crash rate < %1.
  - D1 retention ≥ %30.
  - Rating ≥ 4.3.
  - Yorumlarda P0 sinyali var mı.

## Son kontrol (PM imzası)
- [ ] qa.md = GO.
- [ ] monetization.md kurallara uyumlu.
- [ ] Privacy policy link çalışıyor.
- [ ] Android AAB imzalı ve test edildi.
- [ ] iOS status netleşti (ready veya backlog).
- [ ] Release notes yazıldı.
- [ ] `git tag -a game/<id>-v1.0.0`.

**Karar**: SHIP / DELAY
