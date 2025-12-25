using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using SoundMasterGui.ViewModels;

namespace SoundMasterGui.Views.PathBuilder;

public partial class Builder : UserControl
{
    private PathBuilderViewModel _viewModel;
    public Builder()
    {
        InitializeComponent();
        DataContext = TilesGridBuilder.PathBuilderViewModel;
        
        if(DataContext is PathBuilderViewModel vm)
            _viewModel = vm;
        else
            throw new Exception("DataContext is not PathBuilderViewModel");
        
        SetTimeGrid();
    }

    private void SetTimeGrid()
    {
        var grid = TimeGrid;

        if (grid is null)
            throw new Exception("Grid not found");

        var duration = _viewModel.PathDuration;
        var pixelsPerSecond = _viewModel.PixelsPerSecond();

        grid.Margin = new Thickness(_viewModel.PianoWidth, 0, 0, 0);
        
        for (var i = 0; i < duration; i++)
        {
            Debug.WriteLine($"{i} - {duration} - {pixelsPerSecond}");
            
            var column = new ColumnDefinition{Width= new GridLength(pixelsPerSecond, GridUnitType.Pixel)};
            var text = new TextBlock{Text=$"| {i}s", Foreground = Brushes.Black};
            
            grid?.ColumnDefinitions.Add(column);
            Grid.SetColumn(text, i);
            grid?.Children.Add(text);
        }
    }
}