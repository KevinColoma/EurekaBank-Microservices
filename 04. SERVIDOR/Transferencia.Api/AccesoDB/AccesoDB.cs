using Microsoft.Data.SqlClient;

namespace Transferencia.Api.AccesoDB
{
    public class AccesoDB
    {
        public SqlConnection GetConnection()
        {
            try
            {
                // La cadena de conexión se lee desde la variable de entorno EUREKA_DB_CONNECTION (archivo .env, no versionado)
                string? connectionString = Environment.GetEnvironmentVariable("EUREKA_DB_CONNECTION");
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    throw new InvalidOperationException(
                        "Falta la variable de entorno EUREKA_DB_CONNECTION. Copia .env.example a .env y define la cadena de conexión.");
                }

                // Devuelve una conexión lista para usar (sin abrir)
                return new SqlConnection(connectionString);
            }
            catch (Exception ex)
            {
                // Lanza una excepción específica para manejo en niveles superiores
                throw new Exception("Error al obtener la conexión a la base de datos: " + ex.Message, ex);
            }
        }
    }
}
