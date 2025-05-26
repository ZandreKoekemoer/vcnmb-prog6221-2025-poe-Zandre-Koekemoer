using System;
using System.Collections.Generic;
using System.Speech.Synthesis;

namespace poePROG6221.Bot
{
    public class CyberBot
    {
        private string userName;
        private string userInterest = "";
        private SpeechSynthesizer synth;
        private Dictionary<string, List<string>> keywordResponses;
        private Dictionary<string, string> preventionTips;
        private Dictionary<string, string> sentimentMap;

        // Reference Microsoft Docs. Delegates - C# Programming Guide.

        // According to Microsoft Docs (2023) A delegate is a type-safe function pointer used to call methods indirectly.
        // I used it here to simplify response output handling.
        private delegate void ResponseHandler(string message, ConsoleColor color);

        public CyberBot(string name)
        {
            userName = name;
            synth = new SpeechSynthesizer();
            synth.SelectVoiceByHints(VoiceGender.Female);
            Responses();
        }
        //After user has entered his name,program runs to the next part of the program
        public void Start()
        {
            Utilities.PrintWithColor(synth, $"Hi {userName}, I'm here to help you stay safe online.", ConsoleColor.Cyan);

            ResponseHandler respond = (message, color) => Utilities.PrintWithColor(synth, message, color);
            //if the user has entered his name,bot asks user on what he wants to know
            while (true)
            {
                Console.WriteLine("\nAsk me about phishing, password, malware, privacy or scams. Type 'exit' to quit.");
                Console.Write("You: ");
                string input = Console.ReadLine()?.Trim().ToLower();

                if (input == "exit") break;
                //If user hasnt entered anything,the Bot replies with the statement saying that he needs to type something
                if (string.IsNullOrEmpty(input))
                {
                    respond("Please type something.", ConsoleColor.Red);
                    continue;
                }

                if (CombinedSentiment(input)) continue;
                if (HandleSentiment(input)) continue;
                if (Interest(input)) continue;
                if (Confusion(input)) continue;
                if (Keyword(input)) continue;
                // If no feature matches, give a default response
                respond("I don’t understand that. Try keywords like 'phishing' or 'password'.", ConsoleColor.Red);
            }
             
            respond("Thanks for chatting. Stay safe!", ConsoleColor.Green);
        }
         // This method initializes all chatbot responses and data
        private void Responses()
        {
            // Reference Stack Overflow. How to create a Dictionary of List<string> in C#.

            // According to Stack OverFlow (2021) It demonstrates how to organize multiple values under a single key using List<string> within a Dictionary.
            // I used this approach to store multiple possible responses for each cybersecurity keyword in my chatbot.

            keywordResponses = new Dictionary<string, List<string>>
            {
                //If user enters an option multiple times,it will cycle to the next answer in line
                { "phishing", new List<string>
                {
                    "Phishing is a scam where attackers steal personal info using fake emails or sites.",
                    "Phishing is a cyber-attack using deceptive messages to trick you into revealing sensitive data.",
                    "Phishing emails often look real but link to malicious websites.",
                    "Phishing attacks may use urgent language to pressure you into giving up information."
                }},
                { "password", new List<string>
                {
                    "Use strong passwords with upper/lower case, numbers, and symbols.",
                    "Never reuse passwords across sites, and consider using a password manager.",
                    "Change your passwords regularly and avoid personal info like birthdates.",
                    "Don’t write passwords down; use secure vaults to manage them safely."
                }},
                { "privacy", new List<string>
                {
                    "Don’t overshare on social media. Protect your personal info.",
                    "Limit app permissions and regularly review privacy settings on your accounts.",
                    "Use private browsing modes and clear cookies often.",
                    "Be cautious about what data you allow apps to collect."
                }},
                { "malware", new List<string>
                {
                    "Malware is harmful software that can steal data or damage your system.",
                    "It often installs through suspicious downloads or email attachments.",
                    "Ransomware is a type of malware that locks your files until you pay.",
                    "Spyware secretly tracks what you do and sends it to hackers."
                }},
                { "scam", new List<string>
                {
                    "Online scams use fake offers or threats to trick you into giving up money or info.",
                    "Scammers may pose as official institutions like banks or government.",
                    "Scams often urge urgency, like 'your account will be locked' to make you panic.",
                    "Social media scams can include fake giveaways, job offers, or romance traps."
                }}
            };

            preventionTips = new Dictionary<string, string>
            {
                { "phishing", "Tip: Always verify the sender’s email address and don’t click on suspicious links." },
                { "password", "Tip: Use two-factor authentication wherever possible." },
                { "privacy", "Tip: Use privacy-focused browsers and avoid connecting to public Wi-Fi without a VPN." },
                { "malware", "Tip: Keep your software updated and use reliable antivirus tools." },
                { "scam", "Tip: Be skeptical of deals that sound too good to be true and report suspicious messages." }
            };
            // Stores emotional reactions and supportive replies
            sentimentMap = new Dictionary<string, string>
            {
                { "worried", "It’s okay to be worried! Cybersecurity can feel complex, but I’m here to help you understand it step-by-step." },
                { "curious", "Curiosity is the best way to learn, keep asking great questions!" },
                { "frustrated", "I understand it can be frustrating. Let’s work through it together!" },
                { "scared", "It is ok to feel scared especially about Cybersecurity where people can steal personal information" }
            };
        }

