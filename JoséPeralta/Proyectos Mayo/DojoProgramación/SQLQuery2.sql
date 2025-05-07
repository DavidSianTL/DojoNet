USE master;
go

DROP DATABASE IF EXISTS DojoProgramacion;
go

-- 1. Crear la base de datos
CREATE DATABASE DojoProgramacion;
GO

USE DojoProgramacion;
GO

-- 2. Crear tabla Candidatos
CREATE TABLE Candidatos (
    IdCandidato INT PRIMARY KEY IDENTITY,
    Nombre NVARCHAR(100),
    Email NVARCHAR(100)
);

-- 3. Crear tabla Categorias
CREATE TABLE Categorias (
    IdCategoria INT PRIMARY KEY IDENTITY,
    NombreCategoria NVARCHAR(100)
);

-- 4. Crear tabla Lenguajes
CREATE TABLE Lenguajes (
    IdLenguaje INT PRIMARY KEY IDENTITY,
    NombreLenguaje NVARCHAR(100)
);

-- 5. Crear tabla Ejercicios
CREATE TABLE Ejercicios (
    IdEjercicio INT PRIMARY KEY IDENTITY,
    Titulo NVARCHAR(100),
    Descripcion NVARCHAR(MAX),
    IdCategoria INT,
    IdLenguaje INT,
    FOREIGN KEY (IdCategoria) REFERENCES Categorias(IdCategoria),
    FOREIGN KEY (IdLenguaje) REFERENCES Lenguajes(IdLenguaje)
);

-- 6. Crear tabla Resultados
CREATE TABLE Resultados (
    IdResultado INT PRIMARY KEY IDENTITY,
    IdCandidato INT,
    IdEjercicio INT,
    FechaEjecucion DATE,
    Puntaje INT,
    FOREIGN KEY (IdCandidato) REFERENCES Candidatos(IdCandidato),
    FOREIGN KEY (IdEjercicio) REFERENCES Ejercicios(IdEjercicio)
);

-- 7. Crear tabla Roles
CREATE TABLE Roles (
    IdRol INT PRIMARY KEY IDENTITY,
    NombreRol NVARCHAR(50)
);

-- 8. Crear tabla Permisos
CREATE TABLE Permisos (
    IdPermiso INT PRIMARY KEY IDENTITY,
    NombrePermiso NVARCHAR(100)
);

-- 9. Crear tabla CandidatosRoles
CREATE TABLE CandidatosRoles (
    IdCandidato INT,
    IdRol INT,
    PRIMARY KEY (IdCandidato, IdRol),
    FOREIGN KEY (IdCandidato) REFERENCES Candidatos(IdCandidato),
    FOREIGN KEY (IdRol) REFERENCES Roles(IdRol)
);

-- 10. Crear tabla RolesPermisos
CREATE TABLE RolesPermisos (
    IdRol INT,
    IdPermiso INT,
    PRIMARY KEY (IdRol, IdPermiso),
    FOREIGN KEY (IdRol) REFERENCES Roles(IdRol),
    FOREIGN KEY (IdPermiso) REFERENCES Permisos(IdPermiso)
);

-- 11. Crear tabla Logs
create table Logs(
	IdLog int identity(1,1),
	Accion varchar(50) not null,
	Descripcion varchar(255) not null,
	primary key (IdLog)
);


go

--Insercion de datos 
-- Candidatos
INSERT INTO Candidatos (Nombre, Email) VALUES
('Ana Torres', 'ana.torres@email.com'),
('Carlos Pérez', 'carlos.perez@email.com'),
('Luis Gómez', 'luis.gomez@email.com'),
('María López', 'maria.lopez@email.com'),
('Pedro Martínez', 'pedro.martinez@email.com');

-- Categorías
INSERT INTO Categorias (NombreCategoria) VALUES
('Lógica'),
('Algoritmos'),
('Bases de Datos'),
('Frontend'),
('Backend');

-- Lenguajes
INSERT INTO Lenguajes (NombreLenguaje) VALUES
('C#'),
('JavaScript'),
('Python'),
('SQL'),
('Java');

