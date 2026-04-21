# SmilyBazar Rebuild Blueprint (ASP.NET Core + Angular + SQL Server)

## 1) Website Analysis

> Note: Direct deep crawl of `https://www.smilybazar.com/` was partially restricted in this environment, but the homepage title and marketplace pattern indicate a classified/e-commerce style directory. The architecture below recreates the same core business capability (public discovery + managed listings/content + admin operations), while modernizing UX and scalability.

### Purpose
- Connect buyers with sellers via browsable listings.
- Present business pages (Home, About, Features/Listings, Contact).
- Provide admin-controlled content and listing moderation.

### Typical Features
- Public browsing by category/location.
- Listing details with media.
- Contact/enquiry flow.
- CMS-like sections for banners, pages, testimonials, FAQs.
- Admin authentication and content/listing CRUD.

### User Flow
1. Visitor lands on Home.
2. Searches/browses categories and filters.
3. Opens listing detail page.
4. Sends enquiry/contact.
5. Admin reviews submissions, updates content, tracks dashboard metrics.

### Key Modules
- Identity & access control.
- Listings/catalog management.
- Media management.
- Contact/enquiry management.
- CMS blocks/pages.
- Analytics dashboard.

---

## 2) System Architecture

### High-Level
- **Frontend**: Angular 19+ (Storefront + Admin apps, shared UI library).
- **Backend**: ASP.NET Core Web API (.NET 9 style), JWT auth, role-based authorization.
- **Data**: SQL Server + EF Core Code-First migrations.
- **Infra**: Redis (optional cache), Blob storage (S3/Azure/MinIO), Serilog, OpenTelemetry.

### Layered Architecture
- **API Layer**: Controllers + filters + versioning.
- **Application Layer**: DTOs, services, validators, business rules.
- **Domain Layer**: Entities, enums, value objects.
- **Infrastructure Layer**: EF Core context, repositories, UoW, JWT, storage providers.

### Request Flow
`Controller -> Application Service -> Repository/UoW -> DbContext -> SQL Server`

### Clean Code Principles Applied
- DTO boundary between API and domain.
- SOLID interfaces in services/repositories.
- Centralized exception middleware.
- FluentValidation for request validation.
- Paging/sorting abstractions for list endpoints.

---

## 3) Database Schema (Core)

See `database/schema.sql` for full DDL. Core entities:
- `Users`, `Roles`, `UserRoles`
- `Categories`
- `Listings`
- `ListingImages`
- `Pages` (CMS)
- `SiteSettings`
- `ContactMessages`
- `AuditLogs`

All tables include audit fields:
- `CreatedDate`, `CreatedBy`, `ModifiedDate`, `ModifiedBy`, `IsDeleted`

---

## 4) Backend Code (ASP.NET Core)

Implemented starter production structure under `backend/src`:
- JWT auth endpoints (`/api/auth/login`, `/api/auth/refresh`).
- Listing CRUD endpoints (`/api/listings`).
- Role policies (`Admin`, `Manager`, `Editor`).
- Repository + Unit of Work pattern.
- Global exception middleware and consistent API envelope.

Key files:
- `OnlineBazar.Api/Program.cs`
- `OnlineBazar.Api/Controllers/AuthController.cs`
- `OnlineBazar.Api/Controllers/ListingsController.cs`
- `OnlineBazar.Application/Services/*`
- `OnlineBazar.Infrastructure/Data/AppDbContext.cs`

---

## 5) Frontend Code (Angular)

Two Angular applications are outlined:
- `frontend/apps/storefront`: customer site.
- `frontend/apps/admin`: admin dashboard.

Frontend patterns:
- Lazy-loaded feature modules.
- Core module for interceptors/guards.
- Reusable shared components.
- Route-level SEO metadata.
- API services for listings/content/auth.

---

## 6) Admin Panel Structure

Features:
- JWT login + refresh token.
- Role-based route guards.
- Dashboard KPIs (total listings, pending listings, enquiries, active users).
- CRUD screens:
  - Listings
  - Categories
  - CMS pages
  - Banners/site settings
  - Contact messages
  - Users/roles
- Media upload manager with validation and optimization.

---

## 7) Deployment Steps

### Backend Deployment
1. Build container image for API.
2. Apply EF migrations at startup/job.
3. Configure secrets via env vars (JWT key, DB connection, storage key).
4. Run behind reverse proxy (Nginx/YARP) with HTTPS.

### Frontend Deployment
1. Build Angular storefront and admin separately.
2. Serve via CDN + static host.
3. Configure environment API base URLs.
4. Enable compression + cache headers.

### SQL Server
1. Create production database.
2. Restrict DB user permissions.
3. Enable backup policy and PITR.
4. Add index maintenance and query-store monitoring.

### Observability & Security
- Serilog sink to Seq/ELK.
- OpenTelemetry traces + metrics.
- Rate limiting + CORS policy.
- CSP, XSS, CSRF protections (for cookie flows if used).
- Automated vulnerability scanning in CI.
