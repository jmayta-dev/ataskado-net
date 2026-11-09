# Ataskado

Gestor de tareas personal en C# basado en el formato [todo.txt](https://github.com/todotxt/todo.txt), con temporizadores de enfoque y descanso y métricas por proyecto y contexto.

Es también un proyecto de aprendizaje de **Vertical Slice Architecture (VSA)** con CQRS sin MediatR: cada caso de uso vive en un único archivo dentro de `Features/`.

> **Estado:** en desarrollo. Esta primera versión solo contiene el esqueleto de la solución y el tipo `Result`.

## Requisitos

- [SDK de .NET 10](https://dotnet.microsoft.com/download/dotnet/10.0)
- Windows o Linux

Comprueba que el SDK está instalado:

```bash
dotnet --list-sdks
```

## Estructura

```
Ataskado.slnx
Ataskado.Core/      Dominio, acceso a datos y slices (Features/)
Ataskado.Cli/       Aplicación de consola para uso diario
Ataskado.Tests/     Pruebas (xUnit v3)
```

Más adelante se añadirá `Ataskado.Api/`, una API web que reutiliza los mismos slices.

## Compilar y probar

```bash
dotnet build
dotnet test
```

## Hoja de ruta

- [x] Esqueleto de la solución y tipo `Result`
- [ ] Parser y serializador de todo.txt
- [ ] Consola: `add`, `list`, `done`, `import`, `export`
- [ ] Temporizadores: `start`, `stop`, `log`, `cancel`
- [ ] API web con errores en formato ProblemDetails
- [ ] Estadísticas por tarea y métricas por rango de fechas

## Referencias

- [Formato todo.txt](https://github.com/todotxt/todo.txt)
- [Vertical Slice Architecture in C#: A Step-by-Step Example – Code Maze](https://code-maze.com/vertical-slice-architecture-aspnet-core/)