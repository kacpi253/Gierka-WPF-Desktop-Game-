using System.Windows;
using System.Windows.Controls;
using Gierka.Core;

namespace Gierka.UI
{
    public partial class RankingControl : UserControl
    {
        public RankingControl()
        {
            InitializeComponent();
            LoadRanking();
        }

        private void LoadRanking()
        {
            var ranking = RankingManager.LoadRanking();
            RankingGrid.ItemsSource = ranking;
        }

        private void Powrot(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.NavigateTo(new MainMenuControl());
            }
        }
    }
}
