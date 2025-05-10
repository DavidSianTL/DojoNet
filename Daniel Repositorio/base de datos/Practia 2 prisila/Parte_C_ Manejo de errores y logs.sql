---Parte C: Manejo de errores y logs 
--7.	Ejecuta sp_RegistrarPago con un mes o año sin planilla registrada (para disparar el error).
EXEC sp_RegistrarPago 
    @EmpleadoID = 1,
    @Mes = 12,
    @Anio = 2025,
    @MetodoPago = 'Efectivo';


--8.	Verifica que el error se registró correctamente en la tabla Logs.
SELECT * FROM Logs 
WHERE Error = 1 
ORDER BY Fecha DESC;

