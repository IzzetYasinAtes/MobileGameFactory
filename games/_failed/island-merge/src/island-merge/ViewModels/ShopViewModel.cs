using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IslandMerge.Services;

namespace IslandMerge.ViewModels;

public sealed partial class ShopViewModel : BaseViewModel
{
    private readonly IIapService _iap;
    private readonly IGameSession _session;
    private readonly ISelectedCharacterStore _characters;

    [ObservableProperty]
    private string _statusText = string.Empty;

    [ObservableProperty]
    private string _selectedCharacterImage = "character_momo.png";

    /// <summary>
    /// Reklamsiz satin alindi mi — UI rozet + "Satin Al" butonunu gizlemek icin.
    /// E2E-004 fix: satin alma sonrasi gorsel feedback.
    /// </summary>
    [ObservableProperty]
    private bool _isRemoveAdsOwned;

    public ObservableCollection<IapProduct> Products { get; } = new();

    public ShopViewModel(IIapService iap, IGameSession session, ISelectedCharacterStore characters)
    {
        _iap = iap;
        _session = session;
        _characters = characters;
        Title = "Magaza";
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        if (IsBusy)
        {
            return;
        }
        IsBusy = true;
        try
        {
            await _iap.InitializeAsync().ConfigureAwait(true);
            Products.Clear();
            var products = await _iap.GetProductsAsync().ConfigureAwait(true);
            var starterActive = _session.IsStarterPackOfferActive();
            var removeAdsOwned = _session.Player.RemoveAdsPurchased;
            foreach (var p in products)
            {
                if (p.Sku == IapSku.StarterPack && !starterActive)
                {
                    // 24 saat pencere kapali -> listeleme.
                    continue;
                }
                if (p.Sku == IapSku.RemoveAds && removeAdsOwned)
                {
                    // Zaten alinmis -> listeden kaldir, bunun yerine rozet gosterecegiz.
                    continue;
                }
                Products.Add(p);
            }

            SelectedCharacterImage = CharacterImage(_characters.GetSelected());
            RefreshRemoveAdsState();
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task RestoreAsync()
    {
        IsBusy = true;
        try
        {
            var restored = await _iap.RestoreRemoveAdsAsync().ConfigureAwait(true);
            if (restored)
            {
                await _session.ApplyIapAsync(IapSku.RemoveAds).ConfigureAwait(true);
                StatusText = "Reklamsiz geri yuklendi";
            }
            else
            {
                StatusText = "Geri yuklenecek satin alma yok";
            }
            RefreshRemoveAdsState();
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task PurchaseAsync(IapProduct product)
    {
        var result = await _iap.PurchaseAsync(product.Sku).ConfigureAwait(true);
        if (result.Success)
        {
            await _session.ApplyIapAsync(product.Sku).ConfigureAwait(true);
            StatusText = $"{product.Title}: alindi";
            RefreshRemoveAdsState();
            // Starter pack veya RemoveAds satin alindiysa liste yenilensin (RemoveAds butonu pasiflesir).
            if (product.Sku == IapSku.StarterPack || product.Sku == IapSku.RemoveAds)
            {
                await LoadAsync().ConfigureAwait(true);
            }
        }
        else
        {
            StatusText = $"Basarisiz: {result.FailReason}";
        }
    }

    private void RefreshRemoveAdsState()
    {
        try
        {
            IsRemoveAdsOwned = _session.Player.RemoveAdsPurchased;
        }
        catch (InvalidOperationException)
        {
            // Session henuz yuklenmediyse default kal.
            IsRemoveAdsOwned = false;
        }
    }

    private static string CharacterImage(CharacterId id) => id switch
    {
        CharacterId.Kasif => "character_kasif.png",
        CharacterId.Lila => "character_lila.png",
        CharacterId.Momo => "character_momo.png",
        CharacterId.Papagan => "character_papagan.png",
        _ => "character_momo.png",
    };
}
