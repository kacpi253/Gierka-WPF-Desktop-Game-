using Gierka.Core;
using System.Windows;
using System.Windows.Controls;


namespace Gierka.UI
{
    public partial class MainMenuControl : UserControl
    {


        public MainMenuControl()
        {
            InitializeComponent();


        }
        public void Wejdz_do_gry(object sender, RoutedEventArgs e)
        {

            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {



                mainWindow.NavigateTo(new Gra());

            }

        }
        private void Przejdz_do_ustawien(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {

                mainWindow.NavigateTo(new SettingsControl());


            }
        }

        private void Przejdz_do_rankingu(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.NavigateTo(new RankingControl());
            }
        }

        private void Wyjdz_z_gry(object sender, RoutedEventArgs e)
        {

            Application.Current.Shutdown();
        }

    }
}
