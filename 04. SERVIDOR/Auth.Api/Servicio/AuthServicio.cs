using Microsoft.Data.SqlClient;

namespace Auth.Api.Servicio
{
    public class AuthServicio
    {
        private readonly Auth.Api.AccesoDB.AccesoDB accesoDB = new Auth.Api.AccesoDB.AccesoDB();

        public bool ValidarUsuario(string usuario, string password)
        {
            string sql = @"
            SELECT PASSWORD
            FROM auth
            WHERE USUARIO = @usuario";

            using (var cn = accesoDB.GetConnection())
            {
                try
                {
                    cn.Open(); // Abrimos la conexión

                    using (SqlCommand cmd = new SqlCommand(sql, cn))
                    {
                        // Añadimos los parámetros para evitar inyección SQL
                        cmd.Parameters.AddWithValue("@usuario", usuario);

                        // Ejecutamos el comando para obtener la contraseña almacenada
                        object result = cmd.ExecuteScalar();

                        if (result == null)
                        {
                            Console.WriteLine($"Usuario '{usuario}' no encontrado en la base de datos.");
                            return false;
                        }

                        // Convertimos el resultado a byte[] y luego a cadena hexadecimal
                        byte[] dbPasswordBytes = (byte[])result;
                        string dbPasswordHash = ConvertirBytesAHexadecimal(dbPasswordBytes);

                        // Generamos el hash de la contraseña proporcionada
                        string inputPasswordHash = GenerarHashSHA256(password);

                        // Depuración: imprimir hash generado y almacenado
                        System.Diagnostics.Debug.WriteLine($"Hash proporcionado: {inputPasswordHash}");
                        System.Diagnostics.Debug.WriteLine($"Hash almacenado: {dbPasswordHash}");

                        // Comparamos el hash proporcionado con el almacenado
                        bool sonIguales = inputPasswordHash.Equals(dbPasswordHash, StringComparison.OrdinalIgnoreCase);

                        // Resultado
                        System.Diagnostics.Debug.WriteLine(sonIguales
                            ? "Las credenciales coinciden."
                            : "Las credenciales no coinciden.");

                        return sonIguales;
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de excepciones
                    throw new Exception("Error al validar el usuario: " + ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// Convierte un arreglo de bytes a una cadena hexadecimal.
        /// </summary>
        /// <param name="bytes">El arreglo de bytes a convertir.</param>
        /// <returns>Una representación en cadena hexadecimal de los bytes.</returns>
        private string ConvertirBytesAHexadecimal(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "").ToUpper();
        }

        /// <summary>
        /// Genera un hash SHA-256 para una contraseña dada.
        /// </summary>
        /// <param name="input">La contraseña en texto plano.</param>
        /// <returns>El hash SHA-256 de la contraseña.</returns>
        private string GenerarHashSHA256(string input)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(bytes).Replace("-", "").ToUpper();
            }
        }
    }
}
