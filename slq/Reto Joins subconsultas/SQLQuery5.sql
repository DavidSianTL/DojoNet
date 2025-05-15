--SECCIÓN
--3: FULL OUTER JOIN


SELECT v.VueloID, v.Origen AS VueloOrigen, v.Destino AS VueloDestino, 
       r.RutaID, r.Origen AS RutaOrigen, r.Destino AS RutaDestino
FROM Vuelos v
FULL OUTER JOIN Rutas r 
ON v.Origen = r.Origen AND v.Destino = r.Destino;
