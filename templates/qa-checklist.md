# QA Checklist — {{title}}

**ID**: `{{id}}`
**Kapı**: qa
**Tester**: qa-tester
**Build**: {{version + commit sha}}

## Smoke (10 dk)
- [ ] Cold start ≤ 2 s.
- [ ] Ana menü açılıyor.
- [ ] Core loop başlat → 1 tur tamamla.
- [ ] Sonuç ekranı + restart.
- [ ] Settings: ses/titreşim toggle.
- [ ] Uçak modu: crash yok.
- [ ] Background → foreground: state korunuyor.

## Functional
| Akış | PASS | NOTES |
|---|---|---|
| Menu navigation | ☐ | |
| Core loop — win | ☐ | |
| Core loop — lose | ☐ | |
| Rewarded ad (test unit) | ☐ | |
| Interstitial gösterim koşulu | ☐ | 4 dk cooldown doğrulama |
| IAP sandbox — remove ads | ☐ | |
| IAP sandbox — cosmetic | ☐ | |
| Settings değişiklik kalıcılığı | ☐ | |

## Edge case
- [ ] Uçak modu açıkken rewarded ad → fallback çalışıyor.
- [ ] Dolu disk: save fail, crash yok.
- [ ] 30 dk background sonrası: session korundu / resume akışı.
- [ ] Ekran dönüşü (landscape varsa).
- [ ] Çağrı / SMS interrupt ortasında oyun.
- [ ] Düşük memory (`adb shell am send-trim-memory <pkg> RUNNING_CRITICAL`).
- [ ] Cihaz tarih ileri/geri alındı (streak).

## Performance (en az 1 low-end cihaz)
| Metrik | Hedef | Ölçüm | PASS |
|---|---|---|---|
| Cold start (3 ortalaması) | ≤ 2.0 s | | ☐ |
| İdle FPS | 60 | | ☐ |
| 10 dk sonu memory peak | ≤ 250 MB | | ☐ |
| Battery 10 dk drain | ≤ %5 | | ☐ |
| APK/AAB boyutu | ≤ 40 MB | | ☐ |

## Cihaz matrisi
| Cihaz | OS | PASS | Notlar |
|---|---|---|---|
| Android low-end (3 GB RAM) | API 24+ | ☐ | |
| Android mid (6 GB RAM) | API 33+ | ☐ | |
| Android tablet | API 30+ | ☐ | |
| iOS iPhone SE (Mac varsa) | iOS 15+ | ☐ | |
| iOS iPhone 14 | iOS 17+ | ☐ | |

## Bug listesi

### P0 (ship blocker)
> P0 varsa NO-GO.
```
## P0-001: <başlık>
- Repro: 1) ... 2) ... 3) ...
- Beklenen: ...
- Gözlenen: ...
- Cihaz: ...
- Atama: maui-developer
- Durum: open
```

### P1 (önemli)
> 2+ P1 varsa NO-GO.

### P2 (nice-to-have)

## Sonuç

**Karar**: GO / NO-GO

**Gerekçe**: {{tek paragraf, rakam kanıtlı}}

**Eylem**:
- GO → store-release agent'a handoff.
- NO-GO → bug listesi developer'a, ne zaman yeni QA turu yapılacağını PM belirler.
