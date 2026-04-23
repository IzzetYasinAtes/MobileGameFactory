using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IslandMerge.Services;

namespace IslandMerge.ViewModels;

public sealed partial class ShopViewModel : BaseViewModel
{
    private readonly IIapService _iap;
    private readonly IGameSession _session;

    [ObservableProperty]
    private string _statusText = string.Empty;

    public ObservableCollection<IapProduct> Products { get; } = new();

    public ShopViewModel(IIapService iap, IGameSession session)
    {
        _iap = iap;
        _session = session;
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
            foreach (var p in products)
            {
                Products.Add(p);
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
        }
        else
        {
            StatusText = $"Basarisiz: {result.FailReason}";
        }
    }
}
