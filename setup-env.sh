#!/bin/bash

echo "Iniciando el entorno de pruebas para FraudGuard..."
docker compose up -d

echo "Esperando a que SQL Server levante por completo (15 segundos)..."
sleep 15

echo "Aplicando migraciones a la base de datos (Entity Framework Core)..."
dotnet ef database update --project FraudGuard.Infrastructure --startup-project FraudGuard.API

echo "¡El entorno local de base de datos está listo y las tablas fueron creadas!"