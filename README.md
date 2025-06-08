# EventServiceProvider

**EventServiceProvider** is a backend microservice responsible for managing events in the Ventixe event system. It is part of a distributed architecture built using ASP.NET Core and integrated with Azure services for security and scalability.

## Overview

This service provides a REST API for managing event-related data. It handles the creation, retrieval, updating, and deletion of events and is secured using JWT authentication. All sensitive configuration values are stored in Azure Key Vault.

## Features

- Create, update, and delete events (authorized users only)
- Fetch all events or specific events by ID
- Fetch events created by a specific user
- Full JWT validation including issuer, audience, and signing key
- Integrated with Azure Key Vault to securely manage secrets and credentials

## Technology Stack

- ASP.NET Core 9
- Entity Framework Core
- SQL Server (Azure)
- Azure App Service
- Azure Key Vault
- Swagger/OpenAPI

## Azure Key Vault Integration

Secrets are not stored in plain text within the project or configuration files. Instead, the application retrieves secrets at runtime using Azure Key Vault and a system-assigned managed identity.

### Secrets used:

| Key                                 | Purpose                          |
|-------------------------------------|----------------------------------|
| `ConnectionStrings--SqlConnection`  | SQL Server connection string     |
| `Jwt--Issuer`                       | Expected token issuer            |
| `Jwt--Audience`                     | Expected token audience          |
| `Jwt--Secret`                       | Symmetric key for signing tokens |

These secrets are accessed via `DefaultAzureCredential` from the `Azure.Identity` package.

## Security

- All endpoints that modify event data require a valid JWT token.
- Authorization is based on user ID claims extracted from the token.
- Tokens are issued by a separate authentication service (AuthServiceProvider).
