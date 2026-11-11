using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gierka.Core
{
    public class User
    {
        // Klucz główny
        public int Id { get; set; }

        // Wymagany login
        public string Login { get; set; }

        // Zahaszowane hasło
        public string PasswordHash { get; set; }

        // Właściwość nawigacyjna do zapisów gry (relacja 1:wiele)
        public virtual ICollection<GameSave> GameSaves { get; set; }
    }
}
