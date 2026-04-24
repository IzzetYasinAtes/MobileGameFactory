using CommunityToolkit.Mvvm.ComponentModel;
using IslandMerge.Services;

namespace IslandMerge.ViewModels;

public sealed partial class SettingsViewModel : BaseViewModel
{
    private readonly IAudio _audio;
    private readonly ISelectedCharacterStore _characters;

    [ObservableProperty]
    private int _volume = 70;

    [ObservableProperty]
    private bool _muted;

    [ObservableProperty]
    private bool _hapticsEnabled = true;

    [ObservableProperty]
    private string _language = "tr";

    [ObservableProperty]
    private string _selectedCharacterImage = "character_momo.png";

    [ObservableProperty]
    private string _selectedCharacterName = "Momo";

    public SettingsViewModel(IAudio audio, ISelectedCharacterStore characters)
    {
        _audio = audio;
        _characters = characters;
        Title = "Ayarlar";
        _volume = audio.Volume;
        _muted = audio.Muted;
        Refresh();
    }

    public void Refresh()
    {
        var id = _characters.GetSelected();
        SelectedCharacterName = CharacterName(id);
        SelectedCharacterImage = CharacterImage(id);
    }

    partial void OnVolumeChanged(int value)
    {
        _audio.Volume = value;
    }

    partial void OnMutedChanged(bool value)
    {
        _audio.Muted = value;
    }

    private static string CharacterName(CharacterId id) => id switch
    {
        CharacterId.Kasif => "Kasif",
        CharacterId.Lila => "Lila",
        CharacterId.Momo => "Momo",
        CharacterId.Papagan => "Papagan",
        _ => "Momo",
    };

    private static string CharacterImage(CharacterId id) => id switch
    {
        CharacterId.Kasif => "character_kasif.png",
        CharacterId.Lila => "character_lila.png",
        CharacterId.Momo => "character_momo.png",
        CharacterId.Papagan => "character_papagan.png",
        _ => "character_momo.png",
    };
}
