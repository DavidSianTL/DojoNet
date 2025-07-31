use DBProyectoGrupalDojoGeko
GO

INSERT INTO Empleados (
    TipoContrato, Pais, Departamento, Municipio, Direccion, Puesto,
    Codigo, DPI, Pasaporte, NombresEmpleado, ApellidosEmpleado,
    CorreoPersonal, CorreoInstitucional, FechaIngreso,
    FechaNacimiento, Telefono, NIT, Genero, Salario, FK_IdEstado
)
VALUES
('Indefinido', 'Guatemala', 'Guatemala', 'Guatemala', 'Zona 1', 'Analista',
 'EMP001', '1234567890101', 'P123456', 'Carlos', 'Ramírez',
 'carlos@gmail.com', 'c.ramirez@empresa.com', '2020-01-15',
 '1990-06-10', '5555-1234', '1234567-8', 'Masculino', 5000.00, 1),

('Temporal', 'Guatemala', 'Sacatepéquez', 'Antigua', 'Calle del Arco', 'Soporte',
 'EMP002', '9876543210101', 'P654321', 'Ana', 'Morales',
 'ana@gmail.com', 'a.morales@empresa.com', '2022-06-01',
 '1992-03-25', '5555-5678', '7654321-9', 'Femenino', 4200.00, 1),

('Indefinido', 'Guatemala', 'Quetzaltenango', 'Xela', 'Zona 3', 'Desarrollador',
 'EMP003', '1928374650101', 'P789123', 'Luis', 'Gómez',
 'luis@gmail.com', 'l.gomez@empresa.com', '2024-02-10',
 '1988-12-20', '5555-8765', '0192837-4', 'Masculino', 6000.00, 1);


 INSERT INTO SolicitudEncabezado (
    FK_IdEmpleado, DiasSolicitadosTotal, FechaIngresoSolicitud,
    FK_IdEstadoSolicitud, FK_IdAutorizador, FechaAutorizacion
)
VALUES
(1, 5, '2023-07-01', 2, 1, '2023-07-02'),  -- Aprobada
(1, 3, '2024-02-15', 2, 1, '2024-02-16'),  -- Aprobada
(1, 4, '2025-07-01', 1, NULL, NULL);       -- Pendiente

-- Ana (IdEmpleado = 2)
INSERT INTO SolicitudEncabezado (
    FK_IdEmpleado, DiasSolicitadosTotal, FechaIngresoSolicitud,
    FK_IdEstadoSolicitud, FK_IdAutorizador, FechaAutorizacion
)
VALUES
(2, 7, '2023-12-10', 2, 1, '2023-12-12'), -- Aprobada
(2, 5, '2025-06-01', 1, NULL, NULL);      -- Pendiente

-- Luis (IdEmpleado = 3)
INSERT INTO SolicitudEncabezado (
    FK_IdEmpleado, DiasSolicitadosTotal, FechaIngresoSolicitud,
    FK_IdEstadoSolicitud, FK_IdAutorizador, FechaAutorizacion
)
VALUES
(3, 2, '2025-07-10', 2, 1, '2025-07-11');  -- Aprobada
