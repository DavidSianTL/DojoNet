use master;
go

alter database DBPracticaFunciones
set single_user
with rollback immediate;
go

drop database if exists DBPracticaFunciones;
go

create database DBPracticaFunciones;
go

use DBPracticaFunciones;
go