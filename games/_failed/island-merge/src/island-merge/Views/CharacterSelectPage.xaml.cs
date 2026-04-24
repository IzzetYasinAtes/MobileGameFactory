using IslandMerge.Controls;
using IslandMerge.ViewModels;

namespace IslandMerge.Views;

public partial class CharacterSelectPage : ContentPage
{
    private CancellationTokenSource? _bounceCts;

    public CharacterSelectPage(CharacterSelectViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        _bounceCts?.Cancel();
        _bounceCts = new CancellationTokenSource();
        var ct = _bounceCts.Token;

        // 4 karakter esanli bounce — faz farki (0/225/450/675 ms).
        _ = SpriteAnimator.HoverBounceAnimation(PortraitKasif, 0, ct);
        _ = SpriteAnimator.HoverBounceAnimation(PortraitLila, 225, ct);
        _ = SpriteAnimator.HoverBounceAnimation(PortraitMomo, 450, ct);
        _ = SpriteAnimator.HoverBounceAnimation(PortraitPapagan, 675, ct);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _bounceCts?.Cancel();
        _bounceCts?.Dispose();
        _bounceCts = null;
        SpriteAnimator.StopAll(PortraitKasif);
        SpriteAnimator.StopAll(PortraitLila);
        SpriteAnimator.StopAll(PortraitMomo);
        SpriteAnimator.StopAll(PortraitPapagan);
    }
}
