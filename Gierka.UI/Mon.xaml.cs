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
    /// Logika interakcji dla klasy Mon.xaml
    /// </summary>
    public partial class Mon : UserControl
    {
        private void WJ_Quest(object sender, RoutedEventArgs e)
        {
            // Jaruzelski (Quest Main Stage 2)
            var state = Core.SessionContext.CurrentGameState;
            int stage = state.GetQuestStage("MainQuest");

            if (stage == 2)
            {
                var dialog = new Event("Jaruzelski: „W jakiej sprawie tu przychodzicie?”",
                   "Powiedz o całej sprawie", "Odejdź bez słowa");
                dialog.ShowDialog();
                if (dialog.SelectedOption == 1)
                {
                    // Nested choice
                    var dialog2 = new Event("Jaruzelski: „Budżet MON jest ustalony ustawą, i tego nie zmienimy. Jeżeli mają strajkować, to lepiej mieć dobrze dofinansowane wojsko.”",
                        "„Jestem pewien, że ustawa nie mówi ostatniego słowa” -500 zł", 
                        "Poddaj się i odejdź");
                    dialog2.ShowDialog();
                    if (dialog2.SelectedOption == 1)
                    {
                        MessageBox.Show("Jaruzelski: „Cóż, faktycznie mógłbym się zgodzić na pewne, nazwijmy to, ustępstwa. Jednak lepiej żebyś przegadał to z Sokołowskim. Rosjanie mogą nie być przychylnie zmniejszaniu budżetu armii kraju układu warszawskiego”");
                        state.SetQuestStage("MainQuest", 21); // 21 = Sent to Sokolowski
                    }
                }
            }
        }

        private void MM_Quest(object sender, RoutedEventArgs e)
        {
             // Moczar (Quest 1 - Side)
             // "Musisz mi pomóc. Słyszałem że masz dobre relację z Gierkiem. Potrzebuję, żebyś załatwił mi z nim spotkanie."
            var state = Core.SessionContext.CurrentGameState;
            int stage = state.GetQuestStage("Quest_Moczar");

            if (stage == 0)
            {
                var dialog = new Event("Moczar: „Mój drogi... Potrzebuję, żebyś załatwił mi z Gierkiem spotkanie.”",
                    "„Załatwię to ale potrzebuję zaliczki” +100 zł",
                    "„Dam rady, możesz na mnie liczyć” +1 Punkt Militarny"); 
                dialog.ShowDialog();
                if (dialog.SelectedOption != 0)
                {
                    state.SetQuestStage("Quest_Moczar", 1);
                }
            }
            else if (stage == 1)
            {
                 var dialog = new Event("Moczar: „Załatwiłeś już spotkanie?”",
                    "„Jeszcze nie, ale wszystko jest na dobrej drodze”-", "-");
                 dialog.ShowDialog();
            }
        }

        private void WS_Quest(object sender, RoutedEventArgs e)
        {
            // Sokolowski (Main Quest Stage 21 -> 3)
            var state = Core.SessionContext.CurrentGameState;
            int stage = state.GetQuestStage("MainQuest");

            if (stage == 21)
            {
                var dialog = new Event("Sokołowski: „Priviet tovarishch”",
                   "Powiedz o całej sprawie", "Odejdź bez słowa");
                dialog.ShowDialog();
                if (dialog.SelectedOption == 1)
                {
                     var dialog2 = new Event("Sokołowski: „Net, nie możecie tak sobie zmniejszać budżetu armii. Jesteście naszą flanką, musicie być cały czas przygotowani.”",
                        "„Na pewno nic nie da się zrobić?” -300 funtów izraelskich", 
                        "„To nasza armia i będziemy z nią robić co chcemy” +1 Punkt Interwencji");
                     dialog2.ShowDialog();
                     if (dialog2.SelectedOption != 0)
                     {
                         // Success
                         state.SetQuestStage("MainQuest", 3); // Ready for Gierek
                     }
                }
            }
        }
        public Mon()
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
