using System;
using System.IO;

namespace CliEsc_Eurekabank_Rest_G02_.ec.edu.monster.service
{
    /// <summary>
    /// Configuración del servidor (IP/host) del API Gateway de EurekaBank.
    /// La IP se guarda en un archivo junto al ejecutable para no tener que
    /// recompilar cuando el servidor host cambia de dirección.
    /// </summary>
    public static class Config
    {
        public const string Puerto = "5069";
        public const string Ruta = "/Eureka/";

        private static readonly string ArchivoConfig =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "servidor.config");

        private static string _ip = CargarIp();

        /// <summary>IP o host del servidor. Al asignarse se persiste en disco.</summary>
        public static string Ip
        {
            get { return _ip; }
            set
            {
                _ip = string.IsNullOrWhiteSpace(value) ? "localhost" : value.Trim();
                GuardarIp(_ip);
            }
        }

        /// <summary>URL base completa del API, ej: http://192.168.1.10:5069/Eureka/</summary>
        public static string BaseUrl
        {
            get { return "http://" + _ip + ":" + Puerto + Ruta; }
        }

        private static string CargarIp()
        {
            try
            {
                if (File.Exists(ArchivoConfig))
                {
                    string ip = File.ReadAllText(ArchivoConfig).Trim();
                    if (!string.IsNullOrWhiteSpace(ip))
                    {
                        return ip;
                    }
                }
            }
            catch
            {
                // Si el archivo está corrupto o inaccesible, se usa el valor por defecto.
            }
            return "localhost";
        }

        private static void GuardarIp(string ip)
        {
            try
            {
                File.WriteAllText(ArchivoConfig, ip);
            }
            catch
            {
                // Persistir es best-effort; si falla, la IP igual queda activa en memoria.
            }
        }
    }
}
