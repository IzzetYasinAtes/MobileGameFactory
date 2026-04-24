using IslandMerge.Controls;
using IslandMerge.ViewModels;

namespace IslandMerge.Views;

public partial class BoardPage : ContentPage
{
    private readonly BoardViewModel _vm;

    public BoardPage(BoardViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Animator hook abonelik.
        _vm.TileMerged += OnTileMerged;
        _vm.QuestCompleted += OnQuestCompleted;

        if (_vm.LoadCommand.CanExecute(null))
        {
            await _vm.LoadCommand.ExecuteAsync(null);
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _vm.TileMerged -= OnTileMerged;
        _vm.QuestCompleted -= OnQuestCompleted;
        SpriteAnimator.StopAll(PetIcon);
    }

    private void OnTileMerged(object? sender, int cellIndex)
    {
        // BoardCanvas-level pop — MAUI Animation API, UI thread.
        BoardCanvasView.TriggerPop(cellIndex);
    }

    private void OnQuestCompleted(object? sender, EventArgs e)
    {
        _ = SpriteAnimator.QuestCompleteJump(PetIcon);
    }
}
