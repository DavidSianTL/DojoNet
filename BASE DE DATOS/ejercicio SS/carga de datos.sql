
use SistemaSeguridad
GO
-- Cargar algunos sistemas
INSERT INTO Sistemas (nombre_sistema, descripcion) 
VALUES ('Sistema de GestionVacacional', 'Sistema de control de vacaciones');

INSERT INTO Sistemas (nombre_sistema, descripcion) 
VALUES ('Sistema de Planilla', 'Sistema de control de pagos de empleados');




USE [SistemaSeguridad]
GO

INSERT INTO [dbo].[Estado_Usuario]  ([descripcion],[fecha_creacion])
     VALUES ('Activo',GETDATE())
GO

INSERT INTO [dbo].[Estado_Usuario]  ([descripcion],[fecha_creacion])
     VALUES ('Inactivo',GETDATE())
GO

INSERT INTO [dbo].[Estado_Usuario]  ([descripcion],[fecha_creacion])
     VALUES ('Bloqueado',GETDATE())
GO


INSERT INTO [dbo].[Estado_Usuario]  ([descripcion],[fecha_creacion])
     VALUES ('Eliminado',GETDATE())
GO

use SistemaSeguridad
GO
-- Cargar usuarios utilizando el procedimiento 'alta_usuario'
EXEC alta_usuario 'admin', 'admin123';
EXEC alta_usuario 'usuario1', 'clave1';
EXEC alta_usuario 'usuario2', 'clave2';
