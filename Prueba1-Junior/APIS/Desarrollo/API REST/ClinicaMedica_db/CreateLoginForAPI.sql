USE master;
GO

CREATE LOGIN clinicaUser WITH PASSWORD = 'drugs123xd!';
GO 

USE ClinicaMedica;
GO

CREATE USER clinicaUser FOR LOGIN clinicaUser;
GO 

ALTER ROLE db_datareader ADD MEMBER clinicaUser;
ALTER ROLE db_datawriter ADD MEMBER clinicaUser;
GO

GRANT EXECUTE TO clinicaUser;
GO 

-- SI SE CREA ESTE USUARIO SE PUEDE CAMBIAR LA CADENA DE CONEXION A:
--   "Server=SKINOFME;Database=ClinicaMedica;User Id=clinicaUser;Password=drugsxd;TrustServerCertificate=True;"
--           (reemplazar)
