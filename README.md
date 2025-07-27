<p align="center">
  <img src="AppGUI/cook-128.ico" alt="Ikona aplikacji" />
</p>

#  RecipeApp 

## Opis aplikacji
RecipeApp to aplikacja desktopowa typu WPF dla systemu Windows, umo�liwiaj�ca wyszukiwanie, przegl�danie oraz zapisywanie przepis�w kulinarnych. Aplikacja korzysta z zewn�trznego API (TheMealDB) oraz w�asnej us�ugi Windows Service do zarz�dzania zapisanymi przepisami.

## G��wne funkcje
- **Wyszukiwanie przepis�w**: U�ytkownik mo�e wyszukiwa� przepisy po nazwie dania. Wyniki pobierane s� z zewn�trznego API.
- **Przegl�danie szczeg��w przepisu**: Po wybraniu przepisu mo�na zobaczy� szczeg�owe informacje, takie jak sk�adniki, instrukcje, zdj�cie, kategoria, region, link do filmu na YouTube oraz �r�d�o.
- **Zapisywanie przepis�w**: Wybrane przepisy mo�na zapisa� lokalnie. Lista zapisanych przepis�w jest dost�pna w aplikacji.
- **Usuwanie zapisanych przepis�w**: U�ytkownik mo�e usuwa� przepisy ze swojej listy zapisanych.
- **Obs�uga offline**: Zapisane przepisy s� dost�pne nawet bez po��czenia z internetem.
- **Integracja z us�ug� Windows**: Zarz�dzanie zapisanymi przepisami odbywa si� przez w�asn� us�ug� Windows (AppMealService), uruchamian� i zatrzymywan� z poziomu aplikacji.

## Struktura projektu
- **AppGUI**: Interfejs u�ytkownika WPF. Obs�uguje nawigacj�, wyszukiwanie, wy�wietlanie i interakcje z u�ytkownikiem.
- **MealsLibrary1**: Logika biznesowa, komunikacja z API, obs�uga plik�w, serializacja przepis�w, kontrakt WCF.
- **AppMealService**: Us�uga Windows, udost�pniaj�ca funkcje zapisu i odczytu przepis�w przez WCF.
- **AppGUITests**: Testy jednostkowe dla logiki aplikacji.

## Wymagania
- .NET Framework 4.7.2
- Windows 10/11
- Uprawnienia administratora do uruchamiania us�ugi Windows

## Instalacja i uruchomienie
1. **Zainstaluj us�ug� Windows**: Skompiluj projekt AppMealService i zainstaluj us�ug� za pomoc� narz�dzia `InstallUtil.exe` lub PowerShell.
2. **Uruchom aplikacj�**: Skompiluj i uruchom projekt AppGUI.
3. **Pierwsze uruchomienie**: Na ekranie powitalnym kliknij "Start", aby uruchomi� us�ug� i przej�� do wyszukiwania przepis�w.

## U�ycie
- Wyszukaj przepis wpisuj�c nazw� dania i klikaj�c "Szukaj".
- Przegl�daj wyniki i kliknij wybrany przepis, aby zobaczy� szczeg�y.
- Dodaj przepis do zapisanych lub usu� go z listy.
- Przegl�daj zapisane przepisy w trybie offline.

## Technologie
- WPF (.NET Framework 4.7.2)
- WCF (Windows Communication Foundation)
- Windows Service
- JSON, HTTP

## Autor
Projekt zaliczeniowy z programowania w j�zyku C#.

## Licencja
Projekt edukacyjny, do u�ytku niekomercyjnego.
