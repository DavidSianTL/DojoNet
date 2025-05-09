SELECT 
    Nombre,
    Departamento,
    Salario,
    RANK() OVER (PARTITION BY Departamento ORDER BY Salario DESC) AS RankingDepto
FROM Empleados;
