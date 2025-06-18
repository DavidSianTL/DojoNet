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
        var html = @"
<div style='
    max-width: 600px;
    margin: 40px auto;
    font-family: ""Segoe UI"", Tahoma, Geneva, Verdana, sans-serif;
    border-radius: 10px;
    overflow: hidden;
    box-shadow: 0 0 15px rgba(0,0,0,0.1);
    background: linear-gradient(to right, #f8f9fa, #ffffff);
    color: #333;
'>
    <div style='
        background-color: #007bff;
        color: #fff;
        padding: 25px 30px;
        text-align: center;
    '>
        <h1 style='margin: 0; font-size: 24px;'>¡Bienvenido a Dojo .NET 2025!</h1>
        <p style='margin: 5px 0 0;'>Tu plataforma de aprendizaje ha sido activada</p>
    </div>

    <div style='padding: 30px;'>
        <p style='font-size: 16px;'>Hola,</p>
        <p style='font-size: 16px; line-height: 1.6;'>
            Hemos generado una <strong>contraseña temporal</strong> para que puedas iniciar sesión en el sistema. Asegúrate de cambiarla lo antes posible por seguridad.
        </p>

        <div style='
            font-size: 22px;
            background-color: #e9ecef;
            border: 1px dashed #6c757d;
            text-align: center;
            padding: 12px 20px;
            border-radius: 6px;
            letter-spacing: 1px;
            margin: 20px 0;
            font-weight: bold;
            color: #212529;
        '>
            " + contrasenia + @"
        </div>

        <p style='font-size: 16px;'>
            Para cambiar tu contraseña, haz clic en el siguiente botón:
        </p>

        <div style='text-align: center; margin: 30px 0;'>
            <a href='" + urlCambioPassword + @"' style='
                background-color: #28a745;
                color: white;
                padding: 14px 28px;
                border-radius: 6px;
                text-decoration: none;
                font-size: 16px;
                font-weight: bold;
                box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
            '>Cambiar Contraseña</a>
        </div>

        <p style='font-size: 14px; color: #6c757d;'>
            Si tú no solicitaste este acceso o consideras que fue un error, simplemente ignora este mensaje.
        </p>

        <hr style='margin: 40px 0; border: none; border-top: 1px solid #dee2e6;' />

        <p style='text-align: center; font-size: 13px; color: #adb5bd;'>
            © 2025 Dojo .NET | Todos los derechos reservados.
        </p>
    </div>
</div>";



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
