using System.Collections.Generic;

namespace ProyectoPaises.Models{

    public class Pais{
        public Name Name {get; set; }
        public Dictionary<string, string> Languages {get; set;}
        public Flags Flags {get; set;}

    }

    public class Name
    {
    public string Common {get; set;}
    }

    public class Flags
    {
    public string Png {get; set;}
    }

    
}
