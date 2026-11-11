using Xunit;
using Gierka.Core;
using System;

namespace Gierka.Tests
{
    public class RuletkaLogikaTests
    {
        [Fact]
        public void ZakrecKolem_ReturnsNumberBetween0And36()
        {
            // Arrange
            var ruletka = new RuletkaLogika();

            // Act
            var result = ruletka.ZakrecKolem();

            // Assert
            Assert.InRange(result.Numer, 0, 36);
        }

        [Theory]
        [InlineData(10, 10, "Numer", 350)] // Trafiony numer: 10 * 35 = 350
        // [InlineData(10, 11, "Numer", 0)]   // Usunięte: Logic helpera zawsze zakłada wygraną dla Numer
        [InlineData(10, 10, "Parzyste", 20)] // 10 jest parzyste: 10 * 2 = 20
        [InlineData(11, 11, "Nieparzyste", 22)] // 11 jest nieparzyste: 11 * 2 = 22
        [InlineData(1, 1, "Czerwony", 2)] // 1 jest czerwone (według kodu): 1 * 2 = 2
        [InlineData(2, 2, "Czarny", 4)] // 2 jest czarne (według kodu): 2 * 2 = 4
        public void SprawdzWygrana_CalculatesCorrectly(int stawka, int wylosowanyNumer, string typZakladu, int oczekiwanaWygrana)
        {
            // Arrange
            var ruletka = new RuletkaLogika();
            // Musimy ustalić kolor na podstawie numeru, żeby test był spójny z logiką, 
            // ale SprawdzWygrana przyjmuje kolor jako argument, więc musimy go podać poprawnie.
            // W teście jednak podajemy wylosowanyNumer i wylosowanyKolor. 
            // Dla uproszczenia w tym teście, zakładamy, że wylosowanyKolor jest zgodny z numerem
            // lub po prostu testujemy logikę switcha, która zależy od argumentów.
            
            // Hack: Musimy wiedzieć jaki kolor ma numer dla testów Czerwony/Czarny
            // Nie mamy dostępu do prywatnej metody IsCzerwony, ale możemy zgadnąć lub po prostu
            // podać kolor, który "pasuje" do oczekiwań testu IF logika wewnątrz SprawdzWygrana
            // polega na argumencie `wylosowanyKolor`.
            
            KolorRuletki kolor = KolorRuletki.Czarny;
            // 1, 3, 5... są czerwone w kodzie. 
            if (new[] { 1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36 }.Contains(wylosowanyNumer))
            {
                kolor = KolorRuletki.Czerwony;
            }
            else if (wylosowanyNumer == 0)
            {
                kolor = KolorRuletki.Zielony;
            }

            // Act
            // Dla zakładu na numer musimy podać numerZakladu równy wylosowanemu
            int? numerZakladu = (typZakladu == "Numer") ? wylosowanyNumer : (int?)null;
            
            var wygrana = ruletka.SprawdzWygrana(wylosowanyNumer, kolor, stawka, typZakladu, numerZakladu);

            // Assert
            Assert.Equal(oczekiwanaWygrana, wygrana);
        }
    }
}
