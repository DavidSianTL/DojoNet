use master;
go

alter database DBDojoEscuela
set single_user
with rollback immediate;
go

drop database if exists DBDojoEscuela;
go

create database DBDojoEscuela;
go

use DBDojoEscuela;
go

create table Estudiantes(
	id_estudiante int identity(1,1),
	nombre_estudiante nvarchar(50),
	fecha_nacimiento datetime default current_timestamp,
	primary key (id_estudiante)
);
go

create table Notas(
	id_nota int identity(1,1),
	fk_id_estudiante int not null,
	asignatura nvarchar(100),
	nota decimal(4,2),
	fecha_registro datetime default current_timestamp,
	primary key (id_nota),
	foreign key (fk_id_estudiante)
		references estudiantes(id_estudiante)
);
go

create table Logs(
	id_log int identity(1,1),
	fk_id_usuario int default 1,
	accion nvarchar(50),
	accion_tabla nvarchar(50),
	fecha_registro datetime default current_timestamp,
	descripcion nvarchar(100),
	primary key (id_log)
	--foreign key (fk_id_usuario)
		--references usuarios (id_usuario)
);
go


create trigger tr_ValidarNotas
	on Notas
	after insert
as
begin
	
	-- Validar que las notas estén entre 0 y 10
	if exists (select 1 from inserted where nota < 0 or nota > 10)
	begin
		
		-- Lanzamos un error
		raiserror('La nota debe estar entre el valor de 0 y 10', 16, 1);

		-- Insertamos el error
		insert into Logs(accion, accion_tabla, descripcion)
			values('Insert', 'Notas', 'Se intentó insertar una nota, fuera de rango');

		-- Revertir la operación
		rollback transaction;

	end

end;
go

--
create trigger tr_ActualizarFechaNotas
	on Notas
	after update
as
begin
	
	-- Actualizar la fecha de registro de notas
	update Notas 
	set
		fecha_registro = current_timestamp
	from Notas n
	join inserted i on n.id_nota = i.id_nota
	where n.id_nota = i.id_nota;

	-- Insertamos el error
		insert into Logs(accion, accion_tabla, descripcion)
			values('Update', 'Notas', 'Nota actualizada correctamente');


end;
go

-- Insertamos datos
insert into Estudiantes(nombre_estudiante) values 
(N'Carlos Pérez'),
(N'Lucía Fernández'),
(N'Mario Ramírez');
go

insert into Notas(fk_id_estudiante, asignatura, nota) values
(1, N'Matemáticas', 8.5),
(2, N'Lengua', 9.2),
(3, N'Historia', 7.3);
go

insert into Logs(accion, accion_tabla, descripcion) values
('Insert', 'Estudiantes', 'Se insertó nuevo estudiante Carlos Pérez'),
('Insert', 'Estudiantes', 'Se insertó nuevo estudiante Lucía Fernández'),
('Insert', 'Estudiantes', 'Se insertó nuevo estudiante Mario Ramírez');
go


-- Probamos los triggers (disparadores)

