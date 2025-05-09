--3: Funciones de Fecha
-- a) Mostrar la fecha actual del sistema
SELECT GETDATE() AS Fecha_Actual;

-- b) Calcular cu�ntos d�as lleva cada empleado en la empresa
SELECT ID, Nombre, FechaIngreso,
       DATEDIFF(DAY, FechaIngreso, GETDATE()) AS Dias_En_Empresa FROM Empleados;

-- c) Sumar 30 d�as a la fecha de ingreso
SELECT ID, Nombre, FechaIngreso,
       DATEADD(DAY, 30, FechaIngreso) AS Fecha_Mas_30_Dias FROM Empleados;

-- d) Mostrar solo el a�o de ingreso
SELECT ID, Nombre, FechaIngreso,
       YEAR(FechaIngreso) AS A�o_Ingreso FROM Empleados;
