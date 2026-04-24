namespace IslandMerge.Services;

public enum CharacterId
{
    None = 0,
    Kasif = 1,
    Lila = 2,
    Momo = 3,
    Papagan = 4,
}

/// <summary>
/// Hafif kullanici tercihi: secilen karakter. Preferences ile kalici.
/// PII degil; kimlik degil; SQLite'a gerek yok.
/// </summary>
public interface ISelectedCharacterStore
{
    bool HasSelection { get; }

    CharacterId GetSelected();

    void SetSelected(CharacterId character);
}

public sealed class SelectedCharacterStore : ISelectedCharacterStore
{
    private const string PrefKey = "selected_character";

    public bool HasSelection => Preferences.Default.ContainsKey(PrefKey);

    public CharacterId GetSelected()
    {
        var value = Preferences.Default.Get(PrefKey, (int)CharacterId.None);
        return Enum.IsDefined(typeof(CharacterId), value) ? (CharacterId)value : CharacterId.None;
    }

    public void SetSelected(CharacterId character) =>
        Preferences.Default.Set(PrefKey, (int)character);
}
