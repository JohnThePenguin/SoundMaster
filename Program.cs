// See https://aka.ms/new-console-template for more information

using System;
using System.IO;
using System.Text;
using SoundMaster;

var sound = new RawSound();
using var player = new Player();
// sound.CreateSample();
// Player.PlayRaw(sound);

var notes = new Dictionary<string, double>
{
    ["C"] = 261.6,
    ["D"] = 293.7,
    ["E"] = 329.6,
    ["F"] = 349.2,
    ["G"] = 392.0,
    ["A"] = 440.0,
    ["B"] = 493.9,
};

var song = new char[] { 'G', 'E', 'E', 'F', 'D', 'D', 'C', 'E', 'G' };

foreach (var note in song)
{
   player.PlayRaw(sound.CreateSample(notes[note.ToString()])); 
}

// Player.PlayRaw(sound.CreateSample(notes["C"]));
// Player.PlayRaw(sound.CreateSample(notes["E"]));
// Player.PlayRaw(sound.CreateSample(notes["B"]));

// const int sampleRate = 44100;
// const double frequency = 640.0;
// const double duration = 2.0;
// const short volume = short.MaxValue;
// const short bitsPerSample = 16;
//
// var wave = new short[sampleRate];
// for (var i = 0; i < sampleRate; i++)
// {
//     wave[i] = (short)(volume * Math.Sin((Math.PI * 2 * frequency) * i / sampleRate));
// }
//
// var waveBytes = Player.ConvertShortWaveToByte(wave);
//
// Console.WriteLine($"Wave: {string.Join(", ", wave[..5])}");
// Console.WriteLine($"Bytes: {string.Join(", ", waveBytes[..10])}");
//
// Player.PlayPcm(waveBytes, sampleRate, 1, bitsPerSample, duration);
//
// while (true)
// {
//     var pressedKey =  Console.ReadKey(true);
//     if (pressedKey.Key == ConsoleKey.UpArrow)
//     {
//         Console.WriteLine("Up");
//     }
// }
