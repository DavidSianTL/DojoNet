
use SistemaSeguridad
GO
-- Cargar algunos sistemas
INSERT INTO Sistemas (nombre_sistema, descripcion) 
VALUES ('Sistema de GestionVacacional', 'Sistema de control de vacaciones');
use SistemaSeguridad
GO
INSERT INTO Sistemas (nombre_sistema, descripcion) 
VALUES ('Sistema de Planilla', 'Sistema de control de pagos de empleados');

use SistemaSeguridad
GO
INSERT INTO Estado_Usuario(descripcion)
VALUES ('Activo');
INSERT INTO Estado_Usuario(descripcion)
VALUES ('Inactivo');
INSERT INTO Estado_Usuario(descripcion)
VALUES ('Bloqueado');
INSERT INTO Estado_Usuario(descripcion)
VALUES ('Eliminado');
 


use SistemaSeguridad
GO
-- Cargar usuarios utilizando el procedimiento 'alta_usuario'
EXEC alta_usuario 'admin', 'admin123';
EXEC alta_usuario 'usuario1', 'clave1';
EXEC alta_usuario 'usuario2', 'clave2';
