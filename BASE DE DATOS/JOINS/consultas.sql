USE BancoDB;
GO
--INNER JOIN

SELECT c.Nombre, c.Email, cu.TipoCuenta, cu.Saldo
FROM Clientes c
INNER JOIN Cuentas cu ON c.ClienteID = cu.ClienteID;

--LEFT JOIN
SELECT c.Nombre, cu.TipoCuenta, cu.Saldo
FROM Clientes c
LEFT JOIN Cuentas cu ON c.ClienteID = cu.ClienteID;

--RIGTH JOIN
SELECT cu.TipoCuenta, c.Nombre
FROM Cuentas cu
RIGHT JOIN Clientes c ON cu.ClienteID = c.ClienteID;
´´
--FULL OUTER JOIN
SELECT c.Nombre, cu.TipoCuenta
FROM Clientes c
FULL OUTER JOIN Cuentas cu ON c.ClienteID = cu.ClienteID;

--CROSS JOIN
SELECT c.Nombre, cu.TipoCuenta
FROM Clientes c
CROSS JOIN Cuentas cu;




