-- 14. Crear un SP que retorne todos los permisos que tiene un rol dado (por nombre)
CREATE PROCEDURE ObtenerPermisosPorRol
    @NombreRol NVARCHAR(50)
AS
BEGIN
    SELECT 
        R.NombreRol,
        P.NombrePermiso
    FROM Roles R
    JOIN RolesPermisos RP ON R.IdRol = RP.IdRol
    JOIN Permisos P ON RP.IdPermiso = P.IdPermiso
    WHERE R.NombreRol = @NombreRol;
END;
