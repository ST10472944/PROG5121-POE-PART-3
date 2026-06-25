using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberAwarenessChatbotGUI
{
    public class QuizManager
    {
        private List<Question> questions;
        private int currentIndex = 0;
        private int score = 0;
        public int Score => score;
        public int TotalQuestions => questions.Count;

        public QuizManager()
        {
            questions = new List<Question>();
            // 12 questions (more than 10)
            questions.Add(new Question("What is the most common method of phishing?", new[] { "Email", "Phone", "Text", "Social Media" }, 0, "Email is most common."));
            questions.Add(new Question("True or False: Reusing passwords is safe.", new[] { "True", "False" }, 1, "Reusing is dangerous."));
            questions.Add(new Question("What to do with a suspicious email?", new[] { "Reply", "Click link", "Report as phishing", "Ignore" }, 2, "Report it."));
            questions.Add(new Question("True or False: HTTPS is secure.", new[] { "True", "False" }, 0, "HTTPS encrypts data."));
            questions.Add(new Question("Which is a strong password?", new[] { "password123", "qwerty", "Tr0ub4dor&3", "admin" }, 2, "Mix of characters."));
            questions.Add(new Question("True or False: Public Wi-Fi is safe for banking.", new[] { "True", "False" }, 1, "Use VPN on public Wi-Fi."));
            questions.Add(new Question("What does 2FA stand for?", new[] { "Two-Factor Authentication", "Double Factor Access", "Two-Factor Authorization", "None" }, 0, "2FA adds extra security."));
            questions.Add(new Question("Signs of a scam email?", new[] { "Spelling errors", "Urgent language", "Generic greeting", "All of above" }, 3, "All are red flags."));
            questions.Add(new Question("True or False: Share your password with friends.", new[] { "True", "False" }, 1, "Never share."));
            questions.Add(new Question("What is ransomware?", new[] { "Locks your files", "A scam email", "A password manager", "A firewall" }, 0, "Ransomware demands payment."));
            questions.Add(new Question("True or False: Software updates are unimportant.", new[] { "True", "False" }, 1, "Updates fix vulnerabilities."));
            questions.Add(new Question("What is social engineering?", new[] { "Manipulating people", "Writing code", "Using antivirus", "Creating passwords" }, 0, "It exploits human psychology."));
        }

        public bool HasMoreQuestions() => currentIndex < questions.Count;
        public Question GetCurrentQuestion() => questions[currentIndex];
        public bool SubmitAnswer(int idx) { if (idx == questions[currentIndex].CorrectIndex) { score++; return true; } return false; }
        public string GetCorrectAnswer() => questions[currentIndex].Options[questions[currentIndex].CorrectIndex];
        public void NextQuestion() => currentIndex++;
    }

    public class Question
    {
        public string QuestionText { get; set; }
        public string[] Options { get; set; }
        public int CorrectIndex { get; set; }
        public string Explanation { get; set; }
        public Question(string text, string[] opts, int correct, string exp) { QuestionText = text; Options = opts; CorrectIndex = correct; Explanation = exp; }
    }
}