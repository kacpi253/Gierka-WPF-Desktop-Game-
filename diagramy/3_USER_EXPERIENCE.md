# Przypadki Użycia i Doświadczenie Użytkownika (UX & Use Cases)

## Diagram Komunikacji – Gracz kręci ruletką
Sekwencja wywołań metod pomiędzy interfejsem okna ruletki, logiką losowania a obiektem stanu gry.

```mermaid
flowchart LR
    Gracz((:Gracz))
    Okno[:OknoRuletki]
    Logika[:RuletkaLogika]
    Stan[:GameState]

    Gracz -->|1. Kliknij Zakrec| Okno
    Okno -->|1.1. ZakrecKolem| Logika
    Logika -->|1.2. Wynik Numer Kolor| Okno
    Okno -->|1.3. SprawdzWygrana| Logika
    Logika -->|1.4. Kwota Wygranej| Okno
    Okno -->|1.5. ZaktualizujPunkty| Stan
    Okno -->|1.6. Wyświetl wynik| Gracz
```

## Podróż Gracza (User Journey)
Ścieżka doświadczeń użytkownika (UX) oraz interakcji z systemem podzielona na 4 etapy rozgrywki.

```mermaid
journey
    title Podróż Gracza w Świecie Gry "Gierka"
    section 🚀 Start & Weryfikacja
      Uruchomienie Aplikacji: 5: Gracz
      Panel Logowania: 3: Gracz
      Błąd logowania (Złe hasło): 1: Gracz, System
      Sukces Logowania: 5: Gracz, System
    section 🌍 Eksploracja Świata
      Menu Główne (Wybór Misji): 4: Gracz
      Podróż do Kasyna: 5: Gracz
      Interakcja z Krupierem: 4: Gracz
    section 🎰 Emocje w Kasynie (Ruletka)
      Analiza stołu i zakładów: 3: Gracz
      Postawienie "Va Banque" na Czerwone: 4: Gracz
      Oczekiwanie na wynik (Spinning...): 2: Gracz
      Wielka Wygrana (+1000 PLN): 5: Gracz, System
    section 💾 Zakończenie Sesji
      Powrót do Menu: 3: Gracz
      Auto-Zapis (W tle): 5: Gracz
      Wyjście z gry (Satysfakcja): 5: Gracz
```