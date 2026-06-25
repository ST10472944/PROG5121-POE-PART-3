using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace CyberAwarenessChatbotGUI
{
    public partial class MainWindow : Window
    {
        private Chatbot chatbot;
        private string userName;
        private DatabaseHelper db;
        private QuizManager quiz;
        private int logDisplayCount = 10;

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            AudioPlayer.PlayGreeting();

            chatbot = new Chatbot();
            chatbot.ResponseGenerated += AppendBotResponse;

            db = new DatabaseHelper();
            ActivityLogger.AddEntry("Application started");

            lblAsciiArt.Content = GetAsciiArt();
            LoadTasks();

            AppendSystemMessage("Welcome! Please enter your name.");
            txtUserInput.Focus();
        }

        private string GetAsciiArt()
        {
            return @"
╔═══════════════════════════════════════════════════════════════════════════════╗
║     ██████╗██╗   ██╗██████╗ ███████╗██████╗                                   ║
║    ██╔════╝╚██╗ ██╔╝██╔══██╗██╔════╝██╔══██╗                                  ║
║    ██║      ╚████╔╝ ██████╔╝█████╗  ██████╔╝                                  ║
║    ██║       ╚██╔╝  ██╔══██╗██╔══╝  ██╔══██╗                                  ║
║    ╚██████╗   ██║   ██████╔╝███████╗██║  ██║                                  ║
║     ╚═════╝   ╚═╝   ╚═════╝ ╚══════╝╚═╝  ╚═╝                                  ║
║              C Y B E R   A W A R E N E S S   B O T   (POE)                    ║
║                 South Africa Cybersecurity Initiative                         ║
╚═══════════════════════════════════════════════════════════════════════════════╝";
        }

        private void AppendSystemMessage(string msg) => AppendText($"[System] {msg}", Colors.Gray);
        private void AppendUserMessage(string msg) => AppendText($"[You] {msg}", Colors.White);
        private void AppendBotResponse(string response) => Dispatcher.Invoke(() => AppendText($"[Bot] {response}", Colors.LightGreen));

        private void AppendText(string text, Color color)
        {
            var para = new Paragraph(new Run(text)) { Foreground = new SolidColorBrush(color) };
            txtChatLog.Document.Blocks.Add(para);
            txtChatLog.ScrollToEnd();
        }

        // --------------------- Chat with NLP ---------------------
        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            string input = txtUserInput.Text.Trim();
            if (string.IsNullOrWhiteSpace(input)) return;

            AppendUserMessage(input);
            txtUserInput.Clear();

            if (string.IsNullOrEmpty(userName))
            {
                userName = input;
                chatbot.SetUserName(userName);
                AppendBotResponse($"Hello {userName}! I'm your Cybersecurity Assistant. I can help with cybersecurity tips, tasks, quiz, and more.");
                ActivityLogger.AddEntry($"User set name to {userName}");
                return;
            }

            // Check for NLP commands
            ProcessCommand(input);
        }

        private void ProcessCommand(string input)
        {
            string lower = input.ToLower();

            if (lower.Contains("add task") || lower.Contains("new task"))
            {
                MainTabControl.SelectedIndex = 1; // Tasks tab
                txtTaskTitle.Focus();
                AppendBotResponse("Switching to Tasks tab. Please enter task details.");
                ActivityLogger.AddEntry("User switched to Tasks tab to add a task.");
                return;
            }
            else if (lower.Contains("start quiz") || lower.Contains("quiz"))
            {
                MainTabControl.SelectedIndex = 2; // Quiz tab
                AppendBotResponse("Switching to Quiz tab. Click 'Start Quiz' to begin.");
                ActivityLogger.AddEntry("User switched to Quiz tab.");
                return;
            }
            else if (lower.Contains("show log") || lower.Contains("activity log"))
            {
                MainTabControl.SelectedIndex = 3; // Log tab
                RefreshLogDisplay();
                AppendBotResponse("Here is the activity log.");
                ActivityLogger.AddEntry("User viewed activity log.");
                return;
            }
            else if (lower.Contains("help"))
            {
                string helpText = "You can ask about cybersecurity tips, or use NLP commands:\n- 'add task' (opens Tasks tab)\n- 'start quiz' (opens Quiz tab)\n- 'show log' (opens Activity Log)\n- 'exit' to quit.";
                AppendBotResponse(helpText);
                return;
            }

            // Otherwise, use chatbot for cybersecurity tips
            chatbot.ProcessInput(input);
        }

        private void btnExit_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();

        // --------------------- Task Management ---------------------
        private void LoadTasks()
        {
            DataTable dt = db.GetTasks();
            lvTasks.ItemsSource = dt.DefaultView;
        }

        private void btnAddTask_Click(object sender, RoutedEventArgs e)
        {
            string title = txtTaskTitle.Text.Trim();
            if (string.IsNullOrEmpty(title))
            {
                MessageBox.Show("Please enter a task title.");
                return;
            }
            string desc = txtTaskDesc.Text.Trim();
            string reminder = dpReminder.SelectedDate?.ToString("yyyy-MM-dd");
            bool success = db.AddTask(title, desc, reminder);
            if (success)
            {
                ActivityLogger.AddEntry($"Task added: '{title}'");
                MessageBox.Show("Task added successfully.");
                txtTaskTitle.Clear();
                txtTaskDesc.Clear();
                dpReminder.SelectedDate = null;
                LoadTasks();
            }
            else
            {
                MessageBox.Show("Failed to add task. Check database connection.");
            }
        }

        private void btnRefreshTasks_Click(object sender, RoutedEventArgs e) => LoadTasks();

        private void btnDeleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (lvTasks.SelectedItem == null) { MessageBox.Show("Select a task."); return; }
            DataRowView row = (DataRowView)lvTasks.SelectedItem;
            int id = Convert.ToInt32(row["id"]);
            if (MessageBox.Show("Delete this task?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (db.DeleteTask(id))
                {
                    ActivityLogger.AddEntry($"Task deleted (ID: {id})");
                    LoadTasks();
                }
            }
        }

        private void btnCompleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (lvTasks.SelectedItem == null) { MessageBox.Show("Select a task."); return; }
            DataRowView row = (DataRowView)lvTasks.SelectedItem;
            int id = Convert.ToInt32(row["id"]);
            if (db.MarkComplete(id))
            {
                ActivityLogger.AddEntry($"Task completed (ID: {id})");
                LoadTasks();
            }
        }

        // --------------------- Quiz ---------------------
        private void btnStartQuiz_Click(object sender, RoutedEventArgs e)
        {
            quiz = new QuizManager();
            ShowQuestion();
            ActivityLogger.AddEntry("Quiz started.");
        }

        private void ShowQuestion()
        {
            if (quiz.HasMoreQuestions())
            {
                var q = quiz.GetCurrentQuestion();
                lblQuestion.Content = q.QuestionText;
                pnlAnswers.Children.Clear();
                int optionIndex = 0;
                foreach (var opt in q.Options)
                {
                    var rb = new RadioButton { Content = opt, Foreground = Brushes.White, Margin = new Thickness(5) };
                    rb.Tag = optionIndex;
                    pnlAnswers.Children.Add(rb);
                    optionIndex++;
                }
                btnSubmitAnswer.Visibility = Visibility.Visible;
                btnNextQuestion.Visibility = Visibility.Collapsed;
                lblFeedback.Content = "";
                lblScore.Content = $"Score: {quiz.Score}/{quiz.TotalQuestions}";
            }
            else
            {
                lblQuestion.Content = "Quiz Finished!";
                pnlAnswers.Children.Clear();
                btnSubmitAnswer.Visibility = Visibility.Collapsed;
                btnNextQuestion.Visibility = Visibility.Collapsed;
                int score = quiz.Score;
                int total = quiz.TotalQuestions;
                lblFeedback.Content = $"Your score: {score}/{total}. " + (score >= total * 0.7 ? "Great job!" : "Keep learning!");
                ActivityLogger.AddEntry($"Quiz completed. Score: {score}/{total}");
            }
        }

        private void btnSubmitAnswer_Click(object sender, RoutedEventArgs e)
        {
            RadioButton selected = null;
            foreach (RadioButton rb in pnlAnswers.Children)
                if (rb.IsChecked == true) { selected = rb; break; }
            if (selected == null) { MessageBox.Show("Select an answer."); return; }

            int selectedIndex = (int)selected.Tag;
            bool correct = quiz.SubmitAnswer(selectedIndex);
            lblFeedback.Content = correct ? "Correct!" : $"Incorrect. The correct answer was: {quiz.GetCorrectAnswer()}";
            btnSubmitAnswer.Visibility = Visibility.Collapsed;
            btnNextQuestion.Visibility = Visibility.Visible;
            ActivityLogger.AddEntry($"Quiz answer: {(correct ? "Correct" : "Wrong")}");
            lblScore.Content = $"Score: {quiz.Score}/{quiz.TotalQuestions}";
        }

        private void btnNextQuestion_Click(object sender, RoutedEventArgs e)
        {
            quiz.NextQuestion();
            ShowQuestion();
        }

        // --------------------- Activity Log ---------------------
        private void btnRefreshLog_Click(object sender, RoutedEventArgs e) => RefreshLogDisplay();
        private void btnShowMoreLog_Click(object sender, RoutedEventArgs e)
        {
            logDisplayCount += 10;
            RefreshLogDisplay();
        }

        private void RefreshLogDisplay()
        {
            var logs = ActivityLogger.GetRecentLogs(logDisplayCount);
            lstActivityLog.ItemsSource = logs;
        }
    }
}