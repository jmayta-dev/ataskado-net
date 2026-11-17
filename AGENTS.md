# AGENTS.md

Guía para agentes de IA que trabajan en este repositorio C# / .NET.

## Proyecto
- **Stack:** C# 14 / .NET 10 (`net10.0` en todos los proyectos).
- **Solución:** `src/Ataskado.slnx`:
  - `src/Ataskado.Core` — dominio de la aplicación
  - `src/Ataskado.Cli` — cliente CLI de la aplicación
  - `src/Ataskado.Tests` — pruebas

## Comandos
```bash
dotnet restore src/Ataskado.slnx
dotnet build src/Ataskado.slnx --no-restore -warnaserror
dotnet test src/Ataskado.slnx --no-build
dotnet format src/Ataskado.slnx --verify-no-changes
```
Ejecútalos desde la raíz del repositorio. Antes de dar una tarea por terminada, los cuatro deben pasar sin errores.

## Estilo de código
- No reformatees archivos que no tocaste.
- Namespaces con alcance de archivo (`namespace X;`); en lo posible, un tipo público por archivo.
- `async`/`await` de punta a punta; nada de `.Result` ni `.Wait()`. Propaga `CancellationToken`.
- Prefiere `record` para DTOs inmutables y `sealed` para clases no diseñadas para herencia.
- Nombres: `PascalCase` para tipos y miembros públicos, `_camelCase` para campos privados, sufijo `Async` en métodos asíncronos.
- Inyección de dependencias por constructor; evita `static` con estado y service locators.
- Usa `ILogger<T>` con plantillas estructuradas (`"Usuario {UserId}"`), no interpolación.

## Pruebas
- Toda corrección de bug o funcionalidad nueva incluye pruebas.
- Patrón Arrange / Act / Assert
- Convención de nombres orientado a inglés plano separado con guiones bajo. El test debe ser nombrado como si se estuviese describiendo el escenario a un no-programador:
  Ejemplo: Se tiene un test que verifica que `DeliveryService` identifica de manera apropiada un despacho con una fecha incorrecta como inválida. El nombre pasa por fases de renombramiento hasta llegar a la convención que queremos:
  - La forma de siempre: `public void IsDeliveryValid_InvalidDate_ReturnsFalse()`
  - En inglés plano quedaría como: `public void Delivery_with_invalid_date_should_be_considered_invalid()`
  - Un poco más específico pero verboso: `public void Delivery_with_past_date_should_be_considered_invalid()`
  - Quitamos `considered` cuidando que no pierda significado: `public void Delivery_with_past_date_should_be_invalid()`
  - La palabra `should be` es otro antipatrón común ya que una prueba es un hecho único y atómico sobre una unidad de comportamiento. No hay lugar para un deseo o anhelo al enunciar un hecho. Reemplazamos `should be` con `is`: `public void Delivery_with_past_date_is_invalid()`
  - Y, por último, no es necesario evitar la gramática básica del inglés. Los artículos ayudan a que el test se lea sin errores. Añadimos el artículo `a` al nombre y obtenemos la versión final: `public void Delivery_with_a_past_date_is_invalid()`
- Usa el framework y la librería de mocks ya presentes en el repo; no agregues otros.

## Dependencias
- No agregues paquetes NuGet sin justificarlo en el PR.
- Si existe `Directory.Packages.props`, las versiones se gestionan ahí, no en el `.csproj`.

## Seguridad
- Nunca escribas secretos en código ni en `appsettings*.json`; usa `dotnet user-secrets` o variables de entorno.
- De preferencia usa stored procedures para trabajar con SQL. En caso no se pueda, consultas SQL siempre parametrizadas (o vía EF Core); nada de concatenar strings.

## Límites
- No DEBES modificar ni escribir código a menos que te lo pidan explícitamente.
- No modifiques migraciones ya aplicadas ni archivos generados (`*.g.cs`, `obj/`, `bin/`) salvo que se pida.
- Cambios pequeños y enfocados; si algo es ambiguo, pregunta antes de hacer refactors amplios.