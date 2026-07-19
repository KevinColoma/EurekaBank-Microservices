using WS_Eurekabank_CLICON.ec.edu.monster.controlador;
using WS_Eurekabank_CLICON.ec.edu.monster.modelo;
using WS_Eurekabank_CLICON.ec.edu.monster.vista;

try
{
    var modelo = new EurekabankService();
    var vista = new EurekabankVista();
    var controlador = new EurekabankControlador(modelo, vista);

    await controlador.MenuPrincipal();
}
catch (HttpRequestException ex)
{
    Console.WriteLine($"\n❌ Error de conexión: No se pudo conectar al servidor.");
    Console.WriteLine($"Detalles: {ex.Message}");
}
catch (TaskCanceledException ex)
{
    Console.WriteLine($"\n❌ Error de tiempo: La solicitud tardó demasiado.");
    Console.WriteLine($"Detalles: {ex.Message}");
}
catch (OperationCanceledException ex)
{
    Console.WriteLine($"\n❌ Operación cancelada: {ex.Message}");
}
catch (Exception ex)
{
    Console.WriteLine($"\n❌ Error inesperado: {ex.GetType().Name}");
    Console.WriteLine($"Mensaje: {ex.Message}");
    if (ex.InnerException != null)
    {
        Console.WriteLine($"Causa: {ex.InnerException.Message}");
    }
}

Console.WriteLine("\nPresione cualquier tecla para salir...");
Console.ReadKey();