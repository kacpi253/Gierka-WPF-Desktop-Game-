using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace Gierka.UI
{
    /// <summary>
    /// Logika interakcji dla klasy Gdansk.xaml
    /// </summary>
    public partial class Gdansk : UserControl
    {
        private void LW_Quest(object sender, RoutedEventArgs e)
        {
            var state = Core.SessionContext.CurrentGameState;
            int stage = state.GetQuestStage("MainQuest");

            if (stage == 0)
            {
                var dialog = new Event("Lech Wałęsa: „Potrzebuję załatwić jedną sprawę w sejmie, dasz rady mi pomóc? Postaram się odwdzięczyć.”",
                    "Tak", "Nie");
                dialog.ShowDialog();
                if (dialog.SelectedOption == 1)
                {
                    state.SetQuestStage("MainQuest", 1);
                    MessageBox.Show("Lech Wałęsa: „Porozmawiaj z Edwardem, żeby wprowadził podwyżki. Strajk wisi na włosku, a nikt z nas go nie chce. Lepiej załatwić to polubownie”");
                }
            }
            else if (stage == 4)
            {
                // Finalizacja
                state.SetQuestStage("MainQuest", 5); // Done
                MessageBox.Show("Lech Wałęsa: „Udało ci się. Szczerze to trochę nawet w ciebie nie wierzyłem a tu proszę. Oczywiście wiem jak to u was wygląda. Możesz się nie bać, część tych podwyżek w podziękowaniu trafi do ciebie.”\n(Otrzymano nagrody)");
                
                // Rewards: -2 punkty strajku, +2000 zł, +1 punkt polityczny.
                state.StrikePoints = Math.Max(0, state.StrikePoints - 2);
                state.PolishZlotyPoints += 2000;
                state.PoliticalPoints += 1;
            }
            else
            {
                MessageBox.Show("Lech Wałęsa: „Działaj, działaj!”");
            }
        }
        private void MJ_Quest(object sender, RoutedEventArgs e)
        {
            // Mieczysław Jagielski (Quest 2)
            var state = Core.SessionContext.CurrentGameState;
            int stage = state.GetQuestStage("Quest_Jagielski");

            if (stage == 0)
            {
                var dialog = new Event("Jagielski: „Dobrze, że cię widzę, potrzebuję pilnie pomocy. Stoczniowcy zaczynają protestować, potrzebuję tutaj oddziałów prewencji, ale wojsko nie chce ich zapewnić. Musisz jakoś przegadać Jaruzelskiemu do rozumu.”",
                    "„Załatwię to ale potrzebuję zaliczki” +100 zł", 
                    "„Dam rady, możesz na mnie liczyć” +1 Punkt Polityczny");
                dialog.ShowDialog();
                if (dialog.SelectedOption != 0)
                {
                    state.SetQuestStage("Quest_Jagielski", 1);
                }
            }
            else if (stage == 1)
            {
                var dialog = new Event("Jagielski: „Załatwiłeś już wsparcie?”",
                   "„Jeszcze nie, ale wszystko jest na dobrej drodze”-", "-");
                dialog.ShowDialog();
            }
        }
        private void AW_Quest(object sender, RoutedEventArgs e)
        {
            // Anna Walentynowicz (Quest 3)
            var state = Core.SessionContext.CurrentGameState;
            int stage = state.GetQuestStage("Quest_Walentynowicz");

            if (stage == 0)
            {
                 var dialog = new Event("Walentynowicz: „To wy! To przez was nie mam pracy. Jak ja mam niby wyżywić rodzinę! Pomyśleliście może chociaż przez chwilę o tym?”",
                    "„Bądź cicho bo rozkaże cię aresztować” +1 Punkt Strajku", 
                    "„Porozmawiam z Sekretarzem i zobaczymy co się da zrobić”");
                 dialog.ShowDialog();
                 if (dialog.SelectedOption != 0)
                 {
                     state.SetQuestStage("Quest_Walentynowicz", 1);
                 }
            }
            else
            {
                var dialog = new Event("Walentynowicz: „Czy mogę już wrócić do pracy?”",
                   "„Jeszcze nie, ale wszystko jest na dobrej drodze”-", "-");
                dialog.ShowDialog();
            }
        }
        public Gdansk()
        {
            InitializeComponent();
        }

        private void Powrot(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {



                mainWindow.NavigateTo(new Gra());

            }
        }
    }
}
