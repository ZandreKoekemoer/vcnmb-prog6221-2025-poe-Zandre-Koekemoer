using System;
using System.Media;

namespace poePROG6221.Bot
{
    public static class VoicePlayer
    {
        public static void PlayGreeting()
        {
            try
            {
                // According to MicrosoftDocs (2025) the SoundPlayer class allows WAV files to be played in .NET applications.
                /*
                MicrosoftDocs (2025)
                The SoundPlayer class enables playback of .wav audio files using methods like Play, PlaySync, and Load.
                It is part of the System.Media namespace and supports playing audio from files, streams, or embedded resources.
                */

                //Combines the directory of the file location and the name
                string path = @"C:\Users\zandr\source\repos\poePROG6221\poePROG6221\SOUNDS\greeting.wav - Copy.wav";
                using (SoundPlayer player = new SoundPlayer(path))
                {
                    player.Load();
                    player.PlaySync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error playing greeting audio: " + ex.Message);
            }
        }
    }
}
