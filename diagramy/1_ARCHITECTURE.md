# Architektura i Wdrożenie (Architecture & Deployment)

## Diagram Komponentów
Przedstawia podział aplikacji na moduły (`Gierka.UI`, `Gierka.Core`, `Gierka.Tests`) oraz komunikację z bazą danych SQLite za pośrednictwem Entity Framework.

```mermaid
flowchart TD
    subgraph Aplikacja Gierka
        UI["«Component»<br/>Gierka.UI"]
        Core["«Component»<br/>Gierka.Core"]
    end

    Tests["«Component»<br/>Gierka.Tests"]
    DB[("«Database»<br/>SQLite<br/>gierka.db")]

    UI -.->|Używa| Core
    Tests -.->|Testuje| Core
    Core -->|Entity Framework| DB

    classDef component fill:#ff007f,stroke:#333,stroke-width:2px,color:#fff;
    classDef database fill:#ffff99,stroke:#333,stroke-width:2px,color:#aaa;
    class UI,Core,Tests component;
    class DB database;
```

## Diagram Wdrożenia
Obrazuje fizyczne artefakty wdrożeniowe działające w środowisku uruchomieniowym .NET 8 Runtime na komputerze użytkownika.

```mermaid
flowchart LR
    subgraph Komputer Użytkownika
        subgraph Środowisko .NET 8 Runtime
            Exe("«Artifact»<br/>Gierka.exe")
            Dll("«Artifact»<br/>Gierka.Core.dll")
        end
        DB[("«Artifact»<br/>gierka.db")]

        Exe -.->|Zależy od| Dll
        Dll -->|Odczyt/Zapis| DB
    end
```

## Diagram Klas
Zawiera strukturę klas podzieloną na przestrzenie nazw `GierkaUI` oraz `GierkaCore`, kontekst bazy danych `GierkaDbContext` oraz modele encji (`User`, `GameSave`).

```mermaid
classDiagram
    namespace GierkaUI {
        class MainWindow {
            +Login_Click()
        }
        class Gra {
            +GameState CurrentState
            +UpdateUI()
        }
        class Ruletka {
            -RuletkaLogika _logika
            +BtnSpin_Click()
        }
    }

    namespace GierkaCore {
        class AuthService {
            +Register(string login, string pass)
            +Login(string login, string pass)
        }
        class GameState {
            +int PolishZlotyPoints
            +int PoliticalPoints
            +int MilitaryPoints
            +int CurrentTurnYear
        }
        class GameService {
            +LoadLatestGame(int userId)
            +SaveGame(User, GameState)
        }
        class RuletkaLogika {
            -Random _random
            +ZakrecKolem()
            +SprawdzWygrana()
        }
    }

    class GierkaDbContext {
        +DbSet~User~ Users
        +DbSet~GameSave~ GameSaves
        +OnConfiguring()
    }

    class User {
        +int Id
        +string Login
        +string PasswordHash
    }

    class GameSave {
        +int SaveId
        +int UserId
        +string SaveName
        +string GameStateJSON
    }

    MainWindow ..> AuthService : używa
    Gra o-- GameState : zawiera
    Gra ..> GameService : zapis/odczyt
    Ruletka o-- RuletkaLogika : zawiera

    AuthService ..> GierkaDbContext : używa
    GameService ..> GierkaDbContext : używa

    GierkaDbContext ..> User : zarządza
    GierkaDbContext ..> GameSave : zarządza

    User "1" --> "*" GameSave : posiada
```