# 🛍️ Bustan Mall — E-Commerce Platform

A backend-focused **E-Commerce Web API** for **Bustan Mall**, designed to connect customers with stores inside the mall. Customers can browse products, manage carts and orders, while store owners can apply to create stores and manage their products after approval.

## 📌 Project Overview

Bustan Mall provides a multi-store shopping experience where:

- Customers can register and authenticate securely.
- Customers can browse products and categories.
- Customers can manage addresses, shopping carts, orders, and reviews.
- Store owners can submit applications to create a store.
- Approved stores can manage their products through the platform.
- The API exposes organized business functionality through RESTful endpoints.

## ✨ Key Features

### 👤 Authentication & User Management
- User registration and login.
- ASP.NET Core Identity integration.
- JWT Bearer authentication.
- Role-based authorization.
- Password management and token-based account operations.

### 🏪 Store Management
- Store application workflow.
- Store owner management.
- Product management for stores.
- Shop-product relationships.

### 🛒 Shopping Experience
- Product browsing.
- Category management.
- Shopping cart operations.
- Order creation and management.
- Address management.
- Product reviews.

### 📦 Product Management
- Create and update products.
- Product categorization.
- Product images and related content.
- Store-specific product management.

### 📧 Supporting Services
- Email service integration.
- Configurable mail settings.
- HTTP context access for authenticated request handling.

## 🏗️ Architecture

The project follows a service/repository-oriented backend structure:

```text
E-come/
├── Controllers/
│   ├── AccountController.cs
│   ├── AddressController.cs
│   ├── ApplyShopController.cs
│   ├── CactegoryController.cs
│   ├── CartController.cs
│   ├── OrderController.cs
│   ├── ProductController.cs
│   ├── ReviewController.cs
│   └── ShopProductsController.cs
│
├── DTO/
│   ├── AccountDTO/
│   ├── AddressDTO/
│   ├── ApplyShopDTO/
│   ├── CartDTO/
│   ├── CategoryDTO/
│   └── ...
│
├── Model/
├── services/
│   ├── Business/
│   └── IRepository/
│
├── Program.cs
└── appsettings.json
```

## 🔄 Application Flow

```text
Client
  ↓
REST API
  ↓
Controller
  ↓
Service / Repository
  ↓
Entity Framework Core
  ↓
SQL Server
```

### Authentication Flow

```text
Login / Registration
        ↓
ASP.NET Core Identity
        ↓
JWT Token
        ↓
Authorization Header
        ↓
JWT Validation
        ↓
Protected API Endpoints
```

## 🧰 Tech Stack

| Technology | Usage |
|---|---|
| **C#** | Backend development |
| **.NET 7** | Application framework |
| **ASP.NET Core Web API** | RESTful API |
| **Entity Framework Core** | Data access / ORM |
| **SQL Server** | Relational database |
| **ASP.NET Core Identity** | User and role management |
| **JWT Bearer** | Authentication & authorization |
| **Swagger / OpenAPI** | API documentation and testing |
| **Repository / Service Pattern** | Business and data access organization |
| **DTOs** | API data contracts |
| **Dependency Injection** | Service composition |
| **Newtonsoft.Json** | JSON serialization configuration |

The project targets **.NET 7** and uses SQL Server through Entity Framework Core. fileciteturn93file0

## 🔐 Security

The API implements:

- JWT Bearer authentication.
- Issuer and audience validation.
- Signing-key validation.
- Token lifetime validation.
- ASP.NET Core Identity for users and roles.
- Protected API endpoints through authentication and authorization middleware.
- Swagger Bearer authentication support for API testing.

JWT configuration and authentication middleware are registered in `Program.cs`. fileciteturn95file0

> **Security Note:** Environment-specific secrets, JWT signing keys, database connection strings, and SMTP credentials should be stored using User Secrets, environment variables, or another secure configuration provider rather than committed to source control.

## 📚 API Areas

The API is organized around the following controllers:

- **Account** — authentication and user operations
- **Address** — customer addresses
- **Apply Shop** — store-owner applications
- **Category** — product categories
- **Cart** — shopping cart operations
- **Order** — order management
- **Product** — product management
- **Review** — product reviews
- **Shop Products** — products associated with stores

These areas are implemented through controllers and registered application services/repositories. fileciteturn91file0 fileciteturn95file0

## 🚀 Getting Started

### Prerequisites

- .NET 7 SDK
- SQL Server / SQL Server Express
- Visual Studio 2022 or another compatible IDE

### 1. Clone the repository

```bash
git clone https://github.com/Abdallah72296/E-Commerce.git
cd E-Commerce
```

### 2. Configure the database

Update the `DefaultConnection` in `appsettings.json` or, preferably, configure it through a secure configuration mechanism.

### 3. Configure JWT

Configure the JWT issuer, audience, signing key, and token duration using secure configuration.

### 4. Configure email settings

If email functionality is enabled, configure the SMTP settings using environment variables or User Secrets.

### 5. Run the application

```bash
dotnet restore
dotnet run --project E-come
```

### 6. Open Swagger

Once the application is running, open the Swagger UI from the application's configured URL to explore and test the API.

## 💡 Engineering Highlights

- Built a multi-store E-Commerce backend around real business entities and workflows.
- Implemented authentication and authorization using ASP.NET Core Identity and JWT.
- Organized business functionality using services and repository abstractions.
- Used DTOs to separate API contracts from domain/data models.
- Implemented shopping-cart and order-management functionality.
- Added store-application and store-product management workflows.
- Integrated Swagger/OpenAPI for API discovery and testing.

## 👨‍💻 Developer

**Abdallah Abdeltawab** — Backend .NET Developer

- GitHub: [Abdallah72296](https://github.com/Abdallah72296)
- LinkedIn: [Abdallah Abdeltawab](https://www.linkedin.com/in/abdallah-abdeltawab-54b58b226)

---

⭐ If you find this project useful, feel free to explore the repository and its implementation details.
