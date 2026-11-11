using System.Windows; // Potrzebne do MessageBox i ogólnie WPF
using Gierka.Core;    // Musimy zaimportować nasz projekt Core!
using System.Windows.Controls;
using Gierka.UI;
namespace Gierka.UI
{

   
    public partial class LoginControl : UserControl
    {
        // Tworzymy jedną instancję naszego serwisu logowania
        private readonly AuthService _authService;
        
        public LoginControl()
        {
            InitializeComponent();
            _authService = new AuthService(); // Inicjalizujemy serwis
        }

        // Ta funkcja uruchomi się po kliknięciu "Zaloguj"
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text;
            string password = PasswordTextBox.Password; // Pobieramy hasło z PasswordBox

            var user = _authService.Login(login, password);

            if (user != null)
            {

                // Logowanie udane!
                MessageBox.Show($"Witaj, {user.Login}! Zalogowano pomyślnie.");
                MainWindow mainWindow = Window.GetWindow(this) as MainWindow;

                SessionContext.CurrentUser = user;

                SessionContext.CurrentGameState = GameService.LoadLatestGame(user.Id);     // Ładujemy najnowszy stan gry zalogowanego użytkownika

                if (mainWindow != null)
                {
                    // 2. Użyj jego funkcji NavigateTo, aby wczytać nowy widok (Menu)
                    mainWindow.NavigateTo(new MainMenuControl());
                    
                    mainWindow.Height = SessionContext.CurrentGameState.ResHeight;
                    mainWindow.Width = SessionContext.CurrentGameState.ResWidth;
                }
            }
            else
            {
                // Logowanie nieudane
                MessageBox.Show("Błędny login lub hasło.");
            }
        }

        // Ta funkcja uruchomi się po kliknięciu "Zarejestruj"
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text;
            string password = PasswordTextBox.Password;

            bool success = _authService.Register(login, password);

            if (success)
            {
                MessageBox.Show("Rejestracja pomyślna! Możesz się teraz zalogować.");
            }
            else
            {
                MessageBox.Show("Rejestracja nieudana. (Login może być zajęty lub dane są nieprawidłowe).");
            }
        }
    }
}