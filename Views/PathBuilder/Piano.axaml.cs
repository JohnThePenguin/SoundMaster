using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using System.Diagnostics;
using Avalonia.Controls.Shapes;
using SoundMasterGui.ViewModels;

namespace SoundMasterGui.Views.PathBuilder;

public partial class Piano : UserControl
{
    private readonly string[] _noteNames = [ "A", "A#", "B", "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#" ];
    
    public Piano()
    {
        InitializeComponent();
        DataContext = new PathBuilderViewModel();
        
        var panel = this.FindControl<StackPanel>("PianoPanel");
        
        var A0 = 27.5;
        var notes = new List<(string, double)>();

        for (var i = 0; i < 7 * 12 + 3; i++)
        {
            var f = Math.Round(A0 * Math.Pow(2, (double)i / 12), 2);
            var name = _noteNames[i % 12] + ((i + 9)/ 12).ToString();
            notes.Add((name, f));
            Debug.WriteLine($"{name} = {f}");

            PathBuilderViewModel.IndexFrequencyBind[i] = f;
            PathBuilderViewModel.IndexToneNameBind[i] = name;
        }

        foreach (var note in notes)
        {
            var button = new Button
            {
                Content = note.Item1,
                   
            };
            button.Classes.Add("tile");
            button.Classes.Add(note.Item1.Contains('#') ? "semitone" : "tone");
            button.Click += (sender, args) => (DataContext as PathBuilderViewModel).PlayFrequency(note.Item2);
            panel?.Children.Add(button);
        }
    }
}