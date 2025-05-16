USE MASTER;
GO

-- Si la base de datos existe, ponerla en modo SINGLE_USER y eliminarla
IF DB_ID('DBPracticaNormalizacion') IS NOT NULL
BEGIN
    ALTER DATABASE DBPracticaNormalizacion SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE DBPracticaNormalizacion;
END
GO

CREATE DATABASE DBPracticaNormalizacion;
GO

USE DBPracticaNormalizacion;
GO