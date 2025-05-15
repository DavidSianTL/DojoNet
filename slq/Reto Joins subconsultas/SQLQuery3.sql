--1.1
--Mostrar los nombres de los clientes y los vuelos que han tomado.
Select 
c.Nombre,
v.VueloID
From Clientes as c
inner join Boletos as b 
on c.ClienteID=b.ClienteID
inner join Asientos as a
on a.AsientoID=b.AsientoID
inner join Vuelos as v 
on v.VueloID=a.VueloID




--Listar el ID del vuelo, el nombre del piloto y la fecha del vuelo.

select 
v.vueloid,
p.nombre,
v.fecha
from Pilotos as p
inner join Vuelos as v 
on v.PilotoID=p.PilotoID


--Obtener los nombres de alimentos servidos en cada vuelo, junto con el origen y
--destino del vuelo.

Select 
a.Nombre,
v.origen,
v.destino
From Alimentos as a 
inner join VuelosAlimentos as va
on va.AlimentoID=a.AlimentoID
inner join Vuelos as v 
on v.VueloID=va.VueloID



--1.4
--Mostrar nombre del cliente, asiento y el destino del vuelo reservado.

Select 
c.Nombre,
b.asientoid,
v.destino
from clientes as c 
inner join boletos as b
on c.clienteid=b.clienteid
inner join asientos as a 
on b.asientoid=a.asientoid
inner join vuelos as v 
on v.VueloID=a.VueloID