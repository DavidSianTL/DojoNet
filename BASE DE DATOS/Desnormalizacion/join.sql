SELECT 
    p.IDPlanilla,
    e.NombreEmpleado,
    pu.NombrePuesto AS Puesto,
    j.TipoJornada AS Jornada,
    pu.SalarioBase,
    p.FechaPago,
    p.HorasExtras,
    tp.Descripcion AS TipoPago,
    p.Descuento,
    p.PagoNeto
FROM Planilla p
JOIN Empleado e ON p.IDEmpleado = e.IDEmpleado
JOIN Puesto pu ON p.IDPuesto = pu.IDPuesto
JOIN Jornada j ON p.IDJornada = j.IDJornada
JOIN TipoPago tp ON p.IDTipoPago = tp.IDTipoPago;


CREATE VIEW VistaPlanillaCompleta AS
SELECT 
    p.IDPlanilla,
    e.NombreEmpleado,
    pu.NombrePuesto AS Puesto,
    j.TipoJornada AS Jornada,
    pu.SalarioBase,
    p.FechaPago,
    p.HorasExtras,
    tp.Descripcion AS TipoPago,
    p.Descuento,
    p.PagoNeto
FROM Planilla p
JOIN Empleado e ON p.IDEmpleado = e.IDEmpleado
JOIN Puesto pu ON p.IDPuesto = pu.IDPuesto
JOIN Jornada j ON p.IDJornada = j.IDJornada
JOIN TipoPago tp ON p.IDTipoPago = tp.IDTipoPago;

