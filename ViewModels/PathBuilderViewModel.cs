using System;
using System.Diagnostics;
using SoundMaster;

namespace SoundMasterGui.ViewModels;

public class PathBuilderViewModel : IDisposable
{
    public static Sound SelectedSound { get; private set; }   
    private static readonly Player Player = new Player();
    public static double[] IndexFrequencyBind = new double[100];

    public PathBuilderViewModel()
    {
        SelectedSound = new Sound();
    }

    public void PlayIndex(int index)
    {
        Debug.WriteLine($"Playing at index {index} = {IndexFrequencyBind[index]}");
        PlayFrequency(IndexFrequencyBind[index]);
    }

    public void PlayFrequency(double frequency)
    {
        var sample = SelectedSound.GetSample(frequency);
        Debug.WriteLine(sample);
        Player.PlayRaw(SelectedSound.GetSample(frequency));
    }

    public void Dispose()
    {
       Player.Dispose(); 
    }
}