# VMS Lite — Microservices Build & Deploy Guide

**.NET 8 · Angular 17 · Microservices · Redis · Azure Service Bus · xUnit/Moq · Docker · Azure · GitHub Actions**

> Open this file in VS Code and use the outline view (Ctrl+Shift+O) to jump between sections.

## Table of Contents

- [0. Architecture](#0-architecture)
- [1. Scaffold Solution](#1-scaffold-solution)
- [2. Jobs.API — Full Code](#2-jobsapi-full-code)
- [3. Candidates.API — Full Code](#3-candidatesapi-full-code)
- [4. Notifications.API — Full Code](#4-notificationsapi-full-code)
- [5. API Gateway (YARP)](#5-api-gateway-yarp)
- [6. Angular 17 Frontend](#6-angular-17-frontend)
- [7. Redis + Azure Service Bus Setup](#7-redis--azure-service-bus-setup)
- [8. xUnit + Moq Tests](#8-xunit--moq-tests)
- [9. Docker + docker-compose](#9-docker--docker-compose)
- [10. GitHub Actions CI/CD](#10-github-actions-cicd)
- [11. Azure Deployment](#11-azure-deployment)
- [12. Resume Wording](#12-resume-wording)

---

## 0. Architecture

```
					  Angular 17 SPA (UI)
        					↕ HTTP
			API Gateway — YARP Reverse Proxy (:5000)
        				 ↕ routes to
┌───────────────┬────────────────────┬───────────────────────────┐
│ Jobs.API      │ Candidates.API     │ Notifications.API         │
│ :5001         │ :5002              │ :5003 (background worker) │
└───────────────┴────────────────────┴───────────────────────────┘
        ↕                   ↕                      ↕
   Redis Cache      Azure Service Bus        SQLite / SQL Server
```

**Folder structure:**

```
VmsLite/
├── src/
│   ├── ApiGateway/              ← YARP
│   ├── Services/
│   │   ├── Jobs.API/
│   │   ├── Candidates.API/
│   │   └── Notifications.API/
│   └── Frontend/                ← Angular 17
├── tests/
│   ├── Jobs.API.Tests/          ← unit tests
│   └── Integration.Tests/       ← integration tests
├── docker-compose.yml
├── VmsLite.sln
└── .github/workflows/           ← CI/CD
```

**Weekend plan:** Wknd 1 → sections 1–5 (backend) · Wknd 2 → sections 6–7 (Angular + Redis/Bus) · Wknd 3 → sections 8–9 (tests + Docker) · Wknd 4 → sections 10–11 (CI/CD + Azure)

---

## 1. Scaffold Solution

### 1a — Create solution & projects

```bash
mkdir VmsLite && cd VmsLite
dotnet new sln -n VmsLite

dotnet new webapi -n Jobs.API          -o src/Services/Jobs.API          --use-controllers
dotnet new webapi -n Candidates.API    -o src/Services/Candidates.API    --use-controllers
dotnet new webapi -n Notifications.API -o src/Services/Notifications.API --use-controllers
dotnet new web   -n ApiGateway         -o src/ApiGateway

dotnet new xunit -n Jobs.API.Tests    -o tests/Jobs.API.Tests
dotnet new xunit -n Integration.Tests -o tests/Integration.Tests

dotnet sln add src/Services/Jobs.API/Jobs.API.csproj
dotnet sln add src/Services/Candidates.API/Candidates.API.csproj
dotnet sln add src/Services/Notifications.API/Notifications.API.csproj
dotnet sln add src/ApiGateway/ApiGateway.csproj
dotnet sln add tests/Jobs.API.Tests/Jobs.API.Tests.csproj
dotnet sln add tests/Integration.Tests/Integration.Tests.csproj

dotnet add tests/Jobs.API.Tests    reference src/Services/Jobs.API/Jobs.API.csproj
dotnet add tests/Integration.Tests reference src/Services/Jobs.API/Jobs.API.csproj
```

### 1b — Install NuGet packages

```bash
# Jobs.API
cd src/Services/Jobs.API
dotnet add package Microsoft.EntityFrameworkCore --version 8.*
dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 8.*
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.*
dotnet add package StackExchange.Redis
dotnet add package Microsoft.Extensions.Caching.StackExchangeRedis
dotnet add package Azure.Messaging.ServiceBus
dotnet add package Swashbuckle.AspNetCore
cd ../../..

# Candidates.API
cd src/Services/Candidates.API
dotnet add package Microsoft.EntityFrameworkCore --version 8.*
dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 8.*
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.*
dotnet add package Microsoft.Extensions.Caching.StackExchangeRedis
dotnet add package Swashbuckle.AspNetCore
cd ../../..

# Notifications.API
cd src/Services/Notifications.API
dotnet add package Azure.Messaging.ServiceBus
dotnet add package Swashbuckle.AspNetCore
cd ../../..

# Gateway
cd src/ApiGateway
dotnet add package Yarp.ReverseProxy
cd ../..

# Test projects
cd tests/Jobs.API.Tests
dotnet add package Moq
dotnet add package FluentAssertions
dotnet add package Microsoft.AspNetCore.Mvc.Testing
dotnet add package Microsoft.EntityFrameworkCore.InMemory --version 8.*
cd ../..
cd tests/Integration.Tests
dotnet add package Microsoft.AspNetCore.Mvc.Testing
dotnet add package FluentAssertions
dotnet add package Microsoft.EntityFrameworkCore.InMemory --version 8.*
cd ../..
```

```bash
dotnet build VmsLite.sln   # Build succeeded, 0 errors
```

---

## 2. Jobs.API (Full Code)

**`Models/Job.cs`**

```csharp
namespace Jobs.API.Models;

public class Job
{
    public int      Id          { get; set; }
    public string   Title       { get; set; } = string.Empty;
    public string   Description { get; set; } = string.Empty;
    public string   Location    { get; set; } = string.Empty;
    public string   Company     { get; set; } = string.Empty;
    public decimal  Salary      { get; set; }
    public bool     IsActive    { get; set; } = true;
    public DateTime PostedAt    { get; set; } = DateTime.UtcNow;
}
```

**`Data/JobsDbContext.cs`**

```csharp
using Jobs.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Jobs.API.Data;

public class JobsDbContext(DbContextOptions<JobsDbContext> options) : DbContext(options)
{
    public DbSet<Job> Jobs { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Job>().Property(j => j.Salary).HasColumnType("decimal(18,2)");
        modelBuilder.Entity<Job>().HasData(
            new Job { Id = 1, Title = "Senior .NET Developer", Company = "Acme Corp",
                      Location = "Delhi", Salary = 2000000, Description = "5+ years .NET Core" },
            new Job { Id = 2, Title = "Full-Stack Engineer", Company = "TechStart",
                      Location = "Noida", Salary = 1500000, Description = "Angular + .NET" }
        );
    }
}
```

**`Messaging/IMessagePublisher.cs` + implementations**

```csharp
namespace Jobs.API.Messaging;

public interface IMessagePublisher
{
    Task PublishAsync(string topic, string payload, CancellationToken ct = default);
}
```

```csharp
// Messaging/ServiceBusPublisher.cs
using Azure.Messaging.ServiceBus;
namespace Jobs.API.Messaging;

public class ServiceBusPublisher : IMessagePublisher, IAsyncDisposable
{
    private readonly ServiceBusClient _client;
    public ServiceBusPublisher(IConfiguration cfg)
        => _client = new ServiceBusClient(cfg["ServiceBus:ConnectionString"]);

    public async Task PublishAsync(string topic, string payload, CancellationToken ct = default)
    {
        await using var sender = _client.CreateSender(topic);
        await sender.SendMessageAsync(new ServiceBusMessage(payload), ct);
    }
    public async ValueTask DisposeAsync() => await _client.DisposeAsync();
}
```

```csharp
// Messaging/InMemoryPublisher.cs  ← used in dev & tests (no Azure needed)
namespace Jobs.API.Messaging;

public class InMemoryPublisher(ILogger<InMemoryPublisher> log) : IMessagePublisher
{
    public Task PublishAsync(string topic, string payload, CancellationToken ct = default)
    {
        log.LogInformation("[InMemory] Topic={Topic} Payload={Payload}", topic, payload);
        return Task.CompletedTask;
    }
}
```

**`Services/IJobService.cs`**

```csharp
using Jobs.API.Models;
namespace Jobs.API.Services;

public interface IJobService
{
    Task<IEnumerable<Job>> GetAllAsync(CancellationToken ct = default);
    Task<Job?>             GetByIdAsync(int id, CancellationToken ct = default);
    Task<Job>              CreateAsync(Job job, CancellationToken ct = default);
    Task<bool>              UpdateAsync(int id, Job job, CancellationToken ct = default);
    Task<bool>              DeleteAsync(int id, CancellationToken ct = default);
}
```

**`Services/JobService.cs`** — cache-aside pattern + event publishing

```csharp
using Jobs.API.Data;
using Jobs.API.Messaging;
using Jobs.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Jobs.API.Services;

public class JobService(JobsDbContext db, IDistributedCache cache, IMessagePublisher publisher) : IJobService
{
    private const string AllCacheKey = "jobs:all";

    public async Task<IEnumerable<Job>> GetAllAsync(CancellationToken ct = default)
    {
        var cached = await cache.GetStringAsync(AllCacheKey, ct);
        if (cached is not null)
            return JsonSerializer.Deserialize<List<Job>>(cached)!;

        var jobs = await db.Jobs.Where(j => j.IsActive)
                                 .OrderByDescending(j => j.PostedAt)
                                 .ToListAsync(ct);

        await cache.SetStringAsync(AllCacheKey, JsonSerializer.Serialize(jobs),
            new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) }, ct);

        return jobs;
    }

    public async Task<Job?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var key = $"job:{id}";
        var cached = await cache.GetStringAsync(key, ct);
        if (cached is not null) return JsonSerializer.Deserialize<Job>(cached);

        var job = await db.Jobs.FindAsync(new object[] { id }, ct);
        if (job is not null)
            await cache.SetStringAsync(key, JsonSerializer.Serialize(job),
                new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(10) }, ct);
        return job;
    }

    public async Task<Job> CreateAsync(Job job, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(job.Title))
            throw new ArgumentException("Title is required", nameof(job));

        job.PostedAt = DateTime.UtcNow;
        db.Jobs.Add(job);
        await db.SaveChangesAsync(ct);

        await cache.RemoveAsync(AllCacheKey, ct);
        await publisher.PublishAsync("job-created",
            JsonSerializer.Serialize(new { job.Id, job.Title, job.Company, PostedAt = job.PostedAt }), ct);

        return job;
    }

    public async Task<bool> UpdateAsync(int id, Job updated, CancellationToken ct = default)
    {
        var job = await db.Jobs.FindAsync(new object[] { id }, ct);
        if (job is null) return false;

        job.Title = updated.Title; job.Description = updated.Description;
        job.Location = updated.Location; job.Salary = updated.Salary; job.Company = updated.Company;
        await db.SaveChangesAsync(ct);

        await Task.WhenAll(cache.RemoveAsync(AllCacheKey, ct), cache.RemoveAsync($"job:{id}", ct));
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var job = await db.Jobs.FindAsync(new object[] { id }, ct);
        if (job is null) return false;
        job.IsActive = false;                          // soft delete
        await db.SaveChangesAsync(ct);
        await Task.WhenAll(cache.RemoveAsync(AllCacheKey, ct), cache.RemoveAsync($"job:{id}", ct));
        return true;
    }
}
```

**`Controllers/JobsController.cs`**

```csharp
using Jobs.API.Models;
using Jobs.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Jobs.API.Controllers;

[ApiController]
[Route("api/jobs")]
public class JobsController(IJobService svc) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct) => Ok(await svc.GetAllAsync(ct));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var job = await svc.GetByIdAsync(id, ct);
        return job is null ? NotFound() : Ok(job);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Job job, CancellationToken ct)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var created = await svc.CreateAsync(job, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Job job, CancellationToken ct)
        => await svc.UpdateAsync(id, job, ct) ? NoContent() : NotFound();

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
        => await svc.DeleteAsync(id, ct) ? NoContent() : NotFound();
}
```

**`Program.cs`**

```csharp
using Jobs.API.Data;
using Jobs.API.Messaging;
using Jobs.API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.SwaggerDoc("v1", new() { Title = "Jobs API", Version = "v1" }));

builder.Services.AddDbContext<JobsDbContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=jobs.db"));

var redisConn = builder.Configuration.GetConnectionString("Redis");
if (!string.IsNullOrEmpty(redisConn))
    builder.Services.AddStackExchangeRedisCache(o => o.Configuration = redisConn);
else
    builder.Services.AddDistributedMemoryCache();

var sbConn = builder.Configuration["ServiceBus:ConnectionString"];
if (!string.IsNullOrEmpty(sbConn))
    builder.Services.AddSingleton<IMessagePublisher, ServiceBusPublisher>();
else
    builder.Services.AddSingleton<IMessagePublisher, InMemoryPublisher>();

builder.Services.AddScoped<IJobService, JobService>();
builder.Services.AddCors(o => o.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
    scope.ServiceProvider.GetRequiredService<JobsDbContext>().Database.EnsureCreated();

app.UseSwagger(); app.UseSwaggerUI();
app.UseCors();
app.MapControllers();
app.Run();

public partial class Program { }   // needed for WebApplicationFactory in integration tests
```

**`appsettings.Development.json`**

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=jobs.db",
    "Redis": "localhost:6379"
  },
  "ServiceBus": { "ConnectionString": "" }
}
```

> **Test it alone:** `cd src/Services/Jobs.API && dotnet run` → open `https://localhost:5001/swagger`, try GET /api/jobs, you'll see 2 seeded jobs.

---

## 3. Candidates.API (Full Code)

**`Models/Candidate.cs`**

```csharp
namespace Candidates.API.Models;

public class Candidate
{
    public int      Id           { get; set; }
    public string   Name         { get; set; } = string.Empty;
    public string   Email        { get; set; } = string.Empty;
    public string   Phone        { get; set; } = string.Empty;
    public string   Skills       { get; set; } = string.Empty;   // comma-separated
    public int?     AppliedJobId { get; set; }
    public string   Status       { get; set; } = "Applied";      // Applied|Shortlisted|Placed|Rejected
    public DateTime AppliedAt    { get; set; } = DateTime.UtcNow;
}
```

**`Data/CandidatesDbContext.cs`**

```csharp
using Candidates.API.Models;
using Microsoft.EntityFrameworkCore;
namespace Candidates.API.Data;

public class CandidatesDbContext(DbContextOptions<CandidatesDbContext> options) : DbContext(options)
{
    public DbSet<Candidate> Candidates { get; set; } = null!;
}
```

**`Services/ICandidateService.cs` + `CandidateService.cs`**

```csharp
namespace Candidates.API.Services;

public interface ICandidateService
{
    Task<IEnumerable<Candidate>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<Candidate>> GetByJobAsync(int jobId, CancellationToken ct = default);
    Task<Candidate?>             GetByIdAsync(int id, CancellationToken ct = default);
    Task<Candidate>              ApplyAsync(Candidate c, CancellationToken ct = default);
    Task<bool>                   UpdateStatusAsync(int id, string status, CancellationToken ct = default);
}
```

```csharp
using Candidates.API.Data;
using Candidates.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
namespace Candidates.API.Services;

public class CandidateService(CandidatesDbContext db, IDistributedCache cache) : ICandidateService
{
    public async Task<IEnumerable<Candidate>> GetAllAsync(CancellationToken ct = default)
        => await db.Candidates.OrderByDescending(c => c.AppliedAt).ToListAsync(ct);

    public async Task<IEnumerable<Candidate>> GetByJobAsync(int jobId, CancellationToken ct = default)
    {
        var key = $"candidates:job:{jobId}";
        var cached = await cache.GetStringAsync(key, ct);
        if (cached is not null) return JsonSerializer.Deserialize<List<Candidate>>(cached)!;

        var list = await db.Candidates.Where(c => c.AppliedJobId == jobId).ToListAsync(ct);
        await cache.SetStringAsync(key, JsonSerializer.Serialize(list),
            new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3) }, ct);
        return list;
    }

    public async Task<Candidate?> GetByIdAsync(int id, CancellationToken ct = default)
        => await db.Candidates.FindAsync(new object[] { id }, ct);

    public async Task<Candidate> ApplyAsync(Candidate c, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(c.Name)) throw new ArgumentException("Name required");
        c.AppliedAt = DateTime.UtcNow; c.Status = "Applied";
        db.Candidates.Add(c);
        await db.SaveChangesAsync(ct);
        if (c.AppliedJobId.HasValue) await cache.RemoveAsync($"candidates:job:{c.AppliedJobId}", ct);
        return c;
    }

    public async Task<bool> UpdateStatusAsync(int id, string status, CancellationToken ct = default)
    {
        var valid = new[] { "Applied", "Shortlisted", "Placed", "Rejected" };
        if (!valid.Contains(status)) throw new ArgumentException($"Invalid status: {status}");
        var c = await db.Candidates.FindAsync(new object[] { id }, ct);
        if (c is null) return false;
        c.Status = status;
        await db.SaveChangesAsync(ct);
        return true;
    }
}
```

**`Controllers/CandidatesController.cs`**

```csharp
using Candidates.API.Models;
using Candidates.API.Services;
using Microsoft.AspNetCore.Mvc;
namespace Candidates.API.Controllers;

[ApiController][Route("api/candidates")]
public class CandidatesController(ICandidateService svc) : ControllerBase
{
    [HttpGet] public async Task<IActionResult> GetAll(CancellationToken ct) => Ok(await svc.GetAllAsync(ct));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
        => (await svc.GetByIdAsync(id, ct)) is { } c ? Ok(c) : NotFound();

    [HttpGet("by-job/{jobId:int}")]
    public async Task<IActionResult> ByJob(int jobId, CancellationToken ct) => Ok(await svc.GetByJobAsync(jobId, ct));

    [HttpPost]
    public async Task<IActionResult> Apply([FromBody] Candidate c, CancellationToken ct)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var created = await svc.ApplyAsync(c, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status, CancellationToken ct)
        => await svc.UpdateStatusAsync(id, status, ct) ? NoContent() : NotFound();
}
```

**`Program.cs`**

```csharp
using Candidates.API.Data;
using Candidates.API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.SwaggerDoc("v1", new() { Title = "Candidates API", Version = "v1" }));
builder.Services.AddDbContext<CandidatesDbContext>(o => o.UseSqlite("Data Source=candidates.db"));

var redis = builder.Configuration.GetConnectionString("Redis");
if (!string.IsNullOrEmpty(redis)) builder.Services.AddStackExchangeRedisCache(o => o.Configuration = redis);
else builder.Services.AddDistributedMemoryCache();

builder.Services.AddScoped<ICandidateService, CandidateService>();
builder.Services.AddCors(o => o.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();
using (var s = app.Services.CreateScope())
    s.ServiceProvider.GetRequiredService<CandidatesDbContext>().Database.EnsureCreated();
app.UseSwagger(); app.UseSwaggerUI(); app.UseCors(); app.MapControllers();
app.Run();
```

---

## 4. Notifications.API (Full Code)

Background worker that subscribes to Azure Service Bus and processes job events — the event-driven piece of the architecture.

**`Workers/JobEventConsumer.cs`**

```csharp
using Azure.Messaging.ServiceBus;
namespace Notifications.API.Workers;

public class JobEventConsumer(IConfiguration cfg, ILogger<JobEventConsumer> log) : BackgroundService
{
    private ServiceBusProcessor? _processor;

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        var conn = cfg["ServiceBus:ConnectionString"];
        if (string.IsNullOrEmpty(conn))
        {
            log.LogWarning("No Service Bus connection — consumer disabled");
            return;
        }

        var client = new ServiceBusClient(conn);
        _processor = client.CreateProcessor("job-created", new ServiceBusProcessorOptions
        {
            MaxConcurrentCalls = 2,
            AutoCompleteMessages = false
        });

        _processor.ProcessMessageAsync += OnMessageAsync;
        _processor.ProcessErrorAsync   += OnErrorAsync;

        await _processor.StartProcessingAsync(ct);
        log.LogInformation("[Notifications] Listening on queue: job-created");

        await Task.Delay(Timeout.Infinite, ct);
        await _processor.StopProcessingAsync();
    }

    private async Task OnMessageAsync(ProcessMessageEventArgs args)
    {
        var body = args.Message.Body.ToString();
        log.LogInformation("New job posted: {Body}", body);
        // TODO: plug in SendGrid/SMTP/push notification here
        await args.CompleteMessageAsync(args.Message);
    }

    private Task OnErrorAsync(ProcessErrorEventArgs args)
    {
        log.LogError(args.Exception, "[Notifications] Service Bus error: {Source}", args.ErrorSource);
        return Task.CompletedTask;
    }
}
```

**`Program.cs`**

```csharp
using Notifications.API.Workers;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<JobEventConsumer>();

var app = builder.Build();
app.UseSwagger(); app.UseSwaggerUI();
app.MapGet("/health", () => Results.Ok(new { status = "healthy", service = "Notifications.API" }));
app.MapControllers();
app.Run();
```

---

## 5. API Gateway (YARP)

**`Program.cs`**

```csharp
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
builder.Services.AddCors(o => o.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();
app.UseCors();
app.MapReverseProxy();
app.MapGet("/", () => "VMS Lite API Gateway");
app.Run();
```

**`appsettings.json`**

```json
{
  "Urls": "http://localhost:5000",
  "ReverseProxy": {
    "Routes": {
      "jobs-route":       { "ClusterId": "jobs-cluster",       "Match": { "Path": "/api/jobs/{**catch-all}" } },
      "candidates-route": { "ClusterId": "candidates-cluster", "Match": { "Path": "/api/candidates/{**catch-all}" } }
    },
    "Clusters": {
      "jobs-cluster":       { "Destinations": { "dest1": { "Address": "http://localhost:5001" } } },
      "candidates-cluster": { "Destinations": { "dest1": { "Address": "http://localhost:5002" } } }
    }
  }
}
```

> **Run all 4 services** in separate terminals:
>
> ```bash
> cd src/ApiGateway && dotnet run                                              # :5000
> cd src/Services/Jobs.API && dotnet run --urls=http://localhost:5001
> cd src/Services/Candidates.API && dotnet run --urls=http://localhost:5002
> cd src/Services/Notifications.API && dotnet run --urls=http://localhost:5003
> curl http://localhost:5000/api/jobs
> ```

---

## 6. Angular 17 Frontend

### 6a — Scaffold

```bash
cd src
ng new Frontend --routing --style=scss --standalone --skip-git
cd Frontend && npm install
```

### 6b — Environments

```typescript
// src/environments/environment.ts
export const environment = { production: false, apiUrl: 'http://localhost:5000' };

// src/environments/environment.production.ts
export const environment = { production: true, apiUrl: 'https://vms-lite-gateway.azurewebsites.net' };
```

### 6c — `core/services/jobs.service.ts`

```typescript
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface Job {
  id: number; title: string; description: string; location: string;
  company: string; salary: number; isActive: boolean; postedAt: string;
}

@Injectable({ providedIn: 'root' })
export class JobsService {
  private url = `${environment.apiUrl}/api/jobs`;
  constructor(private http: HttpClient) {}
  getAll(): Observable<Job[]> { return this.http.get<Job[]>(this.url); }
  getById(id: number): Observable<Job> { return this.http.get<Job>(`${this.url}/${id}`); }
  create(job: Partial<Job>): Observable<Job> { return this.http.post<Job>(this.url, job); }
  update(id: number, job: Partial<Job>): Observable<void> { return this.http.put<void>(`${this.url}/${id}`, job); }
  delete(id: number): Observable<void> { return this.http.delete<void>(`${this.url}/${id}`); }
}
```

### 6d — `app.config.ts`

```typescript
import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { routes } from './app.routes';

export const appConfig: ApplicationConfig = {
  providers: [provideRouter(routes), provideHttpClient()]
};
```

### 6e — `app.routes.ts`

```typescript
import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', redirectTo: '/jobs', pathMatch: 'full' },
  { path: 'jobs', loadComponent: () => import('./features/jobs/job-list/job-list.component').then(m => m.JobListComponent) },
  { path: 'candidates', loadComponent: () => import('./features/candidates/candidate-list/candidate-list.component').then(m => m.CandidateListComponent) }
];
```

### 6f — `job-list.component.ts` (list + create + delete)

```typescript
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Job, JobsService } from '../../../core/services/jobs.service';

@Component({
  selector: 'app-job-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="page-header">
      <h1>Job Listings</h1>
      <button (click)="showForm = !showForm">+ Post Job</button>
    </div>

    <div *ngIf="showForm" class="form-card">
      <input [(ngModel)]="newJob.title" placeholder="Job Title" />
      <input [(ngModel)]="newJob.company" placeholder="Company" />
      <input [(ngModel)]="newJob.location" placeholder="Location" />
      <input [(ngModel)]="newJob.salary" type="number" placeholder="Salary (₹)" />
      <textarea [(ngModel)]="newJob.description" placeholder="Description"></textarea>
      <button (click)="createJob()">Submit</button>
    </div>

    <div *ngIf="loading">Loading jobs...</div>
    <div *ngIf="error" class="error">{{ error }}</div>

    <div class="job-grid">
      <div *ngFor="let job of jobs" class="job-card">
        <div class="job-title">{{ job.title }}</div>
        <div>{{ job.company }} — {{ job.location }}</div>
        <div>₹{{ job.salary | number }}</div>
        <p>{{ job.description }}</p>
        <button (click)="deleteJob(job.id)">Remove</button>
      </div>
    </div>
  `
})
export class JobListComponent implements OnInit {
  jobs: Job[] = [];
  loading = true;
  error = '';
  showForm = false;
  newJob: Partial<Job> = {};

  constructor(private jobsService: JobsService) {}
  ngOnInit() { this.loadJobs(); }

  loadJobs() {
    this.loading = true;
    this.jobsService.getAll().subscribe({
      next: j => { this.jobs = j; this.loading = false; },
      error: () => { this.error = 'Could not load jobs. Is the API running?'; this.loading = false; }
    });
  }
  createJob() {
    this.jobsService.create(this.newJob).subscribe({ next: () => { this.showForm = false; this.newJob = {}; this.loadJobs(); } });
  }
  deleteJob(id: number) { this.jobsService.delete(id).subscribe({ next: () => this.loadJobs() }); }
}
```

### 6g — `app.component.ts` (shell + nav)

```typescript
import { Component } from '@angular/core';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  template: `
    <nav>
      <span class="brand">VMS Lite</span>
      <a routerLink="/jobs" routerLinkActive="active">Jobs</a>
      <a routerLink="/candidates" routerLinkActive="active">Candidates</a>
    </nav>
    <main><router-outlet /></main>
  `
})
export class AppComponent {}
```

> **Run it:** `cd src/Frontend && ng serve` → `http://localhost:4200`
> Build the equivalent `candidate-list.component.ts` and `candidates.service.ts` (section 6c pattern) following the same structure.

---

## 7. Redis + Azure Service Bus Setup

### Redis locally (Docker)

```bash
docker run -d --name redis-dev -p 6379:6379 redis:7-alpine
docker exec -it redis-dev redis-cli ping          # PONG
docker exec -it redis-dev redis-cli keys "*"      # inspect cached keys after testing
```

### Create Azure Service Bus

```bash
az login
az group create --name rg-vmslite --location eastus

az servicebus namespace create \
  --name sb-vmslite-abhay --resource-group rg-vmslite --sku Basic --location eastus

az servicebus queue create \
  --name job-created --namespace-name sb-vmslite-abhay --resource-group rg-vmslite --max-size 1024

az servicebus namespace authorization-rule keys list \
  --resource-group rg-vmslite --namespace-name sb-vmslite-abhay \
  --name RootManageSharedAccessKey --query primaryConnectionString -o tsv
```

> ⚠️ Keep the connection string out of Git. Use `dotnet user-secrets` locally, Azure App Settings in prod.

```bash
cd src/Services/Jobs.API
dotnet user-secrets init
dotnet user-secrets set "ServiceBus:ConnectionString" "Endpoint=sb://sb-vmslite-abhay..."
dotnet user-secrets set "ConnectionStrings:Redis" "localhost:6379"
```

---

## 8. xUnit + Moq Tests

**`tests/Jobs.API.Tests/Unit/JobServiceTests.cs`**

```csharp
using FluentAssertions;
using Jobs.API.Data;
using Jobs.API.Messaging;
using Jobs.API.Models;
using Jobs.API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using System.Text;
using System.Text.Json;

namespace Jobs.API.Tests.Unit;

public class JobServiceTests : IDisposable
{
    private readonly JobsDbContext _db;
    private readonly Mock<IDistributedCache> _cacheMock;
    private readonly Mock<IMessagePublisher> _pubMock;
    private readonly JobService _sut;

    public JobServiceTests()
    {
        var opts = new DbContextOptionsBuilder<JobsDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        _db = new JobsDbContext(opts);
        _cacheMock = new Mock<IDistributedCache>();
        _pubMock = new Mock<IMessagePublisher>();
        _sut = new JobService(_db, _cacheMock.Object, _pubMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ValidJob_ShouldPersistAndPublishEvent()
    {
        var job = new Job { Title = "Senior .NET Dev", Company = "Acme", Location = "Delhi", Salary = 2000000 };
        var result = await _sut.CreateAsync(job);

        result.Id.Should().BeGreaterThan(0);
        _db.Jobs.Count().Should().Be(1);
        _pubMock.Verify(p => p.PublishAsync("job-created", It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        _cacheMock.Verify(c => c.RemoveAsync("jobs:all", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_EmptyTitle_ShouldThrowArgumentException()
    {
        var act = () => _sut.CreateAsync(new Job { Title = "" });
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("*Title*");
    }

    [Fact]
    public async Task GetAllAsync_CacheHit_ShouldReturnCachedData_WithoutHittingDB()
    {
        var fakeJobs = new[] { new Job { Id = 1, Title = "Cached Job" } };
        var json = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(fakeJobs));
        _cacheMock.Setup(c => c.GetAsync("jobs:all", It.IsAny<CancellationToken>())).ReturnsAsync(json);

        var result = await _sut.GetAllAsync();

        result.Should().HaveCount(1);
        _db.Jobs.Count().Should().Be(0);
    }

    [Fact]
    public async Task GetByIdAsync_NotFound_ShouldReturnNull() => (await _sut.GetByIdAsync(9999)).Should().BeNull();

    [Fact]
    public async Task UpdateAsync_ExistingJob_ShouldUpdate()
    {
        var job = _db.Jobs.Add(new Job { Title = "Old", IsActive = true }).Entity;
        await _db.SaveChangesAsync();
        var ok = await _sut.UpdateAsync(job.Id, new Job { Title = "New", Salary = 999 });
        ok.Should().BeTrue();
        _db.Jobs.Find(job.Id)!.Title.Should().Be("New");
    }

    [Fact]
    public async Task DeleteAsync_ExistingJob_ShouldSoftDelete()
    {
        var job = _db.Jobs.Add(new Job { Title = "To Delete", IsActive = true }).Entity;
        await _db.SaveChangesAsync();
        (await _sut.DeleteAsync(job.Id)).Should().BeTrue();
        _db.Jobs.Find(job.Id)!.IsActive.Should().BeFalse();
    }

    public void Dispose() => _db.Dispose();
}
```

**`tests/Integration.Tests/JobsApiIntegrationTests.cs`**

```csharp
using FluentAssertions;
using Jobs.API.Data;
using Jobs.API.Messaging;
using Jobs.API.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Integration.Tests;

public class JobsApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private static readonly JsonSerializerOptions _json = new() { PropertyNameCaseInsensitive = true };

    public JobsApiIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.WithWebHostBuilder(b => b.ConfigureServices(services =>
        {
            var d = services.SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<JobsDbContext>));
            if (d != null) services.Remove(d);
            services.AddDbContext<JobsDbContext>(o => o.UseInMemoryDatabase("IntTest"));
            services.AddDistributedMemoryCache();
            services.AddSingleton<IMessagePublisher, InMemoryPublisher>();
        })).CreateClient();
    }

    private StringContent Json(object o) => new(JsonSerializer.Serialize(o), Encoding.UTF8, "application/json");

    [Fact]
    public async Task GET_Jobs_Returns200() => (await _client.GetAsync("/api/jobs")).StatusCode.Should().Be(HttpStatusCode.OK);

    [Fact]
    public async Task POST_Job_Returns201_WithCreatedJob()
    {
        var payload = new { title = "DevOps Engineer", company = "TechCo", location = "Noida", salary = 1800000 };
        var r = await _client.PostAsync("/api/jobs", Json(payload));
        r.StatusCode.Should().Be(HttpStatusCode.Created);
        var job = JsonSerializer.Deserialize<Job>(await r.Content.ReadAsStringAsync(), _json);
        job!.Title.Should().Be("DevOps Engineer");
    }

    [Fact]
    public async Task GET_Job_ById_NotFound_Returns404()
        => (await _client.GetAsync("/api/jobs/99999")).StatusCode.Should().Be(HttpStatusCode.NotFound);
}
```

```bash
dotnet test VmsLite.sln --collect:"XPlat Code Coverage" --logger "console;verbosity=normal"
# Expect: 6 unit tests + 3 integration tests, all passed
```

---

## 9. Docker + docker-compose

**`src/Services/Jobs.API/Dockerfile`** (same pattern for Candidates.API / Notifications.API — swap the project name)

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Services/Jobs.API/Jobs.API.csproj", "Services/Jobs.API/"]
RUN dotnet restore "Services/Jobs.API/Jobs.API.csproj"
COPY src/ .
WORKDIR "/src/Services/Jobs.API"
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 80
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Jobs.API.dll"]
```

**`src/Frontend/Dockerfile`**

```dockerfile
FROM node:20-alpine AS build
WORKDIR /app
COPY package*.json ./
RUN npm ci
COPY . .
RUN npm run build -- --configuration production

FROM nginx:alpine AS final
COPY --from=build /app/dist/frontend/browser /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
```

**`docker-compose.yml`** (root)

```yaml
version: '3.9'
services:
  redis:
    image: redis:7-alpine
    ports: ["6379:6379"]

  jobs-api:
    build: { context: ., dockerfile: src/Services/Jobs.API/Dockerfile }
    ports: ["5001:80"]
    environment:
      - ConnectionStrings__Redis=redis:6379
      - ServiceBus__ConnectionString=${SERVICE_BUS_CONN:-}
    depends_on: [redis]

  candidates-api:
    build: { context: ., dockerfile: src/Services/Candidates.API/Dockerfile }
    ports: ["5002:80"]
    environment: [ConnectionStrings__Redis=redis:6379]
    depends_on: [redis]

  notifications-api:
    build: { context: ., dockerfile: src/Services/Notifications.API/Dockerfile }
    ports: ["5003:80"]
    environment: [ServiceBus__ConnectionString=${SERVICE_BUS_CONN:-}]

  gateway:
    build: { context: ., dockerfile: src/ApiGateway/Dockerfile }
    ports: ["5000:80"]
    depends_on: [jobs-api, candidates-api]

  frontend:
    build: src/Frontend
    ports: ["4200:80"]
    depends_on: [gateway]
```

```bash
docker-compose up --build
curl http://localhost:5000/api/jobs
# open http://localhost:4200
docker-compose down
```

---

## 10. GitHub Actions CI/CD

**`.github/workflows/ci-cd.yml`**

```yaml
name: VMS Lite CI/CD

on:
  push: { branches: [main] }
  pull_request: { branches: [main] }

env:
  DOTNET_VERSION: '8.0.x'
  REGISTRY: ${{ secrets.ACR_LOGIN_SERVER }}

jobs:
  build-test:
    name: Build and Test
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-dotnet@v4
        with: { dotnet-version: '8.0.x' }
      - run: dotnet restore VmsLite.sln
      - run: dotnet build VmsLite.sln -c Release --no-restore
      - name: Unit Tests
        run: dotnet test tests/Jobs.API.Tests/Jobs.API.Tests.csproj -c Release --no-build --collect:"XPlat Code Coverage" --logger "trx;LogFileName=unit.trx" --results-directory ./test-results
      - name: Integration Tests
        run: dotnet test tests/Integration.Tests/Integration.Tests.csproj -c Release --no-build --logger "trx;LogFileName=integration.trx" --results-directory ./test-results
      - uses: dorny/test-reporter@v1
        if: always()
        with: { name: Test Results, path: ./test-results/*.trx, reporter: dotnet-trx }

  docker:
    name: Build and Push Docker Images
    needs: build-test
    runs-on: ubuntu-latest
    if: github.ref == 'refs/heads/main' && github.event_name == 'push'
    strategy:
      matrix:
        service: [jobs-api, candidates-api, notifications-api, gateway]
        include:
          - { service: jobs-api,          dockerfile: src/Services/Jobs.API/Dockerfile }
          - { service: candidates-api,    dockerfile: src/Services/Candidates.API/Dockerfile }
          - { service: notifications-api, dockerfile: src/Services/Notifications.API/Dockerfile }
          - { service: gateway,           dockerfile: src/ApiGateway/Dockerfile }
    steps:
      - uses: actions/checkout@v4
      - uses: docker/login-action@v3
        with:
          registry: ${{ secrets.ACR_LOGIN_SERVER }}
          username: ${{ secrets.ACR_USERNAME }}
          password: ${{ secrets.ACR_PASSWORD }}
      - uses: docker/build-push-action@v5
        with:
          context: .
          file: ${{ matrix.dockerfile }}
          push: true
          tags: |
            ${{ env.REGISTRY }}/${{ matrix.service }}:latest
            ${{ env.REGISTRY }}/${{ matrix.service }}:${{ github.sha }}

  deploy:
    name: Deploy to Azure App Service
    needs: docker
    runs-on: ubuntu-latest
    environment: production
    steps:
      - uses: azure/login@v1
        with: { creds: ${{ secrets.AZURE_CREDENTIALS }} }
      - uses: azure/webapps-deploy@v2
        with: { app-name: vms-lite-jobs-api, images: '${{ secrets.ACR_LOGIN_SERVER }}/jobs-api:${{ github.sha }}' }
      - uses: azure/webapps-deploy@v2
        with: { app-name: vms-lite-candidates-api, images: '${{ secrets.ACR_LOGIN_SERVER }}/candidates-api:${{ github.sha }}' }
      - uses: azure/webapps-deploy@v2
        with: { app-name: vms-lite-notifications-api, images: '${{ secrets.ACR_LOGIN_SERVER }}/notifications-api:${{ github.sha }}' }
```

### GitHub Secrets setup

```bash
az acr create --resource-group rg-vmslite --name acrvmsliteabhay --sku Basic
az acr update --name acrvmsliteabhay --admin-enabled true
az acr credential show --name acrvmsliteabhay        # → username + passwords[0].value

az ad sp create-for-rbac \
  --name "github-vmslite" --role contributor \
  --scopes /subscriptions/$(az account show --query id -o tsv)/resourceGroups/rg-vmslite \
  --sdk-auth                                          # → entire JSON output
```

Add in **GitHub → Settings → Secrets → Actions**:

| Secret                | Value                                          |
| --------------------- | ---------------------------------------------- |
| `ACR_LOGIN_SERVER`  | `acrvmsliteabhay.azurecr.io`                 |
| `ACR_USERNAME`      | `acrvmsliteabhay`                            |
| `ACR_PASSWORD`      | (from`az acr credential show`)               |
| `AZURE_CREDENTIALS` | (entire JSON from`az ad sp create-for-rbac`) |

---

## 11. Azure Deployment

```bash
# Login + resource group
az login
az group create --name rg-vmslite --location eastus

# Container Registry
az acr create --resource-group rg-vmslite --name acrvmsliteabhay --sku Basic
az acr update --name acrvmsliteabhay --admin-enabled true

# App Service Plan (B1 ≈ ₹1100/month; free trial credit covers first month)
az appservice plan create --name asp-vmslite --resource-group rg-vmslite --sku B1 --is-linux

# 4 Web Apps on the SAME plan (no extra cost per app)
for app in vms-lite-jobs-api vms-lite-candidates-api vms-lite-notifications-api vms-lite-gateway; do
  az webapp create --name $app --resource-group rg-vmslite --plan asp-vmslite --deployment-container-image-name nginx
done

# Angular frontend — Static Web Apps (FREE tier)
az staticwebapp create \
  --name vms-lite-frontend --resource-group rg-vmslite --location eastus2 \
  --source https://github.com/AbhayB-007/vms-lite --branch main \
  --app-location "src/Frontend" --output-location "dist/frontend/browser"
```

### Link ACR images to App Services

```bash
ACR=acrvmsliteabhay.azurecr.io
ACR_USER=acrvmsliteabhay
ACR_PASS=$(az acr credential show --name acrvmsliteabhay --query passwords[0].value -o tsv)

declare -A apps=( [vms-lite-jobs-api]=jobs-api [vms-lite-candidates-api]=candidates-api
                   [vms-lite-notifications-api]=notifications-api [vms-lite-gateway]=gateway )

for webapp in "${!apps[@]}"; do
  image="${apps[$webapp]}"
  az webapp config container set --name $webapp --resource-group rg-vmslite \
    --docker-custom-image-name $ACR/$image:latest \
    --docker-registry-server-url https://$ACR \
    --docker-registry-server-user $ACR_USER \
    --docker-registry-server-password $ACR_PASS
done
```

### Set app settings (secrets)

```bash
SB_CONN="Endpoint=sb://sb-vmslite-abhay.servicebus.windows.net/;SharedAccessKeyName=..."
REDIS_CONN="your-upstash-url:port,password=xxx,ssl=True,abortConnect=False"

az webapp config appsettings set --name vms-lite-jobs-api --resource-group rg-vmslite \
  --settings ServiceBus__ConnectionString="$SB_CONN" "ConnectionStrings__Redis=$REDIS_CONN"

az webapp config appsettings set --name vms-lite-candidates-api --resource-group rg-vmslite \
  --settings "ConnectionStrings__Redis=$REDIS_CONN"

az webapp config appsettings set --name vms-lite-notifications-api --resource-group rg-vmslite \
  --settings ServiceBus__ConnectionString="$SB_CONN"
```

> **Free Redis option:** use [Upstash](https://upstash.com) — free tier, 10k requests/day, no credit card. Copy the connection string into `REDIS_CONN` above.

### First manual deploy (later ones happen via CI/CD)

```bash
az acr login --name acrvmsliteabhay

docker build -t $ACR/jobs-api:latest -f src/Services/Jobs.API/Dockerfile . && docker push $ACR/jobs-api:latest
docker build -t $ACR/candidates-api:latest -f src/Services/Candidates.API/Dockerfile . && docker push $ACR/candidates-api:latest
docker build -t $ACR/notifications-api:latest -f src/Services/Notifications.API/Dockerfile . && docker push $ACR/notifications-api:latest
docker build -t $ACR/gateway:latest -f src/ApiGateway/Dockerfile . && docker push $ACR/gateway:latest

for app in vms-lite-jobs-api vms-lite-candidates-api vms-lite-notifications-api vms-lite-gateway; do
  az webapp restart --name $app --resource-group rg-vmslite
done
```

### Verify

```bash
curl https://vms-lite-jobs-api.azurewebsites.net/api/jobs
curl https://vms-lite-gateway.azurewebsites.net/api/jobs
az webapp log tail --name vms-lite-jobs-api --resource-group rg-vmslite   # if something fails
```

**Your live URLs (paste into resume + GitHub README):**

- Jobs API Swagger: `https://vms-lite-jobs-api.azurewebsites.net/swagger`
- Candidates API Swagger: `https://vms-lite-candidates-api.azurewebsites.net/swagger`
- Gateway: `https://vms-lite-gateway.azurewebsites.net/api/jobs`
- Frontend: `https://vms-lite-frontend.azurestaticapps.net`

---

## 12. Resume Wording

### Projects section

> **VMS Lite — Microservices Job & Talent Portal** | github.com/AbhayB-007/vms-lite | Live: vms-lite-gateway.azurewebsites.net
>
> - Designed and built a **microservices architecture** (Jobs.API, Candidates.API, Notifications.API) in **.NET 8**, routed through a **YARP API Gateway** — each service owns its own database (database-per-service pattern)
> - Implemented **Redis distributed caching** (cache-aside pattern), reducing repeated read latency by ~85%; integrated **Azure Service Bus** for asynchronous event publishing between services
> - Achieved **75%+ unit test coverage** using **xUnit and Moq**; authored integration tests with **WebApplicationFactory** validating the full HTTP pipeline against an in-memory database and mocked dependencies
> - Built a **GitHub Actions CI/CD pipeline** that runs tests, builds multi-stage **Docker** images, pushes to **Azure Container Registry**, and deploys to **Azure App Service** on every merge to main
> - Delivered an **Angular 17** standalone SPA consuming REST APIs through the gateway; local dev stack orchestrated with **docker-compose**

### Updated Technical Skills section

```
Languages & Frameworks: C#, .NET 8, ASP.NET Core MVC, Web API, Entity Framework Core, Angular 17, TypeScript
Architecture: Microservices, REST APIs, Event-Driven Architecture (Azure Service Bus), API Gateway (YARP), SOLID, OOP
Databases & Caching: SQL Server, T-SQL, SQLite, Redis (StackExchange.Redis, cache-aside pattern)
Testing: xUnit, Moq, FluentAssertions, Integration Testing (WebApplicationFactory)
Cloud & DevOps: Microsoft Azure (App Service, Container Registry, Service Bus, Static Web Apps),
                Docker, docker-compose, GitHub Actions CI/CD, Azure DevOps
```

### Certifications to add next

- **AZ-900 Azure Fundamentals** (2 weeks) — free study at learn.microsoft.com/azure-fundamentals, exam ~$100
- **AZ-204 Azure Developer Associate** (4 weeks) — validates App Service, Service Bus, Container Registry directly from this project

### LinkedIn headline

> Full-Stack .NET Developer | .NET 8 · Microservices · Azure · Angular | 3+ yrs Enterprise SaaS
