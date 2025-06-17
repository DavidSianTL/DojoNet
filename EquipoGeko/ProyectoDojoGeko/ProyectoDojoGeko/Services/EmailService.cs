using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Options;
using ProyectoDojoGeko.Models;

public class EmailService
{
    // Inyectamos las opciones de configuración de EmailSettings
    private readonly EmailSettings _settings;

    // Constructor que recibe las opciones de configuración de EmailSettings
    public EmailService(IOptions<EmailSettings> options)
    {
        _settings = options.Value;
    }

    // Creamos la función asíncrona para enviar el correo electrónico utilizando MailKit
    public async Task EnviarCorreoConMailjetAsync(string destino, string contrasenia, string urlCambioPassword)
    {
        // Validar que el destino no sea nulo o vacío
        var mensaje = new MimeMessage();
        mensaje.From.Add(new MailboxAddress(_settings.FromName, _settings.FromEmail));
        mensaje.To.Add(MailboxAddress.Parse(destino));
        mensaje.Subject = "Bienvenido - Cambia tu contraseña";

        // Creamos el cuerpo del mensaje en HTML
        var html = $@"
            <p>Hola,</p>
            <p>Tu contraseña temporal es:</p>
            <p><strong>{contrasenia}</strong></p>
            <p>Haz clic para cambiarla:</p>
            <a href='{urlCambioPassword}' style='
                display:inline-block;
                padding:10px 15px;
                background-color:#007bff;
                color:#fff;
                text-decoration:none;
                border-radius:5px;'>Cambiar contraseña</a>
            <p>Saludos,<br/>Equipo Dojo .NET 2025</p>";

        var builder = new BodyBuilder { HtmlBody = html };
        mensaje.Body = builder.ToMessageBody();

        // Configurar el cliente SMTP de Mailjet
        using var smtp = new SmtpClient();

        // Conectar al servidor SMTP de Mailjet
        await smtp.ConnectAsync("in-v3.mailjet.com", 587, SecureSocketOptions.StartTls);

        // Autenticarse con las credenciales de Mailjet
        await smtp.AuthenticateAsync(_settings.ApiKey, _settings.ApiSecret);

        // Enviar el mensaje
        await smtp.SendAsync(mensaje);

        // Desconectar del servidor SMTP
        await smtp.DisconnectAsync(true);
    }
}
