SELECT 
    COUNT(*) AS TotalEmpleados,
    AVG(Salario) AS PromedioSalario,
    MAX(Salario) AS SalarioMaximo,
    MIN(FechaIngreso) AS AntigüedadMaxima
FROM Empleados;

-- Agrupado por departamento
SELECT 
    Departamento,
    COUNT(*) AS Cantidad,
    SUM(Salario) AS TotalSalarios
FROM Empleados
GROUP BY Departamento;
