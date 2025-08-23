# SmartTaskPro — Task & Team Management API

A clean, production-style ASP.NET Core Web API that showcases SOLID design, Entity Framework Core, ASP.NET Identity (JWT auth), and flexible project/task management.

---

## ✨ Features

* ASP.NET Core 8 Web API (RESTful)
* EF Core + SQL Server with migrations
* ASP.NET Identity for users/roles
* JWT authentication & role-based authorization (Admin, Manager, Member)
* Repository/Service architecture with DI
* Custom middleware (global error handler & request logging)
* Swagger/OpenAPI with JWT "Authorize" button
* Pagination & filtering for tasks (optional)

---

## 🚀 Quick Start

### Prerequisites

* .NET 8 SDK
* SQL Server (LocalDB/Express/Developer) or Azure SQL
* Visual Studio 2022 or VS Code
* EF Core CLI tools (optional): `dotnet tool install --global dotnet-ef`

### 1) Clone & Restore

```bash
git clone <your-fork-or-repo-url>
cd SmartTaskPro
dotnet restore
```

### 2) Configure `appsettings.json`

Create/update **`appsettings.Development.json`** at the project root:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=SmartTaskProDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  },
  "Jwt": {
    "Key": "replace-with-a-long-random-secret-key",
    "Issuer": "SmartTaskPro",
    "Audience": "SmartTaskProClient",
    "ExpiresMinutes": 60
  },
  "Swagger": { "Enabled": true },
  "Logging": { "LogLevel": { "Default": "Information", "Microsoft": "Warning" } },
  "AllowedHosts": "*"
}
```

> **Security tip:** Use \[dotnet user-secrets] in development or environment variables in production to avoid committing secrets.

### 3) Create Database & Run Migrations

```bash
# from the project folder containing SmartTaskPro.csproj
 dotnet ef migrations add InitialCreate
 dotnet ef database update
```

### 4) (Optional) Seed Roles & Admin

If you included an `IdentitySeeder`, run seeding at startup (e.g., in `Program.cs`) or via a one-off command. Typical defaults:

* Roles: **Admin**, **Manager**, **Member**
* Dev admin user: `admin@smarttaskpro.local` / `Passw0rd!` (set in seeder or secrets)

### 5) Run the API

```bash
dotnet run
```

* Swagger UI (Dev): **[https://localhost:5001/swagger](https://localhost:5001/swagger)** or **[http://localhost:5000/swagger](http://localhost:5000/swagger)**

---

## 🔐 Authentication Flow (JWT)

1. **Register** a user: `POST /api/auth/register` (email, password, display name)
2. **Login**: `POST /api/auth/login` → returns a **JWT token**
3. In Swagger, click **Authorize** ➜ enter: `Bearer <your-jwt>`
4. Access protected endpoints (e.g., `/api/projects`, `/api/tasks`) based on **roles**

**Headers for protected calls:**

```
Authorization: Bearer <token>
Content-Type: application/json
```

---

## 🧭 API Overview

### Auth

* `POST /api/auth/register` — Create user
* `POST /api/auth/login` — Get JWT token

### Projects

* `GET /api/projects` — List (authorized)
* `GET /api/projects/{id}` — Get by Id
* `POST /api/projects` — Create (Manager/Admin)
* `PUT /api/projects/{id}` — Update (Manager/Admin)
* `DELETE /api/projects/{id}` — Delete (Admin)

### Tasks

* `GET /api/tasks` — List with **pagination/filtering** (e.g., `?page=1&pageSize=20&status=Todo&priority=High`)
* `GET /api/tasks/{id}` — Get by Id
* `POST /api/tasks` — Create task (Manager/Admin)
* `PUT /api/tasks/{id}` — Update task
* `DELETE /api/tasks/{id}` — Delete task

> Endpoint names may vary slightly depending on your controllers; update this section to match your code.

---

## 🏗️ Project Structure

```
SmartTaskPro/
├─ Controllers/
│  ├─ AuthController.cs
│  ├─ ProjectsController.cs
│  └─ TasksController.cs
├─ Data/
│  ├─ AppDbContext.cs
│  └─ IdentitySeeder.cs (optional)
├─ Middleware/
│  ├─ ErrorHandlingMiddleware.cs
│  └─ LoggingMiddleware.cs (optional)
├─ Models/
│  ├─ Base/
│  │  └─ BaseEntity.cs
│  ├─ Entities/
│  │  ├─ AppUser.cs
│  │  ├─ Project.cs
│  │  └─ TaskItem.cs
│  └─ Enums/
│     ├─ TaskStatus.cs
│     └─ TaskPriority.cs
├─ DTOs/
│  ├─ Projects/
│  └─ Tasks/
├─ Services/
│  ├─ Interfaces/
│  ├─ Implementations/
│  └─ Jwt/
├─ Repositories/
│  ├─ Interfaces/
│  └─ Implementations/
├─ Mapping/
│  └─ MappingProfile.cs
├─ Program.cs
├─ appsettings.json
└─ SmartTaskPro.csproj
```

---

## ⚙️ Configuration Details

### Swagger/OpenAPI

`Program.cs` services:

```csharp
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SmartTaskPro API", Version = "v1" });
    // JWT security definition + XML comments recommended
});
```

Middleware order:

```csharp
// app.UseMiddleware<ErrorHandlingMiddleware>();
if (app.Environment.IsDevelopment()) { app.UseSwagger(); app.UseSwaggerUI(); }
app.UseAuthentication();
app.UseAuthorization();
```

### Custom Middleware

* **ErrorHandlingMiddleware**: global exception catcher → logs & returns JSON error
* **LoggingMiddleware** (optional): logs request/response metadata

### App Settings

```json
{
  "ConnectionStrings": { "DefaultConnection": "<your-connection-string>" },
  "Jwt": {
    "Key": "<secret>",
    "Issuer": "SmartTaskPro",
    "Audience": "SmartTaskProClient",
    "ExpiresMinutes": 60
  },
  "Swagger": { "Enabled": true }
}
```

---

## 🧪 Testing

* Unit tests with **xUnit**/**MSTest** for services/business logic
* Integration tests for controllers with an in-memory or test database
* Postman or Swagger for manual testing of endpoints

---

## 🐳 (Optional) Docker

Provide a `Dockerfile` and (if using SQL Server in a container) a `docker-compose.yml`:

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SmartTaskPro.dll"]
```

---

## 🧰 Common CLI Commands

```bash
# Restore/build/run
 dotnet restore && dotnet build
 dotnet run

# Migrations
 dotnet ef migrations add <Name>
 dotnet ef database update
 dotnet ef database update 0   # drop all
 dotnet ef migrations remove   # remove last migration

# NuGet packages
 dotnet add package Swashbuckle.AspNetCore
 dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```

---

## 🛠️ Troubleshooting

* **Login returns 401**: Ensure JWT `Key/Issuer/Audience` set, token added as `Bearer <token>` in `Authorization` header
* **DB errors**: Verify connection string and run migrations
* **Swagger doesn’t show**: Ensure `app.Environment.IsDevelopment()` or set `Swagger.Enabled=true` and enable in Program
* **Role-based 403**: Seed roles and assign the right role to your test user

---

## 📄 License

MIT (or your preferred license)

---

## 🙌 Contributing

PRs welcome! For major changes, open an issue to discuss your ideas first.
