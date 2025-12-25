using System;
using Avalonia.Input;
using Avalonia.Interactivity;
using SoundMasterGui.ViewModels;

namespace SoundMasterGui.Views.PathBuilder;

using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

public partial class MainGrid : UserControl
{
    public static readonly StyledProperty<int> ColumnsProperty =
        AvaloniaProperty.Register<MainGrid, int>(
            nameof(Columns), 
            defaultValue: 100
        );
    
    private readonly PathBuilderViewModel _viewModel;

    public MainGrid()
    {
        InitializeComponent();
        DataContext = TilesGridBuilder.PathBuilderViewModel;

        if (DataContext is PathBuilderViewModel vm)
            _viewModel = vm;
        else
            throw new Exception("DataContext is not PathBuilderViewModel"); 
        
        var grid = this.FindControl<Grid>("Grid");
        if(grid is null)
            throw new Exception("Grid not found");

        var columnWidth = _viewModel.TileWidth;
        grid.Width = columnWidth * Columns;
        
        for (var i = 0; i < 7 * 12 + 3; i++)
            grid?.RowDefinitions.Add(new RowDefinition{Height=new GridLength(26)});

        for (var c = 0; c < Columns; c++)
        {
            grid?.ColumnDefinitions.Add(new ColumnDefinition{Width=new GridLength(columnWidth)});
        }

        for (var i = 0; i < 7 * 12 + 3; i++)
        {
            for (var j = 0; j < Columns; j++)
            {
                var (ci, cj) = (i, j);
                var border = new Border
                {
                    BorderThickness = new Thickness(1),
                    BorderBrush = Brushes.LightGray,
                    Background = Brushes.Transparent,
                };
                border.AddHandler(
                    InputElement.PointerPressedEvent,
                    (s, e) => Border_CellPressed(s, e, cj, ci),
                    RoutingStrategies.Bubble,
                    handledEventsToo: true
                    );
                grid?.Children.Add(border);
                Grid.SetRow(border, i);
                Grid.SetColumn(border, j);
            }
        }
    }
    public int Columns
    {
        get => GetValue(ColumnsProperty); 
        set => SetValue(ColumnsProperty, value);
    }

    private void Grid_handleBrickChange(Border border, bool right = false, int column = 0, int row = 0)
    {
        if (border.Child == null)
            Grid_addBrick(border, column, row);
        else if (right)
            Grid_removeBrick(border);
    }

    private void Grid_addBrick(Border border, int column, int row)
    {
        _viewModel.AddSelectedSound(column, row);
        
        var button = new Button
        {
            Background = Brushes.GreenYellow,
            Foreground = Brushes.DarkGreen,
            FontSize = 10,
            BorderThickness = new Thickness(1),
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
        };
        button.Content = PathBuilderViewModel.IndexToneNameBind[row];
        button.Click += (_, _) =>
        {
            Debug.WriteLine("Playing at row " + row);
            _viewModel.PlayIndex(row);
        };
        _viewModel.PlayIndex(row);
        border.Child = button;
    }

    private static void Grid_removeBrick(Border border)
    {
       border.Child = null; 
    }

    private void Border_CellPressed(object? sender, PointerPressedEventArgs e, int column, int row)
    {
        Debug.WriteLine($"Border_CellPressed ({column}, {row})");
        if (sender is Border border)
        {
            Grid_handleBrickChange(border, e.Properties.IsRightButtonPressed, column, row);
        }
    }

    public static void ColumnsPropertyChanged(AvaloniaPropertyChangedEventArgs e)
    {
        
    }
}