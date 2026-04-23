using IslandMerge.ViewModels;

namespace IslandMerge.Views;

public partial class BoardPage : ContentPage
{
    private readonly BoardViewModel _vm;

    public BoardPage(BoardViewModel vm)
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
