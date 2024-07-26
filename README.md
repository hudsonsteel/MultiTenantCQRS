# Multi-Tenant CQRS with RabbitMQ and SQL Server

## Overview

This project demonstrates a multi-tenant architecture using CQRS (Command Query Responsibility Segregation) pattern, RabbitMQ for messaging, and SQL Server for data storage. It is designed to handle various operations for a system with multiple tenants, using RabbitMQ to handle events and commands across different services.

![image](https://github.com/user-attachments/assets/ce048045-1800-4205-874d-c338f1108488)

![image](https://github.com/user-attachments/assets/bc2dcaad-09b8-4f97-85a2-d6bfc8ae246b)

## Features

- **Multi-Tenant Architecture**: Supports multiple tenants with separate schemas for read and write operations.
- **CQRS Pattern**: Separates command and query responsibilities to enhance scalability and maintainability.
- **Event-Driven**: Utilizes RabbitMQ to manage events and commands between different components.
- **CRUD Operations**: Basic operations for managing customer orders and related entities.
- **Docker Support**: Docker configuration to containerize and deploy the application.

## Architecture

- **Services**: 
  - **Write API**: Handles commands to create, update, and delete orders.
  - **Read API**: Provides query endpoints for retrieving order information.
  
- **Messaging**: 
  - **RabbitMQ**: Manages communication between services using events and commands.

- **Database**: 
  - **SQL Server**: Stores tenant-specific data in separate schemas.

## Getting Started

### Prerequisites

- [.NET 8 or later](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/get-started)
- [RabbitMQ](https://www.rabbitmq.com/download.html)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

### Setup

1. **Clone the Repository**

   ```bash
   git clone https://github.com/hudsonsteel/multi-tenant-cqrs.git
   cd multi-tenant-cqrs

2. **Configure Docker**

- Ensure Docker is running.
- Update docker-compose.yml with your configuration details for SQL Server and RabbitMQ.

3. **Build and Run**

bash
Copy code
>> docker-compose down --volumes
>> docker-compose build --no-cache
>> docker-compose up

4. **Run Migrations**

- **Database Initialization:** The application is designed to automatically handle database schema creation during startup. When the application initializes, it checks for the existence of the necessary schemas and tables. If they do not exist, the application will create them based on the provided scripts and configurations.
  
- **Automatic Schema Creation:** Ensure that the application has the necessary permissions to create schemas and tables in your SQL Server instance. This process is managed automatically by the application to simplify deployment and ensure that the database is always in the correct state.

5. **Access the APIs**
- **Write API:** http://localhost:5000/api
- ![image](https://github.com/user-attachments/assets/25cfe08e-493f-410f-bd6e-f79bd682c2e4)
- **Read API:** http://localhost:5001/api
- ![image](https://github.com/user-attachments/assets/8f4a0a4f-9fd3-48ad-8d2f-f8a064281aaf)

### REST API Multi Tenant.postman_collection.json
- The REST API Multi Tenant.postman_collection.json file contains a collection of all API endpoints for the Multi-Tenant CQRS project. It includes endpoint details, request and response examples, and test cases.

### Location
Find this file in the root folder of the project or under 0 - Solution Items.

### Usage
- Import into Postman: Import the JSON file to visualize, test, and interact with the API endpoints.
- **Documentation:** Provides structured documentation and examples for each endpoint.

### How to Import
1. Open Postman.
2. Click `Import` > `File`.
3. Select the `REST API Multi Tenant.postman_collection.json` file.
4. Click `Import`.
   
