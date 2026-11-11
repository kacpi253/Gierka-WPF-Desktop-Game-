using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Gierka.Core; // Pamiętaj o dodaniu usinga do Core

namespace Gierka.UI
{
    public partial class JednorekiGierek : UserControl
    {

        private JednorekiLogika _logika = new JednorekiLogika();
        private Random _randomUI = new Random(); //  efekt wizualny

        private string GetSciezka(SymbolTyp symbol)
        {
            return $"pack://application:,,,/Gierka.UI;component/Images/SymboleAutomat/symbol_{symbol.ToString().ToLower().ToUpper().ToLower()}.png";
        }

        public JednorekiGierek()
        {
            InitializeComponent();
            // Ustawienie grafik startowych
            UstawObrazek(Beben1, SymbolTyp.Zeton);
            UstawObrazek(Beben2, SymbolTyp.Zeton);
            UstawObrazek(Beben3, SymbolTyp.Zeton);
            UpdateWallet();

        }

        private async void BtnSpin_Click(object sender, RoutedEventArgs e)
        {
            BtnSpin.IsEnabled = false; 
            TxtInfo.Text = "Maszyna losująca ruszyła...";

            int stawka = 10;
            if (SessionContext.CurrentGameState == null || SessionContext.CurrentGameState.PolishZlotyPoints < stawka)
            {
                TxtInfo.Text = "Obywatelu! Brak Ci złotówek na hazard!";
                BtnSpin.IsEnabled = true;
                return;
            }
            SessionContext.CurrentGameState.PolishZlotyPoints -= stawka;
            UpdateWallet();

            // 1. Pobieramy wynik z CORE (Logic)
            // Logika w Core zwraca wynik i wygraną (mnożnik stawki)
            var wynikGry = _logika.Zakrec(stawka);

            // 2. Animacja "Spin"
            // Uruchamiamy 3 niezależne taski (asynchroniczne procesy)
            var t1 = KrecBebnem(Beben1, 20, wynikGry.Wynik[0]); // Krótko
            var t2 = KrecBebnem(Beben2, 30, wynikGry.Wynik[1]); // Średnio
            var t3 = KrecBebnem(Beben3, 40, wynikGry.Wynik[2]); // Długo 

            await Task.WhenAll(t1, t2, t3);

            // 3. Obsługa wyniku i aktualizacja zasobów
            if (wynikGry.Wygrana > 0)
            {
                SessionContext.CurrentGameState.PolishZlotyPoints += wynikGry.Wygrana;
                TxtInfo.Text = $"WYGRAŁEŚ {wynikGry.Wygrana} ZŁ! GRATULACJE OBYWATELU!";
            }
            else
            {
                TxtInfo.Text = $"Straciłeś {stawka} Zł. Spróbuj ponownie. Partia liczy na Ciebie.";
            }

            // 4. Aktualizacja wyświetlanych zasobów w tle (na kontrolce Gra)
            AktualizujStanGryNaEkranie();

            BtnSpin.IsEnabled = true; // Odblokowanie przycisku
        }

        // Metoda pomocnicza do odświeżania punktów w tle, na kontrolce "Gra"
        private void AktualizujStanGryNaEkranie()
        {
            // Przechodzimy z JednorekiGierek (UserControl) do głównego okna
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;

            // Sprawdzamy, czy aktualnie wyświetlana jest kontrolka "Gra"
            if (mainWindow != null && mainWindow.MainContentArea.Content is Gra graControl)
            {
                // Wywołujemy metodę odświeżającą zasoby w kontrolce "Gra"
                graControl.AktualizujZasoby();
            }
        }

        // Metoda animująca pojedynczy bęben
        private async Task KrecBebnem(Image obrazek, int iloscMigniec, SymbolTyp wynikKoncowy)
        {
            // Szybkie miganie losowymi obrazkami
            for (int i = 0; i < iloscMigniec; i++)
            {
                // Losujemy byle co, żeby tylko migało
                SymbolTyp losowy = (SymbolTyp)_randomUI.Next(0, 5);
                UstawObrazek(obrazek, losowy);

                // Czekamy chwilę (symulacja obrotu)
                await Task.Delay(50 + (i * 5)); // Coraz wolniej pod koniec
            }

            // Na samym końcu ustawiamy ten właściwy wynik z Core
            UstawObrazek(obrazek, wynikKoncowy);
        }

        private void UstawObrazek(Image img, SymbolTyp symbol)
        {
            try
            {
                img.Source = new BitmapImage(new Uri(GetSciezka(symbol)));
            }
            catch
            {
                // Zabezpieczenie jakby brakowało pliku
            }
        }


        private void UpdateWallet()
        {
            if (SessionContext.CurrentGameState != null)
            {
                WalletText.Text = $"Twoje złotówki: {SessionContext.CurrentGameState.PolishZlotyPoints} PLN";
            }
        }


        private void Powrot(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;

            if (mainWindow != null)
            {



                mainWindow.NavigateTo(new Casino());

            }

        }
    }
}