USE ClinicaMedica;
GO

CREATE PROCEDURE sp_GetPacientes

AS 
BEGIN 
	SELECT * FROM Pacientes
END;