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
}
