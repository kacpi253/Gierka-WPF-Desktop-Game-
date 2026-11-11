using Gierka.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Gierka.UI;


namespace Gierka.UI
{
    public partial class SettingsControl : UserControl
    {


        public SettingsControl()
        {
            InitializeComponent();
        }

        // W xaml wystarczy tylko skopiowac przycisk i zmienic wartosc w polu Content
        // funckja bierze informacje o przycisku, pobiera dane z pola Content
        // i wpisuje je do bazy danych jednoczesnie zmieniajac aktualna rozdzielczosc
        // musi byc ona wieksza od 399 na 399
        private void Change_Resolution(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;


            Button btn = (Button)sender;
            string res = btn.Content.ToString();

            var parts = res.Split('x');
            int width = int.Parse(parts[0]);
            int height = int.Parse(parts[1]);

            if (width > 399 && height > 399)
            {
                mainWindow.Width = width;
                mainWindow.Height = height;

                SessionContext.CurrentGameState.ResHeight = height;
                SessionContext.CurrentGameState.ResWidth = width;
            }
        }

  
        private void Powrot(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;

            if (mainWindow != null)
            {
                mainWindow.NavigateTo(new MainMenuControl());
            }
        }

        private void GoToVolumeGame(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.NavigateTo(new FlappyGameControl());
            }
        }
    }
}
