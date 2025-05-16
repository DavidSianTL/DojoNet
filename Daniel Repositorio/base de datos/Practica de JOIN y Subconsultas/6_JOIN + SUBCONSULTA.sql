--6. 6: JOIN + SUBCONSULTAS

--6.1 Mostrar los vuelos con el precio promedio de asiento más alto (usa subconsulta con AVG y JOIN).
Select V.VueloID, V.Origen, V.Destino,
(select AVG(A.Precio) from Asientos A
Where A.VueloID  = V.VueloID) as 'Precio promedio' from Vuelos V
Order by (Select avg (A.Precio) from Asientos A
Where A.VueloID = V.VueloID) desc

--6.2 Mostrar el nombre del cliente que pagó el boleto más caro.
Select C.ClienteID, C.Nombre, A.Precio AS 'Precio Pagado' From Clientes C
Join Boletos B ON C.ClienteID = B.ClienteID
Join Asientos A ON B.AsientoID = A.AsientoID
Where A.Precio = (
    Select MAX(Precio)From Asientos
);
--6.3 Listar los vuelos con alimentos cuyo precio es mayor al promedio de todos los alimentos.
Select Distinct V.VueloID, V.Origen, V.Destino,A.Nombre, A.Precio From Vuelos V
Join VuelosAlimentos VAL on V.VueloID = VAL.VueloID
Join Alimentos A ON VAL.AlimentoID = A.AlimentoID
Where A.Precio > (
	select AVG (Precio) from Alimentos
);
