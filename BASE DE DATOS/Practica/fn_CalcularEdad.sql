use EmpresaDB
go
CREATE FUNCTION fn_CalcularEdad (@FechaNacimiento DATE)
RETURNS INT
AS
BEGIN
    RETURN DATEDIFF(YEAR, @FechaNacimiento, GETDATE()) -
           CASE WHEN MONTH(@FechaNacimiento) > MONTH(GETDATE()) 
                 OR (MONTH(@FechaNacimiento) = MONTH(GETDATE()) AND DAY(@FechaNacimiento) > DAY(GETDATE()))
                THEN 1 ELSE 0 END;
END;
