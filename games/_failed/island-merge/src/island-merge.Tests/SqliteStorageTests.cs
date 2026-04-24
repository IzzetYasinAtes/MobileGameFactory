using IslandMerge.Models;
using IslandMerge.Services;
using Microsoft.Extensions.Logging.Abstractions;

namespace IslandMerge.Tests;

public sealed class SqliteStorageTests : IDisposable
{
    private readonly string _tempDir;

    public SqliteStorageTests()
    {
        _tempDir = Path.Combine(Path.GetTempPath(), "islandmerge-test-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_tempDir);
    }

    public void Dispose()
    {
        try
        {
            if (Directory.Exists(_tempDir))
            {
                Directory.Delete(_tempDir, recursive: true);
            }
        }
        catch
        {
            // best effort
        }
    }

    [Fact]
    public async Task InitializeAsync_FreshDb_CreatesTables()
    {
        var path = Path.Combine(_tempDir, "fresh.db3");
        var storage = new SqliteStorage(NullLogger<SqliteStorage>.Instance, path);

        await storage.InitializeAsync();
        var player = await storage.GetOrCreatePlayerAsync();

        Assert.True(File.Exists(path));
        Assert.NotNull(player);
        Assert.True(player.Id > 0);
    }

    [Fact]
    public async Task InitializeAsync_CorruptDb_RecreatesAndSeeds()
    {
        var path = Path.Combine(_tempDir, "corrupt.db3");

        // Fake corrupt DB: random bytes.
        var garbage = new byte[1024];
        new Random(42).NextBytes(garbage);
        // Gecersiz SQLite header (basta magic string olmaz)
        File.WriteAllBytes(path, garbage);
        Assert.True(File.Exists(path));

        var storage = new SqliteStorage(NullLogger<SqliteStorage>.Instance, path);
        await storage.InitializeAsync();

        // Dosya yeniden olusturuldu mu? Player tablosu calisir mi?
        var player = await storage.GetOrCreatePlayerAsync();
        Assert.NotNull(player);
        Assert.True(player.Id > 0);
        Assert.Equal(100, player.EnergyMax);
    }
}
