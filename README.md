# OnlineBazar - Production-Ready Ecommerce (ASP.NET Core MVC + EF Core + SQL Server)

## 1. Website Analysis

### Purpose
The original commerce flow was assumed to support browsing products, viewing details, customer authentication, and admin-side catalog management. This rebuild keeps the same core ecommerce goals but introduces a clean, modern UI and maintainable architecture.

### Features & User Flow
1. Visitor lands on Home page, sees featured products and CTAs.
2. Visitor browses Product Catalog and opens Product Details.
3. Authenticated users can log in via Identity.
4. Admin/Manager users enter Admin Area for dashboard analytics and product CRUD.
5. Admin uploads product images and updates storefront content.

### Key Modules
- Public storefront (Home, Products, About, Contact)
- Identity/authentication & role authorization
- Admin area with Dashboard + Product Management
- Application service layer and data access abstractions

## 2. System Architecture

### Layered Design
- **Presentation Layer**: MVC controllers + Razor views (`Controllers`, `Areas/Admin/Controllers`, `Views`)
- **Application Layer**: business services and DTOs (`Services`, `DTOs`, `ViewModels`)
- **Domain Layer**: entities (`Models`)
- **Infrastructure Layer**: EF Core context, repositories, UoW (`Data`, `Repositories`)

### Patterns Used
- Repository + Unit of Work
- DTO mapping for API/view transport decoupling
- Role-based authorization with ASP.NET Identity
- Soft delete (`IsDeleted`) + audit fields (`CreatedDate`, `ModifiedDate`, etc.)

### Clean Code Practices
- Dependency injection for all service/repository dependencies
- Segregated interfaces per service responsibility
- Central startup and middleware wiring in `Program.cs`

## 3. Database Schema (SQL Server, Code-First)

See full SQL DDL in `database/schema.sql`.

Core tables include:
- `AspNetUsers`, `AspNetRoles`, etc. (Identity)
- `Categories`
- `Products`
- `Orders`
- `OrderItems`
- `SiteContents`

Relationships:
- Category 1..* Products
- User 1..* Orders
- Order 1..* OrderItems
- Product 1..* OrderItems

## 4. Backend Implementation (ASP.NET Core)

### Authentication & Authorization
- Identity configured with strong password policy.
- Roles seeded: `Admin`, `Manager`, `Customer`.
- Admin area protected with `[Authorize(Roles = "Admin,Manager")]`.

### Services & Repositories
- `ProductService` handles catalog/CRUD workflows.
- `OrderService` provides sales KPIs.
- `DashboardService` aggregates analytics.
- `GenericRepository<T>` + `UnitOfWork` encapsulate EF operations.

### Exception Handling
- Global production exception middleware via `UseExceptionHandler`.
- Guard clauses + explicit not-found checks in service/controller methods.

## 5. Razor Views / UI Implementation

### Frontend
- Bootstrap 5 responsive layout.
- Modern gradient brand styling in `wwwroot/css/site.css`.
- SEO-ready metadata in shared layout.
- Reusable layout structure for public and admin experiences.

### Razor Components
- Shared layouts:
  - `Views/Shared/_Layout.cshtml` (public)
  - `Areas/Admin/Views/Shared/_AdminLayout.cshtml` (admin)
- Entity-specific views:
  - Storefront product listing/details
  - Admin product index/create/edit

## 6. Admin Panel Structure

Area-based modular admin architecture (`Areas/Admin`):
- `DashboardController` → overview KPI cards
- `ProductsController` → product CRUD + image upload
- Authorization at controller level for secure operations

Recommended next CRUD modules (same pattern):
- Categories
- Orders & status workflow
- SiteContent CMS blocks/banners
- Users/Roles management

## 7. Deployment Steps

1. Install .NET 8 SDK and SQL Server (or Azure SQL).
2. Set connection string in `appsettings.Production.json`.
3. Apply migrations:
   - `dotnet ef migrations add InitialCreate`
   - `dotnet ef database update`
4. Publish:
   - `dotnet publish -c Release -o ./publish`
5. Deploy behind reverse proxy (IIS/Nginx) with HTTPS.
6. Configure environment variables:
   - `ASPNETCORE_ENVIRONMENT=Production`
   - secure secrets in Key Vault/user-secrets.
7. Set up logging, backups, and SQL indexing strategy.

---

## Improvements over original implementation
- Completely new visual theme and responsive storefront.
- Explicit layered architecture for maintainability and scale.
- DTO and service boundaries to reduce controller complexity.
- Soft-delete + audit fields for enterprise-grade data governance.
- Role-secured admin area with extensible module strategy.
