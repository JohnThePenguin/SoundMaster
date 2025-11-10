namespace SoundMaster;

using SampleArray = short[];

public static class RawSoundConsts
{
    public const int SampleRate = 44100;
}

public class RawSound
{
    public readonly short BitsPerSample = 16;
    
    public int SampleRate { get; protected set; } = RawSoundConsts.SampleRate;

    private SampleArray _buffer = new short[RawSoundConsts.SampleRate];
    public SampleArray Buffer { get => _buffer; protected set => _buffer = value; }

    public double Duration
    {
        get => (double)Buffer.Length / SampleRate;
        set { Array.Resize(ref _buffer, (int)(SampleRate * value));
            Buffer = [];
        }
    }

    public override string ToString()
        => $"RawSound : BufferLenght = {Buffer.Length}, SampleRate = {SampleRate}, Duration = {Duration}, BitsPerSample = {BitsPerSample}";
}