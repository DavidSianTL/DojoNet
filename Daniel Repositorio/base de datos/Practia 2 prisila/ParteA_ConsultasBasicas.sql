-- Parte A: Consultas básicas
-- 1. Empleados activos:
SELECT * FROM Empleados WHERE Activo = 1;

--2. Pagos realizados en mayo de 2025:
SELECT * FROM Pagos WHERE MONTH(FechaPago) = 5 AND YEAR(FechaPago) = 2025;

--3. Total pagado a cada empleado:
SELECT E.EmpleadoID, E.Nombre + ' ' + E.Apellido AS NombreCompleto, SUM(Pg.MontoPagado) AS TotalPagado
FROM Pagos Pg
JOIN Planilla Pl ON Pg.PlanillaID = Pl.PlanillaID
JOIN Empleados E ON Pl.EmpleadoID = E.EmpleadoID
GROUP BY E.EmpleadoID, E.Nombre, E.Apellido;


