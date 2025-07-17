# SqlServerBrowser

[![License: MIT](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-8.0-blue)](https://dotnet.microsoft.com/)
[![Build](https://img.shields.io/badge/build-passing-brightgreen)]()
[![NuGet](https://img.shields.io/nuget/v/Gniewomir.SqlServerBrowser.svg)](https://www.nuget.org/packages/Gniewomir.SqlServerBrowser)

📄 [Wersja angielska](./readme.md)

Lekka biblioteka .NET do wykrywania instancji SQL Server w sieci za pomocą protokołu UDP (port 1434).

## Funkcje

- Wysyła zapytanie broadcastowe w celu wykrycia dostępnych instancji SQL Server.
- Parsuje odpowiedzi i zwraca nazwy instancji.
- Działa na **.NET 8**.
- Prosta implementacja bez dodatkowych zależności.

## Instalacja

Zainstaluj przez NuGet:

```bash
dotnet add package Gniewomir.SqlServerBrowser
```

## Użycie

```csharp
using Gniewomir.SqlServerBrowser;

var instances = await Browser.QueryServerInstances("192.168.0.255", timeoutms: 1000);
foreach (var instance in instances)
{
    Console.WriteLine(instance);
}
```

> Biblioteka wysyła pakiet UDP zawierający bajt `0x02` na port 1434 i oczekuje odpowiedzi od usług SQL Server Browser.

## Uwagi

- Obsługiwane są tylko adresy IPv4.
- Timeout podawany jest w milisekundach.
- Host może być adresem IP lub nazwą hosta, ale najlepsze efekty daje użycie adresów broadcastowych.

## Licencja

[MIT](LICENSE)

## Struktura projektu

- `SqlBrowserServiceImpelemntation.cs` – główna logika odpowiadająca za broadcast i analizę odpowiedzi.
- `AdressUtils.cs` – pomocnicze metody do obliczania adresów broadcastowych (obecnie nieużywane).
- `Gniewomir.SqlServerBrowser.csproj` – plik projektu dla .NET 8.

## Autor

Stworzone przez [Gniewko Steclik](https://github.com/GniewkoSteclik)
