using System.IO;
using System;

public class Program{
    public static void ingresarSemestre(){
        int iSemestre;
        do{
            try{
                Console.Write("Ingrese el numero del semestre que cursa: ");
                
                iSemestre=int.Parse(Console.ReadLine());
                Console.Clear();
                if(iSemestre<1 || iSemestre>10){
                    Console.WriteLine("Error de rango, debe ingresar un número entre 1 10\n");
                }else{
                    Console.Write($"Usted está cursando el {iSemestre} semestre  \nEnter para continuar...");
                    Console.ReadKey();
                    Console.Clear();
                    return;
                }
            }catch(FormatException ){
                Console.Clear();
                Console.WriteLine("Error inesperado, debe ingresar un número entero del 1 al 10 \n");
            }catch(Exception e){
                Console.WriteLine(e);
            }
            
        }while(true);

    }
    
    public static void pedirEdad(){
        int edad;
        do{
            try{
                Console.Write("ingrese su edad usuario: ");
                
                edad = int.Parse(Console.ReadLine());
                Console.Clear();

                if(edad<0||edad>120){
                    if(edad<0){
                        Console.WriteLine("Error de línea de tiempo, usted no ha nacido \n");
                    }else if(edad>120){
                        Console.WriteLine("Error de especie, usted es un dinosaurio \n");
                    }
                }else{
                    Console.Write($"Usted tiene {edad}, se le desea lo mejor\nEnter para continuar...");
                    Console.ReadKey();
                    Console.Clear();
                    return;
                }

            }catch(FormatException){
                Console.Clear();
                Console.WriteLine("Error de formato, ingrese un número entero");
            }

        }while(true);
    }

    public static void aplicaciondeLog(){
        
        try{
            Console.Write("ingresa dos números:\n");
            Console.Write("Ingresa el primer número (dividendo): ");
            int iNumero1 = int.Parse(Console.ReadLine());
            Console.Write("Ingresa el segundo numero (divisor) ");
            int iNumero2 = int.Parse(Console.ReadLine());
            Console.Clear();
            
            float division = iNumero1/iNumero2;
            
            Console.WriteLine($"El resultado de la división es: {division}. ");

        }catch(Exception ex){
            Console.WriteLine("Un error ha ocurrido revisa el archivo 'log.txt'");
            File.AppendAllText("log.txt", DateTime.Now + " - " +ex.Message + Environment.NewLine);
        }



    }

    public static void menu(){

        do{
            
            Console.Write("Menú: \n1. Ingresar numero entre 1 y 10\n2. Pedir edad\n otra opcion que aun falta\n4. Salir\n\nElija una opción: ");
            try{
                
            int opcion = int.Parse(Console.ReadLine());
            Console.Clear();
            switch(opcion){
                case 1:
                    ingresarSemestre();
                    break;
                case 2:
                    pedirEdad();
                    break;
                case 3:
                    aplicaciondeLog();
                    break;
                case 4:
                    return;
                default:
                    Console.WriteLine("Número fuera de rango, ingrese un numero del 1 al 4\n");
                    break;
            }
            }catch(FormatException){
                Console.Clear();
                Console.WriteLine("Error de formato, ingrese un número entero \n");
            }

        }while(true);
    }
    public static void Main(){
        menu();
    }
}