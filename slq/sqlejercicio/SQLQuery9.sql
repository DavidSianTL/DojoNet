INSERT INTO Estado_Usuario (descripcion) VALUES ('Activo');

INSERT INTO Estado_Usuario (descripcion) VALUES ('Inactivo');





EXEC sp_AltaUsuario
    @usuario = 'juanperez',
    @nom_usuario = 'Juan Pérez',
    @contrasenia = '123456',
    @fk_id_estado = 1;



	SELECT * FROM Usuario WHERE usuario = 'juanperez';



	SELECT * FROM Bitacora WHERE accion LIKE '%Alta%';


	-----SP de Baja 

	EXEC sp_BajaUsuario @id_usuario = 1;


	SELECT * FROM Usuario WHERE id_usuario = 1;


	SELECT * FROM Bitacora WHERE accion LIKE '%Baja%';


	---SP de Modificar

	EXEC sp_AltaUsuario
    @usuario = 'mlopez',
    @nom_usuario = 'Maria Lopez',
    @contrasenia = 'clave123',
    @fk_id_estado = 1;



	EXEC sp_ModificarUsuario
    @id_usuario = 2,
    @nom_usuario = 'Maria L. Gómez',
    @contrasenia = 'nuevaClave456',
    @fk_id_estado = 1;


	SELECT * FROM Usuario WHERE id_usuario = 2;


	SELECT * FROM Bitacora WHERE accion LIKE '%Modificación%';


