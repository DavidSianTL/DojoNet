----5: JOIN + FILTRADO / AGRUPACIÓN
----5.1 ¿Qué piloto ha realizado más vuelos? Usa GROUP BY con JOIN
Select P.PilotoID, P.Nombre, Count(V.VueloID) AS 'Total Vuelos' From Pilotos P
Join Vuelos V ON P.PilotoID = V.PilotoID
Group by P.PilotoID, P.Nombre, P.Licencia
Order by Count(V.VueloID) Desc

---5.2 Mostrar los vuelos que ofrecen más de 2 alimentos distintos.
Select VA.VueloID, Count(Distinct VA.AlimentoID) AS 'Cantidad de alimentos' From VuelosAlimentos va
Group By VA.VueloID Having Count(Distinct VA.AlimentoID) > 2;

---5.3 Mostrar clientes que han reservado boletos en vuelos a Tegucigalpa.
Select C.ClienteID, C.Nombre from Clientes C
Join Boletos B on C.ClienteID = B.ClienteID
Join Asientos A on B.AsientoID = A.AsientoID
Join Vuelos V on  A.VueloID = V.VueloID where V.Destino = 'Tegucigalpa'

