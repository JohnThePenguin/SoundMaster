using System;
using System.IO;
using System.Threading;

namespace SoundMaster;

using OpenTK.Audio.OpenAL;

public class Player: System.IDisposable
{
    private ALDevice _alDevice;
    private ALContext _alContext;

    public Player()
    {
       _alDevice = ALC.OpenDevice(null); 
       _alContext = ALC.CreateContext(_alDevice, (int[])null);
       ALC.MakeContextCurrent(_alContext);
    }

    public void Dispose()
    {
        ALC.MakeContextCurrent(ALContext.Null);
        ALC.DestroyContext(_alContext);
        ALC.CloseDevice(_alDevice);
    }
    
    public void PlayRaw(byte[] pcm, int sampleRate, int channels, int bitsPerSample, double duration = 1.0)
    {
        int buf = AL.GenBuffer();
        int src = AL.GenSource();

        var fmt = (channels, bitsPerSample) switch
        {
            (1, 8)  => ALFormat.Mono8,
            (1, 16) => ALFormat.Mono16,
            (2, 8)  => ALFormat.Stereo8,
            (2, 16) => ALFormat.Stereo16,
            _ => throw new NotSupportedException("Only 8/16-bit mono/stereo supported here.")
        };

        AL.BufferData(buf, fmt, pcm, sampleRate);
        AL.Source(src, ALSourcei.Buffer, buf);
        AL.SourcePlay(src);

        Thread.Sleep((int)(duration * 1000));

        AL.SourceStop(src);
        AL.DeleteSource(src);
        AL.DeleteBuffer(buf);
    }

    public void PlayRaw(RawSound sound)
    {
        PlayRaw(ConvertShortWaveToByte(sound.Buffer), sound.SampleRate, 1, sound.BitsPerSample, sound.Duration);
    }

    public static MemoryStream CreateWavStream(byte[] wave, int sampleRate, short bitsPerSample)
    {
        
        var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);
        var blockAlign = (short)(bitsPerSample / 8);
        var subchunkTwoSize = sampleRate * 1 * blockAlign;

        writer.Write(new[] { 'R', 'I', 'F', 'F' });
        writer.Write(36 + subchunkTwoSize);
        writer.Write(new[] { 'W', 'A', 'V', 'E' });

        writer.Write(new[] { 'f', 'm', 't', ' ' });
        writer.Write(16);
        writer.Write((short)1);
        writer.Write((short)1);
        writer.Write(sampleRate);
        writer.Write(sampleRate * blockAlign);
        writer.Write(blockAlign);
        writer.Write(bitsPerSample);

        writer.Write(new[] { 'd', 'a', 't', 'a' });
        writer.Write(subchunkTwoSize);
        writer.Write(wave);

        stream.Position = 0;
        return stream;
    }

    public static byte[] ConvertShortWaveToByte(short[] wave)
    {
        var waveBytes = new byte[wave.Length * sizeof(short)];
        for (var i = 0; i < waveBytes.Length; i += 2)
        {
            BitConverter.GetBytes(wave[i/2]).CopyTo(waveBytes, i);
        }
        return waveBytes;
    }

    public static void SaveStreamToWav(MemoryStream stream, string fileName)
    {
        var fileStream = new FileStream("output.wav", FileMode.Create);
        stream.WriteTo(fileStream);
        fileStream.Close();
    }
}