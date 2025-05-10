--- Parte B: Funciones y procedimientos

--4. Listar empleados con su edad usando fn_CalcularEdad:
SELECT 
    EmpleadoID,
    Nombre,
    Apellido,
    dbo.fn_CalcularEdad(FechaNacimiento) AS Edad
FROM Empleados;


---5. Ejecutar sp_RegistrarPago para un empleado sin pago aún:
EXEC sp_RegistrarPago 
    @EmpleadoID = 1,
    @Mes = 4,
    @Anio = 2025,
    @MetodoPago = 'Transferencia';

	-- verificacion de que se realizo el SP
	SELECT Pg.*
FROM Pagos Pg
JOIN Planilla Pl ON Pg.PlanillaID = Pl.PlanillaID
WHERE Pl.EmpleadoID = 1 AND Pl.Mes = 4 AND Pl.Anio = 2025;

---6. Ver los registros de la tabla Logs:
SELECT * FROM Logs ORDER BY Fecha DESC;
