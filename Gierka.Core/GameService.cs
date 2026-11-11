// W Gierka.Core/GameService.cs
using System.Linq;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace Gierka.Core
{
    public static class GameService
    {
        //private readonly GameService _gameService = new GameService();
        //
        // W Gierka.Core/GameService.cs, dodaj tę funkcję:

        public static GameState LoadLatestGame(int userId)
        {
            using (var context = new GierkaDbContext())
            {
                // 1. Znajdź ostatni zapis dla tego użytkownika (np. "Autosave")
                var save = context.GameSaves
                                  .Where(s => s.UserId == userId)
                                  .OrderByDescending(s => s.SaveId) // Wybierz najnowszy
                                  .FirstOrDefault();

                if (save == null)
                {
                    return new GameState(); // Jeśli nie ma zapisu, zwróć nowy (0 punktów)
                }

                // 2. Deserializacja (Zamiana tekstu JSON z bazy na obiekt C#)
                // Użyj GameState.cs do odtworzenia obiektu
                GameState loadedState = System.Text.Json.JsonSerializer.Deserialize<GameState>(save.GameStateJSON);

                return loadedState;
            }
        }

        // Funkcja do ZAPISU lub AKTUALIZACJI stanu gry
        public static bool SaveGame(User user, GameState gameState, string saveName)
        {
            if (user == null || gameState == null) return false;

            // 1. Zamień obiekt GameState (punkty) na tekst JSON
            string jsonState = JsonSerializer.Serialize(gameState);

            using (var context = new GierkaDbContext())
            {
                // **Szukaj istniejącego zapisu** (np. "Autosave" dla tego gracza)
                var existingSave = context.GameSaves
                                          .FirstOrDefault(s => s.UserId == user.Id && s.SaveName == saveName);

                if (existingSave != null)
                {
                    // AKTUALIZACJA: Zmień tylko treść JSON-a
                    existingSave.GameStateJSON = jsonState;
                }
                else
                {
                    // TWORZENIE: Stwórz nowy rekord w bazie
                    var newSave = new GameSave
                    {
                        UserId = user.Id,
                        SaveName = saveName,
                        GameStateJSON = jsonState
                    };
                    context.GameSaves.Add(newSave);
                }

                // Zapisz wszystkie zmiany do pliku gierka.db
                context.SaveChanges();

                return true;
            }
        }
    }
}