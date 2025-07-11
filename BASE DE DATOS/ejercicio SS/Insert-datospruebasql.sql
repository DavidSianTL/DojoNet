USE [SistemaSeguridad]
GO

SET IDENTITY_INSERT [dbo].[CorreosPendientes] OFF

-- Insertar 20 registros de prueba
INSERT INTO [dbo].[CorreosPendientes] (
    [Destinatario], [CC], [Asunto], [Cuerpo], [RutaAdjunto], [Estado], [FechaCreacion]
)
VALUES 
('juan.perez@example.com', 'maria.lopez@example.com', 'Revisión mensual', 'Favor revisar el informe adjunto.', 'C:\Adjuntos\Informe1.pdf', 'Pendiente', GETDATE()),
('ana.garcia@example.com', NULL, 'Recordatorio de reunión', 'La reunión será a las 10:00 AM.', NULL, 'Pendiente', GETDATE()),
('carlos.mendez@example.com', 'laura.gomez@example.com', 'Reporte financiero', 'Adjunto se encuentra el reporte.', 'C:\Adjuntos\ReporteFinanciero.xlsx', 'Enviado', GETDATE()),
('sofia.ortiz@example.com', 'jose.ramirez@example.com', 'Agenda semanal', 'Agenda de la semana adjunta.', 'C:\Adjuntos\Agenda.pdf', 'Pendiente', GETDATE()),
('luis.alvarez@example.com', NULL, 'Solicitud de soporte', 'Necesito ayuda con el sistema.', NULL, 'Pendiente', GETDATE()),
('patricia.navarro@example.com', 'andres.hernandez@example.com', 'Cambio de contraseña', 'Tu contraseña ha sido cambiada.', NULL, 'Enviado', GETDATE()),
('roberto.sanchez@example.com', NULL, 'Notificación de pago', 'Tu pago ha sido recibido.', NULL, 'Enviado', GETDATE()),
('veronica.mora@example.com', 'isabel.cruz@example.com', 'Confirmación de cita', 'Confirmamos tu cita médica.', NULL, 'Pendiente', GETDATE()),
('miguel.castillo@example.com', NULL, 'Entrega de documentos', 'Documentos enviados en adjunto.', 'C:\Adjuntos\Documentos.zip', 'Pendiente', GETDATE()),
('fernanda.diaz@example.com', NULL, 'Aprobación requerida', 'Favor revisar y aprobar.', NULL, 'Pendiente', GETDATE()),
('david.reyes@example.com', 'paola.salazar@example.com', 'Resumen de actividades', 'Aquí está el resumen semanal.', NULL, 'Pendiente', GETDATE()),
('nancy.romero@example.com', NULL, 'Tarea pendiente', 'Tienes una tarea sin completar.', NULL, 'Pendiente', GETDATE()),
('ricardo.fuentes@example.com', NULL, 'Factura adjunta', 'Se adjunta la factura del mes.', 'C:\Adjuntos\Factura.pdf', 'Enviado', GETDATE()),
('yesenia.luna@example.com', 'julio.martinez@example.com', 'Reporte de asistencia', 'Se encuentra el reporte adjunto.', 'C:\Adjuntos\Asistencia.csv', 'Pendiente', GETDATE()),
('hector.molina@example.com', NULL, 'Reunión de equipo', 'La reunión será el lunes.', NULL, 'Pendiente', GETDATE()),
('claudia.galvez@example.com', 'karla.vargas@example.com', 'Actualización de sistema', 'Se ha actualizado el sistema.', NULL, 'Enviado', GETDATE()),
('jorge.flores@example.com', NULL, 'Confirmación de registro', 'Gracias por registrarte.', NULL, 'Enviado', GETDATE()),
('melissa.campos@example.com', NULL, 'Boletín mensual', 'Aquí está el boletín.', 'C:\Adjuntos\Boletin.pdf', 'Pendiente', GETDATE()),
('daniel.guerrero@example.com', 'rocio.perez@example.com', 'Solicitud aprobada', 'Tu solicitud fue aprobada.', NULL, 'Enviado', GETDATE()),
('monica.espinoza@example.com', NULL, 'Feliz cumpleaños', '¡Feliz cumpleaños de parte del equipo!', NULL, 'Pendiente', GETDATE());