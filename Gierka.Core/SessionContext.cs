// W Gierka.Core/SessionContext.cs
namespace Gierka.Core
{
    public static class SessionContext
    {
        // Przechowuje zalogowanego Usera (ustawiane przy logowaniu)
        public static User CurrentUser { get; set; }

        // Przechowuje aktualny stan gry (modyfikowane przy dodawaniu punktów)
        public static GameState CurrentGameState { get; set; }
    }
}