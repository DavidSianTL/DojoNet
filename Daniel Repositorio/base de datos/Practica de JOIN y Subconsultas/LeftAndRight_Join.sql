---2. LEFT / RIGHT JOIN
---2.1 Listar todos los vuelos y, si existen, los alimentos disponibles en cada uno.
---(LEFT JOIN desde vuelos hacia alimentos)
Select V.VueloID, V.Origen, V.Destino, A.Nombre as 'Alimentos' from Vuelos V
left join VuelosAlimentos AL on V.VueloID = Al.VueloID
left join Alimentos A on AL.AlimentoID = A.AlimentoID


---2.2Listar todos los alimentos y en qué vuelos se han servido. Incluir alimentos que no se han servido en ningún vuelo.
Select A.Nombre As 'Alimento', A.Precio, V.VueloID, V.Origen, V.Destino from VuelosAlimentos AL
Right join Alimentos A on AL.AlimentoID = A.AlimentoID
left join Vuelos V on AL.VueloID = V.VueloID


---2.3. Mostrar todas las rutas posibles y los vuelos programados para cada una, incluso si no se ha usado la ruta aún.

Select R.RutaID, R.Origen As 'Ruta de origen', R.Destino as 'Ruta de destino', V.VueloID,V.Fecha, V.Hora
from Rutas R
Left join Vuelos V on R.Origen = V.Origen and R.Destino = V.Destino
----