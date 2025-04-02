using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


{
    public class Empleado
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Cargo { get; set; }
        public string Departamento { get; set; }
        public int DiasDisponibles { get; set; }

        public Empleado(string id, string nombre, string cargo, string departamento, int diasDisponibles)
        {
            Id = id;
            Nombre = nombre;
            Cargo = cargo;
            Departamento = departamento;
            DiasDisponibles = diasDisponibles;
        }
    }
}
