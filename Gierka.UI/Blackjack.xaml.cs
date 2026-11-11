using Gierka.Core;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Gierka.UI
{
    public partial class Blackjack : UserControl
    {
        private BlackjackLogika _logic = new BlackjackLogika();
        private List<int> _playerHand = new List<int>();
        private List<int> _dealerHand = new List<int>();
        private bool _gameInProgress = false;
        private int _currentBet = 0;

        public Blackjack()
        {
            InitializeComponent();
            this.Loaded += Blackjack_Loaded;
        }

        private void Blackjack_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateWallet();
        }

        private void StartGame(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(BetInput.Text, out int bet) && bet > 0)
            {
                if (SessionContext.CurrentGameState.PolishZlotyPoints < bet)
                {
                    StatusText.Text = "Brak środków, obywatelu!";
                    return;
                }

                _currentBet = bet;
                SessionContext.CurrentGameState.PolishZlotyPoints -= _currentBet;
                UpdateWallet();

                _gameInProgress = true;
                _playerHand.Clear();
                _dealerHand.Clear();

                BtnDeal.IsEnabled = false;
                BetInput.IsEnabled = false;
                BtnHit.IsEnabled = true;
                BtnStand.IsEnabled = true;

                // Initial Deal
                _playerHand.Add(_logic.CiagnijKarte());
                _dealerHand.Add(_logic.CiagnijKarte());
                _playerHand.Add(_logic.CiagnijKarte());
                // Dealer second card hidden logic implies we just draw it later or draw but hide.
                // Simplified: Dealer draws 1 visible card now. Second one comes at the end.
                
                UpdateUI();
                CheckBustOrBlackjack();
            }
            else
            {
                StatusText.Text = "Wpisz poprawną stawkę!";
            }
        }

        private void Hit(object sender, RoutedEventArgs e)
        {
            PlayerDraw();
            UpdateUI();
            CheckBustOrBlackjack();
        }

        private void Stand(object sender, RoutedEventArgs e)
        {
            EndGame();
        }

        private void PlayerDraw()
        {
            _playerHand.Add(_logic.CiagnijKarte());
        }

        private void CheckBustOrBlackjack()
        {
            int playerScore = _logic.ObliczPunkty(_playerHand);
            if (playerScore > 21)
            {
                StatusText.Text = "Fura! Przegrałeś (Mieszkanie przepadło).";
                BtnHit.IsEnabled = false;
                BtnStand.IsEnabled = false;
                FinishGame(false);
            }
            else if (playerScore == 21)
            {
                 // Auto stand or just let user click stand? Let's just update UI.
                 StatusText.Text = "BlackJack! Masz 21!";
            }
        }

        private async void EndGame()
        {
            // Dealer draws
            while (_logic.ObliczPunkty(_dealerHand) < 17)
            {
                await Task.Delay(800);
                _dealerHand.Add(_logic.CiagnijKarte());
                UpdateUI(showDealer: true);
            }

           // UpdateUI(showDealer: true);

            int playerScore = _logic.ObliczPunkty(_playerHand);
            int dealerScore = _logic.ObliczPunkty(_dealerHand);


            if (playerScore == 21 && dealerScore !=21)
            {
                StatusText.Text = $"Wygrałeś {_currentBet * 3} PLN! BlackJack! Władza gratuluje.";
                SessionContext.CurrentGameState.PolishZlotyPoints += _currentBet * 3;
                FinishGame(true);
                UpdateWallet();
                return;
            }
            else if (dealerScore > 21 || playerScore > dealerScore)
            {
                StatusText.Text = $"Wygrałeś {_currentBet * 2} PLN! Władza gratuluje.";
                SessionContext.CurrentGameState.PolishZlotyPoints += _currentBet * 2;
                FinishGame(true);
            }
            else if (playerScore == dealerScore)
            {
                 StatusText.Text = "Remis. Zwrot stawki.";
                 SessionContext.CurrentGameState.PolishZlotyPoints += _currentBet;
                 FinishGame(true); // Technically not a win, but return handling
            }
            else
            {
                StatusText.Text = "Przegrałeś. Partia bierze wszystko.";
                FinishGame(false);
            }
            UpdateWallet();
        }

        private void FinishGame(bool won)
        {
            _gameInProgress = false;
            BtnDeal.IsEnabled = true;
            BetInput.IsEnabled = true;
            BtnHit.IsEnabled = false;
            BtnStand.IsEnabled = false;
        }

        private void UpdateUI(bool showDealer = false)
        {
            // Rendery kart
            RenderCards(PlayerCardsPanel, _playerHand);
            RenderCards(DealerCardsPanel, _dealerHand); // Simplified: always show all dealer cards drawn so far
            
            PlayerScoreText.Text = $"Punkty: {_logic.ObliczPunkty(_playerHand)}";
            
            // Dealer score
            if (_dealerHand.Count > 0)
                DealerScoreText.Text = $"Punkty: {_logic.ObliczPunkty(_dealerHand)}";
            else
                DealerScoreText.Text = "Punkty: 0";
        }
        
        // Helper to update global wallet display (if we could reach it easier, but for now just updating Logic)
        private void UpdateWallet()
        {
             if (SessionContext.CurrentGameState != null)
             {
                 WalletText.Text = $"Twoje złotówki: {SessionContext.CurrentGameState.PolishZlotyPoints} PLN";
             }
        }

        private void RenderCards(StackPanel panel, List<int> cards)
        {
            panel.Children.Clear();
            foreach (var card in cards)
            {
                Border b = new Border
                {
                    Width = 90,
                    Height = 135,
                    CornerRadius = new CornerRadius(5),
                    Background = Brushes.White,
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(1),
                    Margin = new Thickness(5)
                };

                TextBlock tb = new TextBlock
                {
                    Text = _logic.NazwaKarty(card),
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    FontWeight = FontWeights.Bold,
                    FontSize = 40
                };
                
                // Color for suits (simulated)
                // Just random color/suit or none for now.
                
                b.Child = tb;
                panel.Children.Add(b);
            }
        }

        private void BackToCasino(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            mainWindow?.NavigateTo(new Casino());
        }
    }
}
