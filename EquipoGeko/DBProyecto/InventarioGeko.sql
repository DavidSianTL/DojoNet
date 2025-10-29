USE DBProyectoGrupalDojoGeko;
GO

-- Verificar si las tablas ya existen antes de crearlas
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Inventario_Equipos')
BEGIN
    -- Tabla de Equipos (Catálogo maestro)
    CREATE TABLE Inventario_Equipos (
        IdEquipo INT PRIMARY KEY IDENTITY(1,1),
        TipoEquipo NVARCHAR(100) NOT NULL, -- Laptop, Monitor, Celular, etc.
        Descripcion NVARCHAR(255),
        Modelo NVARCHAR(100) NOT NULL,
        Marca NVARCHAR(100),
        Procesador NVARCHAR(255),
        Almacenamiento NVARCHAR(100),
        RAM NVARCHAR(50),
        SistemaOperativo NVARCHAR(100),
        EspecificacionesTecnicas NVARCHAR(500)
    );
    PRINT 'Tabla Inventario_Equipos creada exitosamente';
END
ELSE
BEGIN
    PRINT 'La tabla Inventario_Equipos ya existe';
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Inventario')
BEGIN
    -- Tabla de Inventario (Items específicos)
    CREATE TABLE Inventario (
        IdInventario INT PRIMARY KEY IDENTITY(1,1),
        IdEquipo INT NOT NULL,
        IdEmpleadoAsignado INT NULL, -- NULL si está disponible
        NumeroSerie NVARCHAR(100) UNIQUE,
        AnioFabricacion NVARCHAR(50),
        NumeroFactura NVARCHAR(100),
        Condicion NVARCHAR(50) NOT NULL, -- Bueno, Nuevo, Reparar, etc.
        Contrasena NVARCHAR(255),
        UsuarioAsignado NVARCHAR(100),
        CartaResponsabilidad NVARCHAR(500), -- URL del documento
        FechaAsignacion DATETIME,
        FechaRegistro DATETIME DEFAULT GETDATE(),
        Estado NVARCHAR(20) DEFAULT 'Activo', -- Activo, Inactivo, Baja

        FOREIGN KEY (IdEquipo) REFERENCES Inventario_Equipos(IdEquipo),
        FOREIGN KEY (IdEmpleadoAsignado) REFERENCES Empleados(IdEmpleado)
    );
    PRINT 'Tabla Inventario creada exitosamente';
END
ELSE
BEGIN
    PRINT 'La tabla Inventario ya existe';
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Asignaciones_Inventario')
BEGIN
    -- Tabla de Asignaciones (Histórico de asignaciones)
    CREATE TABLE Asignaciones_Inventario (
        IdAsignacion INT PRIMARY KEY IDENTITY(1,1),
        IdInventario INT NOT NULL,
        IdEmpleado INT NOT NULL,
        IdEquipoTeam INT NOT NULL, -- Referencia a la tabla Equipos existente
        FechaAsignacion DATETIME NOT NULL,
        FechaDevolucion DATETIME NULL,
        Observaciones NVARCHAR(500),

        FOREIGN KEY (IdInventario) REFERENCES Inventario(IdInventario),
        FOREIGN KEY (IdEmpleado) REFERENCES Empleados(IdEmpleado),
        FOREIGN KEY (IdEquipoTeam) REFERENCES Equipos(IdEquipo) -- Referencia a tabla Equipos existente
    );
    PRINT 'Tabla Asignaciones_Inventario creada exitosamente';
END
ELSE
BEGIN
    PRINT 'La tabla Asignaciones_Inventario ya existe';
END
GO
