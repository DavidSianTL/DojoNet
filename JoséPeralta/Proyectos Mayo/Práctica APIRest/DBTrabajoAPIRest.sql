use master;
go

-- Si la base de datos existe, ponerla en modo SINGLE_USER y eliminarla
if DB_ID('DBTrabajoAPIRest') is not null
begin
    alter database DBTrabajoAPIRest set single_user with rollback immediate;
    drop database DBTrabajoAPIRest;
end
go

create database DBTrabajoAPIRest;
go

use DBTrabajoAPIRest;
go

-- Tabla Usuarios
create table Usuarios(
	idUsuario int primary key identity(1,1),
	nombreCompleto varchar(50),
	usuario varchar(25),
	contrasenia nvarchar(max),
	token nvarchar(max)
);

-- Tabla Especialidades (debe crearse primero por las relaciones)
create table Especialidades (
    idEspecialidad int primary key identity(1,1),
    nombre varchar(100) not null
);
go

-- Tabla Pacientes
create table Pacientes (
    idPaciente int primary key identity(1,1),
    nombre varchar(100) not null,
    email varchar(100) not null unique,
    telefono varchar(20),
    fechaNacimiento date not null
);
go

-- Tabla Medicos
create table Medicos (
    idMedico int primary key identity(1,1),
    nombre varchar(100) not null,
    fk_IdEspecialidad int,
    email varchar(100) not null unique,
    constraint FK_Medico_Especialidad foreign key (fk_IdEspecialidad) 
        references Especialidades(idEspecialidad)
);
go

-- Tabla Citas
create table Citas (
    idCita int primary key identity(1,1),
    fk_IdPaciente int not null,
    fk_IdMedico int not null,
    fecha date not null,
    hora time not null,
    constraint FK_Cita_Paciente foreign key (fk_IdPaciente) 
        references Pacientes(idPaciente),
    constraint FK_Cita_Medico foreign key (fk_IdMedico) 
        references Medicos(idMedico)
);
go
-- Datos iniciales
insert into Usuarios (nombreCompleto, usuario, contrasenia) values
('José Peralta', 'Peghoste', 'holamundo1@');
go

insert into Especialidades (nombre) values 
('Cardiología'),
('Pediatría'),
('Dermatología'),
('Neurología');
go

insert into Medicos (nombre, fk_IdEspecialidad, email) values
('Dr. Juan Pérez', 1, 'jperez@clinica.com'),
('Dra. María Gómez', 2, 'mgomez@clinica.com'),
('Dr. Carlos López', 3, 'clopez@clinica.com');
go

insert into Pacientes (nombre, email, telefono, fechaNacimiento) values
('Ana Rodríguez', 'arodriguez@mail.com', '555-1234', '1985-07-15'),
('Luis Martínez', 'lmartinez@mail.com', '555-5678', '1990-11-22'),
('Sofía García', 'sgarcia@mail.com', '555-9012', '1978-03-30');
go

insert into Citas (fk_IdPaciente, fk_IdMedico, fecha, hora) values
(1, 1, '2023-11-15', '09:00:00'),
(2, 2, '2023-11-15', '10:30:00'),
(3, 3, '2023-11-16', '11:00:00');
go

select * from Usuarios;
select * from Especialidades;
select * from Citas;
select * from Pacientes;
select * from Medicos;