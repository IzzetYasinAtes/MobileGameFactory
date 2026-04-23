using IslandMerge.Models;
using IslandMerge.ViewModels;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

namespace IslandMerge.Controls;

/// <summary>
/// 7x9 merge board. SkiaSharp ile dogrudan cizim.
/// Drag-and-drop: PAN gesture ile basit start-end tespiti + MergeCommand tetikleme.
/// Tier'a gore renk + tier numarasi placeholder gosterimi.
/// </summary>
public sealed class BoardCanvas : SKCanvasView
{
    public static readonly BindableProperty CellsProperty = BindableProperty.Create(
        nameof(Cells),
        typeof(IList<BoardCellVm>),
        typeof(BoardCanvas),
        propertyChanged: (bindable, _, _) => ((BoardCanvas)bindable).InvalidateSurface());

    public IList<BoardCellVm>? Cells
    {
        get => (IList<BoardCellVm>?)GetValue(CellsProperty);
        set => SetValue(CellsProperty, value);
    }

    public static readonly BindableProperty MergeCommandProperty = BindableProperty.Create(
        nameof(MergeCommand),
        typeof(System.Windows.Input.ICommand),
        typeof(BoardCanvas));

    public System.Windows.Input.ICommand? MergeCommand
    {
        get => (System.Windows.Input.ICommand?)GetValue(MergeCommandProperty);
        set => SetValue(MergeCommandProperty, value);
    }

    public static readonly BindableProperty FogMaskProperty = BindableProperty.Create(
        nameof(FogMask),
        typeof(IReadOnlyList<bool>),
        typeof(BoardCanvas),
        propertyChanged: (b, _, _) => ((BoardCanvas)b).InvalidateSurface());

    public IReadOnlyList<bool>? FogMask
    {
        get => (IReadOnlyList<bool>?)GetValue(FogMaskProperty);
        set => SetValue(FogMaskProperty, value);
    }

    private int? _dragStartCell;
    private int? _dragEndCell;

    public BoardCanvas()
    {
        EnableTouchEvents = true;
        Touch += OnTouch;
    }

    private void OnTouch(object? sender, SKTouchEventArgs e)
    {
        switch (e.ActionType)
        {
            case SKTouchAction.Pressed:
                _dragStartCell = CellAt(e.Location);
                _dragEndCell = _dragStartCell;
                e.Handled = true;
                break;
            case SKTouchAction.Moved:
                _dragEndCell = CellAt(e.Location);
                e.Handled = true;
                break;
            case SKTouchAction.Released:
                _dragEndCell = CellAt(e.Location);
                if (_dragStartCell is int src && _dragEndCell is int dst && src != dst)
                {
                    if (MergeCommand is not null && MergeCommand.CanExecute(null))
                    {
                        MergeCommand.Execute((src, dst));
                    }
                }
                _dragStartCell = null;
                _dragEndCell = null;
                e.Handled = true;
                break;
            case SKTouchAction.Cancelled:
                _dragStartCell = null;
                _dragEndCell = null;
                break;
        }
    }

    private int? CellAt(SKPoint p)
    {
        var info = CanvasSize;
        if (info.Width <= 0 || info.Height <= 0)
        {
            return null;
        }

        var cellW = info.Width / BoardConstants.Width;
        var cellH = info.Height / BoardConstants.Height;
        if (p.X < 0 || p.Y < 0 || p.X >= info.Width || p.Y >= info.Height)
        {
            return null;
        }
        var cx = (int)(p.X / cellW);
        var cy = (int)(p.Y / cellH);
        cx = Math.Clamp(cx, 0, BoardConstants.Width - 1);
        cy = Math.Clamp(cy, 0, BoardConstants.Height - 1);
        return cy * BoardConstants.Width + cx;
    }

    protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
    {
        base.OnPaintSurface(e);
        var canvas = e.Surface.Canvas;
        canvas.Clear(SKColor.Parse("#0E3B39"));

        var info = e.Info;
        var cellW = info.Width / (float)BoardConstants.Width;
        var cellH = info.Height / (float)BoardConstants.Height;

        using var gridPaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 1,
            Color = SKColor.Parse("#14524C"),
            IsAntialias = true,
        };

        // Grid lines
        for (var i = 0; i <= BoardConstants.Width; i++)
        {
            var x = i * cellW;
            canvas.DrawLine(x, 0, x, info.Height, gridPaint);
        }
        for (var j = 0; j <= BoardConstants.Height; j++)
        {
            var y = j * cellH;
            canvas.DrawLine(0, y, info.Width, y, gridPaint);
        }

        var cells = Cells;
        if (cells is null)
        {
            return;
        }

        using var textPaint = new SKPaint
        {
            Color = SKColors.White,
            IsAntialias = true,
        };
        using var labelFont = new SKFont
        {
            Size = Math.Min(cellW, cellH) * 0.35f,
            Embolden = true,
        };

        foreach (var cell in cells)
        {
            if (cell.Chain == ItemChain.None || cell.Tier <= 0)
            {
                continue;
            }
            var cx = cell.Index % BoardConstants.Width;
            var cy = cell.Index / BoardConstants.Width;
            var left = cx * cellW;
            var top = cy * cellH;
            var pad = Math.Min(cellW, cellH) * 0.1f;
            var rect = new SKRect(left + pad, top + pad, left + cellW - pad, top + cellH - pad);

            using var fill = new SKPaint
            {
                Color = ColorForChain(cell.Chain, cell.Tier),
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
            };
            canvas.DrawRoundRect(rect, 12, 12, fill);

            var label = $"{ChainGlyph(cell.Chain)}{cell.Tier}";
            canvas.DrawText(label, rect.MidX, rect.MidY + labelFont.Size * 0.35f, SKTextAlign.Center, labelFont, textPaint);
        }

        // Drag overlay
        if (_dragStartCell is int src)
        {
            using var overlay = new SKPaint
            {
                Color = new SKColor(245, 166, 35, 96),
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 4,
                IsAntialias = true,
            };
            var sx = (src % BoardConstants.Width) * cellW;
            var sy = (src / BoardConstants.Width) * cellH;
            canvas.DrawRoundRect(new SKRect(sx + 2, sy + 2, sx + cellW - 2, sy + cellH - 2), 12, 12, overlay);
        }
    }

    private static SKColor ColorForChain(ItemChain chain, int tier)
    {
        var lighten = Math.Clamp(tier * 30, 0, 220);
        return chain switch
        {
            ItemChain.Stone => new SKColor((byte)(120 + lighten / 4), (byte)(120 + lighten / 4), (byte)(120 + lighten / 4)),
            ItemChain.Wood => new SKColor((byte)(110 + lighten / 3), (byte)(60 + lighten / 6), 20),
            ItemChain.Crystal => new SKColor(80, (byte)(160 + lighten / 6), (byte)(220)),
            ItemChain.Shell => new SKColor(230, 180, 140),
            ItemChain.Relic => new SKColor(180, 140, 50),
            ItemChain.Ember => new SKColor(220, 90, 40),
            ItemChain.Ice => new SKColor(170, 200, 240),
            _ => SKColors.DarkGray,
        };
    }

    private static string ChainGlyph(ItemChain chain) => chain switch
    {
        ItemChain.Stone => "T",
        ItemChain.Wood => "O",
        ItemChain.Crystal => "K",
        ItemChain.Shell => "S",
        ItemChain.Relic => "R",
        ItemChain.Ember => "A",
        ItemChain.Ice => "B",
        _ => "?",
    };
}
