----4: CROSS JOIN

--4.1 Generar una lista de todas las posibles combinaciones de pilotos y aeromosas
Select P.PilotoID, P.Nombre AS 'Piloto nombre', A.AeromosaID, A.Nombre AS 'Aeromosa nombre'
From Pilotos P cross join Aeromosas A
ORDER BY P.PilotoID, A.AeromosaID;

---¿Cuántas combinaciones hay?
---R// pues como hay 3 pilotos: Miguel, Julia y Ernesto y 5 aoromosas: Laura, Paola, Verónica, Monica, Gabrieral
--- entonces 3 * 5 = 15 combinaciones posibles.

--4.2Simular emparejamientos posibles entre todos los alimentos y vuelos (sin importar disponibilidad real
Select V.VueloID, V.Origen, V.Destino, A.AlimentoID, A.Nombre AS 'Alimento',
CASE 
	When Exists (
		Select 1 
		From VuelosAlimentos VA 
               Where VA.VueloID = V.VueloID and VA.AlimentoID = A.AlimentoID
           ) Then 'Disponible' 
           else 'No disponible' 
       end AS 'Estado' From Vuelos V cross join Alimentos A
Order By V.VueloID, A.AlimentoID;
