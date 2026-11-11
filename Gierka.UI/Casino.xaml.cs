using Gierka.Core;
using System.Windows;
using System.Windows.Controls;

namespace Gierka.UI
{
    public partial class Casino : UserControl
    {
        public Casino()
        {
            InitializeComponent();
            WalletText.Text = $"Twoje z³otówki: {SessionContext.CurrentGameState.PolishZlotyPoints} PLN";
        }

        private void OpenJednoreki(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            mainWindow?.NavigateTo(new JednorekiGierek());
        }

        private void OpenBlackjack(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            mainWindow?.NavigateTo(new Blackjack());
        }

        private void OpenRoulette(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            mainWindow?.NavigateTo(new Roulette());
        }

        private void BackToGame(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            mainWindow?.NavigateTo(new Gra());
        }
    }
}
