-- Tabla de Empresas
CREATE TABLE Empresas (
    EmpresaID INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(100) NOT NULL
);

-- Modificación en tabla de Usuarios
ALTER TABLE Usuarios ADD EmpresaID INT;

-- Llave foránea para enlazar usuarios con empresas
ALTER TABLE Usuarios 
ADD CONSTRAINT FK_Usuarios_Empresas FOREIGN KEY (EmpresaID) REFERENCES Empresas(EmpresaID);

-- Modificación en tabla de Usuarios para incluir estado
ALTER TABLE Usuarios ADD Estado VARCHAR(20) DEFAULT 'Pendiente';

-- Procedimiento almacenado para activar usuario
CREATE PROCEDURE ActivarUsuario
    @UsuarioID INT
AS
BEGIN
    UPDATE Usuarios
    SET Estado = 'Activo'
    WHERE UsuarioID = @UsuarioID;
END;

-- Tabla de Roles
CREATE TABLE Roles (
    RolID INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(50) NOT NULL
);

-- Relación Usuarios-Roles
CREATE TABLE UsuarioRoles (
    UsuarioID INT,
    RolID INT,
    PRIMARY KEY (UsuarioID, RolID),
    FOREIGN KEY (UsuarioID) REFERENCES Usuarios(UsuarioID),
    FOREIGN KEY (RolID) REFERENCES Roles(RolID)
);

-- Procedimiento para validar autorización antes de ejecutar acciones
CREATE PROCEDURE ValidarAutorizacion
    @UsuarioID INT,
    @RolRequerido VARCHAR(50)
AS
BEGIN
    IF EXISTS (
        SELECT 1 FROM UsuarioRoles UR
        JOIN Roles R ON UR.RolID = R.RolID
        WHERE UR.UsuarioID = @UsuarioID AND R.Nombre = @RolRequerido
    )
    BEGIN
        PRINT 'Autorización concedida';
        RETURN 1;
    END
    ELSE
    BEGIN
        PRINT 'Autorización denegada';
        RETURN 0;
    END;
END;
