# Baza Danych i Przepływ Danych (Database & Data Flow)

## Diagram ERD – Model Danych
Przedstawia schemat relacyjny tabel `USERS` oraz `GAMESAVES` w relacji jeden-do-wielu wykorzystywany przez Entity Framework.

```mermaid
erDiagram
    USERS ||--o{ GAMESAVES : posiada

    USERS {
        int Id PK "Unikalny identyfikator"
        string Login "Nazwa użytkownika"
        string PasswordHash "Zahaszowane hasło"
    }

    GAMESAVES {
        int SaveId PK "Identyfikator zapisu"
        int UserId FK "Klucz obcy do Users"
        string SaveName "Nazwa zapisu (np. Autosave)"
        string GameStateJSON "Zserializowany stan gry"
    }
```

## Diagram Przepływu Danych (DFD)
Pokazuje przepływ informacji pomiędzy graczem, modułem rozgrywki, autoryzacją, stanem gry a bazami danych.

```mermaid
flowchart TD
    Gracz((Gracz))
    Ruletka[Rozgrywka - Ruletka]
    Auth[Logowanie / Rejestracja]
    StanGry[Zarządzanie Stanem Gry]
    UsersDB[(Baza: Users)]
    SavesDB[(Baza: GameSaves)]

    Gracz -->|Decyzja i Stawka| Ruletka
    Ruletka -->|Wynik losowania| Gracz

    Gracz -->|Login i Hasło| Auth
    Auth -->|Token / Sukces| Gracz

    Ruletka -->|Zmiana punktów| StanGry
    StanGry -->|Aktualny Stan| Gracz

    Auth <-->|Weryfikacja| UsersDB

    StanGry <-->|Odczyt zapisu| SavesDB
    StanGry -->|Zapis Autosave| SavesDB
```