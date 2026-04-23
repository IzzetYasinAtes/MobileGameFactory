using CommunityToolkit.Mvvm.ComponentModel;
using IslandMerge.Services;

namespace IslandMerge.ViewModels;

public sealed partial class SettingsViewModel : BaseViewModel
{
    private readonly IAudio _audio;

    [ObservableProperty]
    private int _volume = 70;

    [ObservableProperty]
    private bool _muted;

    [ObservableProperty]
    private bool _hapticsEnabled = true;

    [ObservableProperty]
    private string _language = "tr";

    public SettingsViewModel(IAudio audio)
    {
        _audio = audio;
        Title = "Ayarlar";
        _volume = audio.Volume;
        _muted = audio.Muted;
    }

    partial void OnVolumeChanged(int value)
    {
        _audio.Volume = value;
    }

    partial void OnMutedChanged(bool value)
    {
        _audio.Muted = value;
    }
}
