# SqlServerBrowser

[![License: MIT](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-8.0-blue)](https://dotnet.microsoft.com/)
[![Build](https://img.shields.io/badge/build-passing-brightgreen)]()
[![NuGet](https://img.shields.io/nuget/v/Gniewomir.SqlServerBrowser.svg)](https://www.nuget.org/packages/Gniewomir.SqlServerBrowser)

📄 [Polish version](./readme-pl.md)
[readme-pl.md](readme-pl.md)
A lightweight .NET library for discovering SQL Server instances over the network using UDP protocol (port 1434).

## Features

- Sends a broadcast query to detect available SQL Server instances.
- Parses response data and returns instance names.
- Targets **.NET 8**.
- Simple and[readme-pl.md](readme-pl.md) dependency-free implementation.

## Installation

Install via NuGet:

```bash
dotnet add package Gniewomir.SqlServerBrowser
```

## Usage

```csharp
using Gniewomir.SqlServerBrowser;

var instances = await Browser.QueryServerInstances("192.168.0.255", timeoutms: 1000);
foreach (var instance in instances)
{
    Console.WriteLine(instance);
}
```

> The library sends a UDP packet with byte `0x02` to port 1434 and waits for responses from SQL Server Browser services.

## Notes

- Only IPv4 addresses are supported.
- Timeout is in milliseconds.
- Host can be an IP or hostname; however, broadcast addresses work best for discovery.

## License

[MIT](LICENSE)

## Project Structure

- `SqlBrowserServiceImpelemntation.cs` – core logic for broadcasting and parsing responses.
- `AdressUtils.cs` – helper for calculating broadcast IPs (currently unused in main logic).
- `Gniewomir.SqlServerBrowser.csproj` – project file targeting `.NET 8.

## Author

Created by [Gniewko Steclik](https://github.com/GniewkoSteclik)
