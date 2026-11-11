// W Gierka.Core/GameState.cs
namespace Gierka.Core
{
    public class GameState
    {
        // Punkty Wpływu - będą modyfikowane
        public int PolishZlotyPoints { get; set; } = 0;
        public int PoliticalPoints{ get; set; } = 0;
        public int MilitaryPoints { get; set; } = 0;
        public int IsraelFuntsPoints { get; set; } = 0;
        public int StrikePoints { get; set; } = 0;
        public int InterventionPoints { get; set; } = 0;


        // Inne parametry stanu gry
        public int CurrentTurnYear { get; set; } = 1976;
        public int liczbaTur{ get; set; } = 0;

        public string LastLocation { get; set; } = "Sejm";

        public float ResHeight{ get; set; } = 600;
        public float ResWidth{ get; set; } = 800;

        public int ActionPoints { get; set; } = 4;

        // System Questów
        public System.Collections.Generic.Dictionary<string, int> QuestFlags { get; set; } = new System.Collections.Generic.Dictionary<string, int>();

        public int GetQuestStage(string questName)
        {
            return QuestFlags.ContainsKey(questName) ? QuestFlags[questName] : 0;
        }

        public void SetQuestStage(string questName, int stage)
        {
            QuestFlags[questName] = stage;
        }
    }
}