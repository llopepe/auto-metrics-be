# 🚗📊 AutoMetrics – Documentación Oficial  
**Microservicio para gestión de métricas automotrices – Clean Architecture, .NET 8, CQRS, EF Core y más**

---

## 🏷️ Badges

![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat&logo=dotnet&logoColor=white)  
![Clean Architecture](https://img.shields.io/badge/Clean%20Architecture-✔-2ea44f?style=flat)  
![CQRS](https://img.shields.io/badge/CQRS-Pattern-blue)  
![MediatR](https://img.shields.io/badge/MediatR-Enabled-orange)  
![EF Core](https://img.shields.io/badge/Entity%20Framework-Core-512BD4)  
![Mapster](https://img.shields.io/badge/Mapster-Fast%20Mapper-yellow)  
![Serilog](https://img.shields.io/badge/Serilog-Logging-critical)  
![Swagger](https://img.shields.io/badge/Swagger-API%20Docs-brightgreen)  
![Health Checks](https://img.shields.io/badge/Health%20Checks-Enabled-success)  

---

# 📁 Estructura del Proyecto

AutoMetrics está dividido en **Framework Core** y **Microservicio AutoMetricsService**.

---

# 🧱 Arquitectura del Framework

El framework sigue un diseño modular, desacoplado y alineado con **Clean Architecture**, pensado para proyectos empresariales y microservicios.

---

## 📦 1. Core.Framework.Application

### **Common (Lógica Transversal)**  
Incluye componentes reutilizables:

- **Behaviours (MediatR)**
  - Validación automática  
  - Medición de performance  

- **Enums**  
  Códigos estandarizados de error  

- **Exceptions**  
  Excepciones personalizadas mapeadas a HTTP  

- **Extensions**  
  Utilidades como información de build  

- **Middleware**  
  Manejo global de errores  

- **Models**  
  DTOs simples  

- **Security**  
  Atributos de autorización personalizados  

- **Wrappers**  
  Formatos estándar de respuesta (resultado, error, paginación)

---

### **Interfaces (Acceso a Datos)**  
Define contratos desacoplados de infraestructura:

- `IUnitOfWork`
- Repositorios genéricos (CRUD común)

📌 **Resumen:**  
- *Common* → lógica transversal  
- *Interfaces* → contratos de datos

---

## 📦 2. Core.Framework.Domain

### **Common**
- `BaseEntity`
- `BaseEvent`

### **Interfaces**
- `ISoftDeletable` (baja lógica)

---

## 📦 3. Core.Framework.Infrastructure

### **Common**
- Excepciones personalizadas

### **Data**
- `AppDbContext` (EF Core)
- `UnitOfWork` (transacciones atómicas)

### **Repositories**
- `BaseRepository` (CRUD genérico)

---

# 🏗️ Arquitectura del Microservicio AutoMetricsService

El microservicio utiliza **Clean Architecture** con las capas:

API → Application → Domain
↑ 			↓
└─── Infrastructure

---

## 🔹 1. Domain Layer  
📁 Proyecto: **AutoMetricsService.Domain**

Incluye **solo lógica de negocio**:

### **Entities/**
- Car  
- CarTax  
- Center  
- Sale  
- AuditLog  

### **EntitiesCustom/**
Clases de agregación/calculo:

- SaleAmountResultCustom  
- SalesVolumeCenterCustom  
- PercentageGlobalCustom  

### **Events/**
- `SaleCreatedEvent`

---

## 🔹 2. Application Layer  
📁 Proyecto: **AutoMetricsService.Application**

Responsable de **casos de uso (CQRS)**:

### **Common/**
- Extensiones  
- Configuración Mapster  
- Swagger  
- Paginación  

### **Interfaces/Repositories/**
Contratos:

- ICarRepository  
- ICarTaxRepository  
- ICenterRepository  
- ISaleRepository  

### **Sales/**
Commands + Queries:

#### **CreateSale/**
- `CreateSaleCommand`  
- `CreateSaleCommandValidator`

#### **Dto/**
- SaleDto  
- PercentageGlobalDto  
- SalesVolumeCenterDto  
- TotalSalesVolumeDto  

#### **Queries/**
- GetPercentageGlobalWithPagination  
- GetSalesByCenterWithPagination  
- GetSaleWithPagination  
- GetTotalSalesVolume  

#### **EventHandlers/**
- `SaleCreatedEventHandler`

---

## 🔹 3. Infrastructure Layer  
📁 Proyecto: **AutoMetricsService.Infrastructure**

Implementación real:

### **Data/Configurations/**
Fluent API para EF Core:

- CarConfiguration  
- CarTaxConfiguration  
- CenterConfiguration  
- SaleConfiguration  

### **DbContext**
- `ApplicationDbContext.cs`

### **Repositories**
Implementación concreta de interfaces:

- CarRepository  
- CarTaxRepository  
- CenterRepository  
- SaleRepository  

### **DbInitializer.cs**
Carga inicial de datos.

---

## 🔹 4. API Layer  
📁 Proyecto: **AutoMetricsService.Api**

Endpoints REST

### **SalesController**
- Crear ventas  
- Listar (paginado)  
- Consultas agregadas  
- Totales por centro  
- Totales globales  

Otros archivos relevantes:

- `Program.cs` (Swagger, Middlewares, Hosting)  
- `DependencyInjection.cs`  
- `appsettings.json`

---

# 🌐 Flujo General (Clean Architecture)

┌─────────────┐   ┌─────────────────┐ 	┌────────────┐
│ API 		  │ → │ Application 	│ → │ Domain 	 │
└──────▲──────┘   └───────▲─────────┘   └─────▲──────┘
	   │ 	   			  │ 				  │
└──────────── Infrastructure ───────────────────┘

---

# 🚀 Beneficios logrados con la arquitectura

- ✔ Separación estricta de responsabilidades  
- ✔ Fácil testing  
- ✔ Escalabilidad modular  
- ✔ Dominio puro con eventos  
- ✔ Limpieza y mantenibilidad  

---

# 🧩 Patrones utilizados

- Clean Architecture  
- CQRS  
- Repository Pattern  
- Unit of Work  
- Dependency Injection  
- Decorator Pattern (Pipeline Behaviours)  
- DTO + Mapster  
- Health Checks  

---

# 🛠️ Decisiones Técnicas (con justificación)

### 1. **.NET 8**
- LTS  
- Mejor rendimiento  
- Menor memoria  
- Ideal para microservicios  

### 2. **Clean Architecture**
- Aislamiento de capas  
- Escalabilidad  
- Facilidad de testing  

### 3. **MediatR + CQRS**
- Desacoplamiento total  
- Commands y queries claros  
- Behaviours reutilizables  

### 4. **Pipeline Behaviours**
Incluye:

- LoggingBehaviour  
- ValidationBehaviour  
- PerformanceBehaviour  

Ventajas:  
✔ Cross-cutting centralizado  
✔ Trazabilidad unificada  

### 5. **FluentValidation**
- Reemplaza DataAnnotations  
- Escalable  
- Limpio  

### 6. **EF Core + Base de datos en memoria**
EF Core: ORM moderno  
InMemory DB:  
- Rápida para desarrollo  
- Ideal para automatización  
- Sin necesidad de SQL Server inicial  

### 7. **Mapster**
Más rápido que AutoMapper  
Sin reflection runtime  
Perfecto para microservicios  

### 8. **Swagger**
- Testing interactivo  
- Documentación viva  

### 9. **Serilog**
- Logs estructurados  
- Soporte multi-sink  

### 10. **Health Checks**
Ideal para:  
Kubernetes, Docker, Balanceadores.

### 11. **Microservicios**
- Escalado independiente  
- Resiliencia  
- Mantenimiento modular  

---

# ▶️ Cómo ejecutar y probar AutoMetrics

El microservicio incluye **datos precargados** gracias al archivo:

📁 *AutoMetricsService.Infrastructure/Data/DbInitializer.cs*

---

## 🔧 Requisitos

- .NET 8 SDK

---

## 🟦 1. Restaurar dependencias

dotnet restore

## 🟦 2. Ejecutar la API

dotnet run --project AutoMetricsService.Api

Verás en consola:
Checking database status…
Database created (EnsureCreated).
Seeding database...
Centers seeded.
Cars seeded.
CarTaxes seeded.
Sales seeded.
Database ready.

## 🟦 3. Probar la API

Abrí:

👉 https://localhost:5001/swagger

Ya tendrás datos iniciales listos para probar sin cargar manualmente.

# 🧪 Testing automatizado

dotnet test

Los tests usan base InMemory, totalmente aislada