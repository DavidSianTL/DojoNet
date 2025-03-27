    using System;

    class Coche
    {

        private int velocidad = 80;

        public void MostrarVelocidad()
        {

            Console.WriteLine($"La velocidad del coche es {velocidad} KM/H ");

        }

        public void EstablecerVelocidad(int nuevaVelocidad)
        {
            velocidad = nuevaVelocidad;
        }

    }

    class Program
    {
        static void Main()
        {
            Coche miCoche = new Coche();
            miCoche.EstablecerVelocidad(120);

            miCoche.MostrarVelocidad();
        }
    }