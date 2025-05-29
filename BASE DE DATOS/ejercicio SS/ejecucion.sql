use SistemaSeguridad
GO

-- Alta de usuario
EXEC alta_usuario 'juanperez', 'Juan perez','1234abc';
use SistemaSeguridad
GO
-- Alta de usuario
EXEC alta_usuario 'prisilaf', 'prisila flores','12345679';

use SistemaSeguridad
GO
-- Alta de usuario
EXEC alta_usuario 'prueba', 'prueba','999999';

use SistemaSeguridad
GO
-- Alta de usuario
EXEC baja_usuario 3;


use SistemaSeguridad
GO
-- Cambio de contraseña
EXEC cambio_contraseña 1, 'nuevoPass123';

use SistemaSeguridad
GO
-- Validar acceso
EXEC validar_acceso 'juanperez', '1234abc';

EXEC validar_acceso 'juanperez','nuevoPass123';


use SistemaSeguridad
GO
-- Cerrar sesión
EXEC cerrar_sesion 1;

use SistemaSeguridad
GO
-- Baja de usuario
EXEC SistemaSeguridad..baja_usuario 2;
