using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace CyberAwarenessChatbotGUI
{
    public class DatabaseHelper
    {
        // Change the password to your MySQL root password
        private string connectionString = "server=localhost;database=cyberbot;uid=root;pwd=#THAPEL0mkt;";

        public bool AddTask(string title, string description, string reminderDate)
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "INSERT INTO tasks (title, description, reminder_date) VALUES (@title, @desc, @reminder)";
                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@title", title);
                        cmd.Parameters.AddWithValue("@desc", (object)description ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@reminder", string.IsNullOrEmpty(reminderDate) ? (object)DBNull.Value : reminderDate);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"DB Error: {ex.Message}");
                return false;
            }
        }

        public DataTable GetTasks(bool includeCompleted = false)
        {
            DataTable dt = new DataTable();
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = includeCompleted ? "SELECT * FROM tasks ORDER BY id DESC" : "SELECT * FROM tasks WHERE is_completed = FALSE ORDER BY id DESC";
                    using (var adapter = new MySqlDataAdapter(sql, conn))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"DB Error: {ex.Message}");
            }
            return dt;
        }

        public bool DeleteTask(int id)
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "DELETE FROM tasks WHERE id = @id";
                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch { return false; }
        }

        public bool MarkComplete(int id)
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "UPDATE tasks SET is_completed = TRUE WHERE id = @id";
                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch { return false; }
        }
    }
}