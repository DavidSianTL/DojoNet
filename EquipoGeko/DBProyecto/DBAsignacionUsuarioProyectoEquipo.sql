-- Usamos nuestra DB
USE DBProyectoGrupalDojoGeko;
GO

-- =================================================================
-- CONFIGURACIÓN DEL USUARIO AdminDev PARA PRUEBAS
-- =================================================================

-- Declaramos variables para almacenar los IDs que vamos a necesitar
DECLARE @IdUsuarioAdminDev INT;
DECLARE @IdEmpleadoAdminDev INT;
DECLARE @IdRolEmpleado INT;
DECLARE @IdRolTeamLider INT;
DECLARE @IdRolAutorizador INT;
DECLARE @IdEquipoDojoNet INT;
DECLARE @IdProyectoGDG INT;

-- 1. Buscamos los IDs del usuario y el empleado
SELECT 
    @IdUsuarioAdminDev = IdUsuario, 
    @IdEmpleadoAdminDev = FK_IdEmpleado 
FROM Usuarios 
WHERE Username = 'AdminDev';

-- 2. Buscamos los IDs de los roles que vamos a asignar
SELECT @IdRolEmpleado = IdRol FROM Roles WHERE NombreRol = 'Empleado';
SELECT @IdRolTeamLider = IdRol FROM Roles WHERE NombreRol = 'TeamLider';
SELECT @IdRolAutorizador = IdRol FROM Roles WHERE NombreRol = 'Autorizador';

-- 3. Creamos el equipo 'Dojo .Net' y obtenemos su ID
IF NOT EXISTS (SELECT 1 FROM Equipos WHERE Nombre = 'Dojo .Net')
BEGIN
    INSERT INTO Equipos (Nombre, Descripcion, FK_IdEstado)
    VALUES ('Dojo .Net', 'Equipo para el proyecto Dojo .Net', 1);
END
SELECT @IdEquipoDojoNet = IdEquipo FROM Equipos WHERE Nombre = 'Dojo .Net';

-- 4. Buscamos el ID del proyecto 'GDG'
SELECT @IdProyectoGDG = IdProyecto FROM Proyectos WHERE Nombre = 'GDG';

-- Verificamos que todos los IDs se hayan encontrado antes de continuar
IF @IdUsuarioAdminDev IS NOT NULL AND @IdRolEmpleado IS NOT NULL AND @IdRolTeamLider IS NOT NULL AND @IdRolAutorizador IS NOT NULL AND @IdEquipoDojoNet IS NOT NULL AND @IdProyectoGDG IS NOT NULL
BEGIN
    -- 5. Asignamos los roles al usuario 'AdminDev'
    -- Usamos sp_InsertarUsuariosRol para evitar duplicados si el script se corre de nuevo
    EXEC sp_InsertarUsuariosRol @FK_IdUsuario = @IdUsuarioAdminDev, @FK_IdRol = @IdRolEmpleado;
    EXEC sp_InsertarUsuariosRol @FK_IdUsuario = @IdUsuarioAdminDev, @FK_IdRol = @IdRolTeamLider;
    EXEC sp_InsertarUsuariosRol @FK_IdUsuario = @IdUsuarioAdminDev, @FK_IdRol = @IdRolAutorizador;

    -- 6. Asignamos el empleado al equipo 'Dojo .Net' con el rol de 'TeamLider'
    IF NOT EXISTS (SELECT 1 FROM EmpleadosEquipo WHERE FK_IdEmpleado = @IdEmpleadoAdminDev AND FK_IdEquipo = @IdEquipoDojoNet)
    BEGIN
        EXEC sp_AsignarEmpleadoAEquipo @FK_IdEquipo = @IdEquipoDojoNet, @FK_IdEmpleado = @IdEmpleadoAdminDev, @FK_IdRol = @IdRolTeamLider;
    END

    -- 7. Asignamos el equipo 'Dojo .Net' al proyecto 'GDG'
    IF NOT EXISTS (SELECT 1 FROM EquiposProyecto WHERE FK_IdEquipo = @IdEquipoDojoNet AND FK_IdProyecto = @IdProyectoGDG)
    BEGIN
        EXEC sp_AsignarEquipoAProyecto @FK_IdProyecto = @IdProyectoGDG, @FK_IdEquipo = @IdEquipoDojoNet;
    END

    PRINT 'Configuración del usuario AdminDev completada con éxito.';
END
ELSE
BEGIN
    PRINT 'Error: No se pudieron encontrar todos los IDs necesarios. Revisa que el usuario, los roles y el proyecto existan.';
END
GO

-- =================================================================
-- VERIFICACIÓN DE LAS RELACIONES CREADAS
-- =================================================================

-- 1. Ver los roles del usuario 'AdminDev'
SELECT u.Username, r.NombreRol
FROM Usuarios u
JOIN UsuariosRol ur ON u.IdUsuario = ur.FK_IdUsuario
JOIN Roles r ON ur.FK_IdRol = r.IdRol
WHERE u.Username = 'AdminDev';
GO

-- 2. Ver los miembros del equipo 'Dojo .Net' y su rol
SELECT eq.Nombre AS NombreEquipo, em.NombresEmpleado, r.NombreRol
FROM Equipos eq
JOIN EmpleadosEquipo ee ON eq.IdEquipo = ee.FK_IdEquipo
JOIN Empleados em ON ee.FK_IdEmpleado = em.IdEmpleado
JOIN Roles r ON ee.FK_IdRol = r.IdRol
WHERE eq.Nombre = 'Dojo .Net';
GO

-- 3. Ver los equipos asignados al proyecto 'GDG'
SELECT p.Nombre AS NombreProyecto, eq.Nombre AS NombreEquipo
FROM Proyectos p
JOIN EquiposProyecto ep ON p.IdProyecto = ep.FK_IdProyecto
JOIN Equipos eq ON ep.FK_IdEquipo = eq.IdEquipo
WHERE p.Nombre = 'GDG';
GO