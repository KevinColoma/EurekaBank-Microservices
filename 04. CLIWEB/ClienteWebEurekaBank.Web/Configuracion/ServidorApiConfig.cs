using System.Text.Json;

namespace ClienteWebEurekaBank.Web.Configuracion;

/// <summary>
/// Guarda, en tiempo de ejecución, la URL base del API (IP del servidor host).
/// Permite cambiarla desde la interfaz sin recompilar ni editar appsettings.json.
/// El valor se persiste en un archivo JSON para sobrevivir a reinicios.
/// </summary>
public interface IServidorApiConfig
{
    string UrlBase { get; }
    void Actualizar(string urlBase);
}

public sealed class ServidorApiConfig : IServidorApiConfig
{
    private readonly string _rutaArchivo;
    private readonly object _bloqueo = new();
    private string _urlBase;

    public ServidorApiConfig(string urlBaseInicial, string rutaArchivo)
    {
        _rutaArchivo = rutaArchivo;
        // Prioridad: valor persistido (cambiado desde la UI) sobre el de appsettings.json.
        _urlBase = LeerPersistido() ?? urlBaseInicial;
    }

    public string UrlBase
    {
        get
        {
            lock (_bloqueo)
            {
                return _urlBase;
            }
        }
    }

    public void Actualizar(string urlBase)
    {
        if (string.IsNullOrWhiteSpace(urlBase))
        {
            throw new ArgumentException("La URL base no puede estar vacía.", nameof(urlBase));
        }

        var limpia = urlBase.Trim();
        lock (_bloqueo)
        {
            _urlBase = limpia;
            Persistir(limpia);
        }
    }

    private string? LeerPersistido()
    {
        try
        {
            if (File.Exists(_rutaArchivo))
            {
                var json = File.ReadAllText(_rutaArchivo);
                var datos = JsonSerializer.Deserialize<Persistencia>(json);
                if (!string.IsNullOrWhiteSpace(datos?.UrlBase))
                {
                    return datos!.UrlBase;
                }
            }
        }
        catch
        {
            // Si el archivo está corrupto o inaccesible, se usa el valor inicial.
        }
        return null;
    }

    private void Persistir(string urlBase)
    {
        try
        {
            var json = JsonSerializer.Serialize(new Persistencia { UrlBase = urlBase });
            File.WriteAllText(_rutaArchivo, json);
        }
        catch
        {
            // Persistir es best-effort; si falla, el valor igual queda activo en memoria.
        }
    }

    private sealed class Persistencia
    {
        public string UrlBase { get; set; } = string.Empty;
    }
}
