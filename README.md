# OnlineBazar Rebuild

Production-oriented blueprint and starter implementation for rebuilding SmilyBazar with:
- ASP.NET Core Web API
- Angular (Storefront + Admin)
- SQL Server
- Entity Framework Core (Code-First)
- JWT Authentication + Role-based authorization

## Repository Layout
- `docs/SmilyBazar-Rebuild-Blueprint.md` - full analysis + architecture + deployment plan.
- `database/schema.sql` - SQL schema with relationships and audit fields.
- `backend/src` - clean layered ASP.NET Core starter.
- `frontend/apps/storefront` - customer-facing Angular shell.
- `frontend/apps/admin` - admin panel Angular shell.

## Next Implementation Steps
1. Create .sln and .csproj files and wire dependency injection by projects.
2. Add EF Core migrations and seed roles/users.
3. Add Angular workspace (`angular.json`) and build pipelines.
4. Integrate object storage for images and CDN transforms.
5. Add automated tests and CI/CD workflows.
