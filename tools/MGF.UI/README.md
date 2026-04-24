# MGF.UI — Shared MAUI component library

Tüm oyunların kullandığı ortak UI katmanı. Tutarlı UX, accessibility, juice pre-wired.

## Kurulum (her oyun için)

```xml
<!-- games/<id>/src/<id>/<id>.csproj -->
<ItemGroup>
    <ProjectReference Include="..\..\..\..\tools\MGF.UI\MGF.UI.csproj" />
</ItemGroup>
```

## Component'ler (v1.0)

### Controls
- `PrimaryButton` — scale + haptic + SFX pre-wired
- `CurrencyPill` — coin/energy display + "+" tap → shop
- `RewardModal` — popup with reward animation
- `ToastService` — singleton, top-banner toast
- `PopupQueue` — oturum başına max 1 modal, priority queue

### Accessibility
- `AccessibilityPrefs` — reduced motion, sound, haptic, color blind toggle

### Juice primitives
- `ScaleButtonBehavior` — attached behavior, button scale pop + haptic
- `ParticleService` — pool-based, ≤80 particle, SkiaSharp draw
- `ScreenShake` — page-level Perlin translation
- `HitStop` — task-based UI freeze

### ResourceDictionary
- `Theme/Palette.xaml` — renk tokens
- `Theme/Typography.xaml` — font stack
- `Theme/Spacing.xaml` — 4/8/16/24/32 grid
- `Theme/Animations.xaml` — easing curves

## Oyun-özel override

Her oyun `Resources/Styles/<id>-theme.xaml` ile override:
```xml
<ResourceDictionary>
    <Color x:Key="PrimaryBrand">#1F7A6C</Color>
    <FontFamily x:Key="HeadlineFont">LilitaOne</FontFamily>
</ResourceDictionary>
```

## Bakım

- **Owner**: Infrastructure agent
- **Versioning**: semver, major değişiklik breaking change
- **Test**: unit test `tools/MGF.UI.Tests/`
- **Branch**: `infra/mgf-ui-<konu>`
