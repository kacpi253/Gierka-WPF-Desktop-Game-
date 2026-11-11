using Gierka.Core;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Gierka.UI
{
    public partial class Roulette : UserControl
    {
        private RuletkaLogika _logic = new RuletkaLogika();
        private Random _randomUI = new Random();
        private bool _isSpinning = false;

        private string _selectedBetType = null;
        private int? _selectedNumber = null;

        public Roulette()
        {
            InitializeComponent();
            UpdateWallet();
        }

        private void BetRed(object sender, RoutedEventArgs e) { UpdateWallet(); SetBet("Czerwony"); }
        private void BetBlack(object sender, RoutedEventArgs e) { UpdateWallet(); SetBet("Czarny"); }
        private void BetParzyste(object sender, RoutedEventArgs e) { UpdateWallet(); SetBet("Parzyste"); }
        private void BetNieparzyste(object sender, RoutedEventArgs e) { UpdateWallet(); SetBet("Nieparzyste"); }

        private void BetNumber(object sender, RoutedEventArgs e)
        {
            UpdateWallet();
            if (int.TryParse(NumberInput.Text, out int num) && num >= 0 && num <= 36)
            {
                SetBet("Numer", num);
            }
            else
            {
                StatusText.Text = "Podaj poprawny numer (0-36)";
            }
        }

        private void SetBet(string type, int? number = null)
        {
            _selectedBetType = type;
            _selectedNumber = number;
            UpdateWallet();
            if (number.HasValue)
                StatusText.Text = $"Postawiono na: {type} {number}";
            else
                StatusText.Text = $"Postawiono na: {type}";
        }

        private async void SpinWheel(object sender, RoutedEventArgs e)
        {
            if (_isSpinning) return;
            if (_selectedBetType == null)
            {
                StatusText.Text = "Najpierw wybierz zakład!";
                return;
            }

            if (!int.TryParse(BetInput.Text, out int bet) || bet <= 0)
            {
                StatusText.Text = "Nieprawidłowa stawka!";
                return;
            }

            if (SessionContext.CurrentGameState.PolishZlotyPoints < bet)
            {
                StatusText.Text = "Brak funduszy na budowę socjalizmu!";
                return;
            }

            _isSpinning = true;
            WynikLiczba.Foreground = Brushes.White;
            SessionContext.CurrentGameState.PolishZlotyPoints -= bet;
            UpdateWallet();
            BtnSpin.IsEnabled = false;
            StatusText.Text = "Koło się kręci...";
            // Animation
            int spinDuration = 20; // frames
            for (int i = 0; i < spinDuration; i++)
            {
                int tempNum = _randomUI.Next(0, 37);
                WynikLiczba.Text = tempNum.ToString();
                
                // Color simulation
                if (tempNum == 0) border.Background = Brushes.Green;
                else if (tempNum % 2 == 0) border.Background = Brushes.Black; // Simplified vis
                else border.Background = Brushes.Red;

                await Task.Delay(50 + (i * 10));
            }

            // Real Result
            var result = _logic.ZakrecKolem();
            WynikLiczba.Text = result.Numer.ToString();
            
            string colorName = "";
            if (result.Kolor == KolorRuletki.Zielony) { border.Background = Brushes.Green; colorName = "Zielony"; }
            else if (result.Kolor == KolorRuletki.Czarny) { border.Background = Brushes.Black; colorName = "Czarny"; }
            else { border.Background = Brushes.Red; colorName = "Czerwony"; }
            
            WynikKolorText.Text = colorName;

            // Check Win
            int winAmount = _logic.SprawdzWygrana(result.Numer, result.Kolor, bet, _selectedBetType, _selectedNumber);

            if (winAmount > 0)
            {
                SessionContext.CurrentGameState.PolishZlotyPoints += winAmount;
                UpdateWallet();
                StatusText.Text = $"Wygrałeś {winAmount} PLN! Bogacisz się Obywatelu!";
                StatusText.Foreground = Brushes.Yellow;
                StatusText.FontWeight = FontWeights.Bold;
            }
            else
            {
                StatusText.Text = "Przegraną przyjmij z godnością.";
                StatusText.Foreground = Brushes.White;
            }

            _isSpinning = false;
            BtnSpin.IsEnabled = true;
        }

        private void UpdateWallet()
        {
            if (SessionContext.CurrentGameState != null)
            {
                WalletText.Text = $"Twoje złotówki: {SessionContext.CurrentGameState.PolishZlotyPoints} PLN";
            }
        }


        private void BackToCasino(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            mainWindow?.NavigateTo(new Casino());
        }
    }
}
