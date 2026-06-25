using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberAwarenessChatbotGUI
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ReminderDate { get; set; }  // store as string for display
        public bool IsCompleted { get; set; }
    }
}