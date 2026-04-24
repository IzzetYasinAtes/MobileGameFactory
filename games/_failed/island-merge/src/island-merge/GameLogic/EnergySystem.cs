using IslandMerge.Models;

namespace IslandMerge.GameLogic;

/// <summary>
/// Saf enerji hesabi — clock'u disariya enjekte et (test edilebilir).
/// L1-15: cooldown = 0 (her tuketim aninda dolar).
/// L16+: cooldown = 90s per point, cap = 100.
/// </summary>
public static class EnergySystem
{
    public static bool HasCooldown(int level) => level >= BoardConstants.EnergyBarrierLevel;

    /// <summary>
    /// Enerji dusur. Level 1-15 otomatik geri doldugu icin net sonuc degisimi 0 olabilir.
    /// </summary>
    public static int Consume(Player player, int cost, long nowUtc)
    {
        if (cost <= 0)
        {
            return player.Energy;
        }

        if (!HasCooldown(player.CurrentLevel))
        {
            // Bariyer yok: enerji dusur ve aninda tam dolu'ya don.
            player.Energy = player.EnergyMax;
            player.LastEnergyRefillUtc = nowUtc;
            return player.Energy;
        }

        player.Energy = Math.Max(0, player.Energy - cost);
        if (player.Energy < player.EnergyMax && player.LastEnergyRefillUtc == 0)
        {
            player.LastEnergyRefillUtc = nowUtc;
        }

        return player.Energy;
    }

    /// <summary>
    /// Cooldown'a bagli pasif rejenerasyon. Maks 100 sabit.
    /// </summary>
    public static int Regenerate(Player player, long nowUtc)
    {
        if (!HasCooldown(player.CurrentLevel))
        {
            player.Energy = player.EnergyMax;
            player.LastEnergyRefillUtc = nowUtc;
            return player.Energy;
        }

        if (player.Energy >= player.EnergyMax)
        {
            player.LastEnergyRefillUtc = nowUtc;
            return player.Energy;
        }

        var elapsed = nowUtc - player.LastEnergyRefillUtc;
        if (elapsed <= 0)
        {
            return player.Energy;
        }

        var gained = (int)(elapsed / BoardConstants.EnergyCooldownSeconds);
        if (gained <= 0)
        {
            return player.Energy;
        }

        var newEnergy = Math.Min(player.EnergyMax, player.Energy + gained);
        var consumedSeconds = gained * BoardConstants.EnergyCooldownSeconds;
        player.Energy = newEnergy;
        player.LastEnergyRefillUtc += consumedSeconds;

        if (player.Energy >= player.EnergyMax)
        {
            player.LastEnergyRefillUtc = nowUtc;
        }

        return player.Energy;
    }

    /// <summary>Rewarded ad veya IAP araciligiyla direkt enerji ekler (cap 100 asabilir mi? hayir: cap'e kadar).</summary>
    public static int Grant(Player player, int amount)
    {
        if (amount <= 0)
        {
            return player.Energy;
        }

        player.Energy = Math.Min(player.EnergyMax, player.Energy + amount);
        return player.Energy;
    }

    /// <summary>IAP 500 enerji paketi cap'i asar (dizayn karari: paket, extra kredi olarak kalir).</summary>
    public static int GrantOvercap(Player player, int amount)
    {
        if (amount <= 0)
        {
            return player.Energy;
        }

        player.Energy += amount;
        return player.Energy;
    }
}
