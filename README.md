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

## API externa — Pregunta 3

Fuente: `https://jsonplaceholder.typicode.com/todos`

| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | `/api/tareas-externas` | Lista tareas externas (mapeadas a DTO) |
| GET | `/api/tareas-externas/{id}` | Una tarea externa por id |

**Respuesta ejemplo:**

```json
{
  "externalId": 1,
  "titulo": "delectus aut autem",
  "completado": false
}
```

**Errores:** `404` si el id no existe; `503` si la API externa no responde.

## ML.NET — Análisis de sentimiento (Pregunta 4, Opción A)

| Método | Ruta | Descripción |
|--------|------|-------------|
| POST | `/api/ml/sentimiento` | Clasifica un comentario como Positivo o Negativo |

**Request:**

```json
{
  "comentario": "La tarea fue completada correctamente y el sistema funciona bien"
}
```

**Response:**

```json
{
  "comentario": "La tarea fue completada correctamente y el sistema funciona bien",
  "sentimiento": "Positivo"
}
```

### Dataset

Archivo: `ML/Data/sentimiento-data.csv` (columnas `Texto`, `Sentimiento`).

### Modelo ML.NET

- **Tipo:** clasificación de texto (multiclass con 2 etiquetas: Positivo, Negativo).
- **Algoritmo:** SDCA Maximum Entropy (`Microsoft.ML`).
- **Entrenamiento:** al iniciar la API, si no existe `ML/ModelsGenerated/sentiment-model.zip`, se entrena desde el CSV y se guarda el modelo.
- **Predicción:** `SentimentAnalysisService` carga el modelo y expone el endpoint vía `MlController`.
