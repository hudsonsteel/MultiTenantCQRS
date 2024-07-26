# Use a imagem base do .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Defina o diretório de trabalho
WORKDIR /app

# Copie os arquivos do projeto
COPY *.sln ./
COPY MultiTenantCQRS.Contract.Event/*.csproj MultiTenantCQRS.Contract.Event/

COPY MultiTenantCQRS.WriteApi/*.csproj MultiTenantCQRS.WriteApi/
COPY MultiTenantCQRS.Write.Application/*.csproj MultiTenantCQRS.Write.Application/
COPY MultiTenantCQRS.Write.Domain/*.csproj MultiTenantCQRS.Write.Domain/
COPY MultiTenantCQRS.Write.SqlServer/*.csproj MultiTenantCQRS.Write.SqlServer/
COPY MultiTenantCQRS.Write.RabbitMq/*.csproj MultiTenantCQRS.Write.RabbitMq/

COPY MultiTenantCQRS.ReadApi/*.csproj MultiTenantCQRS.ReadApi/
COPY MultiTenantCQRS.Read.Application/*.csproj MultiTenantCQRS.Read.Application/
COPY MultiTenantCQRS.Read.Domain/*.csproj MultiTenantCQRS.Read.Domain/
COPY MultiTenantCQRS.Read.SqlServer/*.csproj MultiTenantCQRS.Read.SqlServer/
COPY MultiTenantCQRS.Read.RabbitMq/*.csproj MultiTenantCQRS.Read.RabbitMq/

# Restore dependencies
RUN dotnet restore

# Copie o restante dos arquivos e construa o projeto
COPY . ./
RUN dotnet publish -c Release -o /app/out

# Defina o diretório de trabalho e o ponto de entrada
WORKDIR /app/out
ENTRYPOINT ["dotnet"]
