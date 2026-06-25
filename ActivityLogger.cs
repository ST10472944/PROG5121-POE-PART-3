using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberAwarenessChatbotGUI
{
    public static class ActivityLogger
    {
        private static List<string> _log = new List<string>();

        public static void AddEntry(string action)
        {
            string entry = $"{DateTime.Now:yyyy-MM-dd HH:mm} - {action}";
            _log.Add(entry);
            if (_log.Count > 100) _log.RemoveAt(0);
        }

        public static List<string> GetRecentLogs(int count = 10)
        {
            int start = Math.Max(0, _log.Count - count);
            return _log.GetRange(start, _log.Count - start);
        }

        public static List<string> GetAllLogs() => _log;
    }
}