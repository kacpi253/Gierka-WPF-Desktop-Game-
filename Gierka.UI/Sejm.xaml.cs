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
    /// Logika interakcji dla klasy Sejm.xaml
    /// </summary>
    public partial class Sejm : UserControl
    {
        public Sejm()
        {
            InitializeComponent();
        }
        private void JKM_Quest(object sender, RoutedEventArgs e)
        {
            // Korwin
            MessageBox.Show("Korwin: „Proszę pana, jak powiedział wybitny krakowianin, yyyy Stanisław Lem, yyyy: wwww, na dnie oceanu, w morzu, w rowie mariańskim są RURURU RURKOWCE”.\n(Bohater: „Jakiś szaleniec” - odchodzi)");
        }
        private void JC_Quest(object sender, RoutedEventArgs e)
        {
             // Józef Cyrankiewicz - Not specified in user prompt? 
             // Maybe placeholder or side talk.
             MessageBox.Show("Józef Cyrankiewicz: „Praca wre, towarzyszu.”");
        }
        private void EG_Quest(object sender, RoutedEventArgs e)
        {
            // Edward Gierek (Main Quest Stage 1 & 3 & 4)
            var state = Core.SessionContext.CurrentGameState;
            int stage = state.GetQuestStage("MainQuest");

            if (stage == 1)
            {
                var dialog = new Event("Edward Gierek: „O co chodzi towarzyszu?”",
                   "Powiedz o całej sprawie", "Odejdź bez słowa");
                dialog.ShowDialog();
                if (dialog.SelectedOption == 1)
                {
                    MessageBox.Show("Edward Gierek: „Sacre Bleu, jak to podwyżki. Partia nie ma pieniędzy. Musielibyśmy zabrać wojsku…… Dobra słuchaj. Musisz pójść do Jaruzelskiego i mu to wytłumaczyć. Ja jestem gotowy dać im podwyżki, ale muszę mieć pewność, że armia się nie zbuntuje. Liczę na ciebie.”");
                    state.SetQuestStage("MainQuest", 2);
                }
            }
            else if (stage == 3) // Returning from military
            {
                 MessageBox.Show("Edward Gierek: „A więc udało ci się załatwić tą sprawę. Świetnie, idź teraz powiedz Bolkowi, żeby tymi podwyżkami uspokoił sytuację”");
                 state.SetQuestStage("MainQuest", 4);
            }
            else
            {
                // Quest 4: Secret party mission?
                var dialog = new Event("Edward Gierek: „Mój drogi, obserwuję cię już od dłuższego czasu. Widzę że jesteś bardzo zaangażowany w działanie naszej partii. Jeżeli byłbyś w stanie jakoś udowodnić mi swoje oddanie partii to mógłbym cię wprowadzić w ciekawe miejsce.”",
                    "„Mam nadzieję że to załatwi sprawę” -1000 zł", 
                    "„Opinie współtowarzyszy powinny wystarczyć jako dowód oddania” -3 punkty polityczne");
                // Option 3 is separate "Pomyślę o waszej propozycji i tu wrócę"
                
                dialog.ShowDialog();
                // Logic handled by Event internally for rewards?
                // Actually rewards like -1000 zł are handled by Event automatically if parsed.
                // But Stage update needs manual check? 
                // The prompt implies this unlocks "Casino" -> "ciekawe miejsce"?
                // Or "Wprowadzę cię w ciekawe miejsce".
                
                /* 
                 * "Casino" button exists in MainWindow/Gra.
                 * Maybe this unlocks access?
                 */
            }
        }
        private void Powrot(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {



                mainWindow.NavigateTo(new Gra());

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
