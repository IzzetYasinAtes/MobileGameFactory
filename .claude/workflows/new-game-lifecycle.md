# Workflow: Yeni Oyun Yaşam Döngüsü

PM'in her oyun için izlediği 7 kapılı üretim hattı.

## Kapı diyagramı

```
[intake] -> [research] -> [design] -> [build] -> [qa] -> [release] -> [shipped]
   PM        Market       Designer    MAUI Dev    QA       Store/Release  PM
```

Paralel koşabilen: `monetization` build + qa ile eş zamanlı; `mcp-infrastructure` gerektiğinde yan yolda.

## Kapı 1: Intake
**Tetik**: `/new-game "<fikir>"`
**Agent**: PM
**Skill**: `game-intake`
**Girdi**: sahibin 1-2 cümle fikri.
**Çıktı**: `docs/games/<id>/brief.md`, DB kaydı.
**Done**: `gate_advance("research")` + `log_append`.

## Kapı 2: Research
**Agent**: market-analyst
**Skill**: `research-sprint`
**Girdi**: brief.md
**Çıktı**: `docs/games/<id>/market.md` (400–600 kelime).
**Done kriteri**:
- 3 rakip teardown.
- Fit önerisi.
- Diferansiyasyon (3 madde).
**PM onayı**: Rakip sayısı 3 ✓, fit önerisi somut ✓ → `gate_advance("design")`.

## Kapı 3: Design
**Agent**: game-designer
**Skill**: `design-brief`
**Girdi**: brief.md + market.md
**Çıktı**: `docs/games/<id>/design.md` (600–900 kelime).
**Done kriteri**:
- Core loop 1 cümle.
- Progression + difficulty tanımlı.
- Monetization noktaları iskeleti (detay monetization agent'da).
- Retention hook 3 adet.
**PM onayı**: Session süresi 60–180 s ✓, açık soru ≤2 ✓ → `gate_advance("build")`.

## Kapı 4: Build (paralel: Monetization entegrasyon notu)
**Agent**: maui-developer (primary) + monetization (yan)
**Skill**: —
**Girdi**: design.md
**Çıktı**: `src/<id>/` MAUI solution, Android Release build yeşil, unit testler yeşil.
**Done kriteri**:
- `dotnet build -f net10.0-android -c Release` ✓.
- xUnit testler ✓.
- `IAdService`, `IIapService` stub'larıyla entegrasyon hazır.
- Monetization agent'tan entegrasyon notu alındıysa uygulandı.
**PM onayı**: Build yeşil ✓, test yeşil ✓ → `gate_advance("qa")`.

## Kapı 5: QA (paralel: Monetization denetimi)
**Agent**: qa-tester (primary) + monetization (denetim)
**Skill**: `qa-pass` + `monetization-audit`
**Girdi**: build + design.md
**Çıktı**: `qa.md` + `monetization.md`.
**Done kriteri**:
- qa.md: GO kararı.
- Smoke + edge + perf ölçümleri PASS.
- P0 bug = 0, P1 bug ≤ 1.
- monetization.md: kurallara uyum ✓.
**Bug varsa**: developer'a geri dön, döngü kapanana kadar `gate_advance` yok.
**PM onayı**: GO ✓ → `gate_advance("release")`.

## Kapı 6: Release
**Agent**: store-release
**Skill**: `release-prep`
**Girdi**: qa.md GO + final build.
**Çıktı**: `release.md` (500–800 kelime) + asset checklist.
**Done kriteri**:
- ASO blok tamam.
- Asset checklist'te her satır ✓ (veya net "eksik" işaret).
- Privacy policy URL set.
- Windows→iOS gerçeği netleşmiş: Android ready + iOS status (ready|blocked).
**PM onayı**: Android ready ✓ → `gate_advance("shipped")`.

## Kapı 7: Shipped
**Agent**: PM
**Aksiyonlar**:
- `git tag -a game/<id>-v1.0.0`.
- Sahibe ship özeti (Android path, iOS status, store submission komutları).
- `log_append(gate="shipped", decision="v1.0.0", why="qa GO + release hazır")`.

## Hata durumları
- Agent blocked → `message_send(to="project-manager", type="escalation", ...)`. PM 3 saniye düşün, karar ver.
- Kapı fail → önceki kapıya geri dönüş yoktur; mevcut kapıda düzelt ve tekrar dene. Gerekirse `game_meta_patch` ile kayıt.
- Infra bug → mcp-infrastructure agent'a yönlendir; oyun state'i aynı kalır.

## Token disiplini
- Her agent, kendi kapısında **tek** artifact + **tek** log çağırır.
- Subagent prompt'ları ≤200 kelime rapor sınırlı.
- Detay doküman dosyada; MCP mesajı sadece özet.
