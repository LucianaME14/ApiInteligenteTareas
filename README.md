# ApiInteligenteTareas

API REST para gestión de tareas (.NET 8, EF Core, SQLite).

## Ejecución local

```bash
cd ApiInteligenteTareas
dotnet restore
dotnet ef database update
dotnet run
```

Swagger: `http://localhost:5230/swagger`

## Migraciones

```bash
dotnet ef migrations add NombreMigracion
dotnet ef database update
```

## Endpoints — Tareas

| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | `/api/tareas` | Lista tareas (con filtros opcionales) |
| GET | `/api/tareas/{id}` | Obtiene una tarea |
| POST | `/api/tareas` | Crea tarea |
| PUT | `/api/tareas/{id}` | Actualiza tarea |
| DELETE | `/api/tareas/{id}` | Elimina tarea |

## Filtros — GET /api/tareas (Pregunta 2)

| Parámetro | Ejemplo |
|-----------|---------|
| `estado` | `?estado=Pendiente` |
| `prioridad` | `?prioridad=Alta` |
| `fechaInicio` / `fechaFin` | `?fechaInicio=2026-05-01&fechaFin=2026-05-31` |

Combinables: `?estado=Pendiente&prioridad=Alta`

**Errores 400:** estado o prioridad inválidos; `fechaInicio` mayor que `fechaFin`.
