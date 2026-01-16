"# Development_Sellora"

---

```md
# Development_Sellora

Sellora adalah aplikasi backend berbasis **ASP.NET Core** yang dirancang menggunakan **Clean Architecture** untuk mengelola domain e-commerce seperti Product, Category, dan fitur pendukung lainnya.

Project ini dikembangkan dengan fokus pada:
- Struktur kode yang clean & scalable
- Pemisahan concern (Domain, Application, Infrastructure, API)
- Mudah dikembangkan dan di-maintain

---

## 🧱 Architecture

Project ini menggunakan **Clean Architecture** dengan pembagian layer sebagai berikut:

```

Development_Sellora
│
├── Domain
│   ├── Entities
│   └── Interfaces
│
├── Application
│   ├── DTOs
│   ├── Services
│   ├── Common
│   │   └── Mappings (AutoMapper)
│
├── Infrastructure
│   ├── Data
│   └── Repositories
│
└── Api
└── Controllers

````

### Penjelasan singkat:
- **Domain**  
  Berisi entity inti dan interface repository (tanpa dependency ke layer lain)
- **Application**  
  Berisi business logic, service, DTO, dan mapping
- **Infrastructure**  
  Implementasi repository, database (EF Core)
- **Api**  
  Endpoint HTTP (REST API)

---

## 🚀 Tech Stack

- **.NET 8 / ASP.NET Core**
- **Entity Framework Core**
- **AutoMapper**
- **SQL Server / PostgreSQL** (opsional)
- **Swagger / Scalar**
- **Dependency Injection**

---

## ⚙️ Setup & Running Project

### 1️⃣ Clone repository
```bash
git clone https://github.com/your-username/Development_Sellora.git
cd Development_Sellora
````

### 2️⃣ Restore dependencies

```bash
dotnet restore
```

### 3️⃣ Update connection string

Edit file:

```bash
Api/appsettings.json
```

Contoh:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=SelloraDb;Trusted_Connection=True;"
}
```

### 4️⃣ Jalankan migration

```bash
dotnet ef database update \
  --project Infrastructure/Infrastructure.csproj \
  --startup-project Api/Api.csproj
```

### 5️⃣ Run API

```bash
dotnet run --project Api
```

---

## 📘 API Documentation

Setelah aplikasi berjalan, buka:

* **Swagger / Scalar**

```
http://localhost:{PORT}/swagger
```

atau

```
http://localhost:{PORT}/scalar/v1
```

---

## 📂 Contoh Endpoint

### Update Product

```http
PUT /api/products/{id}
```

Request body:

```json
{
  "name": "Product Name",
  "description": "Description",
  "price": 10000,
  "stock": 10,
  "categoryId": "guid",
  "isActive": true
}
```

Response:

* `204 No Content` → berhasil update
* `404 Not Found` → product tidak ditemukan

---

## 🛠️ Best Practices

* Gunakan **migration incremental**, jangan edit DB manual
* Gunakan `string?` untuk field opsional
* Gunakan `string = null!` untuk field wajib
* Jangan langsung `_context.Update(entity)` tanpa validasi

---

## 🧑‍💻 Author

**Aditya Pratama Febriono**
Backend Developer – Sellora

---

## 📄 License

This project is licensed under the MIT License.

```