        private bool Keyword(string input)
        {
            foreach (var keyword in keywordResponses.Keys)
            {
                if (input.Contains(keyword))
                {
                    // Reference Microsoft. Random.Next Method (System).

                    // According to Microsoft(2023) This documentation explains how to use the Random class to generate pseudo-random numbers for indexing purposes.
                    // I used this to randomly select a different chatbot response each time for variety and natural conversation flow.

                    var responses = keywordResponses[keyword];
                    //If user asks about the same option,it moves onto the next answer
                    var randomResponse = responses[new Random().Next(responses.Count)];
                    Utilities.PrintWithColor(synth, randomResponse, ConsoleColor.Yellow);
                    // Shows a helpful tip after the main answer
                    if (preventionTips.ContainsKey(keyword))
                    {
                        Utilities.PrintWithColor(synth, preventionTips[keyword], ConsoleColor.DarkCyan);
                    }

                    return true;
                }
            }
            return false;
        }
        // This method checks for emotional words like “worried” or “frustrated”
        private bool HandleSentiment(string input)
        {
            // Reference C# Corner. Understanding Dictionary in C#.

            // According to C# Corner (2022) The article describes how to use a Dictionary<string, string> to associate related text values for simplified lookup.
            // I used this to map user sentiment to helpful emotional responses in the chatbot.

            foreach (var sentiment in sentimentMap)
            {
                if (input.Contains(sentiment.Key))
                {
                    Utilities.PrintWithColor(synth, sentiment.Value, ConsoleColor.Magenta);
                    return true;
                }
            }
            return false;
        }
        // This method checks if the user mixes emotions with a topic
        private bool CombinedSentiment(string input)
        {
            foreach (var sentiment in sentimentMap)
            {
                foreach (var keyword in keywordResponses.Keys)
                {
                    if (input.Contains(sentiment.Key) && input.Contains(keyword))
                    {
                        Utilities.PrintWithColor(synth, sentiment.Value, ConsoleColor.Magenta);
                        Keyword(keyword);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool Interest(string input)
        {
            // If user types “interested in” this will store their topic
            if (input.Contains("interested in"))
            {
                userInterest = input.Substring(input.IndexOf("interested in") + 13).Trim();
                Utilities.PrintWithColor(synth, $"Thanks, I’ll remember that you're interested in {userInterest}.", ConsoleColor.Cyan);
                return true;
            }
             // If user types “remind me” it recalls the previous interest
            if (input.Contains("remind me"))
            {
                if (!string.IsNullOrEmpty(userInterest))
                    Utilities.PrintWithColor(synth, $"You told me you’re interested in {userInterest}.", ConsoleColor.Cyan);
                else
                    Utilities.PrintWithColor(synth, "I don’t recall your interest.", ConsoleColor.Red);
                return true;
            }

            return false;
        }
        // This method handles when the user is confused or wants more explanation
        private bool Confusion(string input)
        {
            // Reference GeeksForGeeks. String.Contains Method in C#.
            // I used this to detect if the user expresses confusion or needs clarification in their input.

            string[] confusionPhrases = { "what do you mean", "explain more", "can you clarify", "i don't get it", "not sure", "confused" };

            foreach (var phrase in confusionPhrases)
            {
                if (input.Contains(phrase))
                {
                    if (!string.IsNullOrEmpty(userInterest))
                    {
                        //Bot will move on to the the next answer for whatever option the user has chosen
                        Utilities.PrintWithColor(synth, $"No problem! Let's go over {userInterest} again.", ConsoleColor.Cyan);
                        Keyword(userInterest);
                    }
                    else
                    {
                        //If no topic was mentioned, th bot will reply with this statement to ask what user is referring to
                        Utilities.PrintWithColor(synth, "Sure! Could you tell me what topic you'd like me to explain more?", ConsoleColor.Cyan);
                    }
                    return true;
                }
            }

            return false;
        }
    }
}


/*Stack Overflow. 2021. How to create a Dictionary of List<string> in C#. (Version 2.0) [Source code] https://stackoverflow.com/questions/65588009/how-to-create-a-dictionary-of-liststring. [Accessed 23 May 2025]

Microsoft. 2023. Random.Next Method (System). (Version 2.0) [Source code] https://learn.microsoft.com/en-us/dotnet/api/system.random.next. [Accessed 23 May 2025]

C# Corner. 2022. Understanding Dictionary in C#. (Version 2.0) [Source code] https://www.c-sharpcorner.com/article/understanding-dictionary-in-c-sharp. [Accessed 23 May 2025]

Stack Overflow. 2023. How to detect confusion or unclear intent in chatbot input? (Version 2.0) [Source code] https://stackoverflow.com/questions/58784298/how-to-detect-confusion-or-unclear-intent-in-chatbot-input. [Accessed 26 May 2025]

Microsoft. 2023. Delegates - C# Programming Guide. (Version 2.0) [Source code] https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/delegates/. [Accessed 26 May 2025]
 */
