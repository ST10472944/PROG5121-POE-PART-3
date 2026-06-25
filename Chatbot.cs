using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberAwarenessChatbotGUI
{
    public class Chatbot
    {
        private ResponseHandler responseHandler;
        private string? userName;
        private string? lastTopic;
        private string? lastResponse;
        private string? userInterest;
        private bool waitingForMore;

        public event Action<string>? ResponseGenerated;

        public Chatbot()
        {
            responseHandler = new ResponseHandler();
        }

        public void SetUserName(string name)
        {
            userName = name;
            responseHandler.SetUserName(name);
        }

        public void ProcessInput(string input)
        {
            string lowerInput = input.ToLower();

            // Conversation flow - handle follow-ups
            if (waitingForMore && (lowerInput.Contains("more") || lowerInput.Contains("another") || lowerInput.Contains("explain")))
            {
                string more = responseHandler.GetMoreOnTopic(lastTopic ?? "");
                ResponseGenerated?.Invoke(more);
                waitingForMore = false;
                return;
            }

            // Exit
            if (lowerInput == "exit" || lowerInput == "quit")
            {
                ResponseGenerated?.Invoke($"Goodbye {userName}! Stay safe online.");
                return;
            }

            // Get response with sentiment
            string response = responseHandler.GetResponse(input, out string detectedTopic, out string sentimentAdjusted);
            if (!string.IsNullOrEmpty(sentimentAdjusted))
                response = sentimentAdjusted + " " + response;

            // Store the topic for follow-up questions
            if (!string.IsNullOrEmpty(detectedTopic))
            {
                lastTopic = detectedTopic;
                waitingForMore = true;
            }

            // Memory - store user interest
            if (lowerInput.Contains("interested in") || lowerInput.Contains("like") || lowerInput.Contains("privacy") || lowerInput.Contains("password"))
            {
                userInterest = detectedTopic;
                response += $" I'll remember that you care about {userInterest}.";
            }

            // Recall memory
            if (!string.IsNullOrEmpty(userInterest) && lowerInput.Contains("tell me something"))
            {
                response = $"As someone interested in {userInterest}, you should always {responseHandler.GetRandomPrivacyTip()}";
            }

            ResponseGenerated?.Invoke(response);
        }
    }
}