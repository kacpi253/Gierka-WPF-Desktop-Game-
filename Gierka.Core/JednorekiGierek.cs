using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gierka.Core
{
    // Typy symboli dostępne w grze
    public enum SymbolTyp
    {
        Zeton,    // Najczęstszy
        Stocznia,
        MON,
        Lapowka,
        Gierek    // Najrzadszy (Jackpot)
    }

    public class JednorekiLogika
    {
        private Random _random = new Random();

        // Definicja szansy na wylosowanie (Wagi)
        private readonly Dictionary<SymbolTyp, int> _wagi = new Dictionary<SymbolTyp, int>          //Key, value
        {
            { SymbolTyp.Zeton, 50 },
            { SymbolTyp.Stocznia, 25 },
            { SymbolTyp.MON, 15 },
            { SymbolTyp.Lapowka, 8 },
            { SymbolTyp.Gierek, 2 } // Tylko 2% szans
        };

        // Tablica wypłat (mnożnik stawki za 3 takie same symbole)
        private readonly Dictionary<SymbolTyp, int> _wyplaty = new Dictionary<SymbolTyp, int>
        {
            { SymbolTyp.Zeton, 2 },
            { SymbolTyp.Stocznia, 5 },
            { SymbolTyp.MON, 10 },
            { SymbolTyp.Lapowka, 50 },
            { SymbolTyp.Gierek, 100 }
        };

        // Główna metoda losująca
        public (SymbolTyp[] Wynik, int Wygrana) Zakrec(int stawka)
        {
            SymbolTyp[] wynik = new SymbolTyp[3];

            // Losujemy 3 bębny niezależnie
            wynik[0] = WylosujSymbol();
            wynik[1] = WylosujSymbol();
            wynik[2] = WylosujSymbol();

            int wygrana = 0;

            // Sprawdzamy wygraną (tylko gdy 3 są takie same)
            if (wynik[0] == wynik[1] && wynik[1] == wynik[2])
            {
                wygrana = stawka * _wyplaty[wynik[0]];
            }

            return (wynik, wygrana);
        }

        private SymbolTyp WylosujSymbol()
        {
            // Algorytm losowania ważonego (Weighted Random)
            int sumaWag = _wagi.Values.Sum();
            int los = _random.Next(0, sumaWag);
            int biezacaSuma = 0;

            foreach (var kvp in _wagi)
            {
                biezacaSuma += kvp.Value;
                if (los < biezacaSuma)
                    return kvp.Key;
            }
            return SymbolTyp.Zeton; // Zabezpieczenie
        }
    }
}