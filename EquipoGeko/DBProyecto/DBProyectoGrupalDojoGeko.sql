use master;
go

-- Si la base de datos existe, ponerla en modo SINGLE_USER y eliminarla
if DB_ID('DBProyectoGrupalDojoGeko') is not null
begin
    alter database DBProyectoGrupalDojoGeko set single_user with rollback immediate;
    drop database DBProyectoGrupalDojoGeko;
end
go

CREATE DATABASE DBProyectoGrupalDojoGeko;
GO

USE DBProyectoGrupalDojoGeko;
GO