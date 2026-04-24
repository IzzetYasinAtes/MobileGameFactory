using IslandMerge.Controls;
using IslandMerge.ViewModels;

namespace IslandMerge.Views;

public partial class BiomeSelectPage : ContentPage
{
    private readonly BiomeSelectViewModel _vm;

    public BiomeSelectPage(BiomeSelectViewModel vm)
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
    }

    private void BiomeBorder_Loaded(object? sender, EventArgs e)
    {
        if (sender is not Border border)
        {
            return;
        }

        if (border.BindingContext is not BiomeCardVm card)
        {
            return;
        }

        if (!card.JustUnlocked)
        {
            return;
        }

        // Tek atislik reveal: tekrar oynatma.
        card.JustUnlocked = false;
        _ = SpriteAnimator.UnlockReveal(border);
    }
}
