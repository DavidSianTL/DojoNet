CREATE TABLE Autorizaciones (
    id_autorizacion INT PRIMARY KEY IDENTITY(1,1),
    fk_id_usuario INT NOT NULL,
    fk_id_sistema INT NOT NULL,
    autorizado BIT DEFAULT 0,
    fecha_solicitud DATETIME DEFAULT GETDATE(),
    fecha_autorizacion DATETIME NULL,
    autorizado_por INT NULL,
    FOREIGN KEY (fk_id_usuario) REFERENCES Usuarios(id_usuario),
    FOREIGN KEY (fk_id_sistema) REFERENCES Sistemas(id_sistema),
    FOREIGN KEY (autorizado_por) REFERENCES Usuarios(id_usuario)
);




CREATE PROCEDURE solicitar_autorizacion
    @p_id_usuario INT,
    @p_id_sistema INT
AS
BEGIN
    BEGIN TRY
        INSERT INTO Autorizaciones (fk_id_usuario, fk_id_sistema)
        VALUES (@p_id_usuario, @p_id_sistema);
    END TRY
    BEGIN CATCH
        INSERT INTO LogErrores (fk_id_usuario, mensaje_error, procedimiento)
        VALUES (@p_id_usuario, ERROR_MESSAGE(), 'solicitar_autorizacion');
    END CATCH
END;




CREATE PROCEDURE autorizar_usuario
    @p_id_autorizacion INT,
    @p_id_admin INT
AS
BEGIN
    BEGIN TRY
        UPDATE Autorizaciones
        SET autorizado = 1,
            fecha_autorizacion = GETDATE(),
            autorizado_por = @p_id_admin
        WHERE id_autorizacion = @p_id_autorizacion;
    END TRY
    BEGIN CATCH
        INSERT INTO LogErrores (fk_id_usuario, mensaje_error, procedimiento)
        VALUES (@p_id_admin, ERROR_MESSAGE(), 'autorizar_usuario');
    END CATCH
END;






-- Obtener ID del estado 'Pendiente'
DECLARE @id_estado_pendiente INT;
SELECT @id_estado_pendiente = id_estado FROM Estado_Usuario WHERE descripcion = 'Pendiente';

-- Insertar usuario con estado Pendiente
INSERT INTO Usuarios (usuario, nom_usuario, contrasenia, fk_id_estado)
VALUES (@p_usuario, @p_nom_usuario, @p_password_encriptada, @id_estado_pendiente);



use SistemaSeguridad


select * from Estado_Usuario