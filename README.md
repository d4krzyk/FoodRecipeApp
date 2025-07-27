<p align="center">
  <img src="AppGUI/cook-128.ico" alt="Ikona aplikacji" />
</p>

#  RecipeApp 

## Opis aplikacji
RecipeApp to aplikacja desktopowa typu WPF dla systemu Windows, umo¿liwiaj¹ca wyszukiwanie, przegl¹danie oraz zapisywanie przepisów kulinarnych. Aplikacja korzysta z zewnêtrznego API (TheMealDB) oraz w³asnej us³ugi Windows Service do zarz¹dzania zapisanymi przepisami.

## G³ówne funkcje
- **Wyszukiwanie przepisów**: U¿ytkownik mo¿e wyszukiwaæ przepisy po nazwie dania. Wyniki pobierane s¹ z zewnêtrznego API.
- **Przegl¹danie szczegó³ów przepisu**: Po wybraniu przepisu mo¿na zobaczyæ szczegó³owe informacje, takie jak sk³adniki, instrukcje, zdjêcie, kategoria, region, link do filmu na YouTube oraz Ÿród³o.
- **Zapisywanie przepisów**: Wybrane przepisy mo¿na zapisaæ lokalnie. Lista zapisanych przepisów jest dostêpna w aplikacji.
- **Usuwanie zapisanych przepisów**: U¿ytkownik mo¿e usuwaæ przepisy ze swojej listy zapisanych.
- **Obs³uga offline**: Zapisane przepisy s¹ dostêpne nawet bez po³¹czenia z internetem.
- **Integracja z us³ug¹ Windows**: Zarz¹dzanie zapisanymi przepisami odbywa siê przez w³asn¹ us³ugê Windows (AppMealService), uruchamian¹ i zatrzymywan¹ z poziomu aplikacji.

## Struktura projektu
- **AppGUI**: Interfejs u¿ytkownika WPF. Obs³uguje nawigacjê, wyszukiwanie, wyœwietlanie i interakcje z u¿ytkownikiem.
- **MealsLibrary1**: Logika biznesowa, komunikacja z API, obs³uga plików, serializacja przepisów, kontrakt WCF.
- **AppMealService**: Us³uga Windows, udostêpniaj¹ca funkcje zapisu i odczytu przepisów przez WCF.
- **AppGUITests**: Testy jednostkowe dla logiki aplikacji.

## Wymagania
- .NET Framework 4.7.2
- Windows 10/11
- Uprawnienia administratora do uruchamiania us³ugi Windows

## Instalacja i uruchomienie
1. **Zainstaluj us³ugê Windows**: Skompiluj projekt AppMealService i zainstaluj us³ugê za pomoc¹ narzêdzia `InstallUtil.exe` lub PowerShell.
2. **Uruchom aplikacjê**: Skompiluj i uruchom projekt AppGUI.
3. **Pierwsze uruchomienie**: Na ekranie powitalnym kliknij "Start", aby uruchomiæ us³ugê i przejœæ do wyszukiwania przepisów.

## U¿ycie
- Wyszukaj przepis wpisuj¹c nazwê dania i klikaj¹c "Szukaj".
- Przegl¹daj wyniki i kliknij wybrany przepis, aby zobaczyæ szczegó³y.
- Dodaj przepis do zapisanych lub usuñ go z listy.
- Przegl¹daj zapisane przepisy w trybie offline.

## Technologie
- WPF (.NET Framework 4.7.2)
- WCF (Windows Communication Foundation)
- Windows Service
- JSON, HTTP

## Autor
Projekt zaliczeniowy z programowania w jêzyku C#.

## Licencja
Projekt edukacyjny, do u¿ytku niekomercyjnego.
