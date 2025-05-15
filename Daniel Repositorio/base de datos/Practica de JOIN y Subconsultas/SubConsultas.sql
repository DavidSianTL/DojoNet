
--- sub consultas.
---1. Listar todos los vuelos con su origen, destino y fecha
Select VueloID, Origen, Destino, Fecha, Hora
From Vuelos

--2. Clientes que han comprado boletos para vuelos en una fecha específica 
Select C.ClienteID, C.Nombre, C.Correo From Clientes C
WHere C.ClienteID IN ( Select B.ClienteID
From Boletos B
Where B.AsientoID IN (
Select A.AsientoID From Asientos A
 WHere A.VueloID IN ( Select V.VueloID From Vuelos V
Where V.Fecha = '2025-05-15'
    )
  )
);


---3. Alimentos disponibles en vuelos desde Guatemala 
Select A.AlimentoID, A.Nombre, A.Precio From Alimentos A
Where A.AlimentoID IN (Select VA.AlimentoID From VuelosAlimentos VA
Where VA.VueloID IN (Select V.VueloID From Vuelos V
 Where V.Origen = 'Ciudad de Guatemala'
    )
);


---4. Vuelos con sus pilotos
Select V.VueloID, V.Origen, V.Destino, V.Fecha, V.Hora,
(Select P.Nombre From Pilotos P
Where P.PilotoID = V.PilotoID) AS 'Nombre Piloto' From Vuelos V
Order By V.Fecha, V.Hora;

---5. Clientes con boletos para vuelos a Guatemala 
Select C.ClienteID, C.Nombre From Clientes C
Where C.ClienteID IN (Select B.ClienteID From Boletos B
Where B.AsientoID IN (Select A.AsientoID From Asientos A
Where A.VueloID IN (Select V.VueloID  From Vuelos V
Where V.Destino = 'Ciudad de Guatemala'
   )
 )
);

---6. Vuelos que tienen alimentos servidos 
Select V.VueloID, V.Origen, V.Destino From Vuelos V
Where Exists (
Select 1 From VuelosAlimentos AL
    Where AL.VueloID = V.VueloID
);

--- 7. Pilotos que han volado más de 5 veces
Select P.PilotoID, P.Nombre, P.Licencia,
(Select Count(*) From Vuelos V
Where V.PilotoID = P.PilotoID) AS 'Total vuelos' From Pilotos P
Where (
Select Count(*) From Vuelos V
Where V.PilotoID = P.PilotoID
) > 5
ORDER BY (
Select Count(*) From Vuelos V
Where V.PilotoID = P.PilotoID
) Desc;


--8. Alimentos disponibles en todos los vuelos a San Salvador
--Originalmente no hay alimentos disponibles para el vuelo a San Salvador, realizamos un
--paso 1. insert para ingresar un nuevo producto, osea alimento.
Insert Into Alimentos (AlimentoID, Nombre, Precio)
VALUES (4, 'Café', 3.50);


--Paso 2, verificamos si se agrego
Select * From Alimentos Where AlimentoID = 4;


--paso 3. Relacionar el Café con los vuelos a San Salvador
Insert Into VuelosAlimentos (VueloID, AlimentoID)
Select V.VueloID, 4 From Vuelos V Where V.Destino = 'San Salvador';

--- Realizado la consulta.
Select A.AlimentoID, A.Nombre, A.Precio From Alimentos A
Where not exists ( -- Busca vuelos a San Salvador donde no exista este alimento
Select V.VueloID From Vuelos V
Where V.Destino = 'San Salvador'
And not exists (  -- SI no esta, sigifica que esta en todos
Select 1
From VuelosAlimentos VA Where VA.VueloID = V.VueloID And VA.AlimentoID = A.AlimentoID
    )
);