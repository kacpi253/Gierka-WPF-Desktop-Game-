using Xunit;
using Gierka.Core;
using System;
using System.Text;

namespace Gierka.Tests
{
    public class FuzzTests
    {
        private Random _random = new Random();

        private string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+";
            var stringChars = new char[length];
            for (int i = 0; i < length; i++)
            {
                stringChars[i] = chars[_random.Next(chars.Length)];
            }
            return new string(stringChars);
        }

        [Fact]
        public void Ruletka_SprawdzWygrana_Fuzzing_ShouldNotCrash()
        {
            // Fuzz testing: Podajemy losowe, bezsensowne dane wejściowe
            // Cel: Upewnić się, że metoda nie rzuca nieobsłużonych wyjątków
            
            var ruletka = new RuletkaLogika();
            int iterations = 1000;

            for (int i = 0; i < iterations; i++)
            {
                // Randomize inputs
                int numer = _random.Next(-100, 100); 
                KolorRuletki kolor = (KolorRuletki)_random.Next(0, 5); // 0-2 valid, others invalid potentially if cast works
                int stawka = _random.Next(-1000, 1000);
                string typZakladu = GenerateRandomString(_random.Next(0, 50)); // Random string, possibly empty
                int? numerZakladu = _random.Next(0, 2) == 0 ? null : (int?)_random.Next(-100, 100);

                try
                {
                    // Act
                    var result = ruletka.SprawdzWygrana(numer, kolor, stawka, typZakladu, numerZakladu);
                    
                    // Assert - just survival
                    Assert.True(result >= 0 || result <= 0); // Just check return value exists
                }
                catch (Exception ex)
                {
                    Assert.Fail($"Fuzz test failed (Method Crashed) on iteration {i} with inputs: Numer={numer}, Stawka={stawka}, Typ={typZakladu}. Exception: {ex.Message}");
                }
            }
        }
    }
}
