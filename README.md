# OrderIt

**OrderIt** is a full-featured online food-ordering web application that connects customers, restaurant administrators, and delivery drivers in one platform. Built with ASP.NET Core MVC and Entity Framework Core, it handles the complete order lifecycle — from browsing the menu to placing an order, accepting it, dispatching a driver, and marking it delivered.

---

## Table of Contents

- [Features](#features)
- [Tech Stack](#tech-stack)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [1. Clone the repository](#1-clone-the-repository)
  - [2. Configure the connection string](#2-configure-the-connection-string)
  - [3. Apply the database migrations](#3-apply-the-database-migrations)
  - [4. Run the application](#4-run-the-application)
- [Project Structure](#project-structure)
- [Roles & Workflow](#roles--workflow)
  - [Customer](#customer)
  - [Administrator](#administrator)
  - [Delivery Driver](#delivery-driver)
- [Background Jobs](#background-jobs)
- [Configuration](#configuration)
- [Deployment (Publish)](#deployment-publish)
- [Testing](#testing)
- [Troubleshooting](#troubleshooting)
- [License](#license)

---

## Features

### Customer ("User" role)
- Browse the menu grouped by category with dish images, prices and descriptions.
- Add items to cart, change quantities, and remove items.
- Interactive **Leaflet map** on the checkout page — click or drop a marker to auto-fill the delivery address via reverse geocoding (OpenStreetMap / Nominatim).
- Place orders and track them by status: **Pending → Processing → Under Delivery → Delivered**.
- Cancel a pending order before it is accepted.

### Administrator ("Admin" role)
- Dedicated **sidebar dashboard** with live counters (users, categories, menu items, orders broken down by status).
- Full user management: create, edit, delete, and assign roles.
- Role management (create / delete roles).
- Menu & category management, including uploading dish photos and availability toggles.
- Order management: accept orders, start delivery, cancel orders.

### Delivery Driver ("Delivery" role)
- Sidebar dashboard with driver-specific views.
- **Processing Orders**: claim available orders (`IsTaken`).
- **Accepted Orders**: view claimed orders and mark them **Delivered**.

### Platform
- ASP.NET Core Identity (register / login, email confirmation, 2FA-ready UI, account management).
- **Hangfire** background job that auto-cancels pending orders older than one hour.
- Responsive, RTL-ready UI built on Bootstrap 5 + Bootstrap Icons + Cairo/Roboto fonts.
- Seeded data (roles and starter menu) via EF Core migrations.

---

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core MVC (Razor Views + Razor Pages for Identity) |
| Language / SDK | C# / .NET 9.0 |
| ORM | Entity Framework Core 9 |
| Database | SQL Server |
| Identity | ASP.NET Core Identity (custom `AppUser` extending `IdentityUser`) |
| Background jobs | Hangfire (SQL Server storage) |
| Map | Leaflet + OpenStreetMap tiles + Nominatim reverse geocoding |
| Frontend | Bootstrap 5, Bootstrap Icons, jQuery, Cairo/Roboto fonts |
| Email | MailKit (SMTP via Gmail) |
| Tests | xUnit + Moq |

---

## Getting Started

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- SQL Server (local, container, or cloud — e.g. Azure SQL / shared hosting)
- An SMTP account is optional (only needed for email confirmation/2FA flows)

### 1. Clone the repository

```bash
git clone https://github.com/your-org/OrderIt1.1.git
cd OrderIt1.1
```

### 2. Configure the connection string

The connection string is intentionally **not committed** to the repository for security.

**Option A — user secrets (local development, recommended):**

```bash
cd OrderITDemo
dotnet user-secrets set "ConnectionStrings:DefaultConnection" \
  "Server=localhost;Database=OrderIt;User Id=sa;Password=YourPassword;TrustServerCertificate=True;MultipleActiveResultSets=True;"
```

**Option B — `appsettings.json` / environment variable:**

Set the `ConnectionStrings__DefaultConnection` environment variable, or fill `"DefaultConnection"` in `appsettings.json`.

> `Program.cs` throws a clear startup error if this value is missing or empty.

### 3. Apply the database migrations

```bash
cd OrderITDemo
dotnet ef database update
```

This creates the schema and runs the seed migrations (roles + starter menu).

### 4. Run the application

```bash
dotnet run          # default: http://localhost:5xxx
```

Open the printed URL. Register a new account, then (in a fresh database) assign yourself the **Admin** role via the role/user management screens or directly in the database.

---

## Project Structure

```
OrderIt1.1/
├── OrderITDemo/                      # Main web application
│   ├── Abstraction/                  # Service interfaces (ICartService, IOrderService)
│   ├── Api/                          # Minimal JSON APIs (e.g. user DELETE endpoint)
│   ├── Areas/Identity/               # ASP.NET Core Identity Razor Pages
│   ├── Controllers/                  # MVC controllers
│   │   ├── AdminController.cs        # Dashboard counts
│   │   ├── HomeController.cs         # Landing page + Hangfire recurring job
│   │   ├── MenuController.cs         # Menu & categories (admin + public)
│   │   ├── CartController.cs         # Cart (User role)
│   │   ├── OrderController.cs        # Order lifecycle + driver views
│   │   ├── UserController.cs         # User CRUD + roles
│   │   └── RoleController.cs         # Role management
│   ├── Data/
│   │   ├── ApplicationDbContext.cs   # EF Core context
│   │   └── Migrations/               # EF Core migrations + seed data
│   ├── Models/                       # Entity models
│   ├── Repository/                   # Generic repository + base interface
│   ├── Services/                     # CartService, BackGroundService, EmailSender
│   ├── ViewModels/                   # Views' data models
│   ├── Views/                        # Razor views
│   │   └── Shared/
│   │       ├── _Layout.cshtml        # Public/top-nav layout
│   │       └── _AdminLayout.cshtml   # Admin sidebar layout
│   └── wwwroot/                      # Bootstrap, Leaflet, fonts, CSS, JS, images
├── OrderITDemo.Tests/                # Unit tests (xUnit + Moq)
└── OrderItTest/                      # Additional tests
```

---

## Roles & Workflow

### Customer

1. Browse `Menu/IndexUser` and add dishes to the cart.
2. Open the cart, adjust quantities, and pick a delivery address on the map (address auto-fills on click/drag).
3. Place the order → status becomes **Pending**.
4. Track progress from `Order/Index`; cancel while it is still **Pending**.

### Administrator

1. Log in and land on the sidebar **Dashboard** (`/Admin/Dashboard`).
2. Manage users, roles, categories and menu items from the sidebar.
3. Accept pending orders (`AcceptOrder` → **Processing**), start delivery (`OnDelivery` → **Under Delivery**), or cancel them.
4. Audit everything through filtered views in `Order/ManageOrders`.

### Delivery Driver

1. See **Processing** orders in `Order/DriverIndex`.
2. Claim an order (`IsTaken`) — it moves to **Accepted Orders**.
3. Mark it **Delivered** in `Order/DriverOrders`.

> The driver shows only in the *Manual flow* described in `OrderController`. Actual status transitions: `Pendeing → Processing → UnderDelivery → Delivered` (or `Cancelled`).

---

## Background Jobs

[Hangfire](https://www.hangfire.io/) runs a recurring job (hourly) defined in `HomeController.Index`:

```csharp
RecurringJob.AddOrUpdate(() => CancelOrder(), Cron.Hourly);
```

`BackGroundService.CancelOrder()` automatically flips any **Pending** order older than one hour to **Cancelled**, preventing stale orders from sitting in the queue.

The Hangfire dashboard is exposed at `/dash` (SQL Server storage is configured `UseSqlServerStorage`).

---

## Configuration

| Key | Purpose |
|---|---|
| `ConnectionStrings:DefaultConnection` | SQL Server connection string (required) |
| `EmailSettings:SmtpServer` / `Port` / `Username` / `Password` / `EnableSsl` | SMTP (MailKit) for Identity emails |
| `Branding:*` | Overridable site name, tagline, support email/phone used in admin layout |
| `AllowedHosts` | Host header filtering |

Secrets belong in **user secrets** (dev) or **server environment variables** (prod) — never commit credentials.

---

## Deployment (Publish)

```bash
dotnet publish OrderITDemo -c Release -o ./publish
```

On the hosting server ensure:

1. **Connection string** is set — via an environment variable `ConnectionStrings__DefaultConnection` or by editing `appsettings.json` in the published output (credentials must be configured on the server, not in Git).
2. **Database** is reachable and migrations are applied (or run `dotnet ef database update` targeting the production database).
3. Run the app with your host (e.g. `ExecuteOrderIt.bat` / IIS / Kestrel behind a reverse proxy). The app uses HTTPS redirection and HSTS in production.

> Common production startup error: `InvalidOperationException: The ConnectionString property has not been initialized.` — this means the connection string is empty on the server. Set the environment variable above.

---

## Testing

Two test projects are included:

- `OrderITDemo.Tests/` — controller/service unit tests (xUnit + Moq).
- `OrderItTest/` — additional unit tests.

Run all tests:

```bash
dotnet test OrderITDemo.sln
```

---

## Troubleshooting

| Symptom | Cause / Fix |
|---|---|
| `The ConnectionString property has not been initialized` | Connection string empty on server → set `ConnectionStrings__DefaultConnection` env var or fill `appsettings.json` on the host |
| Database tables not found at runtime | Run `dotnet ef database update` against the production database |
| Emails not sent | Check `EmailSettings` (SMTP app-password for Gmail) |
| Map tiles not loading | The cart page requires internet access to `tile.openstreetmap.org` and `nominatim.openstreetmap.org` |
| Admin sidebar not collapsing on mobile | Ensure `~/js/site.js` is served (contains the sidebar toggle handler) |

---

## License

This is a course/demo project. Replace with the license that applies to your work.

---

*Built with ASP.NET Core MVC, EF Core, SQL Server, Hangfire, Leaflet, and Bootstrap.*