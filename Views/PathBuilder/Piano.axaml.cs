using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using System.Diagnostics;

namespace SoundMasterGui.Views.PathBuilder;

public partial class Piano : UserControl
{
    private readonly string[] _noteNames = [ "A", "A#", "B", "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#" ];
    
    public Piano()
    {
        InitializeComponent();
        
        var panel = this.FindControl<StackPanel>("PianoPanel");
        
        var A0 = 27.5;
        var notes = new List<(string, double)>();

        for (var i = 0; i < 7 * 12 + 3; i++)
        {
            var f = Math.Round(A0 * Math.Pow(2, (double)i / 12), 2);
            var name = _noteNames[i % 12] + ((i + 9)/ 12).ToString();
            notes.Add((name, f));
            Debug.WriteLine($"{name} = {f}");
        }

        foreach (var note in notes)
        {
            var button = new Button
            {
                Content = note.Item1,
                   
            };
            button.Classes.Add("tile");
            button.Classes.Add(note.Item1.Contains('#') ? "semitone" : "tone");
            panel?.Children.Add(button);
        }
    }
}