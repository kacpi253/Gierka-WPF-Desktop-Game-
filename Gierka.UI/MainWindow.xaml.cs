using System.Windows;
using System.Windows.Controls;
using Gierka.UI;
using System.Windows.Media;
using Gierka.Core;
using System.ComponentModel;
namespace Gierka.UI
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            // 1. Po uruchomieniu, natychmiast ładujemy ekran logowania
            NavigateTo(new LoginControl());
            MediaManager.Load("Music/lofi.mp3");
            MediaManager.Load("Music/LInternationale (Instrumental Version).mp3");
            MediaManager.Load("Music/March of the Defenders of Moscow (Extended Parade Instrumental).mp3");
            MediaManager.Load("Music/Uz Maršala Tita - With Marshal Tito (Instrumental).mp3");
            MediaManager.Load("Music/Варшавянка - Warszawianka (Instrumental).mp3");
            MediaManager.Load("Music/Марш артиллеристов - Soviet Artillerymans March (Instrumental).mp3");

        }

        public void NavigateTo(UserControl newContent)      //Przełączanie widoków
        {
            // Podmień zawartość naszego kontenera (MainContentArea)
            MainContentArea.Content = newContent;
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            // Sprawdź, czy mamy aktywną sesję do zapisania
            if (SessionContext.CurrentUser != null && SessionContext.CurrentGameState != null)
            {
                // Użyj istniejącej logiki zapisu
                bool success = GameService.SaveGame(
                    SessionContext.CurrentUser,
                    SessionContext.CurrentGameState,
                    "Autosave"
                );

                if (success)
                {
                    // Opcjonalnie: Poinformuj użytkownika
                    MessageBox.Show("Stan gry automatycznie zapisany!", "Zapis Automatyczny");
                }
                // Jeśli zapis się nie powiedzie, aplikacja i tak się zamknie.
            }


        }
    }
}