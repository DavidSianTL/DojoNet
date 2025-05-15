SELECT c.Nombre, b.BoletoID
FROM Clientes c
INNER JOIN Boletos b ON c.ClienteID = b.ClienteID;



SELECT c.Nombre, b.BoletoID
FROM Clientes c
LEFT JOIN Boletos b ON c.ClienteID = b.ClienteID;



select * from Clientes
Select * from Boletos




SELECT c.Nombre, a.Nombre AS Alimento
FROM Clientes c
CROSS JOIN Alimentos a;
