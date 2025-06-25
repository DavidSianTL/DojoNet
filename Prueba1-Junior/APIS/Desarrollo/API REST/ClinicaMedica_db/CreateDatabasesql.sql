USE master;
GO
CREATE DATABASE ClinicaMedica;
GO
USE ClinicaMedica;
GO 



CREATE TABLE Usuarios (
	id INT PRIMARY KEY IDENTITY(1,1),
	username NVARCHAR(50) NOT NULL,
	email NVARCHAR(50) NOT NULL,
	password NVARCHAR(100) NOT NULL,
	rol NVARCHAR(50) NOT NULL,
	estado BIT DEFAULT 1
);
GO

--SP's
	--SELECT
	CREATE PROCEDURE sp_GetUsuarios
	AS 
	BEGIN 
		SELECT * FROM Usuarios WHERE estado = 1;
	END;
	GO 

	--SELECT BY Id
	CREATE PROCEDURE sp_GetUsuarioById
		@id INT
	AS
	BEGIN 
		SELECT * FROM Usuarios
		WHERE id = @id;
	END;
	GO 

	-- SELECT BY CREDENTIALS
	CREATE PROCEDURE sp_GetUsuarioByCredentials
		@email NVARCHAR(100),
		@password NVARCHAR(100)
	AS
	BEGIN
		SELECT * FROM Usuarios
		WHERE email = @email AND Password = @password AND estado = 1;
	END;
	GO


CREATE TABLE Especialidades (

	id INT PRIMARY KEY IDENTITY(1,1),
	nombre NVARCHAR(50) NOT NULL
);

CREATE TABLE Pacientes(

	id INT PRIMARY KEY IDENTITY(1,1),
	nombre NVARCHAR(50) NOT NULL,
	email NVARCHAR(50) NOT NULL,
	telefono NVARCHAR(25) NOT NULL,
	fecha_nacimiento DATE NOT NULL,
	estado BIT DEFAULT 1
);
GO
-- SP's

	--SELECT
	CREATE PROCEDURE sp_GetPacientes
	AS 
	BEGIN 
		SELECT * FROM Pacientes WHERE estado = 1;
	END;
	GO

	--SELECT BY Id
	CREATE PROCEDURE sp_GetPacienteById
		@id INT
	AS
	BEGIN 
		SELECT * FROM Pacientes 
		WHERE id = @id;
	END;
	GO 

	--INSERT
	CREATE PROCEDURE sp_InsertPaciente
		@nombre NVARCHAR(50),
		@email NVARCHAR(50), 
		@telefono NVARCHAR(25), 
		@fecha_nacimiento DATE
	AS
	BEGIN 
		INSERT INTO Pacientes (nombre, email, telefono, fecha_nacimiento)
		VALUES (@nombre, @email, @telefono, @fecha_nacimiento);
	END;
	GO 

	-- UPDATE 
	CREATE PROCEDURE sp_EditPaciente
		@id INT,
		@nombre NVARCHAR(50),
		@email NVARCHAR(50), 
		@telefono NVARCHAR(25), 
		@fecha_nacimiento DATE, 
		@estado BIT
	AS
	BEGIN
		UPDATE Pacientes
		SET 
			nombre = @nombre,
			email = @email, 
			telefono = @telefono, 
			fecha_nacimiento = @fecha_nacimiento, 
			estado = @estado
		WHERE id = @id;
	END;
	GO 

	-- DELETE
	CREATE PROCEDURE sp_DeletePaciente
		@id INT
	AS
	BEGIN
		BEGIN TRY
			BEGIN TRANSACTION;

				-- Desactivar paciente
				UPDATE Pacientes
				SET estado = 0
				WHERE id = @id;

				-- Desactivar citas relacionadas
				UPDATE Citas
				SET estado = 0
				WHERE FK_IdPaciente = @id;

			COMMIT;
		END TRY
		BEGIN CATCH
			ROLLBACK;
		END CATCH
	END;
	GO 


CREATE TABLE Medicos(

	id INT PRIMARY KEY IDENTITY(1,1),
	nombre NVARCHAR(50) NOT NULL,
	FK_Idespecialidad INT NOT NULL, 
	email NVARCHAR(50) NOT NULL,
	estado BIT DEFAULT 1,

	FOREIGN KEY (FK_Idespecialidad) REFERENCES Especialidades(id)
);
GO 

