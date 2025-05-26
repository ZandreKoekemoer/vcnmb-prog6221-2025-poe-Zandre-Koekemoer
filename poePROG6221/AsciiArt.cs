using System;

namespace poePROG6221.Bot
{
    public static class AsciiArt
    {
        public static void Show()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            //Custom logo stand for Cybersecurity Assistance Bot

            //Reference ASCII Art

            // According to ASCIIArt.eu (n.d.) ASCII art is a graphic design technique that uses printable characters from the ASCII standard.
            /*
            ASCIIArt.eu (n.d.)
            ASCII art is a way of creating pictures on computers using characters like letters, numbers, and symbols.
            */

            Console.WriteLine(@"
   ________    _    ____  
 / ___/ ___|  / \  | __ ) 
| |   \___ \ / _ \ |  _ \ 
| |___ ___) / ___ \| |_) |
 \____|____/_/   \_\____/     
  Cybersecurity Awareness Bot      
            ");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
