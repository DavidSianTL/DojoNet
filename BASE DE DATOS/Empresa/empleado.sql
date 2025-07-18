CREATE TABLE Empleado (
    EmpleadoId INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100),
    Cargo NVARCHAR(100),
    Foto VARBINARY(MAX),
    Firma VARBINARY(MAX),
    DocumentoPDF VARBINARY(MAX)
);