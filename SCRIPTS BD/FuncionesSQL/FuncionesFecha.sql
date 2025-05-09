USE FuncionesSQLDemo

ALTER TABLE Empleados
ADD FechaIngreso DATE

--FUNCIONES DE FECHA

--a) Mostrar la fecha actual del sistema.
SELECT Nombre,
	GETDATE() AS Fecha
FROM Empleados;

--b) Calcular cu�ntos d�as lleva cada empleado en la empresa.
SELECT Nombre,
	FechaIngreso,
	DATEDIFF(DAY, FechaIngreso, GETDATE()) AS Dias
FROM Empleados;

--c) Sumar 30 d�as a la fecha de ingreso.
SELECT Nombre,
	FechaIngreso,
	DATEADD(DAY, 30, FechaIngreso) AS MasDias
FROM Empleados;

--d) Mostrar solo el a�o de ingreso.
SELECT Nombre,
	YEAR(FechaIngreso) AS AnioIngreso
FROM Empleados;
