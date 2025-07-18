USE EmpresaDB;
GO

CREATE TABLE Persona (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100),
    Edad INT,
    Correo NVARCHAR(100)
);