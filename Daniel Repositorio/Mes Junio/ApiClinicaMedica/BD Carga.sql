CREATE DATABASE ClinicaDB;
GO
USE ClinicaDB;
GO

CREATE TABLE Especialidades (
    IdEspecialidad INT PRIMARY KEY IDENTITY,
    Nombre NVARCHAR(100) NOT NULL
);

CREATE TABLE Medicos (
    IdMedico INT PRIMARY KEY IDENTITY,
    Nombre NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100),
    EspecialidadId INT,
    FOREIGN KEY (EspecialidadId) REFERENCES Especialidades(IdEspecialidad)
);

CREATE TABLE Pacientes (
    IdPaciente INT PRIMARY KEY IDENTITY,
    Nombre NVARCHAR(100),
    Email NVARCHAR(100),
    Telefono NVARCHAR(20),
    FechaNacimiento DATE
);

CREATE TABLE Citas (
    IdCita INT PRIMARY KEY IDENTITY,
    PacienteId INT,
    MedicoId INT,
    Fecha DATE,
    Hora TIME,
    FOREIGN KEY (PacienteId) REFERENCES Pacientes(IdPaciente),
    FOREIGN KEY (MedicoId) REFERENCES Medicos(IdMedico)
);
