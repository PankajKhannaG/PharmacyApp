# ABC Pharmacy — Medicine Inventory & Sales Tracker

A Single Page Application for tracking medicine inventory and sales.

- **Backend:** ASP.NET Core 8 Web API + Entity Framework Core (SQL Server)
- **Frontend:** React SPA (Create React App)

## Project structure

```
PharmacyApp/
├── backend/PharmacyApi/        ASP.NET Core Web API
│   ├── Models/                 Medicine, SaleRecord entities
│   ├── Data/                   PharmacyContext (EF Core DbContext) + seed data
│   ├── DTOs/                   Request/response contracts
│   ├── Controllers/            MedicinesController, SalesController
│   └── Program.cs
└── frontend/pharmacy-client/   React SPA
    └── src/
        ├── api/medicineApi.js  fetch wrapper for the API
        ├── components/         MedicineGrid, AddMedicineForm, SaleForm, SearchBar
        └── App.js
```

## A note on the "steps to launch" you listed

`dotnet new console` scaffolds a blank console app, not a web project, so it won't give you
controllers, Kestrel, or Swagger. The steps below (`dotnet new webapi` equivalent — already
scaffolded for you — plus `dotnet ef database update` and `dotnet run`) are what actually stands
up this Web API. Use these instead.

## Prerequisites

- .NET 8 SDK
- Node.js 18+ and npm
- SQL Server (LocalDB, a full instance, or SQL Server in Docker)
- `dotnet-ef` tool: `dotnet tool install --global dotnet-ef` (if not already installed)

## 1. Configure the database connection

Edit `backend/PharmacyApi/appsettings.json` and set `ConnectionStrings:DefaultConnection` to your
SQL Server instance, e.g. for LocalDB:

```json
"DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=PharmacyDb;Trusted_Connection=True;TrustServerCertificate=True;"
```

## 2. Run the backend API

```bash
cd backend/PharmacyApi
dotnet restore
dotnet ef migrations add InitialCreate
dotnet ef database update
dotnet run
```

This starts the API (see `Properties/launchSettings.json` for the URL, default
`https://localhost:7100`) and opens Swagger at `/swagger`. `Program.cs` also calls
`db.Database.Migrate()` on startup, so once the migration exists the database schema stays current
automatically. Three sample medicines are seeded on first run, including one expiring soon and one
low on stock so you can see the color coding immediately.

## 3. Run the frontend SPA

```bash
cd frontend/pharmacy-client
npm install
npm start
```

This opens the app at `http://localhost:3000`. The `proxy` setting in `package.json` forwards
`/api/*` calls to `http://localhost:5100` (the HTTP profile) — adjust it if you run the API on a
different port, or change it to the HTTPS URL.

## Features implemented

- **Grid view** of all medicines (Full Name, Brand, Expiry Date, Quantity, Price) — Notes is
  excluded from the grid as specified, and is only visible via `GET /api/medicines/{id}` in the
  detail DTO.
- **Color indicators**
  - Red row background when expiry date is within 30 days (`isNearExpiry` computed server-side).
  - Yellow row background when quantity is below 10 (`isLowStock`).
  - If a medicine is both expiring and low stock, the expiry (red) styling takes priority.
- **Search** by medicine name (`GET /api/medicines?search=...`), debounced client-side.
- **Add medicine** via a modal form with client + server-side validation.
- **Delete medicine** for basic housekeeping (not explicitly required, but small enough to include).
- **Sale recording**: `POST /api/sales` validates available stock, decrements the medicine's
  quantity, and stores a `SaleRecord` with the price at time of sale and total amount —
  giving you an auditable sales history (`GET /api/sales`) separate from current inventory.

## API summary

| Method | Route                 | Purpose                                   |
|--------|-----------------------|--------------------------------------------|
| GET    | /api/medicines         | List medicines, optional `?search=`       |
| GET    | /api/medicines/{id}    | Full detail (includes Notes)              |
| POST   | /api/medicines          | Add a medicine                            |
| PUT    | /api/medicines/{id}    | Update a medicine                         |
| DELETE | /api/medicines/{id}    | Delete a medicine                         |
| GET    | /api/sales              | List sale history                         |
| POST   | /api/sales              | Record a sale (decrements stock)          |

## Notes on "data stored as JSON in SQL Server"

The brief mentions storing data "in Json on server side in SQL Server." This solution uses standard
relational tables via EF Core (one row per medicine/sale), which is the conventional, queryable way
to do this in SQL Server and is what the search/grid/reporting features need. If you specifically
need the medicine record itself persisted as a JSON blob (e.g. in an `NVARCHAR(MAX)` /
`JSON` column for a document-style pattern), that's a straightforward variant — let me know and I
can adjust the `Medicine` entity and DbContext mapping accordingly.
