using Xunit;
using Gierka.Core;
using System.Diagnostics;

namespace Gierka.Tests
{
    public class PerformanceTests
    {
        [Fact]
        public void Ruletka_SpinningPerformance_ShouldBeFast()
        {
            // Test wydajności: wykonaj 100,000 symulacji koła ruletki
            // Oczekujemy, że zajmie to mniej niż 500ms (z dużym zapasem)
            
            // Arrange
            var ruletka = new RuletkaLogika();
            int iterations = 100000;
            var stopwatch = Stopwatch.StartNew();

            // Act
            for (int i = 0; i < iterations; i++)
            {
                var result = ruletka.ZakrecKolem();
                // 
                Assert.InRange(result.Numer, 0, 36);
            }

            stopwatch.Stop();

            // Assert
            // 100k losowań powinno być błyskawiczne. Dajemy limit 1 sekundy dla pewności na wolniejszych maszynach.
            Assert.True(stopwatch.ElapsedMilliseconds < 1000, $"Performance test failed: {stopwatch.ElapsedMilliseconds}ms for {iterations} iterations");
        }
    }
}
