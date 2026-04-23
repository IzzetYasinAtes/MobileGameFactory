using System.ComponentModel;
using System.Text.Json;
using ModelContextProtocol.Server;
using MobileGameFactory.Mcp.Storage;

namespace MobileGameFactory.Mcp.Tools;

/// <summary>
/// Kapi-seviyesi kararlar icin append-only log. Chatter icin kullanma.
/// Her agent gate kapanisinda TEK log kaydi atar (batch davranisi).
/// </summary>
[McpServerToolType]
public static class LogTools
{
    [McpServerTool(Name = "log_append")]
    [Description("Kapi kararini kaydet. 'decision' zorunlu, 'why' kisa gerekce. Bir kapi icin agent basina 1 cagri yeterli.")]
    public static string Append(FactoryDb db,
        [Description("Kaydeden agent adi")] string agent,
        [Description("Karar ozeti (<= 160 karakter)")] string decision,
        [Description("Kisa gerekce")] string? why = null,
        [Description("Opsiyonel oyun id")] string? gameId = null,
        [Description("Opsiyonel kapi: intake|research|design|build|qa|release|shipped")] string? gate = null)
    {
        return db.Write(conn =>
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = """
                INSERT INTO logs (game_id, agent, gate, decision, why, created_at)
                VALUES ($g, $a, $ga, $d, $w, $now);
                SELECT last_insert_rowid();
                """;
            cmd.Parameters.AddWithValue("$g", (object?)gameId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("$a", agent);
            cmd.Parameters.AddWithValue("$ga", (object?)gate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("$d", decision);
            cmd.Parameters.AddWithValue("$w", (object?)why ?? DBNull.Value);
            cmd.Parameters.AddWithValue("$now", FactoryDb.NowIso());
            var id = (long)cmd.ExecuteScalar()!;
            return JsonSerializer.Serialize(new { ok = true, id });
        });
    }

    [McpServerTool(Name = "log_tail")]
    [Description("Son N karar log'unu getir. Filtre: gameId ve/veya agent.")]
    public static string Tail(FactoryDb db,
        int limit = 20,
        string? gameId = null,
        string? agent = null)
    {
        return db.Read(conn =>
        {
            var where = new List<string>();
            if (gameId is not null) where.Add("game_id=$g");
            if (agent is not null) where.Add("agent=$a");
            var sql = "SELECT id, game_id, agent, gate, decision, why, created_at FROM logs"
                      + (where.Count > 0 ? " WHERE " + string.Join(" AND ", where) : "")
                      + " ORDER BY id DESC LIMIT $l";
            using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            if (gameId is not null) cmd.Parameters.AddWithValue("$g", gameId);
            if (agent is not null) cmd.Parameters.AddWithValue("$a", agent);
            cmd.Parameters.AddWithValue("$l", Math.Clamp(limit, 1, 200));
            using var r = cmd.ExecuteReader();
            var items = new List<object>();
            while (r.Read())
                items.Add(new
                {
                    id = r.GetInt64(0),
                    gameId = r.IsDBNull(1) ? null : r.GetString(1),
                    agent = r.GetString(2),
                    gate = r.IsDBNull(3) ? null : r.GetString(3),
                    decision = r.GetString(4),
                    why = r.IsDBNull(5) ? null : r.GetString(5),
                    createdAt = r.GetString(6)
                });
            return JsonSerializer.Serialize(items);
        });
    }
}
