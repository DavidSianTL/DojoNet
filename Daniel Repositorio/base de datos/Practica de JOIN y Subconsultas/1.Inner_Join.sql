---1: INNER JOIN (Fundamentales)

--1.1 Mostrar los nombres de los clientes y los vuelos que han tomado.
Select C.Nombre As 'Nombre cliente', V.VueloID, V.Origen, V.Destino, V.Fecha From Clientes C
Inner join Boletos B on C.ClienteID = B.ClienteID
inner join Asientos A on B.AsientoID = A.AsientoID
Inner join Vuelos V on A.VueloID = V.VueloID


--- 1.2 Listar el ID del vuelo, el nombre del piloto y la fecha del vuelo
select V.vueloID, P.Nombre as 'Piloto', V.Fecha From Vuelos V
Inner join Pilotos P on V.PilotoID = P.PilotoID


-- 1.3 Obtener los nombres de alimentos servidos en cada vuelo, junto con el origen y destino
Select V.VueloID, V.Origen, V.Destino, A.Nombre as 'Alimento vuelo' from Vuelos V
Inner join VuelosAlimentos AL On V.VueloID = AL.VueloID
Inner join Alimentos A on AL.AlimentoID = A.AlimentoID

---1.4 Mostrar nombre del cliente, asiento y el destino del vuelo reservado
Select C.Nombre as 'Cliente', A.AsientoID, V.Destino, V.Fecha, B.FechaCompra from Clientes C
Inner join Boletos B on C.ClienteID = B.ClienteID
Inner join Asientos A on B.AsientoID = A.AsientoID
inner join Vuelos V on A.VueloID = V.VueloID 


