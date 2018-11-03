# Pharmacy

Pharmacy jest aplikacją konsolową wspomagającą pracę apteki służącą do zarządzania lekami oraz rejestrowania czynności związanych ze sprzedażą leków.

### Technologie
C#, LINQ, MS SQL Server

### Biblioteki/Frameworks
System.Data.SqlClient

## Instalacja:

W celu uruchomienia aplikacji należy stworzyć nową bazę danych lub odtworzyć bazę z przykładowymi danymi.

### I. Nowa baza danych 

1. utworzenie nowej bazy danych PharmacyDb korzystając ze skryptu ```create database PharmacyDb```
1. stworzenie wymaganych tabel po przez wykonanie skryptu **\scripts\Create_tables.sql** 
1. dodanie przykładowych danych po przez wykonanie skryptu **\scripts\Sample_data.sql** 

### II. SampleDB

1. odworzonie bazy z backupu znajdującego się w katalogu **SampleDB**

## Przykładowe dane:

Przykładowe dane zawierają:
1. zestaw leków
1. zamówienia
    
## Dostępne moduły:

### Zarządzanie lekami

Moduł pozwala na tworzenie, modyfikowani, usuwanie, wyszukiwanie i wyświetlanie listy leków. Każdy nowy lek musi mieć uzupełnione: nazwę, producenta, cenę, ilość sztuk oraz informację czy jest na receptę.

#### Dostępne funkcjonalności
* Tworzenie nowego leku
* Modyfikowanie istniejącego leku
* Usuwanie istniejącego wpis
* Wyświetlanie listy leków
* Wyszukiwanie/filtorwanie listy leków

### Wprowadzanie zamówień/Sprzedaż leku

Moduł pozwala na wprowadzanie zamówień związanych ze sprzedażą leków. Podczas sprzedaży leku należy podać datę sprzedaży i ilość sprzedawanych sztuk leku a w przypadku gdy lek jest na receptę także imię i nazwisko pacjenta, jego PESEL i numer recepty.

#### Dostępne funkcjonalności
* Tworzenie zamówienia sprzedaży leku

## Korzystanie z aplikacji:

Do skorzystania z aplikacji użytkownicy nie potrzebują żadnych specjalnych uprawnień. Wszyscy uzytkownicy od razu po uruchomieniu aplikacji posiadają dostęp do wszystkich modułów jak i wprowadzonych leków. 

##### cdn.