using Gierka.Core;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace Gierka.UI
{

    public partial class Event : Window
    {

        void ExecuteCommand(string text, int amount)
        {
            if (commandMap.TryGetValue(text, out var action))
            {
                action(amount);
            }
            else
            {
                Console.WriteLine($"Unknown command: {text}");
            }
        }

        Dictionary<string, Action<int>> commandMap = new()
        {
            { "Punkt Polityczny",   amount => AddPP(amount) },
            { "Punkt Strajku",      amount => AddPS(amount) },
            { "Punkt Interwencji",  amount => AddPI(amount) },
            { "Punkt Militarny",    amount => AddPM(amount) },
            { "zł",                 amount => Addzl(amount) },
            { "funty izraelskie",   amount => Addfi(amount) }
        };

        private static void Addfi(int amount)
        {
            SessionContext.CurrentGameState.IsraelFuntsPoints += amount;
        }
        private static void Addzl(int amount)
        {
            SessionContext.CurrentGameState.PolishZlotyPoints += amount;
        }
        private static void AddPM(int amount)
        {
            SessionContext.CurrentGameState.MilitaryPoints += amount;
        }
        private static void AddPP(int amount)
        {
            SessionContext.CurrentGameState.PoliticalPoints += amount;
        }
        private static void AddPS(int amount)
        {
            SessionContext.CurrentGameState.StrikePoints += amount;
        }
        private static void AddPI(int amount)
        {
            SessionContext.CurrentGameState.InterventionPoints += amount;
        }

        public int SelectedOption { get; private set; } = 0; // 0=None, 1=Yes, 2=No
        private string choice1;
        private string choice2;

        // Konstruktor dla standardowych eventów losowych
        public Event(int shift=0)
        {
            var rand = new Random();

            InitializeComponent();

            using StreamReader reader = new ("../../../Events/events.txt");
            string text = reader.ReadToEnd();
            string[] text2 = text.Split('\n');
                       
            int r = rand.Next((text2.Length-1) / 9);
            r = (r * 3) + (shift * 10);
           
           

            MessageText.Text = text2[r];
            choice1 = text2[r+1];
            YesButton.Content = choice1;
            choice2 = text2[r+2];
            NoButton.Content = choice2;
        }

        // Konstruktor dla Questów (Custom Text)
        public Event(string message, string opt1, string opt2)
        {
            InitializeComponent();
            MessageText.Text = message;
            
            choice1 = opt1;
            YesButton.Content = opt1.Split('”').Last().Trim().Length > 0 ? opt1 : opt1.TrimStart('-').TrimEnd('-'); // Simplified display
            
            choice2 = opt2;
            NoButton.Content = opt2.Split('”').Last().Trim().Length > 0 ? opt2 : opt2.TrimStart('-').TrimEnd('-');
        }
        private void Yes_Click(object sender, RoutedEventArgs e)
        {
            SelectedOption = 1;

            try 
            {
                if (choice1.Contains('”')) 
                {
                    string[] test = choice1.Split('”');
                    if (test.Length > 1) 
                    {
                        string[] words = test[1].Split(',');

                        foreach (var word in words)
                        {
                            string trimmed = word.Trim();
                            if (string.IsNullOrEmpty(trimmed)) continue;

                            string[] parts = trimmed.Split(' ', 2);
                            string numberPart = parts[0];
                            string numberToken = numberPart.Replace("+", "");
                            if (!int.TryParse(numberToken, out int number))
                                continue;
                            string textPart = parts.Length > 1 ? parts[1] : "";
                            ExecuteCommand(textPart, number);
                        }
                    }
                }
            } 
            catch { } // Ignore format errors for simple dialogue options

            Close();
        }

        private void No_Click(object sender, RoutedEventArgs e)
        {
            SelectedOption = 2;

            try 
            {
                if (choice2.Contains('”')) 
                {
                    string[] test = choice2.Split('”');
                    if (test.Length > 1)
                    {
                        string[] words = test[1].Split(',');

                        foreach (var word in words)
                        {
                            string trimmed = word.Trim();
                            if (string.IsNullOrEmpty(trimmed)) continue;

                            string[] parts = trimmed.Split(' ', 2);
                            string numberPart = parts[0];
                            string numberToken = numberPart.Replace("+", "");
                            if (!int.TryParse(numberToken, out int number))
                                continue;
                            string textPart = parts.Length > 1 ? parts[1] : "";
                            ExecuteCommand(textPart, number);
                        }
                    }
                }
            }
            catch { }

            Close();
        }
    }
}