-- Ejercicios
INSERT INTO Ejercicios (Titulo, Descripcion, IdCategoria, IdLenguaje) VALUES
('Suma de números', 'Escriba una función que sume dos números', 1, 1),
('Buscar en arreglo', 'Buscar un número en un arreglo ordenado', 2, 3),
('Consulta JOIN', 'Escriba una consulta JOIN entre dos tablas', 3, 4),
('Formulario simple', 'Cree un formulario con validaciones', 4, 2),
('API REST básica', 'Cree un controlador que devuelva datos JSON', 5, 1);

-- Resultados
INSERT INTO Resultados (IdCandidato, IdEjercicio, FechaEjecucion, Puntaje) VALUES
(1, 1, '2024-05-01', 90),
(1, 2, '2024-05-02', 85),
(2, 3, '2024-05-01', 70),
(3, 4, '2024-05-03', 95),
(4, 5, '2024-05-01', 88);

-- Roles
INSERT INTO Roles (NombreRol) VALUES
('Administrador'),
('Instructor'),
('Candidato'),
('Evaluador');

-- Permisos
INSERT INTO Permisos (NombrePermiso) VALUES
('Ver Resultados'),
('Editar Ejercicios'),
('Asignar Pruebas'),
('Acceder Panel'),
('Enviar Ejercicio');

-- CandidatosRoles
INSERT INTO CandidatosRoles (IdCandidato, IdRol) VALUES
(1, 3),
(2, 3),
(3, 3),
(4, 2),
(4, 1),
(5, 4);

-- RolesPermisos
INSERT INTO RolesPermisos (IdRol, IdPermiso) VALUES
(1, 1), (1, 2), (1, 3), (1, 4), (1, 5),
(2, 1), (2, 2), (2, 3), (2, 5),
(3, 1), (3, 5),
(4, 1), (4, 3);

go

-- SECCION 1: SELECTS BÁSICOS Y AVANZADOS
-- 1. Obtener todos los candidatos con su email.
select * from Candidatos;

-- 2. Listar todos los ejercicios con su categoría y lenguaje de programación.
select e.*, c.NombreCategoria, l.NombreLenguaje from Ejercicios e join Categorias c on e.IdCategoria = c.IdCategoria join Lenguajes l on e.IdLenguaje = l.IdLenguaje;

-- 3. Obtener los resultados de cada candidato con el título del ejercicio y la fecha de ejecución.
select r.IdResultado, can.Nombre, r.FechaEjecucion, e.Titulo, r.Puntaje from Resultados r join Candidatos can on r.IdCandidato = can.IdCandidato join Ejercicios e on r.IdEjercicio = e.IdEjercicio;

-- 4. Listar todos los candidatos que hayan sacado más de 85 puntos en cualquier ejercicio.
select r.IdResultado, can.Nombre, r.FechaEjecucion, e.Titulo, r.Puntaje 
	from Resultados r 
		join Candidatos can 
			on r.IdCandidato = can.IdCandidato 
			join Ejercicios e 
				on r.IdEjercicio = e.IdEjercicio
					where r.Puntaje > 85;

-- 5. Obtener el promedio de puntajes por candidato.
select can.IdCandidato, can.Nombre, AVG(r.Puntaje) as 'PromedioPuntaje'
	from Resultados r 
		join Candidatos can 
			on r.IdCandidato = can.IdCandidato 
				join Ejercicios e 
					on r.IdEjercicio = e.IdEjercicio
						group by can.IdCandidato, can.Nombre;

-- 6. Listar los permisos que tiene asignado el candidato con nombre 'Ana Torres'.
select p.IdPermiso, p.NombrePermiso, r.NombreRol from Candidatos can 
	join CandidatosRoles cr 
		on can.IdCandidato = cr.IdCandidato 
			join Roles r 
				on cr.IdRol = r.IdRol
					join RolesPermisos rp 
						on r.IdRol = rp.IdRol
							join Permisos p
								on rp.IdPermiso = p.IdPermiso
									where can.Nombre = 'Ana Torres';

