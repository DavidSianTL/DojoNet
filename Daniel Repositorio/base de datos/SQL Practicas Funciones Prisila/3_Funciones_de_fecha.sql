--3: Funciones de Fecha
-- a) Mostrar la fecha actual del sistema
SELECT GETDATE() AS Fecha_Actual;

-- b) Calcular cuántos días lleva cada empleado en la empresa
SELECT ID, Nombre, FechaIngreso,
       DATEDIFF(DAY, FechaIngreso, GETDATE()) AS Dias_En_Empresa FROM Empleados;

-- c) Sumar 30 días a la fecha de ingreso
SELECT ID, Nombre, FechaIngreso,
       DATEADD(DAY, 30, FechaIngreso) AS Fecha_Mas_30_Dias FROM Empleados;

-- d) Mostrar solo el año de ingreso
SELECT ID, Nombre, FechaIngreso,
       YEAR(FechaIngreso) AS Año_Ingreso FROM Empleados;
