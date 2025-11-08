// See https://aka.ms/new-console-template for more information

using System;
using System.IO;
using System.Text;
using SoundMaster;

var sound = new RawSound();
using var player = new Player();

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

var song = new char[]
{
    'G', 'E', 'E', 'F', 'D', 'D', 'C', 'E', 'G',
    'G', 'E', 'E', 'F', 'D', 'D', 'C', 'E', 'C',
    'C', 'E', 'E', 'F', 'D', 'D', 'C', 'E', 'G',
    'G', 'E', 'E', 'F', 'D', 'D', 'C', 'E', 'C'
};

foreach (var note in song)
{
   player.PlayRaw(sound.CreateSample(notes[note.ToString()], 0.7)); 
}