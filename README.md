# 🛡️ FraudGuard API - SDET & QA Automation Portfolio

![.NET](https://img.shields.io/badge/.NET-10.0-purple?logo=dotnet)
![Docker](https://img.shields.io/badge/Docker-SQL%20Server-blue?logo=docker)
![xUnit](https://img.shields.io/badge/Testing-xUnit%20%7C%20Moq-green)
![Postman](https://img.shields.io/badge/E2E-Postman%20%7C%20Newman-orange?logo=postman)
![k6](https://img.shields.io/badge/Performance-k6-purple?logo=k6)
![CI/CD](https://img.shields.io/badge/CI%2FCD-GitHub%20Actions-black?logo=github-actions)

## 🎯 Sobre el Proyecto
FraudGuard es un motor de detección de fraude bancario construido bajo principios de **Clean Architecture** y **Patrón Strategy**. 

Este proyecto fue desarrollado íntegramente como una demostración de habilidades para el rol de **SDET (Software Development Engineer in Test)** y **QA Automation**, abarcando todo el ciclo de vida del software: desde el TDD en la capa de dominio, hasta las pruebas de estrés y la orquestación de CI/CD.

## 🚀 Arquitectura y Patrones
- **Clean Architecture:** Separación estricta de responsabilidades (Domain, Application, Infrastructure, API).
- **Patrón Strategy:** Motor de reglas de fraude escalable y dinámico acoplado por inyección de dependencias.
- **Resiliencia y Seguridad:** Middleware global de manejo de errores basado en el estándar moderno **RFC 9457**, y protección contra ataques de fuerza bruta (Rate Limiting de ventana fija).

## 🧪 Estrategia de Pruebas (La Pirámide de QA)

1. **Pruebas Unitarias (Caja Blanca):**
   - Frameworks: `xUnit` + `Moq`.
   - Cobertura del 100% en las reglas de negocio (Capa de Dominio).
   - Uso intensivo del ciclo TDD (Red-Green-Refactor).

2. **Pruebas de Integración (End-to-End en Memoria):**
   - Framework: `Microsoft.AspNetCore.Mvc.Testing` + `EF Core InMemory`.
   - Modificación del ciclo de vida de dependencias en .NET 10 para interceptar llamadas a SQL Server y asegurar persistencia en RAM sin abrir puertos de red.

3. **Pruebas Funcionales Automatizadas (Caja Negra):**
   - Herramientas: `Postman` + `Newman`.
   - Validaciones mediante aserciones en JavaScript (Chai/BBD) verificando códigos HTTP y payloads JSON.
   - Implementación de **Golden Data** mediante el endpoint de QA `/api/testing/seed` (impulsado por `Bogus`) para garantizar el determinismo de las pruebas.

4. **Pruebas de Rendimiento y Carga:**
   - Herramienta: `Grafana k6` (Scripting en JavaScript).
   - Validación empírica del Rate Limiting (Picos de 500+ Req/sec resultando en bloqueos exitosos HTTP 429).

## ⚙️ Integración Continua (CI/CD)
El repositorio cuenta con un pipeline automatizado en **GitHub Actions** que se dispara en cada `push`. El pipeline orquesta:
- Provisión de contenedores efímeros (Ubuntu + SQL Server 2022).
- Ejecución de pruebas unitarias/integración (C#).
- Migraciones automáticas (EF Core).
- Inyección de carga de datos maestros (Golden Data).
- Ejecución de colección automatizada Newman contra la API encendida en background.

## 💻 Ejecución Local

### Requisitos
- .NET 10 SDK
- Docker Desktop
- Node.js & Newman (Para pruebas E2E)
- k6 (Para pruebas de carga)

### Levantar el entorno
```bash
# 1. Levantar base de datos en Docker
./setup-env.sh

# 2. Ejecutar pruebas unitarias e integración en memoria
dotnet test

# 3. Encender la API
dotnet run --project FraudGuard.API

# 4. En otra terminal: Inyectar datos de prueba y ejecutar Newman
curl -X POST http://localhost:5000/api/testing/seed
newman run postman_collection.json
