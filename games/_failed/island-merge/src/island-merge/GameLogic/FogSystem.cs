using IslandMerge.Models;

namespace IslandMerge.GameLogic;

/// <summary>
/// Fog-of-war durumu saf C#. Tile bayraklari disarida tutulur (IReadOnlyList bool mask).
/// Reveal stratejisi:
///  - Her MergesPerFogReveal merge 1 bitisik tile acar.
///  - Quest tamamlanmasi QuestBonusFogReveal kadar ek tile acar.
/// Bitisiklik: sira bazli flood-fill (zaten acik olanin komsusunu sec).
/// </summary>
public sealed class FogSystem
{
    private readonly bool[] _revealed;
    private int _mergeCounter;

    public FogSystem(int tileCount)
    {
        if (tileCount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(tileCount));
        }

        _revealed = new bool[tileCount];
    }

    public int TileCount => _revealed.Length;

    public int RevealedCount
    {
        get
        {
            var n = 0;
            foreach (var r in _revealed)
            {
                if (r)
                {
                    n++;
                }
            }
            return n;
        }
    }

    public bool IsRevealed(int tileIndex)
    {
        if (tileIndex < 0 || tileIndex >= _revealed.Length)
        {
            return false;
        }
        return _revealed[tileIndex];
    }

    public void LoadFromMask(IReadOnlyList<bool> mask)
    {
        var limit = Math.Min(mask.Count, _revealed.Length);
        for (var i = 0; i < limit; i++)
        {
            _revealed[i] = mask[i];
        }
    }

    public IReadOnlyList<bool> Snapshot() => _revealed;

    /// <summary>
    /// Merge sayacini arttir; esige ulasti mi 1 tile ac. Acilan tile indeksi (ya da null).
    /// </summary>
    public int? RegisterMergeAndMaybeReveal()
    {
        _mergeCounter++;
        if (_mergeCounter < BoardConstants.MergesPerFogReveal)
        {
            return null;
        }

        _mergeCounter = 0;
        return RevealNext();
    }

    /// <summary>Quest bonusu — N tile bitisik ac.</summary>
    public IReadOnlyList<int> RevealBatch(int count)
    {
        var opened = new List<int>(count);
        for (var i = 0; i < count; i++)
        {
            var idx = RevealNext();
            if (idx is null)
            {
                break;
            }
            opened.Add(idx.Value);
        }
        return opened;
    }

    /// <summary>Bitisik algoritma: merkezden genisleyen en yakin kapali tile.</summary>
    public int? RevealNext()
    {
        // Hic acilmamissa ortadan basla.
        var hasAny = false;
        foreach (var r in _revealed)
        {
            if (r)
            {
                hasAny = true;
                break;
            }
        }

        if (!hasAny)
        {
            var mid = _revealed.Length / 2;
            _revealed[mid] = true;
            return mid;
        }

        // Acik olan herhangi bir tile'in komsulari icinde ilk kapali olan.
        for (var i = 0; i < _revealed.Length; i++)
        {
            if (!_revealed[i])
            {
                continue;
            }

            foreach (var n in Neighbours(i))
            {
                if (!_revealed[n])
                {
                    _revealed[n] = true;
                    return n;
                }
            }
        }

        return null; // tumuyle acik.
    }

    private static IEnumerable<int> Neighbours(int index)
    {
        var w = BoardConstants.FogWidth;
        var h = BoardConstants.FogHeight;
        var x = index % w;
        var y = index / w;

        if (x > 0)
        {
            yield return index - 1;
        }
        if (x < w - 1)
        {
            yield return index + 1;
        }
        if (y > 0)
        {
            yield return index - w;
        }
        if (y < h - 1)
        {
            yield return index + w;
        }
    }
}
