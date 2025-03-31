using System;

public class Program{
    public static void ingresarSemestre(){
        do{
            try{
                 Console.Write("Ingrese el numero del semestre que cursa (numero del 1 al 10): ");

                string? strOpcion = Console.ReadLine();
                int.TryParse(strOpcion, out int iSemestre);

                if(iSemestre<1||iSemestre>10){
                    Console.WriteLine("Dato invalido");
                }else{
                    Console.WriteLine($"Usted está cursando el semestre {iSemestre}, mucha suerte.");
                    return;
                }

            }catch(FormatException ){
                Console.WriteLine("Error de formato, ingrese un numero entero. ");
            }catch(Exception e){
                Console.WriteLine(e);
                Console.WriteLine("Error inesperado, debe ingresar un número entero del 1 al 10 ");
            }
            
        }while(true);

    }
    public static void pedirEdad(){
        do{
            try{
                
                Console.Write("Bienvenido usuario ingrese su edad: ");
                string? strEdadUsuario = Console.ReadLine();
                
                bool bConversionExitosa=int.TryParse(strEdadUsuario, out int iEdadUsuario);

                if(!bConversionExitosa){
                    
                }
            }catch(Exception){
                Console.WriteLine("Error de formato, ingrese un número entero");
            }

        }while(true);
    }

    public static void Main(){
        // ingresarSemestre();
        pedirEdad();
    }
}