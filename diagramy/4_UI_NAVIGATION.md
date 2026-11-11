# Nawigacja i Stany Interfejsu (UI State Machine)

Diagram przedstawia architekturę nawigacji (`Navigation`) wewnątrz `MainWindow`. Pokazuje w jaki sposób główne moduły WPF i ekrany gry komunikują się i wymieniają widoki podczas rozgrywki (np. przechodzenie między mapą główną a minigrami w Kasynie).

```mermaid
stateDiagram-v2
    [*] --> LoginControl : Otwarcie Aplikacji
    
    LoginControl --> MainMenuControl : Rejestracja/Logowanie Sukces
    
    state MainMenuControl {
        [*] --> Menu
    }
    
    MainMenuControl --> Gra : Rozpocznij
    MainMenuControl --> SettingsControl : Ustawienia (Głośność - Flappy Bird)
    MainMenuControl --> RankingControl : Tablica Wyników
    
    Gra --> Casino : Wejdź do Kasyna
    Gra --> Sejm : Lokacja Sejm (Edward Gierek)
    Gra --> Gdansk : Lokacja Gdańsk (Stocznia)
    Gra --> Mon : Lokacja MON
    
    state Casino {
        [*] --> WybórMinigry
        WybórMinigry --> Blackjack
        WybórMinigry --> Roulette
        WybórMinigry --> JednorekiGierek
        Blackjack --> WybórMinigry
        Roulette --> WybórMinigry
        JednorekiGierek --> WybórMinigry
    }
    
    Casino --> Gra : Wyjście
    Sejm --> Gra : Powrót na Mapę
    Gdansk --> Gra : Powrót na Mapę
    Mon --> Gra : Powrót na Mapę
```