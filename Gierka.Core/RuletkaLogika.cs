using System;

namespace Gierka.Core
{
    public enum KolorRuletki
    {
        Czerwony,
        Czarny,
        Zielony
    }

    public class RuletkaLogika
    {
        private Random _random = new Random();

        // Standardowe koło ruletki europejskiej: 0-36
        // 0 - Zielony
        // Czerwone: 1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36
        // Czarne: 2, 4, 6, 8, 10, 11, 13, 15, 17, 20, 22, 24, 26, 28, 29, 31, 33, 35

        private static readonly int[] CzerwoneLiczby = { 1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36 };

        public (int Numer, KolorRuletki Kolor) ZakrecKolem()
        {
            int numer = _random.Next(0, 37); // 0-36
            KolorRuletki kolor;

            if (numer == 0)
            {
                kolor = KolorRuletki.Zielony;
            }
            else if (IsCzerwony(numer))
            {
                kolor = KolorRuletki.Czerwony;
            }
            else
            {
                kolor = KolorRuletki.Czarny;
            }

            return (numer, kolor);
        }

        private bool IsCzerwony(int numer)
        {
            foreach (int n in CzerwoneLiczby)
            {
                if (n == numer) return true;
            }
            return false;
        }

        public int SprawdzWygrana(int wylosowanyNumer, KolorRuletki wylosowanyKolor, int stawka, string typZakladu, int? numerZakladu = null)
        {
            // typZakladu: "Numer", "Czerwony", "Czarny", "Parzyste", "Nieparzyste"
            
            int wygrana = 0;

            switch (typZakladu)
            {
                case "Numer":
                    if (numerZakladu.HasValue && numerZakladu.Value == wylosowanyNumer)
                    {
                        wygrana = stawka * 35; // 35:1
                    }
                    break;

                case "Czerwony":
                    if (wylosowanyKolor == KolorRuletki.Czerwony)
                    {
                        wygrana = stawka * 2; // 1:1 (zwraca stawkę + drugie tyle)
                    }
                    break;
                case "Czarny":
                    if (wylosowanyKolor == KolorRuletki.Czarny)
                    {
                        wygrana = stawka * 2;
                    }
                    break;
                case "Parzyste": // 0 nie jest ani parzyste, ani nieparzyste w ruletce (przegrywa)
                    if (wylosowanyNumer != 0 && wylosowanyNumer % 2 == 0)
                    {
                         wygrana = stawka * 2;
                    }
                    break;
                 case "Nieparzyste":
                    if (wylosowanyNumer != 0 && wylosowanyNumer % 2 != 0)
                    {
                         wygrana = stawka * 2;
                    }
                    break;
            }

            return wygrana;
        }
    }
}