-- SP's

	--SELECT
	CREATE PROCEDURE sp_GetMedicos

	AS 
	BEGIN 
		SELECT * FROM Medicos WHERE estado = 1;
	END;
	GO 

	--SELECT BY Id
	CREATE PROCEDURE sp_GetMedicoById
		@id INT
	AS
	BEGIN 
		SELECT * FROM Medicos 
		WHERE id = @id;
	END;
	GO 

	--INSERT
	CREATE PROCEDURE sp_InsertMedico
		@nombre NVARCHAR(50),
		@FK_Idespecialidad INT,
		@email NVARCHAR(50)
	AS
	BEGIN
		INSERT INTO Medicos (nombre, FK_Idespecialidad, email)
		VALUES (@nombre, @FK_Idespecialidad, @email);
	END;
	GO 

	-- UPDATE 
	CREATE PROCEDURE sp_EditMedico
		@id INT,
		@nombre NVARCHAR(50),
		@FK_Idespecialidad INT, 
		@email NVARCHAR(50), 
		@estado BIT
	AS
	BEGIN
		UPDATE Medicos
		SET 
			nombre = @nombre,
			FK_Idespecialidad = @FK_Idespecialidad,
			email = @email, 
			estado = @estado
		WHERE id = @id;
	END;
	GO 

	--DELETE
	CREATE PROCEDURE sp_DeleteMedico
		@id INT
	AS
	BEGIN
		BEGIN TRY
			BEGIN TRANSACTION;

				-- Desactivar medico
				UPDATE Medicos
				SET estado = 0
				WHERE id = @id;

				--Desactivar citas relacionadas
				UPDATE Citas 
				SET estado = 0
				WHERE FK_IdMedico = @id;

			COMMIT;
		END TRY
		BEGIN CATCH
			ROLLBACK;
		END CATCH 
	END;
	GO 


CREATE TABLE Citas (

	id INT PRIMARY KEY IDENTITY(1,1),
	FK_IdPaciente INT NOT NULL,
	FK_IdMedico INT NOT NULL,
	fecha DATE NOT NULL,
	hora TIME NOT NULL,
	estado BIT DEFAULT 1,

	FOREIGN KEY (FK_IdPaciente) REFERENCES Pacientes(id),
	FOREIGN KEY (FK_IdMedico) REFERENCES Medicos(id)
);
GO 

-- SP's

	--SELECT
	CREATE PROCEDURE sp_GetCitas

	AS 
	BEGIN 
		SELECT * FROM Citas WHERE estado = 1;
	END;
	GO 

	--SELECT por id
	CREATE PROCEDURE sp_GetCitaById
		@id INT
	AS 
	BEGIN 
		SELECT * FROM Citas WHERE id = @id;
	END;
	GO 

	--INSERT
	CREATE PROCEDURE sp_InsertCita
		@FK_IdMedico INT,
		@FK_IdPaciente INT,
		@fecha DATE,
		@hora TIME
	AS
	BEGIN 
		INSERT INTO Citas (FK_IdMedico, FK_IdPaciente, fecha, hora)
		VALUES (@FK_IdMedico, @FK_IdPaciente, @fecha, @hora);
	END;
	GO 

	--UPDATE
	CREATE PROCEDURE sp_EditCita
		@id INT,
		@FK_IdPaciente INT,
		@FK_IdMedico INT, 
		@fecha DATE,
		@hora TIME,
		@estado BIT
	AS
	BEGIN
		UPDATE Citas
		SET 
			FK_IdPaciente = @FK_IdPaciente,
			FK_IdMedico = @FK_IdMedico,
			fecha = @fecha,
			hora = @hora,
			estado = @estado
		WHERE id = @id;
	END;
	GO 

	--DELETE
	CREATE PROCEDURE sp_DeleteCita
		@id INT
	AS 
	BEGIN 
		BEGIN TRY 
			BEGIN TRANSACTION;
				UPDATE Citas	
				SET estado = 0
				WHERE id = @id;
			COMMIT;
		END TRY 
		BEGIN CATCH
			ROLLBACK;
		END CATCH 
	END;
	GO 
	

-- Insert de usuarios iniciales 
INSERT INTO Usuarios (username, email, password, role) 
VALUES
	('iunior','iunior@email.com', 'password', 'sysAdmin'),
	('Simi','simidrxdd@email.com', 'password', 'doctor')
;



-- Insert de especialidades
INSERT INTO Especialidades (nombre)
VALUES 
	('Cirujano'),
	('Odontologo'),
	('Pediatra')
;