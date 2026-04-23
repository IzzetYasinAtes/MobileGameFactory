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
            foreach (var p in products)
            {
                if (p.Sku == IapSku.StarterPack && !starterActive)
                {
                    // 24 saat pencere kapali -> listeleme.
                    continue;
                }
                Products.Add(p);
            }

            SelectedCharacterImage = CharacterImage(_characters.GetSelected());
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
            // Starter pack satin alindiysa listeden cikar.
            if (product.Sku == IapSku.StarterPack)
            {
                await LoadAsync().ConfigureAwait(true);
            }
        }
        else
        {
            StatusText = $"Basarisiz: {result.FailReason}";
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
