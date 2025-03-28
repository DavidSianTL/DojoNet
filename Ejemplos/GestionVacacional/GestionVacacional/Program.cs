using System;
using System.Collections.Generic;

namespace GestionVacacional
{
    public class Empleado
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Puesto { get; set; }
        public int DiasVacacionesDisponibles { get; set; } = 15; // Asumimos 15 días por defecto

        public Empleado(int id, string nombre, string puesto)
        {
            Id = id;
            Nombre = nombre;
            Puesto = puesto;
        }
    }

    public class SolicitudVacaciones
    {
        public Empleado Empleado { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int DiasSolicitados { get; set; }
        public string Motivo { get; set; }
        public bool Aprobada { get; set; } // Estado de la solicitud {aprobado,rechazado}

        public SolicitudVacaciones(Empleado empleado, DateTime fechaInicio, DateTime fechaFin, string motivo)
        {
            
                Empleado = empleado;
                FechaInicio = fechaInicio;
                FechaFin = fechaFin;
                Motivo = motivo;
                DiasSolicitados = (FechaFin - FechaInicio).Days + 1;
           
            
        }
    }


    public class GestionVacacionesApp
    {
        private List<Empleado> empleados = new List<Empleado>();
        private List<SolicitudVacaciones> solicitudes = new List<SolicitudVacaciones>();

        public void RegistrarEmpleado(int id, string nombre, string puesto)
        {
            empleados.Add(new Empleado(id, nombre, puesto));
            Console.WriteLine($"Empleado {nombre} registrado con éxito.");
        }

        public void SolicitarVacaciones(int idEmpleado, DateTime inicio, DateTime fin, string motivo)
        {
            var empleado = empleados.FirstOrDefault(e => e.Id == idEmpleado);
            if (empleado != null)
            {
                var solicitud = new SolicitudVacaciones(empleado, inicio, fin, motivo);

                if (empleado.DiasVacacionesDisponibles >= solicitud.DiasSolicitados)
                {
                    solicitudes.Add(solicitud);
                    empleado.DiasVacacionesDisponibles -= solicitud.DiasSolicitados;
                    Console.WriteLine("Solicitud de vacaciones registrada.");
                }
                else
                {
                    Console.WriteLine("No tiene suficientes días de vacaciones.");
                }
            }
            else
            {
                Console.WriteLine("Empleado no encontrado.");
            }
        }

        public void AprobarRechazarSolicitud(int idSolicitud, bool aprobar)
        {
            var solicitud = solicitudes.ElementAtOrDefault(idSolicitud);
            if (solicitud != null)
            {
                solicitud.Aprobada = aprobar;
                string estado = aprobar ? "aprobada" : "rechazada";
                Console.WriteLine($"Solicitud {estado}.");
            }
            else
            {
                Console.WriteLine("Solicitud no encontrada.");
            }
        }

        public void VerSolicitudes()
        {
            Console.WriteLine("Solicitudes de Vacaciones:");
            foreach (var solicitud in solicitudes)
            {
                string estado = solicitud.Aprobada ? "Aprobada" : "Pendiente";
                Console.WriteLine($"{solicitud.Empleado.Nombre} - {solicitud.FechaInicio.ToShortDateString()} a {solicitud.FechaFin.ToShortDateString()} - {estado}");
            }
        }
    }




    class Program
    {
        static void Main(string[] args)
        {
            var app = new GestionVacacionesApp();

            // Menú
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Gestión de Vacaciones");
                Console.WriteLine("1. Registrar empleado");
                Console.WriteLine("2. Solicitar vacaciones");
                Console.WriteLine("3. Aprobar/Rechazar solicitud");
                Console.WriteLine("4. Ver solicitudes");
                Console.WriteLine("5. Salir");
                Console.Write("Seleccione una opción: ");

                var opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        Console.Write("ID del empleado: ");
                        int id = int.Parse(Console.ReadLine());
                        Console.Write("Nombre del empleado: ");
                        string nombre = Console.ReadLine();
                        Console.Write("Puesto del empleado: ");
                        string puesto = Console.ReadLine();
                        app.RegistrarEmpleado(id, nombre, puesto);
                        break;

                    case "2":
                        Console.Write("ID del empleado: ");
                        id = int.Parse(Console.ReadLine());
                        Console.Write("Fecha de inicio (yyyy-mm-dd): ");
                        DateTime inicio = DateTime.Parse(Console.ReadLine());
                        Console.Write("Fecha de fin (yyyy-mm-dd): ");
                        DateTime fin = DateTime.Parse(Console.ReadLine());
                        Console.Write("Motivo: ");
                        string motivo = Console.ReadLine();
                        app.SolicitarVacaciones(id, inicio, fin, motivo);
                        break;

                    case "3":
                        Console.Write("ID de la solicitud a aprobar/rechazar: ");
                        int solicitudId = int.Parse(Console.ReadLine());
                        Console.Write("¿Aprobar (true/false)? ");
                        bool aprobar = bool.Parse(Console.ReadLine());
                        app.AprobarRechazarSolicitud(solicitudId, aprobar);
                        break;

                    case "4":
                        app.VerSolicitudes();
                        break;

                    case "5":
                        return;

                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }

                Console.WriteLine("Presione cualquier tecla para continuar...");
                Console.ReadKey();
            }
        }
    }


}
