# Poke Damage Calculator WASM/PWA

This project is the client-side, offline-capable version of the calculator.

- `dotnet run` uses `wwwroot/service-worker.js`, which intentionally does not cache assets during development.
- `dotnet publish` swaps in `wwwroot/service-worker.published.js`, generates `service-worker-assets.js`, and enables offline caching for the published `wwwroot`.
- Serve the published `wwwroot` over `https://` or `localhost`, open it once, then the browser can use the cached app offline.

Useful commands:

```powershell
dotnet build .\Poke.DamageCalc.Blazor.Client\Poke.DamageCalc.Blazor.Client.csproj
dotnet publish .\Poke.DamageCalc.Blazor.Client\Poke.DamageCalc.Blazor.Client.csproj -c Release -o .\artifacts\Poke.DamageCalc.Blazor.Client-publish
dotnet run --project .\Poke.DamageCalc.Blazor.Client\Poke.DamageCalc.Blazor.Client.csproj --urls http://127.0.0.1:5188
```
