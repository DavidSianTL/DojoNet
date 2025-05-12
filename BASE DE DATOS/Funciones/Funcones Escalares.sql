-- Texto
SELECT Nombre
	, UPPER(Nombre) AS NombreMayus
	, LEN(Nombre) AS LargoNombre 
	, LOWER(Nombre) AS NombreMinuscula
FROM Empleados;

-- Fecha
SELECT Nombre
	, FechaIngreso
	, DATEDIFF(DAY, FechaIngreso, GETDATE()) AS DiasEnEmpresa
FROM Empleados;

-- Números
SELECT Nombre
	, Salario
	, ROUND(Salario * 1.05, 2) AS SalarioConAjuste
	, (Salario -ROUND(Salario * 1.05, 2)) AS Diferencia
FROM Empleados;



SELECT 
    Nombre,
    UPPER(SUBSTRING(Nombre, 1, 5)) AS Abreviado,
    FORMAT(FechaIngreso, 'yyyy-MM-dd') AS FechaIngresoFormato,
    DATEDIFF(DAY, FechaIngreso, GETDATE()) AS DiasEnEmpresa
FROM Empleados;