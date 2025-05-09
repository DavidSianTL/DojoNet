--La vista de esta seccion esta en seccion 2 con las demás vistas

CREATE OR ALTER PROCEDURE AsignarRolACandidato
    @IdCandidato INT,
    @IdRol INT
AS
BEGIN
   
    IF NOT EXISTS (
        SELECT 1 
        FROM CandidatosRoles 
        WHERE IdCandidato = @IdCandidato AND IdRol = @IdRol
    )
    BEGIN
      
        INSERT INTO CandidatosRoles (IdCandidato, IdRol)
        VALUES (@IdCandidato, @IdRol);

        PRINT 'Rol asignado correctamente.';
    END
    ELSE
    BEGIN
        PRINT 'El candidato ya tiene ese rol asignado.';
    END
END;
GO

EXEC AsignarRolACandidato @IdCandidato = 3, @IdRol = 2;
GO

CREATE OR ALTER PROCEDURE ObtenerPermisosPorRol
    @NombreRol NVARCHAR(50)
AS
BEGIN
    -- Selecciona todos los permisos asignados al rol indicado por nombre
    SELECT p.NombrePermiso
    FROM Permisos p
    JOIN RolesPermisos rp ON p.IdPermiso = rp.IdPermiso
    JOIN Roles r ON rp.IdRol = r.IdRol
    WHERE r.NombreRol = @NombreRol;
END;
GO

EXEC ObtenerPermisosPorRol @NombreRol = 'Administrador';