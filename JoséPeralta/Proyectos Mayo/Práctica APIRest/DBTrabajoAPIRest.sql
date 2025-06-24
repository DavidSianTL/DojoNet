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
    email varchar(100) not null unique
);
go

-- Tabla de relación muchos a muchos entre Medicos y Especialidades
create table MedicoEspecialidades (
	idMedicoEspecialidad int primary key identity(1,1),
    fk_IdMedico int not null,
    fk_IdEspecialidad int not null,
    constraint FK_MedicoEspecialidad_Medico foreign key (fk_IdMedico) 
        references Medicos(idMedico),
    constraint FK_MedicoEspecialidad_Especialidad foreign key (fk_IdEspecialidad) 
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

-- Insertar médicos (sin la columna fk_IdEspecialidad)
insert into Medicos (nombre, email) values
('Dr. Juan Pérez', 'jperez@clinica.com'),
('Dra. María Gómez', 'mgomez@clinica.com'),
('Dr. Carlos López', 'clopez@clinica.com');
go


-- Insertar relación muchos a muchos entre médicos y especialidades
insert into MedicoEspecialidades (fk_IdMedico, fk_IdEspecialidad) values
(1, 1),  -- Dr. Juan Pérez - Cardiología
(2, 2),  -- Dra. María Gómez - Pediatría
(3, 3),  -- Dr. Carlos López - Dermatología
(1, 3);  -- Dr. Juan Pérez - Dermatología 
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

-- Consultas para verificar los datos
select * from Usuarios;
select * from Especialidades;
select * from Medicos;
select * from MedicoEspecialidades;
select * from Pacientes;
select * from Citas;
go

-- Consulta para ver médicos con sus especialidades
select m.idMedico, m.nombre as Medico, e.nombre as Especialidad
from Medicos m
join MedicoEspecialidades me on m.idMedico = me.fk_IdMedico
join Especialidades e on me.fk_IdEspecialidad = e.idEspecialidad
order by m.nombre, e.nombre;
go

SELECT 
    fk.name AS ForeignKeyName,
    tp.name AS ParentTable,
    cp.name AS ParentColumn,
    tr.name AS ReferencedTable,
    cr.name AS ReferencedColumn
FROM 
    sys.foreign_keys fk
INNER JOIN 
    sys.tables tp ON fk.parent_object_id = tp.object_id
INNER JOIN 
    sys.tables tr ON fk.referenced_object_id = tr.object_id
INNER JOIN 
    sys.foreign_key_columns fkc ON fk.object_id = fkc.constraint_object_id
INNER JOIN 
    sys.columns cp ON fkc.parent_object_id = cp.object_id AND fkc.parent_column_id = cp.column_id
INNER JOIN 
    sys.columns cr ON fkc.referenced_object_id = cr.object_id AND fkc.referenced_column_id = cr.column_id
WHERE 
    tp.name IN ('Medicos', 'MedicoEspecialidades', 'Citas')
    OR tr.name IN ('Medicos', 'MedicoEspecialidades', 'Citas')
ORDER BY 
    tp.name, fk.name;