-- 7. Listar los ejercicios que no han sido resueltos por ningún candidato.
select * from Ejercicios;
select * from Resultados;
select e.IdEjercicio, e.Titulo, e.Descripcion from Ejercicios e left join Resultados r on e.IdEjercicio = r.IdEjercicio where r.IdEjercicio is null;

go


-- SECCION 2: CREACION DE VISTAS
create view VistaResultadosDetallados
as
select can.Nombre, e.Titulo, l.NombreLenguaje, r.Puntaje, r.FechaEjecucion
from Resultados r
join Candidatos can on r.IdCandidato = can.IdCandidato
join Ejercicios e on  r.IdCandidato = e.IdEjercicio
join Lenguajes l on e.IdLenguaje = l.IdLenguaje;

select * from VistaResultadosDetallados;

go

create view VistaPermisosCandidatos
as
select can.Nombre, r.NombreRol, p.NombrePermiso
from Candidatos can 
join CandidatosRoles cr 
on can.IdCandidato = cr.IdCandidato 
join Roles r 
on cr.IdRol = r.IdRol
join RolesPermisos rp 
on r.IdRol = rp.IdRol
join Permisos p
on rp.IdPermiso = p.IdPermiso;

select * from VistaPermisosCandidatos;
go

-- SECCION 3: PROCEDIMIENTOS ALMACENADOS
create procedure RegistrarResultado
	@IdCandidato int,
	@IdEjercicio int,
	@Puntaje int
as
begin
	set nocount on;

	insert into Resultados (IdCandidato, IdEjercicio, FechaEjecucion, Puntaje) values (@IdCandidato, @IdEjercicio, current_timestamp, @Puntaje);

	insert into Logs(Accion, Descripcion) values ('Nuevo Resultado', concat('El candidato número: ', @IdCandidato, ', ha terminado el ejercicio: ', @IdEjercicio, ', con un puntaje de: ', @Puntaje, ', en fecha: ', current_timestamp));

end;

exec RegistrarResultado 1, 1, 100;
select * from Resultados;
select * from Logs;
go

create procedure ObtenerResultadosPorLenguaje
	@IdLenguaje int
as
begin
	set nocount on;

	select r.IdResultado, r.FechaEjecucion, e.Titulo, e.Descripcion, l.NombreLenguaje from Resultados r join Ejercicios e on r.IdEjercicio = e.IdEjercicio join Lenguajes l on e.IdLenguaje = l.IdLenguaje where e.IdLenguaje = @IdLenguaje;

end;

select * from Ejercicios;

exec ObtenerResultadosPorLenguaje 1;
go

-- SECCION 4: BONUS / OPCIONAL
-- 12. Crear una vista que liste todos los ejercicios junto con la cantidad de veces que han sido resueltos.
select e.IdEjercicio, e.Titulo, e.Descripcion, count(e.IdEjercicio) as 'CantidadResuelta' from Ejercicios e left join Resultados r on e.IdEjercicio = r.IdEjercicio group by e.IdEjercicio, e.Titulo, e.Descripcion;
go

-- 13. Crear un SP que permita asignar un rol a un candidato, si no existe ya la relación.
create procedure sp_AsignarRol
	@IdCandidato int,
	@IdRol int
as
begin
	set nocount on;

	if not exists (select 1 from CandidatosRoles where IdCandidato = @IdCandidato and IdRol = @IdRol)

	begin 
		insert into CandidatosRoles(IdCandidato, IdRol)
			values (@IdCandidato, @IdRol);

		select 'Rol asignado correctamente' as mensaje;
	end
	else
	begin
		select 'El candidato ya tiene este rol asignado' as mensaje;
	end

end;

exec sp_AsignarRol 1,1;

select * from CandidatosRoles;

go

-- 14. Crear un SP que retorne todos los permisos que tiene un rol dado (por nombre).
create procedure sp_RetornoPermisosPorRol
	@TipoRol varchar(50)
as
begin
	set nocount on;

	select p.IdPermiso, p.NombrePermiso, r.NombreRol from Roles r join RolesPermisos rp on r.IdRol = rp.IdRol join Permisos p on rp.IdPermiso = p.IdPermiso where r.NombreRol = @TipoRol;

end;

exec sp_RetornoPermisosPorRol 'Candidato';