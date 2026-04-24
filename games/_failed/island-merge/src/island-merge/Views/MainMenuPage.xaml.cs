using IslandMerge.Controls;
using IslandMerge.ViewModels;

namespace IslandMerge.Views;

public partial class MainMenuPage : ContentPage
{
    private readonly MainMenuViewModel _vm;
    private CancellationTokenSource? _idleCts;

    public MainMenuPage(MainMenuViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_vm.LoadCommand.CanExecute(null))
        {
            await _vm.LoadCommand.ExecuteAsync(null);
        }

        // Idle breath — secili karakter portrait'i sonsuz nefes.
        _idleCts?.Cancel();
        _idleCts = new CancellationTokenSource();
        _ = SpriteAnimator.IdleBreathAnimation(SelectedPortrait, _idleCts.Token);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _idleCts?.Cancel();
        _idleCts?.Dispose();
        _idleCts = null;
        SpriteAnimator.StopAll(SelectedPortrait);
    }
}
