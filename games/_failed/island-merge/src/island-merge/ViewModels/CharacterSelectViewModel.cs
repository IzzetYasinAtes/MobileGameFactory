using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IslandMerge.Services;

namespace IslandMerge.ViewModels;

public sealed partial class CharacterSelectViewModel : BaseViewModel
{
    private readonly ISelectedCharacterStore _store;

    public ObservableCollection<CharacterCardVm> Characters { get; } = new();

    [ObservableProperty]
    private CharacterId _selected = CharacterId.Momo;

    public CharacterSelectViewModel(ISelectedCharacterStore store)
    {
        _store = store;
        Title = "Bir karakter sec";
        Characters.Add(new CharacterCardVm
        {
            Id = CharacterId.Kasif,
            Name = "Kasif",
            Description = "Cesur kasif",
            ImageSource = "character_kasif.png",
        });
        Characters.Add(new CharacterCardVm
        {
            Id = CharacterId.Lila,
            Name = "Lila",
            Description = "Nazik kasif",
            ImageSource = "character_lila.png",
        });
        Characters.Add(new CharacterCardVm
        {
            Id = CharacterId.Momo,
            Name = "Momo",
            Description = "Pars yavrusu",
            ImageSource = "character_momo.png",
        });
        Characters.Add(new CharacterCardVm
        {
            Id = CharacterId.Papagan,
            Name = "Papagan",
            Description = "Rehber papagan",
            ImageSource = "character_papagan.png",
        });

        Selected = _store.HasSelection ? _store.GetSelected() : CharacterId.Momo;
        SyncIsSelected();
    }

    [RelayCommand]
    public async Task ChooseAsync(CharacterCardVm card)
    {
        Selected = card.Id;
        _store.SetSelected(card.Id);
        SyncIsSelected();
        await Shell.Current.GoToAsync("//main").ConfigureAwait(true);
    }

    private void SyncIsSelected()
    {
        foreach (var c in Characters)
        {
            c.IsSelected = c.Id == Selected;
        }
    }
}

public partial class CharacterCardVm : ObservableObject
{
    [ObservableProperty]
    private CharacterId _id;

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private string _description = string.Empty;

    [ObservableProperty]
    private string _imageSource = string.Empty;

    [ObservableProperty]
    private bool _isSelected;
}
