using System.ComponentModel;
using System.Text.Json;
using ModelContextProtocol.Server;
using MobileGameFactory.Mcp.Storage;

namespace MobileGameFactory.Mcp.Tools;

/// <summary>
/// Agent'lar arasi kucuk, yapili mesajlar. Dosya yerine SQLite satiri.
/// body kisa tutulmalidir (ozet). Uzun artefaktlar icin ArtifactTools kullanilir.
/// </summary>
[McpServerToolType]
public static class MessageTools
{
    private static readonly string[] Types =
        ["info", "handoff", "question", "escalation", "decision"];

    [McpServerTool(Name = "message_send")]
    [Description("Hedef agent inbox'ina mesaj birak. body <= 400 karakter onerilir.")]
    public static string Send(FactoryDb db,
        [Description("Gonderen agent (orn: game-designer)")] string from,
        [Description("Alici agent (orn: project-manager)")] string to,
        [Description("Mesaj turu: info|handoff|question|escalation|decision")] string type,
        [Description("Kisa konu basligi")] string subject,
        [Description("Kisa govde (ozet + aksiyon/karar). Uzun icerik artifact_register ile baglansin.")] string body,
        [Description("Opsiyonel oyun id")] string? gameId = null)
    {
        if (Array.IndexOf(Types, type) < 0)
            return JsonSerializer.Serialize(new { ok = false, error = $"invalid_type:{type}" });

        return db.Write(conn =>
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = """
                INSERT INTO messages (game_id, from_agent, to_agent, type, subject, body, created_at)
                VALUES ($g, $f, $t, $ty, $s, $b, $now);
                SELECT last_insert_rowid();
                """;
            cmd.Parameters.AddWithValue("$g", (object?)gameId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("$f", from);
            cmd.Parameters.AddWithValue("$t", to);
            cmd.Parameters.AddWithValue("$ty", type);
            cmd.Parameters.AddWithValue("$s", subject);
            cmd.Parameters.AddWithValue("$b", body);
            cmd.Parameters.AddWithValue("$now", FactoryDb.NowIso());
            var id = (long)cmd.ExecuteScalar()!;
            return JsonSerializer.Serialize(new { ok = true, id });
        });
    }

    [McpServerTool(Name = "inbox_pop")]
    [Description("Agent'in okunmamis mesajlarini al ve read_at=now olarak isaretle. En eski ilk.")]
    public static string Pop(FactoryDb db,
        [Description("Agent adi")] string agent,
        [Description("Maks mesaj sayisi (default 20)")] int limit = 20)
    {
        return db.Write(conn =>
        {
            using var sel = conn.CreateCommand();
            sel.CommandText = """
                SELECT id, game_id, from_agent, type, subject, body, created_at
                FROM messages
                WHERE to_agent=$a AND read_at IS NULL
                ORDER BY id ASC
                LIMIT $l
                """;
            sel.Parameters.AddWithValue("$a", agent);
            sel.Parameters.AddWithValue("$l", Math.Clamp(limit, 1, 100));
            using var r = sel.ExecuteReader();
            var ids = new List<long>();
            var items = new List<object>();
            while (r.Read())
            {
                var id = r.GetInt64(0);
                ids.Add(id);
                items.Add(new
                {
                    id,
                    gameId = r.IsDBNull(1) ? null : r.GetString(1),
                    from = r.GetString(2),
                    type = r.GetString(3),
                    subject = r.GetString(4),
                    body = r.GetString(5),
                    createdAt = r.GetString(6)
                });
            }
            r.Close();

            if (ids.Count > 0)
            {
                using var upd = conn.CreateCommand();
                upd.CommandText = $"UPDATE messages SET read_at=$now WHERE id IN ({string.Join(",", ids)})";
                upd.Parameters.AddWithValue("$now", FactoryDb.NowIso());
                upd.ExecuteNonQuery();
            }
            return JsonSerializer.Serialize(items);
        });
    }

    [McpServerTool(Name = "inbox_peek")]
    [Description("Inbox'i read_at isaretlemeden goruntule. Sayac/teshis icin.")]
    public static string Peek(FactoryDb db, string agent, int limit = 20)
    {
        return db.Read(conn =>
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = """
                SELECT id, game_id, from_agent, type, subject, created_at,
                       CASE WHEN read_at IS NULL THEN 0 ELSE 1 END AS read_flag
                FROM messages WHERE to_agent=$a
                ORDER BY id DESC LIMIT $l
                """;
            cmd.Parameters.AddWithValue("$a", agent);
            cmd.Parameters.AddWithValue("$l", Math.Clamp(limit, 1, 100));
            using var r = cmd.ExecuteReader();
            var items = new List<object>();
            while (r.Read())
                items.Add(new
                {
                    id = r.GetInt64(0),
                    gameId = r.IsDBNull(1) ? null : r.GetString(1),
                    from = r.GetString(2),
                    type = r.GetString(3),
                    subject = r.GetString(4),
                    createdAt = r.GetString(5),
                    read = r.GetInt32(6) == 1
                });
            return JsonSerializer.Serialize(items);
        });
    }
}
