# TaskFlow — Full-Stack .NET 9 + React Task Management App

A complete, deployable multi-user task manager with **Admin** and **User** roles, full CRUD,
JWT authentication, and a React SPA frontend. Built to be a real, runnable reference for
Clean Architecture + modern React patterns — not a toy demo.

---

## 1. What this app does

- Users register/log in. Every new signup gets the **User** role by default.
- A seeded **Admin** account (`admin@taskflow.local` / `Admin@12345` — **change this immediately**)
  can manage all users (promote/demote roles, activate/deactivate, delete) and see/edit **every** task.
- Normal users can only see and manage their **own** tasks.
- Tasks have title, description, status (ToDo/InProgress/InReview/Done), priority, due date,
  and can be assigned to another user (admin-only).
- Full CRUD everywhere: Tasks and Users.

---

## 2. Architecture

**Backend — Clean Architecture, 4 layers, one direction of dependency:**

```
TaskFlow.Domain          → Entities, enums. Zero dependencies on anything else.
TaskFlow.Application      → DTOs, interfaces (ports), services (business logic),
                            AutoMapper profiles, FluentValidation validators.
                            Depends only on Domain.
TaskFlow.Infrastructure    → EF Core DbContext, ASP.NET Identity, repositories
                            (the "adapters" that implement Application's interfaces),
                            JWT token generation. Depends on Application + Domain.
TaskFlow.API               → Controllers, middleware, Program.cs (composition root).
                            Wires everything together via Dependency Injection.
```

Why this matters: `TaskService` (business logic) never references EF Core or SQL directly —
it depends on `ITaskRepository`, an interface. Today that interface is implemented with EF Core;
tomorrow you could implement it with Dapper (see the "next projects" list below) and the service
layer wouldn't change at all. That's the core payoff of Clean/Onion Architecture.

**Frontend — Vite + React 18 + TypeScript:**

```
api/          → axios client with request/response interceptors (JWT attach + silent refresh)
context/      → AuthContext (global auth state via Context API)
hooks/        → useAuth, useDebounce (reusable stateful logic)
components/   → Navbar, ProtectedRoute (route guard), ErrorBoundary, TaskFormModal (react-hook-form + zod)
pages/        → Login, Register, Tasks (CRUD table), AdminUsers (CRUD table)
routes/       → React Router v6 config with React.lazy() code-splitting per page
```

### Data flow: end to end

1. **Login** — React form (`react-hook-form` + `zod` validation) → `POST /api/auth/login` →
   API validates credentials via ASP.NET Identity's `UserManager` → issues a JWT access token
   (30 min expiry) + a refresh token (7 days, stored in the `RefreshTokens` SQL table) →
   both tokens + user info saved to `localStorage`, app state updated via `AuthContext`.
2. **Every subsequent API call** — axios request interceptor reads the access token from
   `localStorage` and attaches `Authorization: Bearer <token>` automatically. No component
   ever touches the token directly.
3. **Token expiry** — if a request gets `401`, the axios response interceptor calls
   `POST /api/auth/refresh` once, gets a new token pair, retries the original request
   transparently. If refresh also fails, the user is bounced to `/login`. This is the
   standard "silent refresh" pattern for JWT SPAs.
4. **Fetching tasks** — `TasksPage` calls `GET /api/tasks?page=&search=&status=` →
   `TasksController` → `ITaskService.GetTasksAsync()` → checks `ICurrentUserService.IsAdmin`
   (read from the JWT claims on the request) → if not admin, filters to `OwnerId == currentUserId`
   → `ITaskRepository` (EF Core under the hood) queries SQL Server → results mapped to DTOs →
   JSON back to React → rendered in a table with pagination.
5. **Create/Update/Delete** — same round trip; the API re-checks ownership/role on every
   write (never trust the frontend — the `[Authorize(Roles=...)]` attributes and the
   `CanAccess()` check inside `TaskService` are the real security boundary).
6. **Admin user management** — `UsersController` is decorated `[Authorize(Roles = "Admin")]`
   at the controller level, so a non-admin JWT is rejected by ASP.NET's authorization
   middleware before any code in the controller even runs.

---

## 3. Tech stack (all current, non-deprecated versions as of writing)

| Layer              | Tech                                                                     |
| ------------------ | ------------------------------------------------------------------------ |
| Backend runtime    | .NET 9                                                                   |
| API                | ASP.NET Core Web API (controller-based)                                  |
| ORM                | EF Core 9 (SQL Server provider)                                          |
| Auth               | ASP.NET Core Identity + JWT Bearer (`System.IdentityModel.Tokens.Jwt`) |
| Validation         | FluentValidation                                                         |
| Mapping            | AutoMapper                                                               |
| Logging            | Serilog                                                                  |
| API docs           | Swashbuckle (Swagger/OpenAPI)                                            |
| Database           | SQL Server (Azure SQL Database free tier / LocalDB for dev)              |
| Frontend build     | Vite 6                                                                   |
| Frontend framework | React 18 + TypeScript 5                                                  |
| Routing            | React Router v6                                                          |
| Forms/validation   | react-hook-form + zod                                                    |
| HTTP client        | axios (with interceptors)                                                |
| Styling            | Tailwind CSS 3                                                           |
| Toasts             | react-hot-toast                                                          |

