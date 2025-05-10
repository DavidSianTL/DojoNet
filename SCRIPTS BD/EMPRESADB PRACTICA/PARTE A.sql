USE EmpresaDB;
GO 

--PARTE A - CONSULTAS BÁSICAS

--1.	Consulta todos los empleados activos.
SELECT * FROM Empleados WHERE Activo= 1;

--2.	Muestra los pagos realizados en mayo de 2025.
SELECT * FROM Pagos 
WHERE MONTH(FechaPago)=05 AND YEAR(FechaPago)=2025;

--3.	Muestra el total pagado a cada empleado.
SELECT 
    E.Nombre,
    SUM(P.MontoPagado) AS TotalPagado
FROM Empleados E
JOIN Planilla Pl ON E.EmpleadoID = Pl.EmpleadoID
JOIN Pagos P ON Pl.PlanillaID = P.PlanillaID
GROUP BY E.Nombre;