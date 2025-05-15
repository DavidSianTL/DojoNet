----3: FULL OUTER JOIN

---3.1 Listar todos los vuelos y todas las rutas combinadas
Select R.RutaID, R.Origen As 'Origen Ruta', R.Destino As 'Destino Ruta', V.VueloID,
V.Origen As 'Origen vuelo', V.Destino as 'Destino vuelo', V.Fecha From Rutas R
Full outer join Vuelos V ON R.Origen = V.Origen and R.Destino = V.Destino

---(¿Qué vuelos no tienen ruta? ¿Qué rutas no se han usado en vuelos?)
--- Los vuelos sin ruta serian el Vuelo ID: 104,105, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210.