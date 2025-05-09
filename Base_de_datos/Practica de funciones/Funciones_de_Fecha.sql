-- a) Fecha actual del sistema
SELECT GETDATE() AS Fecha_Actual;

-- b) Días en la empresa
SELECT Nombre, DATEDIFF(DAY, FechaIngreso, GETDATE()) AS Dias_En_Empresa FROM Empleados;

-- c) Sumar 30 días a la fecha de ingreso
SELECT Nombre, DATEADD(DAY, 30, FechaIngreso) AS Nueva_Fecha_Ingreso FROM Empleados;

-- d) Mostrar solo el año de ingreso
SELECT Nombre, YEAR(FechaIngreso) AS Año_Ingreso FROM Empleados;
