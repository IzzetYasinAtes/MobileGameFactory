using Microsoft.Data.Sqlite;

namespace MobileGameFactory.Mcp.Storage;

/// <summary>
/// Tum MCP tool'lari icin tek SQLite baglantisi.
/// Basit ve tek-yazici: tum cagrilar lock ile serilestirilir (MCP stdio tek client).
/// </summary>
public sealed class FactoryDb : IDisposable
{
    private readonly SqliteConnection _conn;
    private readonly object _lock = new();

    private FactoryDb(SqliteConnection conn) => _conn = conn;

    public static FactoryDb Open(string path)
    {
        var dir = Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);

        var cs = new SqliteConnectionStringBuilder
        {
            DataSource = path,
            ForeignKeys = true,
            DefaultTimeout = 10
        }.ToString();

        var conn = new SqliteConnection(cs);
        conn.Open();

        using (var pragma = conn.CreateCommand())
        {
            pragma.CommandText = "PRAGMA journal_mode=WAL; PRAGMA synchronous=NORMAL;";
            pragma.ExecuteNonQuery();
        }

        InitSchema(conn);
        return new FactoryDb(conn);
    }

    public T Read<T>(Func<SqliteConnection, T> op)
    {
        lock (_lock) return op(_conn);
    }

    public T Write<T>(Func<SqliteConnection, T> op)
    {
        lock (_lock)
        {
            using var tx = _conn.BeginTransaction();
            var result = op(_conn);
            tx.Commit();
            return result;
        }
    }

    public static string NowIso() => DateTime.UtcNow.ToString("o");

    private static void InitSchema(SqliteConnection c)
    {
        using var cmd = c.CreateCommand();
        cmd.CommandText = """
            CREATE TABLE IF NOT EXISTS games (
              id          TEXT PRIMARY KEY,
              title       TEXT NOT NULL,
              gate        TEXT NOT NULL DEFAULT 'intake',
              brief       TEXT,
              meta_json   TEXT NOT NULL DEFAULT '{}',
              created_at  TEXT NOT NULL,
              updated_at  TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS messages (
              id          INTEGER PRIMARY KEY AUTOINCREMENT,
              game_id     TEXT,
              from_agent  TEXT NOT NULL,
              to_agent    TEXT NOT NULL,
              type        TEXT NOT NULL,     -- info|handoff|question|escalation|decision
              subject     TEXT NOT NULL,
              body        TEXT NOT NULL,     -- kisa metin; uzun ekler artifacts tablosuna
              created_at  TEXT NOT NULL,
              read_at     TEXT
            );
            CREATE INDEX IF NOT EXISTS idx_msg_inbox ON messages(to_agent, read_at, id);

            CREATE TABLE IF NOT EXISTS logs (
              id          INTEGER PRIMARY KEY AUTOINCREMENT,
              game_id     TEXT,
              agent       TEXT NOT NULL,
              gate        TEXT,
              decision    TEXT NOT NULL,
              why         TEXT,
              created_at  TEXT NOT NULL
            );
            CREATE INDEX IF NOT EXISTS idx_log_game ON logs(game_id, id);

            CREATE TABLE IF NOT EXISTS artifacts (
              id          INTEGER PRIMARY KEY AUTOINCREMENT,
              game_id     TEXT NOT NULL,
              gate        TEXT NOT NULL,
              kind        TEXT NOT NULL,     -- brief|market|design|code|qa|monetization|release
              path        TEXT NOT NULL,
              note        TEXT,
              created_at  TEXT NOT NULL,
              UNIQUE(game_id, kind, path)
            );
            CREATE INDEX IF NOT EXISTS idx_art_game ON artifacts(game_id);
            """;
        cmd.ExecuteNonQuery();
    }

    public void Dispose() => _conn.Dispose();
}
