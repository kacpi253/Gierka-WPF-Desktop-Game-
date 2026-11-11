using System;
using System.Collections.Generic;
using System.Linq;

namespace Gierka.Core
{
    public class BlackjackLogika
    {
        private Random _random = new Random();

        public int ObliczPunkty(List<int> karty)
        {
            int punkty = 0;
            int asy = 0;

            foreach (var karta in karty)
            {
                if (karta == 1) // As
                {
                    asy++;
                    punkty += 11;
                }
                else if (karta >= 10) // Figury (Walet, Dama, Król) i 10
                {
                    punkty += 10;
                }
                else
                {
                    punkty += karta;
                }
            }

            // Korekta dla asów, jeśli przekroczono 21
            while (punkty > 21 && asy > 0)
            {
                punkty -= 10;
                asy--;
            }

            return punkty;
        }

        public int CiagnijKarte()
        {
            // Uproszczona talia: 1-13 (1=As, 11=J, 12=Q, 13=K)
            // Nie symulujemy wyczerpywania talii dla uproszczenia
            return _random.Next(1, 14);
        }

        public string NazwaKarty(int wartosc)
        {
            switch (wartosc)
            {
                case 1: return "As";
                case 11: return "J";
                case 12: return "D";
                case 13: return "K";
                default: return wartosc.ToString();
            }
        }
    }
}
