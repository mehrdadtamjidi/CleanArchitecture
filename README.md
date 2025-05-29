# 🧱 Clean Architecture Template

This repository provides a modular, scalable, and maintainable project structure based on **Clean Architecture** principles using **ASP.NET Core**.

It aims to help developers build enterprise-grade applications with clear separation of concerns, testability, and maintainability.

---

## 📁 Project Structure

```
CleanArchitecture/
├── CleanArchitecture.Api           --> API Layer (Presentation)
├── CleanArchitecture.Application   --> Application Layer (Business Logic)
├── CleanArchitecture.Domain        --> Domain Layer (Entities & Interfaces)
├── CleanArchitecture.Infrastructure --> Infrastructure Layer (External Services)
├── CleanArchitecture.Persistence   --> Persistence Layer (EF Core, Repositories)
├── CleanArchitecture.sln           --> Solution File
```

---

## 🚀 Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

### Setup Instructions

1. **Clone the repository**

   ```bash
   git clone https://github.com/mehrdadtamjidi/CleanArchitecture.git
   cd CleanArchitecture
   ```

2. **Apply migrations & update the database**

   ```bash
   cd CleanArchitecture.Persistence
   dotnet ef database update
   ```

3. **Run the application**

   ```bash
   cd ../CleanArchitecture.Api
   dotnet run
   ```

   The API should be available at: `https://localhost:5001`

---

## 🧪 Testing

Run unit tests using the following command:

```bash
dotnet test
```

---

## 📌 Key Features

- ✅ Clean Architecture layered structure
- 🧠 CQRS pattern using MediatR
- 🧾 Entity Framework Core with code-first migrations
- 🔐 JWT Authentication (optional)
- ⚖️ FluentValidation for input validation
- 🔄 AutoMapper for mapping between DTOs and entities
- 📦 Modular and testable design
- 🐳 Docker-ready (optional support)

---

## 📸 Screenshots / Architecture Diagram

*(Coming soon – add diagrams or UI screenshots here if available)*

---

## 🤝 Contributing

Feel free to fork this repo, contribute, and create pull requests.  
Suggestions, issues, or improvements are always welcome.

---

## 📄 License

This project is licensed under the MIT License.  
See the [LICENSE](LICENSE) file for details.

---

## 🙋 About the Author

Developed and maintained by [Mehrdad Tamjidi](https://github.com/mehrdadtamjidi).  
If you like the project, feel free to ⭐ the repository and follow for updates.
