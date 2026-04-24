using Microsoft.Extensions.Logging;

namespace IslandMerge.Services;

/// <summary>
/// MAUI-side factory: FileSystem.AppDataDirectory cozumlemesi icin.
/// Test projesine link'lenmez (Maui reference yok).
/// </summary>
public partial class SqliteStorage
{
    public SqliteStorage(ILogger<SqliteStorage> logger)
        : this(logger, Path.Combine(FileSystem.AppDataDirectory, "islandmerge.db3"))
    {
    }
}
