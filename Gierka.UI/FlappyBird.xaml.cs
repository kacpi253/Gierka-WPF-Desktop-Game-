using System;
using System.IO; // <--- To jest konieczne do operacji na plikach
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Gierka.UI
{
    public partial class FlappyGameControl : UserControl
    {
        DispatcherTimer gameTimer = new DispatcherTimer();
        double score;
        int gravity = 8;
        bool gameOver;
        Rect planeHitBox;

        public FlappyGameControl()
        {
            InitializeComponent();

            gameTimer.Tick += GameEngine;
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // 1. Fokus na grę
            this.Focus();
            MyCanvas.Focus();

            // 2. Start logiki gry
            StartGame();
        }

        // --- PONIŻEJ STANDARDOWA LOGIKA GRY (BEZ ZMIAN) ---

        private void GameEngine(object sender, EventArgs e)
        {
            Canvas.SetTop(flappyPlane, Canvas.GetTop(flappyPlane) + gravity);
            Canvas.SetLeft(pipeBottom, Canvas.GetLeft(pipeBottom) - 8);
            Canvas.SetLeft(pipeTop, Canvas.GetLeft(pipeTop) - 8);

            scoreText.Content = "Aktualna głośność: " + score;

            if (Canvas.GetLeft(pipeBottom) < -80)
            {
                Canvas.SetLeft(pipeBottom, 800);
                score += 1;
            }
            if (Canvas.GetLeft(pipeTop) < -80)
            {
                Canvas.SetLeft(pipeTop, 950);
                score += 1;
            }

            planeHitBox = new Rect(Canvas.GetLeft(flappyPlane), Canvas.GetTop(flappyPlane), flappyPlane.Width, flappyPlane.Height);
            Rect pipeBottomHitBox = new Rect(Canvas.GetLeft(pipeBottom), Canvas.GetTop(pipeBottom), pipeBottom.Width, pipeBottom.Height);
            Rect pipeTopHitBox = new Rect(Canvas.GetLeft(pipeTop), Canvas.GetTop(pipeTop), pipeTop.Width, pipeTop.Height);

            if (planeHitBox.IntersectsWith(pipeBottomHitBox) ||
                planeHitBox.IntersectsWith(pipeTopHitBox) ||
                Canvas.GetTop(flappyPlane) < -20 ||
                Canvas.GetTop(flappyPlane) > 550)
            {
                EndGame();
            }
        }

        private void Canvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) gravity = -15;
            if (e.Key == Key.Enter && gameOver) StartGame();
        }

        private void Canvas_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) gravity = 8;
        }

        private void StartGame()
        {
            this.Focus();
            gameOver = false;
            score = 0;
            gravity = 8;
            InfoText.Visibility = Visibility.Collapsed;

            Canvas.SetTop(flappyPlane, 200);
            Canvas.SetLeft(pipeBottom, 800);
            Canvas.SetLeft(pipeTop, 950);

            gameTimer.Start();
        }

        private void EndGame()
        {
            gameTimer.Stop();
            gameOver = true;
            InfoText.Text = "Game Over! Enter = Reset";
            InfoText.Visibility = Visibility.Visible;
        }

        private void ExitGame_Click(object sender, RoutedEventArgs e)
        {
            gameTimer.Stop();

            // Save volume
             // Normalize score to 0.0 - 1.0 range assuming reasonable score (e.g., 0-100)
             // Or if score is "volume level" maybe we assume it is between 0 and 100
             double volume = score / 100.0;
             MediaManager.SetVolume(volume);

            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {

                mainWindow.NavigateTo(new SettingsControl());
            }
        }
    }
}