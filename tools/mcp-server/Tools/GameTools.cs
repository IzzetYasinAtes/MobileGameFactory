using System.ComponentModel;
using System.Text.Json;
using Microsoft.Data.Sqlite;
using ModelContextProtocol.Server;
using MobileGameFactory.Mcp.Storage;

namespace MobileGameFactory.Mcp.Tools;

/// <summary>
/// Oyun kaydi, gate ilerletme, oyun listesi.
/// Kapi sirasi: intake -> research -> design -> build -> qa -> release -> shipped.
/// </summary>
[McpServerToolType]
public static class GameTools
{
    private static readonly string[] Gates =
        ["intake", "research", "design", "build", "qa", "release", "shipped"];

    [McpServerTool(Name = "game_create")]
    [Description("Yeni oyun olustur. id slug-case olmali. Ilk gate 'intake'.")]
    public static string Create(FactoryDb db,
        [Description("Slug (orn: neon-bird-15s)")] string id,
        [Description("Insan okunur baslik")] string title,
        [Description("Sahibin 1-2 cumlelik brief'i")] string brief)
    {
        return db.Write(conn =>
        {
            var now = FactoryDb.NowIso();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = """
                INSERT INTO games (id, title, gate, brief, created_at, updated_at)
                VALUES ($id, $title, 'intake', $brief, $now, $now)
                """;
            cmd.Parameters.AddWithValue("$id", id);
            cmd.Parameters.AddWithValue("$title", title);
            cmd.Parameters.AddWithValue("$brief", brief);
            cmd.Parameters.AddWithValue("$now", now);
            cmd.ExecuteNonQuery();
            return JsonSerializer.Serialize(new { ok = true, id, gate = "intake" });
        });
    }

    [McpServerTool(Name = "game_list")]
    [Description("Oyunlari liste. Opsiyonel gate filtresi.")]
    public static string List(FactoryDb db,
        [Description("Opsiyonel: intake|research|design|build|qa|release|shipped")] string? gate = null)
    {
        return db.Read(conn =>
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = gate is null
                ? "SELECT id, title, gate, updated_at FROM games ORDER BY updated_at DESC"
                : "SELECT id, title, gate, updated_at FROM games WHERE gate=$g ORDER BY updated_at DESC";
            if (gate is not null) cmd.Parameters.AddWithValue("$g", gate);
            using var r = cmd.ExecuteReader();
            var rows = new List<object>();
            while (r.Read())
                rows.Add(new { id = r.GetString(0), title = r.GetString(1), gate = r.GetString(2), updatedAt = r.GetString(3) });
            return JsonSerializer.Serialize(rows);
        });
    }

    [McpServerTool(Name = "game_get")]
    [Description("Oyunun tam kaydi (gate, brief, meta).")]
    public static string Get(FactoryDb db, [Description("Oyun id")] string id)
    {
        return db.Read(conn =>
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT id, title, gate, brief, meta_json, created_at, updated_at FROM games WHERE id=$id";
            cmd.Parameters.AddWithValue("$id", id);
            using var r = cmd.ExecuteReader();
            if (!r.Read()) return JsonSerializer.Serialize(new { ok = false, error = "not_found" });
            return JsonSerializer.Serialize(new
            {
                ok = true,
                id = r.GetString(0),
                title = r.GetString(1),
                gate = r.GetString(2),
                brief = r.IsDBNull(3) ? null : r.GetString(3),
                meta = r.GetString(4),
                createdAt = r.GetString(5),
                updatedAt = r.GetString(6)
            });
        });
    }

    [McpServerTool(Name = "gate_advance")]
    [Description("Oyunu bir sonraki kapiya gecir. PM kullanir. Gecilecek kapi dogrulugu kontrol edilir.")]
    public static string Advance(FactoryDb db,
        [Description("Oyun id")] string gameId,
        [Description("Hedef kapi")] string nextGate)
    {
        if (Array.IndexOf(Gates, nextGate) < 0)
            return JsonSerializer.Serialize(new { ok = false, error = $"invalid_gate:{nextGate}" });

        return db.Write(conn =>
        {
            using var read = conn.CreateCommand();
            read.CommandText = "SELECT gate FROM games WHERE id=$id";
            read.Parameters.AddWithValue("$id", gameId);
            var current = read.ExecuteScalar() as string;
            if (current is null) return JsonSerializer.Serialize(new { ok = false, error = "not_found" });

            var curIdx = Array.IndexOf(Gates, current);
            var nextIdx = Array.IndexOf(Gates, nextGate);
            if (nextIdx != curIdx + 1)
                return JsonSerializer.Serialize(new { ok = false, error = $"must_advance_sequentially:from={current}" });

            using var upd = conn.CreateCommand();
            upd.CommandText = "UPDATE games SET gate=$g, updated_at=$now WHERE id=$id";
            upd.Parameters.AddWithValue("$g", nextGate);
            upd.Parameters.AddWithValue("$now", FactoryDb.NowIso());
            upd.Parameters.AddWithValue("$id", gameId);
            upd.ExecuteNonQuery();

            return JsonSerializer.Serialize(new { ok = true, id = gameId, gate = nextGate });
        });
    }

    [McpServerTool(Name = "game_meta_patch")]
    [Description("meta_json uzerine JSON patch (shallow merge). Anahtar: deger ciftleri.")]
    public static string MetaPatch(FactoryDb db,
        string gameId,
        [Description("Merge edilecek JSON objesi (string)")] string patchJson)
    {
        return db.Write(conn =>
        {
            using var read = conn.CreateCommand();
            read.CommandText = "SELECT meta_json FROM games WHERE id=$id";
            read.Parameters.AddWithValue("$id", gameId);
            if (read.ExecuteScalar() is not string cur)
                return JsonSerializer.Serialize(new { ok = false, error = "not_found" });

            var a = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(cur) ?? new();
            var b = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(patchJson) ?? new();
            foreach (var (k, v) in b) a[k] = v;
            var merged = JsonSerializer.Serialize(a);

            using var upd = conn.CreateCommand();
            upd.CommandText = "UPDATE games SET meta_json=$m, updated_at=$now WHERE id=$id";
            upd.Parameters.AddWithValue("$m", merged);
            upd.Parameters.AddWithValue("$now", FactoryDb.NowIso());
            upd.Parameters.AddWithValue("$id", gameId);
            upd.ExecuteNonQuery();
            return JsonSerializer.Serialize(new { ok = true, meta = merged });
        });
    }
}
