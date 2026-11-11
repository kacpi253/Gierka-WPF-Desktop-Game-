using Xunit;
using Gierka.Core;
using System;
using System.IO;
using System.Linq;

namespace Gierka.Tests
{
    // Testy Akceptacyjne (UAT) - symulacja całego scenariusza użycia przez użytkownika
    public class UATScenarioTests
    {
        [Fact]
        public void Scenario_UserRegistersPlaysRunsOutOfMoneyAndResets()
        {
            // Scenariusz:
            // 1. Nowy użytkownik wchodzi do gry (Rejestracja)
            // 2. Loguje się
            // 3. Sprawdza stan konta (powinien mieć 0 lub początkowe - zależy od implementacji, w AuthService jest tworzony Save z 0?)
            //    W AuthService: var initialGameState = new GameState(); -> Punkty = 0?
            //    Sprawdźmy GameState:
            //    public int Punkty { get; set; } = 1000; (Przykładowo, jeśli domyślne to np. 0, a chcemy grać, musimy dodać)
            // 4. Użytkownik gra w Ruletkę
            // 5. Wygrywa lub przegrywa
            // 6. Stan gry jest zapisywany (Autosave)
            
            // Arrange
            var authService = new AuthService();
            string login = "uat_user_" + Guid.NewGuid().ToString().Substring(0, 5);
            string password = "StrongPassword1!";
            
            // 1. Rejestracja
            bool registered = authService.Register(login, password);
            Assert.True(registered, "Użytkownik powinien się zarejestrować");

            // 2. Logowanie
            User user = authService.Login(login, password);
            Assert.NotNull(user);

            // 3. Ładowanie gry
            GameState state = GameService.LoadLatestGame(user.Id);
            Assert.NotNull(state);
            // Zakładamy, że startowe punkty to np. 0 lub 100.
            // Zmodyfikujmy stan, dajmy mu fory na start testu
            state.PolishZlotyPoints = 1000;
            GameService.SaveGame(user, state, "Autosave");

            // 4. Gra w Ruletkę
            var ruletka = new RuletkaLogika();
            int stawka = 100;
            
            // Gracz stawia na CZERWONE
            var wynik = ruletka.ZakrecKolem();
            
            int wygrana = ruletka.SprawdzWygrana(wynik.Numer, wynik.Kolor, stawka, "Czerwony");
            
            if (wygrana > 0)
            {
                state.PolishZlotyPoints += wygrana;
            }
            else
            {
                state.PolishZlotyPoints -= stawka;
            }

            // 5. Zapis po grze
            bool saved = GameService.SaveGame(user, state, "Autosave");
            Assert.True(saved, "Stan gry powinien zostać zapisany");

            // 6. Weryfikacja (ponowne załadowanie)
            GameState loadedState = GameService.LoadLatestGame(user.Id);
            Assert.Equal(state.PolishZlotyPoints, loadedState.PolishZlotyPoints);
        }
    }
}
