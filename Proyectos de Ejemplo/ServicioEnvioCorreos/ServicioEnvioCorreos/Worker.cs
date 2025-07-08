using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using ServicioEnvioCorreos.DAO;
using ServicioEnvioCorreos.Models;

namespace ServicioEnvioCorreos
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly DAOCorreo _dao;
        private readonly CorreoConfiguracion _config;

        public Worker(ILogger<Worker> logger, DAOCorreo dao, CorreoConfiguracion config)
        {
            _logger = logger;
            _dao = dao;
            _config = config;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Procesando correos: {time}", DateTimeOffset.Now);

                try
                {
                    ProcesarCorreos();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error en procesamiento");
                }

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }

        private void ProcesarCorreos()
        {
            
            var correos = _dao.ObtenerCorreosPendientes();

            foreach (var correo in correos)
            {
                try
                {
                    MailMessage mail = new MailMessage();
                    mail.To.Add(correo.Destinatario);
                    if (!string.IsNullOrEmpty(correo.CC)) mail.CC.Add(correo.CC);
                    mail.Subject = correo.Asunto;
                    mail.Body = correo.Cuerpo;
                    mail.IsBodyHtml = true;

                    if (!string.IsNullOrEmpty(correo.RutaAdjunto) && File.Exists(correo.RutaAdjunto))
                    {
                        mail.Attachments.Add(new Attachment(correo.RutaAdjunto));
                    }

                    mail.From = new MailAddress(_config.Remitente);

                    SmtpClient smtp = new SmtpClient(_config.ServidorSMTP, _config.PuertoSMTP)
                    {
                        Credentials = new System.Net.NetworkCredential(_config.Remitente, _config.ClaveApp),
                        EnableSsl = _config.UsarSSL
                    };

                    smtp.Send(mail);

                    _dao.ActualizarEstado(correo.Id, "Enviado");
                }
                catch (Exception ex)
                {
                    _dao.ActualizarEstado(correo.Id, "Error");
                    File.AppendAllText(@"C:\EnvioCorreo\Logs\erroresCorreo.txt", $"{DateTime.Now}: Error con correo Id {correo.Id}: {ex.Message}\n");
                }
            }
        }
    }
}
