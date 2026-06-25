using System;
using System.Collections.Generic;
using System.Linq;

namespace CyberAwarenessChatbotGUI
{
    public class ResponseHandler
    {
        private Dictionary<string, List<string>> keywordResponses;
        private Dictionary<string, List<string>> randomTipLists;
        private Random random;
        private string userName;
        private Dictionary<string, string> sentimentMap;

        // For cycling through tips without repetition
        private Dictionary<string, Queue<string>> tipQueues = new Dictionary<string, Queue<string>>();

        public ResponseHandler()
        {
            random = new Random(Environment.TickCount);
            InitializeResponses();
            InitializeRandomTips();
            InitializeSentiment();
            InitializeTipQueues();
        }

        private void InitializeTipQueues()
        {
            foreach (var topic in randomTipLists.Keys)
            {
                // Create a random permutation of the list
                var shuffled = randomTipLists[topic].OrderBy(x => random.Next()).ToList();
                tipQueues[topic] = new Queue<string>(shuffled);
            }
        }

        private string GetNextTip(string topic)
        {
            if (!tipQueues.ContainsKey(topic) || tipQueues[topic].Count == 0)
            {
                // Re-shuffle when empty
                var shuffled = randomTipLists[topic].OrderBy(x => random.Next()).ToList();
                tipQueues[topic] = new Queue<string>(shuffled);
            }
            return tipQueues[topic].Dequeue();
        }

        public void SetUserName(string name) => userName = name;

        private void InitializeResponses()
        {
            keywordResponses = new Dictionary<string, List<string>>();

            keywordResponses["password"] = new List<string>
            {
                "Use a strong password with at least 12 characters, mixing letters, numbers, and symbols.",
                "Never reuse passwords across accounts. Use a password manager.",
                "Enable two-factor authentication for an extra layer of security."
            };

            keywordResponses["scam"] = new List<string>
            {
                "Scammers create urgency. Always verify through official channels.",
                "Do not click suspicious links. Hover to see the real URL.",
                "If it sounds too good to be true, it probably is."
            };

            keywordResponses["privacy"] = new List<string>
            {
                "Review privacy settings on social media. Limit public sharing.",
                "Use a VPN on public Wi-Fi to protect your data.",
                "Regularly clear cookies and browsing history."
            };

            keywordResponses["help"] = new List<string>
            {
                "You can ask about passwords, scams, privacy, or request a phishing tip. Type 'exit' to quit."
            };
        }

        private void InitializeRandomTips()
        {
            randomTipLists = new Dictionary<string, List<string>>();
            randomTipLists["phishing tip"] = new List<string>
            {
                "Be cautious of emails asking for personal information. Scammers often disguise themselves as trusted organisations.",
                "Look for spelling mistakes and generic greetings like 'Dear Customer' – these are red flags.",
                "Never click on links in unsolicited emails. Type the website address directly into your browser."
            };
            randomTipLists["general tip"] = new List<string>
            {
                "Keep your software updated to protect against known vulnerabilities.",
                "Back up your important files regularly to an external drive or cloud.",
                "Be skeptical of unexpected phone calls asking for your password or OTP."
            };
        }

        private void InitializeSentiment()
        {
            sentimentMap = new Dictionary<string, string>
            {
                { "worried", "It's understandable to feel worried. Let me share some practical steps to protect yourself." },
                { "scared", "I know cybersecurity can be intimidating. Don't worry – I'll help you stay safe." },
                { "frustrated", "I hear your frustration. Let's focus on simple actions you can take right now." },
                { "curious", "That's great! Curiosity is the first step toward better security." }
            };
        }

        public string GetResponse(string input, out string detectedTopic, out string sentimentAdjusted)
        {
            detectedTopic = "";
            sentimentAdjusted = null;
            string lower = input.ToLower();

            // Sentiment detection
            foreach (var kv in sentimentMap)
                if (lower.Contains(kv.Key))
                {
                    sentimentAdjusted = kv.Value;
                    break;
                }

            // Keyword recognition
            foreach (var kv in keywordResponses)
                if (lower.Contains(kv.Key))
                {
                    detectedTopic = kv.Key;
                    int idx = random.Next(kv.Value.Count);
                    return kv.Value[idx];
                }

            // Random phishing tip (guaranteed no immediate repeat)
            if (lower.Contains("phishing tip") || lower.Contains("give me a phishing tip"))
            {
                detectedTopic = "phishing tip";
                return GetNextTip("phishing tip");
            }

            // General tip
            if (lower.Contains("tip") || lower.Contains("advice"))
            {
                detectedTopic = "general tip";
                var list = randomTipLists["general tip"];
                int idx = random.Next(list.Count);
                return list[idx];
            }

            return "I'm not sure I understand. Can you try rephrasing? You can ask about passwords, scams, privacy, or request a phishing tip.";
        }

        public string GetMoreOnTopic(string topic)
        {
            if (topic == "phishing tip")
                return GetNextTip("phishing tip");
            else if (keywordResponses.ContainsKey(topic))
                return keywordResponses[topic][random.Next(keywordResponses[topic].Count)];
            else if (randomTipLists.ContainsKey(topic))
                return randomTipLists[topic][random.Next(randomTipLists[topic].Count)];
            return "That's all I have on that topic right now. Try asking about something else.";
        }

        public string GetRandomPrivacyTip()
        {
            var privacyList = keywordResponses["privacy"];
            return privacyList[random.Next(privacyList.Count)];
        }
    }
}