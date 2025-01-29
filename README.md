## **Założenia przyjęte podczas realizacji zadania**

* **Obsługa żądań** – Endpoint przyjmuje żądania `POST`, zgodnie z wymaganiem, aby parametrem wejściowym był JSON. Zakładam, że JSON znajduje się w body requestu, a nie jako parametr w query stringu.  

* **Architektura i wzorce projektowe** – Pomimo prostoty zadania w konteście logiki biznesowej, zastosowałem wzorzec **mediatora**, aby podkreślić zasadę **Single Responsibility** i oddzielić logikę biznesową od warstwy serwisowej. Obsługę logiki oddelegowałem do klasy **handlera**, a nie pojedynczej metody klasy serwisu.  

* **Segregacja interfejsów** – Dodano interfejs dla klasy `CardService`, zgodnie z zasadą **Interface Segregation Principle (SOLID)**.  

* **Walidacja danych wejściowych** – Domyślnie wykorzystuje mechanizm `ProblemDetails` do zwracania błędów. W bardziej rozbudowanych przypadkach można zastosować **FluentValidation** lub **DataAnnotations**, w zależności od preferencji i potrzeb projektu. Dla pełnej implementacji należałoby również dodać middleware obsługujący wyjątki.  

* **Brak logowania** – Pominąłem logowanie, również ze względu na brak obsługi wyjątków.  

* **Testowanie** – Przykładowe testy jednostkowe dotyczą wyłącznie logiki pobierania akcji dla określonego rodzaju karty. Przyjąłem założenie, że testy klasy zwracającej rodzaj karty nie są konieczne, ponieważ implementacja testowa jest narzucona. Ponadto testy jednostkowe nie obejmują mechanizmów kontroli, takich jak wywołania kontrolera i mediatora.  

* **Lokalizacja logiki biznesowej** – Implementacja metod pobierania typów kart znajduje się w warstwie **infrastruktury**, a nie **domeny**, ponieważ mapa dostępów do akcji dotyczy konfiguracji aplikacji, a nie logiki biznesowej. Metoda `GetCardDetails` traktowana jest jako implementacja testowa (w przyszłości zakłada się pobieranie danych z zewnętrznego endpointu), dlatego również umieściłem ją w warstwie infrastruktury.  

* **Model zwracanych danych** – Informacje o dostępnych akcjach zwracane są w postaci nazw akcji. Akcje te są przechowywane jako suma bitowa wartości wyliczeniowych, co pozwala na zapis w bazie jako pojedyncza liczba. Dzięki rozszerzeniu (`ToList()`) na typie wyliczeniowym, endpoint zwraca listę `List<string>`.  

* **Organizacja macierzy dostępnych akcji** – Macierz dostępnych akcji została odwzorowana w `ActionMap` w `ActionProviderService` jako **słownik**, który rozróżnia typy kart dla większej czytelności. Możliwa optymalizacja polegałaby na uwspólnieniu dostępnych akcji dla większości kart, jednak zdecydowano się na odrębne wpisy w słowniku, aby umożliwić przyszłą elastyczną modyfikację dostępności akcji.  

* **Optymalizacja kodu** – Metoda `GetAllowedActions` mogłaby zostać zoptymalizowana. Obecnie warunki dodawania akcji `Action6` i `Action7` w zależności od wymogu podania PIN-u są jasno wyartykułowane w celu zwiększenia czytelności kodu.  

* **Struktura projektu (DDD)** – Podział solucji na projekty po części przypomina podejście **Domain-Driven Design (DDD)** w uproszczonym modelu (warstwa aplikacji korzysta bezpośrednio z infrastruktury zamiast z interfejsów repozytoriów domenowych):  
  * **API** – obsługuje dostęp do danych oraz zarządza zapytaniami i komendami.  
  * **Application** – zawiera logikę obsługi zapytań i komend, bazując na domenie i infrastrukturze.  
  * **Domain** – zawiera klasy domenowe oraz typy wyliczeniowe.  
  * **Infrastructure** – odpowiedzialna za pobieranie danych na potrzeby aplikacji.  
  * **Tests** – ze względu na brak integracji z innymi mikrousługami, zawiera jedynie przykładowe testy jednostkowe.

