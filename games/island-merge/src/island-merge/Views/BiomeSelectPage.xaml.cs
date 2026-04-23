using IslandMerge.ViewModels;

namespace IslandMerge.Views;

public partial class BiomeSelectPage : ContentPage
{
    public BiomeSelectPage(BiomeSelectViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
