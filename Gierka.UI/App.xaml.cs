using System.Configuration;
using System.Data;
using System.Windows;

namespace Gierka.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Globalne logowanie wyjątków (Monitoring stabilności)
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            
            Gierka.Core.Logger.LogInfo("Aplikacja została uruchomiona.");
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            // Logowanie błędu krytycznego
            Gierka.Core.Logger.LogError("Nieobsłużony wyjątek w aplikacji UI!", e.Exception);

            // Opcjonalnie: Poinformuj użytkownika
            MessageBox.Show("Wystąpił nieoczekiwany błąd. Aplikacja zostanie zamknięta.\nSzczegóły w pliku application.log", "Błąd Krytyczny", MessageBoxButton.OK, MessageBoxImage.Error);

            // Zapobiegnij domyślnemu zamknięciu, jeśli to możliwe, lub pozwól umrzeć
            // e.Handled = true; // Jeśli ustawisz true, aplikacja nie zniknie (uwaga na niestabilny stan)
        }
    }
}
