-- init.sql (Updated for new SA password)
-- Create the database if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'Scheduler_db')
BEGIN
    CREATE DATABASE Scheduler_db;
END
GO

USE Scheduler_db;
GO

-- Create the login for mu_user if it doesn't exist at the server level
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = 'mu_user')
BEGIN
    CREATE LOGIN mu_user WITH PASSWORD = 'inseejam@123';
END
GO

-- Create the database user from the login, within Scheduler_db
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'mu_user')
BEGIN
    CREATE USER mu_user FOR LOGIN mu_user;
END
GO

-- Grant necessary permissions to mu_user on Scheduler_db
ALTER ROLE db_datareader ADD MEMBER mu_user;
ALTER ROLE db_datawriter ADD MEMBER mu_user;
ALTER ROLE db_ddladmin ADD MEMBER mu_user;

PRINT 'Database initialization completed successfully';
GO