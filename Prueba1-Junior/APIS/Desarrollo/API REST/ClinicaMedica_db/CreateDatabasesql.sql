CREATE DATABASE ClinicaMedica;
GO
USE ClinicaMedica;
GO 


CREATE TABLE Especialidades (
	id INT PRIMARY KEY IDENTITY(1,1),
	nombre NVARCHAR(50) NOT NULL
);

CREATE TABLE Pacientes(
	
	id INT PRIMARY KEY IDENTITY(1,1),
	nombre NVARCHAR(50) NOT NULL,
	email NVARCHAR(50) NOT NULL,
	telefono NVARCHAR(50) NOT NULL,
	fecha_nacimiento DATE

);



CREATE TABLE Medicos(

	id INT PRIMARY KEY IDENTITY(1,1),
	nombre NVARCHAR(50) NOT NULL,
	FK_especialidad_id INT NOT NULL, 
	email NVARCHAR(50) NOT NULL,

	FOREIGN KEY (FK_especialidad_id) REFERENCES Especialidades(id)
);

CREATE TABLE Citas (

	id INT PRIMARY KEY IDENTITY(1,1),
	FK_paciente_id INT NOT NULL,
	FK_medico_id INT NOT NULL,

	FOREIGN KEY (FK_paciente_id) REFERENCES Pacientes(id),
	FOREIGN KEY (FK_medico_id) REFERENCES Medicos(id)
);

