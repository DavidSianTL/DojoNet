--13. Crear un SP que permita asignar un rol a un candidato, si no existe ya la relación
CREATE PROCEDURE AsignarRolACandidato
    @IdCandidato INT,
    @IdRol INT
AS
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM CandidatosRoles
        WHERE IdCandidato = @IdCandidato AND IdRol = @IdRol
    )
    BEGIN
        INSERT INTO CandidatosRoles (IdCandidato, IdRol)
        VALUES (@IdCandidato, @IdRol);
        
        PRINT 'Rol asignado exitosamente.';
    END
    ELSE
    BEGIN
        PRINT 'Rol ya asignado a un candidato.';
    END
END;
