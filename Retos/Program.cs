using System;
using System.Dynamic;
using System.Formats.Asn1;
using System.Net.WebSockets;
using System.Security.Cryptography;

public class Program{

      //Funciones 

    public static void imprimirNombre(){
         string? name;
        do{
            Console.Write("Bienvenido usuario, ingresa tu nombre: ");
       name=Console.ReadLine();

        }while(!(name!=null)||name=="");
         Console.WriteLine($"tu nombre es: {name}");

    }

//poo
    public class Persona{
        string nombre="juano";

        int edad=18;

        public string getNombre(){
            return nombre;
        }
        public int getEdad(){
            return edad;
        }
    }


//encapsu
        
        public class Coche{
            private double velocidad;
            
            public void setVelocidad(double nuevaVelocidad){
                velocidad = nuevaVelocidad;
                Console.WriteLine($"La velocida ahora es: {velocidad}");

            }

            }

        
       

//herencia
        public class Animal{
            
            public virtual void hacerSonido(){
                Console.WriteLine("Este animal está haciendo un sonido ");
            }
        }

        public class Perro : Animal{
            public override void hacerSonido(){
                Console.WriteLine("EL perro ladra");
            }
        }
       
    //Crear una clase forma, luegoo  heredar una clase circulo y otra cuadrado y usar override para el polimorfismo con el metodo calcular area heredada de forma

        public class Figura{
            public virtual void calcularArea(double a, double b){
                Console.WriteLine("Calcualndo Area...");
            }
        }

        public class Cuadrado : Figura{
            public override void calcularArea(double b, double a){
                Console.WriteLine($"el area del cuadrado es: {a*b}");
            }
        }

        public class Circulo : Figura{
        public override void calcularArea(double diametro, double pi =3.14){
            double r = diametro/2;
            Console.WriteLine($"el area del circulo es: {pi*(r*r)}");

        }
    }


        // --ABSTRACCIÓN
        public abstract class Figura2{
            public abstract void calcularArea2();
        }

        public class Cuadrado2 : Figura2{
            private float lado_a;
            private float lado_b;
            public override void calcularArea2(){
                Console.WriteLine($"el area del cuadrado es: {lado_a*lado_b}");
            }
        }
        
        public class Circulo2 : Figura2{
            private float diametro;
        public override void calcularArea2(){
            float pi = 3.14f;
            float r= diametro/2;
            Console.WriteLine($"El area del circulo es {pi*(r*r)}");
        }
    }

    public static void Main(){

//retos 
        string miNombre="Erick";
        int miEdad=21;
//-----
        int entero=15;
        string texto="hola Erick";
        bool booleano=false;
//-----
        double b=20; //base
        double a=10; //altura

        double area_Rectangulo = b*a;



//--
        int numero=21;

        if ((numero%2)==0){
            Console.WriteLine("es par");
        }else{
            Console.WriteLine("Es impar");
        }
//--
        imprimirNombre();

//--
        Console.Write("¿Que edad tienes?: ");
        String? Edad;
        int tuEdad;
        do{
            Edad = Console.ReadLine();
        }while(!int.TryParse(Edad, out tuEdad));

        Console.WriteLine($"En 25 años tendrás {tuEdad + 25} años. ");
// poo
        Persona persona = new Persona();
        Console.WriteLine($"Nombre: {persona.getNombre()}, Edad: {persona.getEdad()}");

   
----           


        dynamic variable=123;
        variable="Hola mundo";

        Console.WriteLine($"su objeto ahora es {variable}");

        object objeto =1.23;
        double variable2=(double)objeto;
        Console.WriteLine($"su objeto es: {variable2}");

        
        

    
    } 
}
    

