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

        // A delegate is a type-safe function pointer used to call methods indirectly.
        // I used it here to simplify response output handling.
        private delegate void ResponseHandler(string message, ConsoleColor color);

        public CyberBot(string name)
        {
            userName = name;
            synth = new SpeechSynthesizer();
            synth.SelectVoiceByHints(VoiceGender.Female);
            LoadResponses();
        }

        public void Start()
        {
            Utilities.PrintWithColor(synth, $"Hi {userName}, I'm here to help you stay safe online.", ConsoleColor.Cyan);

            ResponseHandler respond = (message, color) => Utilities.PrintWithColor(synth, message, color);

            while (true)
            {
                Console.WriteLine("\nAsk me about phishing, password, malware, privacy or scams. Type 'exit' to quit.");
                Console.Write("You: ");
                string input = Console.ReadLine()?.Trim().ToLower();

                if (input == "exit") break;
                if (string.IsNullOrEmpty(input))
                {
                    respond("Please type something.", ConsoleColor.Red);
                    continue;
                }

                if (HandleCombinedSentiment(input)) continue;
                if (HandleSentiment(input)) continue;
                if (HandleInterest(input)) continue;
                if (HandleConfusionOrClarification(input)) continue;
                if (RespondToKeyword(input)) continue;

                respond("I don’t understand that. Try keywords like 'phishing' or 'password'.", ConsoleColor.Red);
            }
            //if user enters exit it will close program
            respond("Thanks for chatting. Stay safe!", ConsoleColor.Green);
        }

        private void LoadResponses()
        {
            // Reference Stack Overflow. How to create a Dictionary of List<string> in C#.

            // The reference demonstrates how to organize multiple values under a single key using List<string> within a Dictionary.
            // I used this approach to store multiple possible responses for each cybersecurity keyword in my chatbot.

            keywordResponses = new Dictionary<string, List<string>>
            {
                //If user enters an option multiple times,it will cycle to the next definition
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

            sentimentMap = new Dictionary<string, string>
            {
                { "worried", "It’s okay to be worried! Cybersecurity can feel complex, but I’m here to help you understand it step-by-step." },
                { "curious", "Curiosity is the best way to learn, keep asking great questions!" },
                { "frustrated", "I understand it can be frustrating. Let’s work through it together!" },
                { "scared", "It is ok to feel scared especially about Cybersecurity where people can steal personal information" }
            };
        }

        private bool RespondToKeyword(string input)
        {
            foreach (var keyword in keywordResponses.Keys)
            {
                if (input.Contains(keyword))
                {
                    // Reference Microsoft. Random.Next Method (System).

                    // This documentation explains how to use the Random class to generate pseudo-random numbers for indexing purposes.
                    // I used this to randomly select a different chatbot response each time for variety and natural conversation flow.

                    var responses = keywordResponses[keyword];
                    var randomResponse = responses[new Random().Next(responses.Count)];
                    Utilities.PrintWithColor(synth, randomResponse, ConsoleColor.Yellow);

                    if (preventionTips.ContainsKey(keyword))
                    {
                        Utilities.PrintWithColor(synth, preventionTips[keyword], ConsoleColor.DarkCyan);
                    }

                    return true;
                }
            }
            return false;
        }

        private bool HandleSentiment(string input)
        {
            // Reference C# Corner. Understanding Dictionary in C#.

            // The article describes how to use a Dictionary<string, string> to associate related text values for simplified lookup.
            // I used this to map user sentiment (e.g., 'worried', 'frustrated') to helpful emotional responses in the chatbot.

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

        private bool HandleCombinedSentiment(string input)
        {
            foreach (var sentiment in sentimentMap)
            {
                foreach (var keyword in keywordResponses.Keys)
                {
                    if (input.Contains(sentiment.Key) && input.Contains(keyword))
                    {
                        Utilities.PrintWithColor(synth, sentiment.Value, ConsoleColor.Magenta);
                        RespondToKeyword(keyword);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool HandleInterest(string input)
        {
            //If user types in this statement with whatever option he chose,the bot will recognize what the user was interested in and stoe it into his memory
            if (input.Contains("interested in"))
            {
                userInterest = input.Substring(input.IndexOf("interested in") + 13).Trim();
                Utilities.PrintWithColor(synth, $"Thanks, I’ll remember that you're interested in {userInterest}.", ConsoleColor.Cyan);
                return true;
            }

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

        private bool HandleConfusionOrClarification(string input)
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
                        RespondToKeyword(userInterest);
                    }
                    else
                    {
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