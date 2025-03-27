using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacacionesDeEmpleados
{
    internal class Program
    {
        static void Main(string[] args)
        {
            menu();
        }

        static void menu()
        {

        }

        public class TipoEmpleado
        {
            private int idTipo;
            private string tipo;

            public TipoEmpleado() { }

            public TipoEmpleado(int id, string tipo)
            {
                this.idTipo = id;
                this.tipo = tipo;
            }

            public void setIdTipo(int id)
            {
                this.idTipo = id;
            }

            public int getIdTipo()
            {
                return idTipo;
            }

            public void setTipo(string tipo)
            {
                this.tipo = tipo;
            }

            public string getTipo()
            {
                return tipo;
            }


        }

        public class Empleado : TipoEmpleado
        {
            private int idEmpleado;
            private string nombreEmpleado;

            public Empleado() { }

            public Empleado(int id, string nombre, int idTipo, string tipo) : base(idTipo, tipo)
            {
                this.idEmpleado = id;
                this.nombreEmpleado = nombre;
            }

            public void setIdEmpleado(int id)
            {
                this.idEmpleado = id;
            }

            public int getIdEmpleado()
            {
                return idEmpleado;
            }

            public void setNombreEmpleado(string nombre)
            {
                this.nombreEmpleado = nombre;
            }

            public string getNombreEmpleado()
            {
                return nombreEmpleado;
            }
        }

        public class Vacaciones : Empleado
        {
            private int idVacaciones;
            private int diasVacaciones;
            private DateTime fechaInicio;
            private DateTime fechaFin;
            public Vacaciones() { }
            public Vacaciones(int id, int dias, DateTime inicio, DateTime fin, int idEmpleado, string nombreEmpleado, int idTipo, string tipo) : base(idEmpleado, nombreEmpleado, idTipo, tipo)
            {
                this.idVacaciones = id;
                this.diasVacaciones = dias;
                this.fechaInicio = inicio;
                this.fechaFin = fin;
            }
            public void setIdVacaciones(int id)
            {
                this.idVacaciones = id;
            }
            public int getIdVacaciones()
            {
                return idVacaciones;
            }
            public void setDiasVacaciones(int dias)
            {
                this.diasVacaciones = dias;
            }
            public int getDiasVacaciones()
            {
                return diasVacaciones;
            }
            public void setFechaInicio(DateTime inicio)
            {
                this.fechaInicio = inicio;
            }
            public DateTime getFechaInicio()
            {
                return fechaInicio;
            }
            public void setFechaFin(DateTime fin)
            {
                this.fechaFin = fin;
            }
            public DateTime getFechaFin()
            {
                return fechaFin;
            }
        }


    }
}
