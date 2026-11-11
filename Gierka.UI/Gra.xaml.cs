using Gierka.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Gierka.UI
{
    /// <summary>
    /// Logika interakcji dla klasy Gra.xaml
    /// </summary>
    public class TurnManager
    {
        public event Action OnTurnAdvanced;
        private readonly GameState state;


        public TurnManager(GameState gameState)
        {
            state = gameState;
        }

        public void NextTurn()
        {
            AdvanceTime();
            SessionContext.CurrentGameState.ActionPoints = 4; // Resetuj punkty akcji
            var pop = new Event();
            pop.ShowDialog();
            OnTurnAdvanced?.Invoke();
        }

        private void AdvanceTime()
        {
            SessionContext.CurrentGameState.liczbaTur++;
            if (SessionContext.CurrentGameState.liczbaTur%4 == 0)
            {
                SessionContext.CurrentGameState.CurrentTurnYear++;
            }
        }
    }

    public partial class Gra : UserControl
    {
        private TurnManager turnManager;


        // private MediaPlayer player = new MediaPlayer();

        public Gra()
        {
            InitializeComponent();

            turnManager = new TurnManager(SessionContext.CurrentGameState);
            turnManager.OnTurnAdvanced += AktualizujZasoby;

            AktualizujZasoby();

            // string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Music", "LInternationale (Instrumental Version).mp3");

            string MuzykaStart = "Music/LInternationale (Instrumental Version).mp3";
            MediaManager.Load(MuzykaStart);
            MediaManager.Play();

            // Ustaw głośność (0.0 - 1.0)
            
            //player.Volume = 0.3;

        }
        private void Dodajevent(object sender, RoutedEventArgs e)
        {
            var pop = new Event();
            pop.ShowDialog();
            AktualizujZasoby();

        }
        private void NextTurn(object sender, RoutedEventArgs e)
        {
            turnManager.NextTurn();
            AktualizujZasoby();

        }
        private void DodajPP(object sender, RoutedEventArgs e)
        {
            SessionContext.CurrentGameState.PoliticalPoints += 10;
            AktualizujZasoby();

        }

        private void DodajPM(object sender, RoutedEventArgs e)
        {
            SessionContext.CurrentGameState.MilitaryPoints += 10;
            AktualizujZasoby();
        }

        private void DodajPS(object sender, RoutedEventArgs e)
        {
            SessionContext.CurrentGameState.StrikePoints += 10;
            AktualizujZasoby();
        }

        private void DodajPI(object sender, RoutedEventArgs e)
        {
            SessionContext.CurrentGameState.InterventionPoints += 10;
            AktualizujZasoby();
        }

        private void DodajPieniadze(object sender, RoutedEventArgs e)
        {
            SessionContext.CurrentGameState.PolishZlotyPoints += 1000;
            AktualizujZasoby();
        }

        private void DodajFuntyIzraelskie(object sender, RoutedEventArgs e)
        {
            SessionContext.CurrentGameState.IsraelFuntsPoints += 10;
            AktualizujZasoby();
        }

        private void Gdansk(object sender, RoutedEventArgs e)
        {
            if (SessionContext.CurrentGameState.ActionPoints <= 0)
            {
                MessageBox.Show("Brak punktów akcji! Zakończ turę, aby odnowić siły.", "Brak akcji", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SessionContext.CurrentGameState.ActionPoints--;
            AktualizujZasoby();

            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {


                mainWindow.NavigateTo(new Gdansk());

            }
        }

        private void Sejm(object sender, RoutedEventArgs e)
        {
            if (SessionContext.CurrentGameState.ActionPoints <= 0)
            {
                MessageBox.Show("Brak punktów akcji! Zakończ turę, aby odnowić siły.", "Brak akcji", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SessionContext.CurrentGameState.ActionPoints--;
            AktualizujZasoby();

            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {


                mainWindow.NavigateTo(new Sejm());

            }
        }
        private void Mon(object sender, RoutedEventArgs e)
        {
            if (SessionContext.CurrentGameState.ActionPoints <= 0)
            {
                MessageBox.Show("Brak punktów akcji! Zakończ turę, aby odnowić siły.", "Brak akcji", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SessionContext.CurrentGameState.ActionPoints--;
            AktualizujZasoby();

            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {


                mainWindow.NavigateTo(new Mon());

            }
        }

        private void OtworzKasyno(object sender, RoutedEventArgs e) {
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;

            if (mainWindow != null)
            {
                string MuzykaStart = "Music/casino.mp3";
                MediaManager.Load(MuzykaStart);
                MediaManager.Play();
                mainWindow.NavigateTo(new Casino());

            }
        }

        private void WrocDoMenu(object sender, RoutedEventArgs e)
        {
            MediaManager.Stop();
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                // Optionally stop current music or handle state logic if needed
                mainWindow.NavigateTo(new MainMenuControl());
            }
        }

        public void AktualizujZasoby()
        {
            // Używamy tego samego kodu, co w konstruktorze, aby odświeżyć wyświetlane liczniki
            rok.Text = SessionContext.CurrentGameState.CurrentTurnYear.ToString();
            if (SessionContext.CurrentGameState.liczbaTur % 4 == 1)
                miesiac.Text = "Wiosna";
            if (SessionContext.CurrentGameState.liczbaTur % 4 == 2)
                miesiac.Text = "Lato";
            if (SessionContext.CurrentGameState.liczbaTur % 4 == 3)
                miesiac.Text = "Jesien";
            if (SessionContext.CurrentGameState.liczbaTur % 4 == 0)
                miesiac.Text = "Zima";

            Wyswietleniepolitycznych.Text = SessionContext.CurrentGameState.PoliticalPoints.ToString();
            Wyswietleniemilitarnych.Text = SessionContext.CurrentGameState.MilitaryPoints.ToString();
            Wyswietleniezlotowek.Text = SessionContext.CurrentGameState.PolishZlotyPoints.ToString();
            WyswietlenieFuntow.Text = SessionContext.CurrentGameState.IsraelFuntsPoints.ToString();
            WyswietlenieAkcji.Text = SessionContext.CurrentGameState.ActionPoints.ToString();

            CheckWinConditions(SessionContext.CurrentGameState);
        }

        private void CheckWinConditions(GameState state)
        {
            if (state.PoliticalPoints >= 10)
            {
                EndGame("Na nadzwyczajnym spotkaniu PZPR zostajesz wybrany na przewodniczącego partii. Wygrałeś towarzyszu.", "Zwycięstwo Polityczne", MessageBoxImage.Information);
                return;
            }

            if (state.MilitaryPoints >= 10)
            {
                EndGame("Razem z wojskiem przeprowadzasz wojskowy zamach stanu i wprowadzasz w Polsce stan wojenny. Wygrałeś towarzyszu.", "Zwycięstwo Militarne", MessageBoxImage.Information);
                return;
            }

            if (state.InterventionPoints >= 10)
            {
                EndGame("ZSRR zaniepokojone sytuacją w Polsce wprowadza swoje siły do naszego kraju. Członkowie partii zostają aresztowani, a na ich miejsce ustanowiony zostaje marionetkowy rząd. Przegraliście towarzyszu.", "Interwencja ZSRR", MessageBoxImage.Error);
                return;
            }

            if (state.StrikePoints >= 10)
            {
                EndGame("Robotnicy znaleźli w sobie wystarczająco dużo siły aby postawić sie władzy. Wojsko co prawda starało się walczyć, ale pod naporem 10 milionów Polaków nawet ono musiało polec. Wszyscy członkowie partii zostają aresztowani i skazani za zdrade ojczyzny. Przegraliście towarzysze.", "Rewolucja", MessageBoxImage.Error);
                return;
            }

            if (state.CurrentTurnYear == 1982)
            {
                EndGame("Wojciech Jaruzelski za poparciem moskwy i wojska wprowadził stan wojenny. Aresztował wszystkich z poprzedniej władzy i rozpoczął rządy twardej pięści. Przegraliście towarzyszu.", "Stan Wojenny", MessageBoxImage.Error);
                return;
            }
        }

        private void EndGame(string message, string title, MessageBoxImage icon)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, icon);
            
            MessageBox.Show("Twoje osiągnięcie zostanie zapisane w rankingu, a konto zostanie trwale usunięte. Gra zostanie teraz zamknięta.", "Koniec gry", MessageBoxButton.OK, MessageBoxImage.Exclamation);

            if (SessionContext.CurrentUser != null)
            {
                try 
                {
                    // 1. Dodaj do rankingu
                    RankingManager.AddEntry(SessionContext.CurrentUser.Login, SessionContext.CurrentGameState.PolishZlotyPoints);

                    // 2. Usuń konto (Permadeath)
                    var auth = new AuthService();
                    auth.DeleteAccount(SessionContext.CurrentUser.Login);

                    // 3. Wyloguj
                    SessionContext.CurrentUser = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Błąd podczas zapisu rankingu lub usuwania konta: {ex.Message}");
                }
            }

            Application.Current.Shutdown();
        }

    }
}
