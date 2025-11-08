namespace SoundMaster;

using SampleArray = short[];

public class RawSound : ICloneable
{
    public int SampleRate { get; private set; }
    public double Frequency { get; private set; }
    public double Duration { get; private set; }
    public short Volume { get; private set; }
    public readonly short BitsPerSample = 16;
    public SampleArray? Buffer { get; private set; }

    public RawSound(int sampleRate = 44100, double frequency = 440.0, double duration = 1.0,
        short volume = short.MaxValue)
    {
        SampleRate = sampleRate;
        Frequency = frequency;
        Duration = duration;
        Volume = volume;
    }

    public object Clone() => MemberwiseClone();

    public RawSound CreateSample(double? f = null, double? d = null)
    {
        var frequency = f ?? this.Frequency;
        var duration = d ?? this.Duration;
        var frames = (int) (SampleRate * duration);
        
        Buffer = new short[frames];
        for (var i = 0; i < frames; i++)
        {
            Buffer[i] = (short)(Volume * Math.Sin(2 * Math.PI * frequency * i / SampleRate));
        }

        if(Clone() is not RawSound clone) throw new InvalidCastException();
        
        clone.Duration = duration;
        clone.Frequency = frequency;
        return clone;
    }
}