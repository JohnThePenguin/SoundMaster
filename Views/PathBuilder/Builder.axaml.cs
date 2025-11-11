using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SoundMasterGui.ViewModels;

namespace SoundMasterGui.Views.PathBuilder;

public partial class Builder : UserControl
{
    public Builder()
    {
        InitializeComponent();
        DataContext = new PathBuilderViewModel();
    }
}