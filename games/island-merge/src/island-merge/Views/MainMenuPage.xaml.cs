using IslandMerge.ViewModels;

namespace IslandMerge.Views;

public partial class MainMenuPage : ContentPage
{
    private readonly MainMenuViewModel _vm;

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
    }
}
