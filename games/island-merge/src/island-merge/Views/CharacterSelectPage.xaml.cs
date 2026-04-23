using IslandMerge.ViewModels;

namespace IslandMerge.Views;

public partial class CharacterSelectPage : ContentPage
{
    public CharacterSelectPage(CharacterSelectViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
