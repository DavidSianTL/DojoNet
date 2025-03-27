using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepasoC_
{
    internal class Program
    {
        static void Main(string[] args)
        {
            login();
        }

        static void login()
        {
            string user = "jose";
            string password = "jose12";

            Console.WriteLine("Ingresa tu usuario: ");
            string pruebaUser = Console.ReadLine();
            Console.WriteLine("Ingresa tu contraseña: ");
            string pruebaPassword = Console.ReadLine();

            if (pruebaUser == user && pruebaPassword == password)
            {
                Console.WriteLine("Bienvenido");
            }
            else
            {
                Console.WriteLine("Usuario o contraseña incorrectos");
            }


        }

        static void menu()
        {

            /* Crea variables para almacenar:
             
                Tu nombre

                Tu edad 

            */
            Console.WriteLine("Ingresa tu nombre: ");
            string nombre = Console.ReadLine();

            Console.WriteLine("Ingresa tu edad: ");
            int edad = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Hola, " + nombre + " tu edad es de " + edad + " años");


            Console.ReadKey();
            Console.Clear();

            /* Declara tres variables:

                Un número entero

                Un texto

                Un valor booleano 
            */
            int numero = 7;
            string hola = "mundo";
            bool chapin = true;

            Console.WriteLine("int numero = " + numero + ", string hola = " + hola + ", bool chapin: " + chapin);

            Console.ReadKey();
            Console.Clear();

            /* Calcula el área de un rectángulo con dos variables: base y altura. */
            Console.WriteLine("Cálcular el área de un rectángulo");
            Console.WriteLine("Ingresa la base: ");
            double baseRectangulo = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("Ingresa la altura: ");
            double alturaRectangulo = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine("El área del rectángulo es: " + (baseRectangulo * alturaRectangulo));


            Console.ReadKey();
            Console.Clear();

            /* Crea un if que verifique si un número es par o impar. */
            Console.WriteLine("Verificar si un número es par o impar");
            Console.WriteLine("Ingresa un número: ");
            int numeroParImpar = Convert.ToInt32(Console.ReadLine());

            if (numeroParImpar % 2 == 0)
            {
                Console.WriteLine("El número " + numeroParImpar + " es par");
            }
            else
            {
                Console.WriteLine("El número " + numeroParImpar + " es impar");
            }


            Console.ReadKey();
            Console.Clear();

            /* Crea un método que reciba tu nombre y lo imprima en pantalla. */
            Console.WriteLine("Método que recibe tu nombre y lo imprime en pantalla");
            miNombre();

            Console.ReadKey();
            Console.Clear();

            /* Pide al usuario su edad y dile cuántos años tendrá en 10 años. */
            Console.WriteLine("Ingresa tu edad actual: ");
            int edadActual = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Tu edad actual es: " + edadActual + " y dentro de 10 años, tendrás: " + (edadActual+10));

            Console.ReadKey();
            Console.Clear();

            /* Crea una clase Persona con laspropiedades nombre y edad. Luego crea un objeto e imprime sus valores. */
            Console.WriteLine("Clase Persona con propiedades nombre y edad");
            Persona persona = new Persona();
            persona.nombre = "José Peralta";
            persona.edad = 20;

            persona.mostrarDatos();

            Console.ReadKey();
            Console.Clear();

            /* Crea una variable dynamic, almacena un número y luego cámbiala a un texto. */
            dynamic variable = 7;
            Console.WriteLine("Variable dynamic: " + variable);

            variable = "Hola mundo";
            Console.WriteLine("Variable dynamic: " + variable);

            Console.ReadKey();

            /* Usa object para almacenar un número decimal y conviértelo a double antes de imprimirlo. */
            object objeto = 10.50M;
            Console.WriteLine("Variable object: " + objeto);

            double objetoDouble = Convert.ToDouble(objeto);
            Console.WriteLine("Variable object ahora es : " + objetoDouble);

            Console.ReadKey();
            Console.Clear();


        }

        /* Crea un método que reciba tu nombre y lo imprima en pantalla. */
        static void miNombre()
        {
            Console.WriteLine("Mi nombre es: José Peralta");
        }

        /* Crea una clase Persona con laspropiedades nombre y edad. Luego crea un objeto e imprime sus valores. */
        class Persona
        {
            public string nombre;
            public int edad;

            public void mostrarDatos()
            {
                Console.WriteLine("Nombre: " + nombre);
                Console.WriteLine("Edad: " + edad);
            }

        }

        /* Crea una clase Coche con una propiedad velocidad privada y un método público para establecer su valor. */
        class Coche
        {
            private double velocidad;

            public void acelerar()
            {
                velocidad += 10;
            }

        }

        /* Un perro hereda características de un animal. */
        class Animal
        {
            public void HacerSonido()
            {
                Console.WriteLine("Hace un sonido");
            }

        }

        class Perro : Animal
        {
            public void ladrar()
            {
                Console.WriteLine("¡Guau guau!");
            }
        }


        /* Crea una clase Figura con un método CalcularArea(), y
            dos clases Cuadrado y Círculo que lo implementen. */

        class Figura
        {
            public virtual void CalcularArea()
            {
                Console.WriteLine("Calculando área");
            }
        }

        class Cuadrado : Figura
        {
            public override void CalcularArea()
            {
                Console.WriteLine("Calculando área de un cuadrado");
            }
        }

        class Circulo : Figura
        {
            public override void CalcularArea()
            {
                Console.WriteLine("Calculando área de un círculo");
            }
        }

        /*  Crea una clase abstracta llamada Figura con un
            método abstracto CalcularArea(), y luego crea dos clases
            que la hereden:

            Cuadrado (con atributo lado).
            Círculo (con atributo radio).

           Cada una debe implementar el
           método CalcularArea() según su fórmula matemática.
        */

        abstract class FiguraAbstracta
        {
            public abstract void CalcularArea();
        }

        class CuadradoAbstracto : FiguraAbstracta
        {
            public double lado;
            public override void CalcularArea()
            {
                Console.WriteLine("Calculando área de un cuadrado");
            }
        }

        class CirculoAbstracto : FiguraAbstracta
        {
            public double radio;
            public override void CalcularArea()
            {
                Console.WriteLine("Calculando área de un círculo");
            }
        }



    }
}