---

## 4. Running locally

### Backend

```bash
cd backend
dotnet restore
cd src/TaskFlow.API
dotnet ef migrations add InitialCreate --project ../TaskFlow.Infrastructure --startup-project .
dotnet ef database update --project ../TaskFlow.Infrastructure --startup-project .
dotnet run
```

This uses LocalDB by default (`appsettings.Development.json`). API runs at `http://localhost:5000`,
Swagger UI at `http://localhost:5000/swagger`.

> Note: you'll need to run `dotnet tool install --global dotnet-ef` once if you don't have it,
> and the EF Core migration is not pre-generated in this zip (it depends on your exact local
> SQL Server/LocalDB setup) — the two commands above generate and apply it in seconds.

### Frontend

```bash
cd frontend
cp .env.example .env
npm install
npm run dev
```

Runs at `http://localhost:5173`. Already verified to build cleanly with `npm run build`
(confirmed: TypeScript compiles, Vite produces code-split chunks per route).

---

## 5. Deploying to Azure (free tier)

**Services used (all within Azure's free tier / free trial limits):**

- **Azure App Service** (F1 Free tier) — hosts the .NET API
- **Azure SQL Database** (Free offer / Basic tier, ~2GB) — the database
- **Azure Static Web Apps** (Free tier) — hosts the React build

**Steps:**

1. Create an **Azure SQL Database** (server + database). Grab the connection string,
   add your IP (or "Allow Azure services") to the firewall rules.
2. Create an **App Service** (Linux, .NET 9 runtime, F1 Free plan).
   In **Configuration → Connection strings**, add `DefaultConnection` with your Azure SQL
   connection string (type: `SQLAzure`). In **Application settings**, add `Jwt__Key` with a
   long random secret (don't commit real secrets to `appsettings.json`).
3. Get the App Service's **publish profile** (Overview → "Download publish profile") and
   add it as a GitHub secret `AZURE_WEBAPP_PUBLISH_PROFILE`. Update `AZURE_WEBAPP_NAME` in
   `.github/workflows/deploy-backend.yml`, push to `main` — the workflow builds and deploys automatically.
4. Create a **Static Web App**, link it to your GitHub repo, set `app_location` to `/frontend`
   and `output_location` to `dist` (already configured in `.github/workflows/deploy-frontend.yml`).
   Update `VITE_API_BASE_URL` to your live App Service URL.
5. Update the backend's `Cors:AllowedOrigins` (in Azure App Service configuration or
   `appsettings.json`) to include your Static Web App's URL, so the browser doesn't block requests.
6. On first run, `DbSeeder` automatically applies EF Core migrations and creates the
   roles + default admin — **log in once and change that admin password.**

---

## 6. Where to go from here — a project-per-topic roadmap

Cramming Redis, RabbitMQ, MongoDB, gRPC, microservices, and event sourcing into one app
would make it unrunnable on a free tier and hard to learn from. Instead, treat TaskFlow as
project #1, and build these focused projects next — each is small enough to finish in a
weekend and deploy on free Azure/cloud tiers:

1. **TaskFlow (this project)** — Clean Architecture, EF Core, JWT auth/authz, DI, REST API,
   role-based multi-tenancy, React fundamentals (Context, hooks, forms, routing, code-splitting).
2. **"Notes API, take 2" — Dapper + Vertical Slice Architecture** — rebuild a small CRUD API
   using Dapper instead of EF Core, organized by feature (vertical slices) instead of by layer.
   Great for feeling the EF-vs-Dapper and Clean-vs-Vertical-Slice trade-offs directly.
3. **"Live Chat" — SignalR + WebSockets + background services** — a real-time chat/notification
   app. Teaches SignalR hubs, raw WebSockets for comparison, and an `IHostedService` background
   worker (e.g. to expire idle sessions or send scheduled notifications).
4. **"Order Processing" — RabbitMQ + event-driven architecture** — two small services
   (Orders API + Inventory worker) that talk via RabbitMQ messages instead of direct HTTP calls.
   Core event-driven/pub-sub concepts, idempotent consumers, retry/dead-letter queues.
5. **"Product Catalog" — MongoDB + Redis caching** — a read-heavy catalog API using MongoDB
   as the primary store and Redis as a cache-aside layer in front of it. Cache invalidation,
   TTLs, and NoSQL document modeling.
6. **"Microservices Mini-Platform"** — split TaskFlow-like functionality into 2–3 independently
   deployable services (Users service, Tasks service, Notifications service) behind an API
   Gateway (YARP), each with its own database — the "database per service" pattern.
   Add gRPC for service-to-service calls instead of REST.
7. **"Ledger" — DDD + CQRS + Event Sourcing** — a small domain (e.g. a bank account or
   inventory ledger) modeled with rich domain entities/aggregates, MediatR for CQRS
   (commands vs queries), and an event store instead of just persisting current state.
8. **Testing pass** — go back to any of the above and add an xUnit + Moq test suite
   (unit tests for services, integration tests with `WebApplicationFactory` + Testcontainers
   for a real SQL Server/Mongo instance in CI).

Each of these, built and deployed, gives you a genuinely strong, varied portfolio —
and direct hands-on experience with everything on your list, rather than a single
overloaded app that's hard to reason about or explain in an interview.
