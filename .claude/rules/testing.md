# Test Kuralları

## Kapsam katmanları
1. **Unit test** (xUnit) — pure C# core loop, formüller, state machines.
2. **Integration test** — SQLite CRUD, service composition (DI container).
3. **Smoke test** — manuel, her platform build sonrası; 5 dk.
4. **Device matrix test** — QA kapısında; en az 2 Android cihaz (1 low-end, 1 mid).
5. **Release candidate test** — release kapısında; tam QA checklist.

## Unit test hedefi
- Core loop mantığı, skor hesabı, difficulty formülü, save/load — %80+ coverage.
- UI katmanı coverage hedefi yok (smoke yeterli).

## Test adlandırma
```csharp
[Fact]
public void CalculateScore_ComboThreeOrMore_AppliesMultiplier() { ... }
```
Desen: `Method_State_Expected`.

## Cihaz matrisi
| Öncelik | Cihaz profili | Neden |
|---|---|---|
| P0 | Android low-end (3 GB RAM, API 24) | Performans tavanı |
| P0 | Android mid (6 GB RAM, API 33+) | Çoğunluk |
| P1 | Android tablet | UI ölçek |
| P1 | iOS iPhone SE 2. gen | iOS low-end proxy |
| P2 | iOS iPhone 14 | Modern iOS |

## Smoke checklist (her build)
- [ ] Cold start 2 s altında.
- [ ] Ana menü → core loop → geri.
- [ ] 1 full run + skor kaydı.
- [ ] Ses/titreşim ayar değişikliği.
- [ ] Uçak modu — crash yok, offline davranış.
- [ ] Arka plana alma + geri dönüş (state korundu mu?).
- [ ] Ekran dönüşü (landscape destekleniyorsa).

## Edge case kütüphanesi (ship öncesi)
- Dolu disk (yeni save fail etmeli, ama uygulama crash etmemeli).
- SQLite corruption simülasyonu (DB silindi → otomatik re-init).
- Rewarded ad cache yok (offline fallback).
- IAP receipt doğrulama fail.
- Cihaz tarih ayarı geri alındı (day-streak hesabı).
- Low memory (android `onTrimMemory`) altında state korunuyor mu.

## Bug formatı (qa.md içinde)
```
## P0-001: <kısa başlık>
- **Repro**: 1) ... 2) ... 3) ...
- **Beklenen**: ...
- **Gözlenen**: ...
- **Cihaz**: Pixel 5 / Android 13
- **Atama**: maui-developer
- **Durum**: open
```

## GO/NO-GO kriteri
- P0 bug = **NO-GO**.
- 2+ P1 bug = **NO-GO** (tek P1 negotiable).
- Performans hedeflerinden biri aşıldı = **NO-GO**.
- Smoke checklist'te fail = **NO-GO**.
