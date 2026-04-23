using System.ComponentModel;
using System.Text.Json;
using ModelContextProtocol.Server;
using MobileGameFactory.Mcp.Storage;

namespace MobileGameFactory.Mcp.Tools;

/// <summary>
/// Uretilen dosyanin (brief/market/design/code/qa/...) yerini kayit eder.
/// DB'de dosya icerigi tutulmaz; sadece yol + kisa not.
/// Amac: bir oyuna ait tum ciktilari tek yerden gormek (ops/inbox bagimliligi olmadan).
/// </summary>
[McpServerToolType]
public static class ArtifactTools
{
    private static readonly string[] Kinds =
        ["brief", "market", "design", "code", "qa", "monetization", "release", "asset", "notes"];

    [McpServerTool(Name = "artifact_register")]
    [Description("Uretilen dosyayi oyuna baglayarak kayit et. Dosya icerigi DB'ye yazilmaz.")]
    public static string Register(FactoryDb db,
        [Description("Oyun id")] string gameId,
        [Description("Kapi: intake|research|design|build|qa|release")] string gate,
        [Description($"Tur")] string kind,
        [Description("Repo-goreli yol (orn: games/neon-bird/design.md)")] string path,
        [Description("Kisa not")] string? note = null)
    {
        if (Array.IndexOf(Kinds, kind) < 0)
            return JsonSerializer.Serialize(new { ok = false, error = $"invalid_kind:{kind}" });

        return db.Write(conn =>
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = """
                INSERT INTO artifacts (game_id, gate, kind, path, note, created_at)
                VALUES ($g, $ga, $k, $p, $n, $now)
                ON CONFLICT(game_id, kind, path) DO UPDATE SET
                  gate=excluded.gate,
                  note=excluded.note,
                  created_at=excluded.created_at;
                """;
            cmd.Parameters.AddWithValue("$g", gameId);
            cmd.Parameters.AddWithValue("$ga", gate);
            cmd.Parameters.AddWithValue("$k", kind);
            cmd.Parameters.AddWithValue("$p", path);
            cmd.Parameters.AddWithValue("$n", (object?)note ?? DBNull.Value);
            cmd.Parameters.AddWithValue("$now", FactoryDb.NowIso());
            cmd.ExecuteNonQuery();
            return JsonSerializer.Serialize(new { ok = true });
        });
    }

    [McpServerTool(Name = "artifact_list")]
    [Description("Bir oyuna ait kayitli tum ciktilar (yol + tur + not).")]
    public static string List(FactoryDb db, string gameId)
    {
        return db.Read(conn =>
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = """
                SELECT id, gate, kind, path, note, created_at
                FROM artifacts WHERE game_id=$g
                ORDER BY created_at ASC
                """;
            cmd.Parameters.AddWithValue("$g", gameId);
            using var r = cmd.ExecuteReader();
            var items = new List<object>();
            while (r.Read())
                items.Add(new
                {
                    id = r.GetInt64(0),
                    gate = r.GetString(1),
                    kind = r.GetString(2),
                    path = r.GetString(3),
                    note = r.IsDBNull(4) ? null : r.GetString(4),
                    createdAt = r.GetString(5)
                });
            return JsonSerializer.Serialize(items);
        });
    }
}
