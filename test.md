# AI Agent Project Flow

## Purpose

Use this as the default workflow for AI agents working in this repository.
Follow these rules unless the user explicitly asks for a different approach.

---

## Main Projects (Use Only These 3)

1. [YourProjectName].Api
2. [YourProjectName].Domain
3. [YourProjectName].Database
4. [YourProjectName].MVC (use Api from [YourProjectName].Api )

---

# EF Core Rule

1. EF Core in this repository is database-first.
2. Do not create EF Core migrations.
3. Do not design schema from C# models.
4. Database schema must be changed in SQL Server first.
5. After schema changes, re-scaffold EF Core models.

---

# Database Change Flow

1. Update database schema in SQL Server

   * tables
   * columns
   * constraints
   * indexes

2. Store SQL scripts in source control.

3. Re-scaffold EF Core models into
   [YourProjectName].Database.

Example scaffold command

dotnet ef dbcontext scaffold "Server=.;Database=YourDatabaseName;User ID=your_user;Password=your_password;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer --project [YourProjectName].Database/[YourProjectName].Database.csproj --output-dir AppDbContextModels --context-dir AppDbContextModels --context AppDbContext --force

4. Rebuild solution and fix compile errors.

---

# Feature Implementation Flow

1. Locate or create feature folder

Features/<FeatureName>

2. Create request and response models.

3. Implement feature logic.

4. Use AppDbContext directly when accessing the database.

5. Add or update API endpoint.

---

# Request / Response Model Rules

Request and response models are mandatory.

### One Method Rule

One API method must have:

* 1 Request Model
* 1 Response Model

Example

CreateMerchantRequest
CreateMerchantResponse
EditMerchantRequest
EditMerchantResponse

### HTTP Layer Parameters

Parameters must follow HTTP conventions.

Examples

GET

GET /api/merchant/{id}

Controller

public async Task<IActionResult> GetMerchant(int id)

POST

POST /api/merchant

Controller

public async Task<IActionResult> CreateMerchant(CreateMerchantRequest request)

---

### Internal Method Call Rule

When calling internal logic:

* Pass Request Model
* Return Response Model

Example

var response = await MerchantFeature.CreateAsync(request);

Method structure

public async Task<CreateMerchantResponse> CreateAsync(CreateMerchantRequest request)

---

# Coding Conventions

1. Follow existing feature folder structure.
2. Prefer async EF methods

   * ToListAsync
   * FirstOrDefaultAsync
   * SaveChangesAsync
3. Follow existing soft delete style (IsDelete == false) where applicable.
4. Keep controller code simple and readable.

---

# AppDbContext

* AppDbContext is scaffolded from the database.
* Do not convert to code-first.
* Do not remove scaffolded mapping behavior.

---

# Done Checklist

1. Solution builds successfully.
2. API routes compile and run.
3. Database-first workflow was followed.
4. Changed files and behavior are summarized clearly.