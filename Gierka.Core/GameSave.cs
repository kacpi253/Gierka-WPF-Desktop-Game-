using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations; 

namespace Gierka.Core
{
    public class GameSave
    {
        // Klucz główny dla zapisu
        [Key]
        public int SaveId { get; set; }

        public string SaveName { get; set; }                // Np. "Zapis 1 - 1977r."

        // To jest treść naszego zapisu - stan gry jako tekst JSON
        public string GameStateJSON { get; set; }

        // --- Klucz obcy ---
        // To jest ID usera, do którego należy ten zapis
        public int UserId { get; set; }

        // Opcjonalnie: Właściwość nawigacji do właściciela zapisu
        public virtual User User { get; set; }
    }
}
