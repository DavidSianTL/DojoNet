--2: LEFT / RIGHT JOIN


--2.1
--Listar todos los vuelos y, si existen, los alimentos disponibles en cada uno.

--(LEFT JOIN desde vuelos hacia alimentos)

Select 
v.VueloId,
a.Nombre
From Alimentos as a 
left join VuelosAlimentos as va 
on va.AlimentoID=a.AlimentoID
left join Vuelos as v 
on v.VueloID=va.VueloID


--2.2
--Listar todos los alimentos y en qué vuelos se han servido. Incluir alimentos
--que no se han servido en ningún vuelo.
SELECT a.Nombre, v.VueloID
FROM Alimentos a
LEFT JOIN VuelosAlimentos va ON a.AlimentoID = va.AlimentoID
LEFT JOIN Vuelos v ON va.VueloID = v.VueloID;



--2.3
--Mostrar todas las rutas posibles y los vuelos programados para cada una,
--incluso si no se ha usado la ruta aún.
SELECT r.Origen, r.Destino, v.VueloID, v.Fecha
FROM Rutas r
LEFT JOIN Vuelos v ON r.Origen = v.Origen AND r.Destino = v.Destino;
