using IslandMerge.ViewModels;

namespace IslandMerge.Views;

public partial class ShopPage : ContentPage
{
    private readonly ShopViewModel _vm;

    public ShopPage(ShopViewModel vm)
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
