using System;
using poePROG6221.Bot;

namespace poePROG6221
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Cybersecurity Awareness Assistant";
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();

            AsciiArt.Show();
            VoicePlayer.PlayGreeting();

            Console.Write("Please enter your name: ");
            string name = Console.ReadLine();

            CyberBot bot = new CyberBot(name);
            bot.Start();
        }
    }
}


/*References

Microsoft. 2023. System.Media.SoundPlayer Class. (Version 2.0) [Source code] https://learn.microsoft.com/en-us/dotnet/api/system.media.soundplayer. [Accessed 21 April 2025]

Unknown. 2023. ASCII Art Generator – Custom Console Logos. (Version 2.0) [Source code] https://www.ascii-art.de/. [Accessed 21 April 2025]

Norton. 2023. Cybersecurity Basics. (Version 2.0) [Source code] https://us.norton.com/blog/emerging-threats/cybersecurity-101. [Accessed 21 April 2025]

*/
