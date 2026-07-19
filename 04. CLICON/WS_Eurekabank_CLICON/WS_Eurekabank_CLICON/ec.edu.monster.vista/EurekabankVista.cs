using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WS_Eurekabank_CLICON.ec.edu.monster.vista
{
    public class EurekabankVista
    {
        public void MostrarMenuPrincipal()
        {
            Console.WriteLine("╔══════════════════════╗");
            Console.WriteLine("║                      ║");
            Console.WriteLine("║     BIENVENIDO       ║");
            Console.WriteLine("║                      ║");
            Console.WriteLine("╚══════════════════════╝");
            Console.WriteLine("1. Iniciar Sesion");
            Console.WriteLine("2. Salir");
        }

        public void MostrarMenuConversion()
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║   ***      Eurekabank       ***  ║");
            Console.WriteLine("╠══════════════════════════════════╗");
            Console.WriteLine("║ Seleccione una opción:           ║");
            Console.WriteLine("║1. Consultar Movimientos          ║");
            Console.WriteLine("║2. Realizar Depósito              ║");
            Console.WriteLine("║3. Realizar Transferencia         ║");
            Console.WriteLine("║4. Realizar Retiro                ║");
            Console.WriteLine("║5. Volver al menú principal       ║");
            Console.WriteLine("╚══════════════════════════════════╝");
        }

        public int LeerOpcion()
        {
            while (true)
            {
                Console.Write("Ingrese una opción: ");
                string? entrada = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(entrada) && int.TryParse(entrada, out int opcion))
                {
                    return opcion;
                }
                Console.WriteLine("Entrada inválida. Por favor, ingrese un número.");
            }
        }

        public double LeerValor(string mensaje)
        {
            while (true)
            {
                Console.Write(mensaje);
                string? entrada = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(entrada) && double.TryParse(entrada, out double valor))
                {
                    return valor;
                }
                Console.WriteLine("Entrada inválida. Por favor, ingrese un número válido.");
            }
        }

        public void MostrarResultado(double resultado, string mensaje)
        {
            Console.WriteLine($"{mensaje}: {resultado}");
        }

        public void MostrarMensaje(string mensaje)
        {
            Console.WriteLine(mensaje);
        }

        public string LeerTexto(string mensaje)
        {
            Console.Write(mensaje);
            string? texto = Console.ReadLine();
            return texto ?? string.Empty;
        }

        public void MostrarIUsuario()
        {
            Console.Write("╔══════════════════════╗\n");
            Console.Write("║            Usuario:  ║\n");
            Console.Write("╚══════════════════════╝\n");
            Console.SetCursorPosition("║ Usuario:             ║".Length + 1, Console.CursorTop - 1);
        }

        public void MostrarIPassword()
        {
            Console.Write("╔══════════════════════╗\n");
            Console.Write("║          Contraseña: ║\n");
            Console.Write("╚══════════════════════╝\n");
            Console.SetCursorPosition("║ Contraseña:          ║".Length + 1, Console.CursorTop - 1);
        }

        public string ReadPassword()
        {
            string password = string.Empty;
            ConsoleKeyInfo info;
            do
            {
                info = Console.ReadKey(true);
                if (info.Key != ConsoleKey.Backspace && info.Key != ConsoleKey.Enter)
                {
                    password += info.KeyChar;
                }
                else if (info.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password.Substring(0, password.Length - 1);
                }
            } while (info.Key != ConsoleKey.Enter);

            Console.WriteLine();
            return password;
        }
    }
}
