# CleanArchitecture Project Context

## ساختار پروژه

| لایه | پروژه |
|---|---|
| Domain (Entities, Interfaces) | `CleanArchitecture.Domain` |
| Application (CQRS, DTOs, Validators) | `CleanArchitecture.Application` |
| Infrastructure (JWT, Email, Http) | `CleanArchitecture.Infrastructure` |
| Persistence (DbContext, Repositories) | `CleanArchitecture.Persistence` |
| API | `CleanArchitecture.Api` |
| Unit Tests | `CleanArchitecture.Application.UnitTests` |

- **Connection String name:** `ApplicationConnectionString`
- **Database:** `CleanArchitectureDb` روی SQL Server
- **ORM:** EF Core (فعلاً Code First)

---

## تصمیمات معماری

### قرار است به DB First تبدیل شود

دستور scaffold (از ریشه solution اجرا شود):

```powershell
dotnet ef dbcontext scaffold `
  "Data Source=.;Initial Catalog=CleanArchitectureDb;Integrated Security=True;TrustServerCertificate=True;" `
  Microsoft.EntityFrameworkCore.SqlServer `
  --project CleanArchitecture.Persistence `
  --startup-project CleanArchitecture.Api `
  --output-dir ..\CleanArchitecture.Domain\Entities `
  --context-dir . `
  --context ApplicationDbContext `
  --namespace CleanArchitecture.Domain.Entities `
  --context-namespace CleanArchitecture.Persistence `
  --force
```

**بعد از scaffold:**
- کدهای کاستوم `ApplicationDbContext` (شامل `_cleanString`، override های `SaveChanges`، تنظیمات `modelBuilder`) باید در فایل جداگانه `ApplicationDbContext.Custom.cs` به صورت `partial class` نگهداری شوند تا با scaffold بازنویسی نشوند
- `BaseDomainEntity` و `IEntity` بعد از DB First unused می‌شوند
- `RegisterEntityTypeConfiguration` را می‌توان با `modelBuilder.ApplyConfigurationsFromAssembly(assembly)` جایگزین کرد

---

## Visual Studio Template

پروژه به عنوان `dotnet new` template تنظیم شده:

```powershell
# نصب
dotnet new install .

# استفاده
dotnet new cleanarch -n MyNewProject
```

فایل template در `.template.config/template.json` قرار دارد.

**آپدیت template:**
```powershell
dotnet new uninstall d:\GitHub\CleanArchitecture\
dotnet new install d:\GitHub\CleanArchitecture\
```

---

## نکات Git

- در commit message ها **بدون** `Co-Authored-By` باشد
