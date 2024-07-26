print 'Inicio init.sql'
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'MultiTenantCQRS')
BEGIN
    CREATE DATABASE MultiTenantCQRS;
END
GO

USE MultiTenantCQRS;
GO

IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'IndySolftUser')
BEGIN
    CREATE LOGIN IndySolftUser WITH PASSWORD = 'IndySolft&Password1';
    CREATE USER IndySolftUser FOR LOGIN IndySolftUser;
    ALTER ROLE db_owner ADD MEMBER IndySolftUser;
END
