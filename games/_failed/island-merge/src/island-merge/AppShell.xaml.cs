using IslandMerge.Views;

namespace IslandMerge;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute("board", typeof(BoardPage));
        Routing.RegisterRoute("biome-select", typeof(BiomeSelectPage));
        Routing.RegisterRoute("shop", typeof(ShopPage));
        Routing.RegisterRoute("settings", typeof(SettingsPage));
        Routing.RegisterRoute("character-select", typeof(CharacterSelectPage));
    }
}
