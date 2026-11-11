using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
namespace Gierka.UI
{
    public static class MediaManager
    {
        static MediaManager()
        {
            _player.Volume = 0.5; // Ustawienie głośności na 20%
            _player.MediaEnded += (s, e) => _player.Position = TimeSpan.Zero; // Zapętlanie
        }
        private static void Player_MediaEnded(object sender, EventArgs e)
        {
            _player.Position = TimeSpan.Zero;
            _player.Play();
        }

        private static readonly MediaPlayer _player = new MediaPlayer();
        public static void Load(string fileName)
        {
            try
            {
                _player.Open(new Uri(fileName, UriKind.Relative));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd ładowania pliku: {ex.Message}");
            }
            //dfdf
        }
        public static void Play()
        {
            _player.Position = TimeSpan.Zero;
            _player.Play();
        }
        public static void Stop()
        {
            _player.Stop();
        }
        public static void Pause() {
            _player.Pause();
        }

        public static void SetVolume(double volume)
        {
            // Ensure volume is between 0.0 and 1.0
            if (volume < 0) volume = 0;
            if (volume > 1) volume = 1;
            _player.Volume = volume;
        }
    }
}
