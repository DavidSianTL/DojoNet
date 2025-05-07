----12. Crear una vista que liste todos los ejercicios junto con la cantidad de veces que han sido resueltos

CREATE VIEW VistaEjerciciosResueltos AS
SELECT 
    E.IdEjercicio,
    E.Titulo,
    COUNT(R.IdResultado) AS VecesResuelto
FROM Ejercicios E
LEFT JOIN Resultados R ON E.IdEjercicio = R.IdEjercicio
GROUP BY E.IdEjercicio, E.Titulo;
