using System;
using System.Speech.Synthesis;

namespace poePROG6221.Bot
{
    public static class Utilities
    {
        public static void PrintWithColor(SpeechSynthesizer synth, string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            synth.Speak(message);
            Console.WriteLine("Bot: " + message);
            Console.ResetColor();
        }
    }
}
