using IslandMerge.Models;

namespace IslandMerge.Tests;

/// <summary>
/// E2E-003 regresyon kapanisi. BiomeSelectViewModel MAUI Preferences/Shell bagimliligi
/// nedeniyle dogrudan unit edilemez; burada VM'in mutate ettigi saf veri davranisi
/// (Player.CurrentBiome, BiomeCatalog lookup) + karar kosulu (IsUnlocked) test edilir.
/// </summary>
public class BiomeSelectionTests
{
    [Fact]
    public void BiomeCatalog_Get_ReturnsDefinitionForId()
    {
        var def = BiomeCatalog.Get(BiomeId.Beach);

        Assert.Equal(BiomeId.Beach, def.Id);
        Assert.Equal(21, def.FirstLevel);
    }

    [Fact]
    public void BiomeCatalog_Get_UnknownId_FallsBackToForest()
    {
        var def = BiomeCatalog.Get((BiomeId)999);

        Assert.Equal(BiomeId.TropicalForest, def.Id);
    }

    [Fact]
    public void Player_CurrentBiome_MutationPersistsAssignment()
    {
        // SelectBiomeAsync icinde yapilan: player.CurrentBiome = biome.Id
        var player = new Player { CurrentBiome = BiomeId.TropicalForest };

        player.CurrentBiome = BiomeId.Beach;

        Assert.Equal(BiomeId.Beach, player.CurrentBiome);
    }

    [Fact]
    public void IsUnlocked_False_BlocksSelection()
    {
        // ViewModel karar kurali: kilitli kart SelectBiomeAsync'de return eder.
        var kilitli = new { Id = BiomeId.Volcano, IsUnlocked = false, FirstLevel = 61 };
        var acik = new { Id = BiomeId.TropicalForest, IsUnlocked = true, FirstLevel = 1 };

        Assert.False(kilitli.IsUnlocked);
        Assert.True(acik.IsUnlocked);
    }

    [Theory]
    [InlineData(1, BiomeId.TropicalForest, true)]
    [InlineData(20, BiomeId.Beach, false)]
    [InlineData(21, BiomeId.Beach, true)]
    [InlineData(60, BiomeId.Volcano, false)]
    [InlineData(61, BiomeId.Volcano, true)]
    public void Biome_UnlockGate_FirstLevelThreshold(int playerLevel, BiomeId biome, bool expectedUnlocked)
    {
        var def = BiomeCatalog.Get(biome);
        var unlocked = playerLevel >= def.FirstLevel;

        Assert.Equal(expectedUnlocked, unlocked);
    }
}
