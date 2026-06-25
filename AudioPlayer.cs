using System;
using System.Media;
using System.IO;  

namespace CyberAwarenessChatbotGUI
{
    public static class AudioPlayer
    {
        public static void PlayGreeting()
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "greeting.wav");
                if (File.Exists(path))
                {
                    using (SoundPlayer player = new SoundPlayer(path))
                    {
                        player.PlaySync();
                    }
                }
            }
            catch (Exception)
            {
                // Silent fail - program continues
            }
        }
    }
}