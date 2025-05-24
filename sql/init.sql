USE [master];
GO

CREATE LOGIN mu_user WITH PASSWORD = 'inseejam@123';
GO

CREATE DATABASE Scheduler_db;
GO

USE Scheduler_db;
GO

CREATE USER mu_user FOR LOGIN mu_user;
EXEC sp_addrolemember 'db_owner', 'mu_user';
GO
