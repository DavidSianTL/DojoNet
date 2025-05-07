--9. Crear una vista llamada VistaPermisosCandidatos que muestre: Nombre del Candidato, Rol, Permiso

CREATE VIEW VistaPermisosCandidatos AS
SELECT C.Nombre AS NombreCandidato, R.NombreRol,P.NombrePermiso
FROM Candidatos C
JOIN CandidatosRoles CR ON C.IdCandidato = CR.IdCandidato
JOIN Roles R ON CR.IdRol = R.IdRol
JOIN RolesPermisos RP ON R.IdRol = RP.IdRol
JOIN Permisos P ON RP.IdPermiso = P.IdPermiso;
