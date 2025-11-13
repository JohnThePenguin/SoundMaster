using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Avalonia;
using SoundMaster;

namespace SoundMasterGui.ViewModels;

public class PathBuilderViewModel : ViewModelBase, IDisposable
{
    public static Sound SelectedSound { get; private set; }   
    private static readonly Player Player = new Player();
    public static double[] IndexFrequencyBind = new double[100];
    public static string[] IndexToneNameBind = new string[100];
    
    public const int BPM = 110;
    public double BitsPerSecond() => (double)(BPM) / 60;

    public bool PlayingStatus = false;
    public double PlayingTime { get; set; }
    public double PathDuration = 30 / (BPM / 60);
    
    private Dictionary<int, (Sound, double)> _sounds = new Dictionary<int, (Sound, double)>();

    public Point StartPoint => new Point(PlayingTime * BitsPerSecond() * 32 + 70, 0);
    public Point EndPoint => new Point(PlayingTime * BitsPerSecond() * 32 + 70, 1000);
    

    public PathBuilderViewModel()
    {
        SelectedSound = new Sound();
        StartPlaying();
    }

    public void PlayIndex(int index)
    {
        Debug.WriteLine($"Playing at index {index} = {IndexFrequencyBind[index]}");
        PlayFrequency(IndexFrequencyBind[index]);
    }

    public void PlayFrequency(double frequency)
    {
        var sample = SelectedSound.GetSample(frequency);
        Debug.WriteLine($"Playing frequency {frequency} = {sample}");
        Debug.WriteLine(sample);
        Player.PlayRaw(SelectedSound.GetSample(frequency));
    }

    public void StartPlaying()
    {
        RaisePropertyChanged(nameof(PlayingStatus));
        if (!PlayingStatus) PlayLogic();
    }

    public void PlayAtTime(double time)
    {
        var segment = (int) (time *  BitsPerSecond());
        Debug.WriteLine($"Try to play at {segment} = {time}");
        if (double.Abs(segment / BitsPerSecond()) - time < 0.04)
        {
            if(_sounds.TryGetValue(segment, out var soundFreq))
            {
                Debug.WriteLine($"Playing at {segment} = {soundFreq}, ${soundFreq.Item1}");
                Player.PlayRaw(soundFreq.Item1.GetSample(soundFreq.Item2));
            }        
        }
    }

    public async Task PlayLogic()
    {
        PlayingStatus = true;
        try
        {
            while (PlayingStatus)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(50));
                PlayingTime += 0.05;
                if(PlayingTime >= PathDuration) PlayingTime = 0;
                
                PlayAtTime(PlayingTime);
                // Debug.WriteLine($"Time {PlayingTime}");
                RaisePropertyChanged(nameof(EndPoint));
                RaisePropertyChanged(nameof(StartPoint));
            }
        }
        finally
        {
            StopPlaying();
        }
    }

    public void StopPlaying()
    {
        PlayingStatus = false;
        RaisePropertyChanged(nameof(PlayingStatus));
    }

    public void AddSelectedSound(int column, int row)
    {
        _sounds[column] = (SelectedSound, IndexFrequencyBind[row]);
    }

    public void RemoveSound(int segment, Sound sound)
    {
        _sounds.Remove(segment);
    }

    public void Dispose()
    {
       Player.Dispose(); 
    }
}