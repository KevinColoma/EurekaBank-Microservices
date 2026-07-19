using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WS_Eurekabank_CLICON.ec.edu.monster.modelo;
using WS_Eurekabank_CLICON.ec.edu.monster.vista;

namespace WS_Eurekabank_CLICON.ec.edu.monster.controlador
{
    public class EurekabankControlador
    {
        private readonly EurekabankService _modelo;
        private readonly EurekabankVista _vista;

        public EurekabankControlador(EurekabankService modelo, EurekabankVista vista)
        {
            _modelo = modelo;
            _vista = vista;
        }

        public async Task MenuPrincipal()
        {
            bool salir = false;
            while (!salir)
            {
                try
                {
                    _vista.MostrarMenuPrincipal();
                    int opcion = _vista.LeerOpcion();

                    switch (opcion)
                    {
                        case 1:
                            await ManejarLogin();
                            break;
                        case 2:
                            ConfigurarServidor();
                            break;
                        case 3:
                            salir = true;
                            break;
                        default:
                            _vista.MostrarMensaje("Opción no válida.");
                            break;
                    }
                }
                catch (HttpRequestException)
                {
                    _vista.MostrarMensaje("❌ Error de conexión. Intente nuevamente.");
                }
                catch (TaskCanceledException)
                {
                    _vista.MostrarMensaje("❌ Tiempo de espera agotado. Intente nuevamente.");
                }
                catch (Exception ex)
                {
                    _vista.MostrarMensaje($"❌ Error inesperado: {ex.Message}");
                }
            }
        }

        private void ConfigurarServidor()
        {
            _vista.MostrarMensaje($"\nServidor actual: {Config.BaseUrl}");
            string nuevaIp = _vista.LeerTexto("Ingrese la nueva IP o host del servidor (Enter para cancelar): ");

            if (string.IsNullOrWhiteSpace(nuevaIp))
            {
                _vista.MostrarMensaje("Configuración sin cambios.");
                return;
            }

            Config.Ip = nuevaIp;
            _vista.MostrarMensaje($"✓ Servidor actualizado a: {Config.BaseUrl}");
        }

        private async Task ManejarLogin()
        {
            try
            {
                _vista.MostrarMensaje("Inicio de sesión:");
                _vista.MostrarIUsuario();
                string username = _vista.LeerTexto("* ");

                if (string.IsNullOrWhiteSpace(username))
                {
                    _vista.MostrarMensaje("❌ El usuario no puede estar vacío.");
                    return;
                }

                _vista.MostrarIPassword();
                string password = _vista.ReadPassword();

                if (string.IsNullOrWhiteSpace(password))
                {
                    _vista.MostrarMensaje("❌ La contraseña no puede estar vacía.");
                    return;
                }

                string loginResponse = await _modelo.Login(username, password);
                if (loginResponse.Contains("Usuario válido."))
                {
                    _vista.MostrarMensaje("✓ Inicio de sesión exitoso.");
                    await MenuConversion();
                }
                else
                {
                    _vista.MostrarMensaje("❌ Error: Usuario o contraseña incorrectos.");
                }
            }
            catch (HttpRequestException)
            {
                _vista.MostrarMensaje("❌ Error de conexión en el login.");
            }
            catch (TaskCanceledException)
            {
                _vista.MostrarMensaje("❌ Tiempo de espera agotado en el login.");
            }
            catch (Exception ex)
            {
                _vista.MostrarMensaje($"❌ Error en login: {ex.Message}");
            }
        }

        public async Task MenuConversion()
        {
            bool volver = false;
            while (!volver)
            {
                try
                {
                    _vista.MostrarMenuConversion();
                    int opcion = _vista.LeerOpcion();

                    switch (opcion)
                    {
                        case 1:
                            await ManejarMovimientos();
                            break;
                        case 2:
                            await ManejarDeposito();
                            break;
                        case 3:
                            await ManejarTransferencia();
                            break;
                        case 4:
                            await ManejarRetiro();
                            break;
                        case 5:
                            volver = true;
                            break;
                        default:
                            _vista.MostrarMensaje("Opción no válida.");
                            break;
                    }
                }
                catch (HttpRequestException)
                {
                    _vista.MostrarMensaje("❌ Error de conexión. Intente nuevamente.");
                }
                catch (TaskCanceledException)
                {
                    _vista.MostrarMensaje("❌ Tiempo de espera agotado. Intente nuevamente.");
                }
                catch (Exception ex)
                {
                    _vista.MostrarMensaje($"❌ Error inesperado: {ex.Message}");
                }
            }
        }

        private async Task ManejarMovimientos()
        {
            try
            {
                string cuenta = _vista.LeerTexto("Ingrese la cuenta: ");
                if (string.IsNullOrWhiteSpace(cuenta))
                {
                    _vista.MostrarMensaje("❌ La cuenta no puede estar vacía.");
                    return;
                }

                List<Movimiento> movimientos = await _modelo.ObtenerMovimientos(cuenta);

                if (movimientos.Count > 0)
                {
                    Console.WriteLine("\n📋 Movimientos encontrados:");
                    foreach (Movimiento r in movimientos)
                    {
                        Console.WriteLine($"{r.cuenta} - {r.nromov} - {r.fecha} - {r.tipo} - {r.accion} - {r.importe}");
                    }
                }
                else
                {
                    Console.WriteLine("❌ No se encontraron movimientos para esta cuenta.");
                }
                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                Console.ReadKey();
            }
            catch (HttpRequestException)
            {
                _vista.MostrarMensaje("❌ Error al conectar con el servidor.");
            }
            catch (TaskCanceledException)
            {
                _vista.MostrarMensaje("❌ Tiempo de espera agotado.");
            }
            catch (Exception ex)
            {
                _vista.MostrarMensaje($"❌ Error: {ex.Message}");
            }
        }

        private async Task ManejarDeposito()
        {
            try
            {
                string cuenta = _vista.LeerTexto("Ingrese la cuenta: ");
                if (string.IsNullOrWhiteSpace(cuenta))
                {
                    _vista.MostrarMensaje("❌ La cuenta no puede estar vacía.");
                    return;
                }

                double deposito = _vista.LeerValor("Ingrese el importe a depositar: ");
                if (deposito <= 0)
                {
                    _vista.MostrarMensaje("❌ El importe debe ser mayor a 0.");
                    return;
                }

                string depositoRespuesta = await _modelo.RegistrarDeposito(cuenta, deposito);
                _vista.MostrarMensaje($"✓ {depositoRespuesta}");
                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                Console.ReadKey();
            }
            catch (HttpRequestException)
            {
                _vista.MostrarMensaje("❌ Error al conectar con el servidor.");
            }
            catch (TaskCanceledException)
            {
                _vista.MostrarMensaje("❌ Tiempo de espera agotado.");
            }
            catch (Exception ex)
            {
                _vista.MostrarMensaje($"❌ Error: {ex.Message}");
            }
        }

        private async Task ManejarTransferencia()
        {
            try
            {
                string cuenta = _vista.LeerTexto("Ingrese la cuenta origen: ");
                if (string.IsNullOrWhiteSpace(cuenta))
                {
                    _vista.MostrarMensaje("❌ La cuenta origen no puede estar vacía.");
                    return;
                }

                string cuenta2 = _vista.LeerTexto("Ingrese la cuenta destino: ");
                if (string.IsNullOrWhiteSpace(cuenta2))
                {
                    _vista.MostrarMensaje("❌ La cuenta destino no puede estar vacía.");
                    return;
                }

                if (cuenta.Equals(cuenta2))
                {
                    _vista.MostrarMensaje("❌ Las cuentas origen y destino no pueden ser iguales.");
                    return;
                }

                double transferencia = _vista.LeerValor("Ingrese el importe a transferir: ");
                if (transferencia <= 0)
                {
                    _vista.MostrarMensaje("❌ El importe debe ser mayor a 0.");
                    return;
                }

                string transferenciaRespuesta = await _modelo.RegistrarTransferencia(cuenta, cuenta2, transferencia);
                _vista.MostrarMensaje($"✓ {transferenciaRespuesta}");
                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                Console.ReadKey();
            }
            catch (HttpRequestException)
            {
                _vista.MostrarMensaje("❌ Error al conectar con el servidor.");
            }
            catch (TaskCanceledException)
            {
                _vista.MostrarMensaje("❌ Tiempo de espera agotado.");
            }
            catch (Exception ex)
            {
                _vista.MostrarMensaje($"❌ Error: {ex.Message}");
            }
        }

        private async Task ManejarRetiro()
        {
            try
            {
                string cuenta = _vista.LeerTexto("Ingrese la cuenta: ");
                if (string.IsNullOrWhiteSpace(cuenta))
                {
                    _vista.MostrarMensaje("❌ La cuenta no puede estar vacía.");
                    return;
                }

                double retiro = _vista.LeerValor("Ingrese el importe a retirar: ");
                if (retiro <= 0)
                {
                    _vista.MostrarMensaje("❌ El importe debe ser mayor a 0.");
                    return;
                }

                string retiroRespuesta = await _modelo.RegistrarRetiro(cuenta, retiro);
                _vista.MostrarMensaje($"✓ {retiroRespuesta}");
                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                Console.ReadKey();
            }
            catch (HttpRequestException)
            {
                _vista.MostrarMensaje("❌ Error al conectar con el servidor.");
            }
            catch (TaskCanceledException)
            {
                _vista.MostrarMensaje("❌ Tiempo de espera agotado.");
            }
            catch (Exception ex)
            {
                _vista.MostrarMensaje($"❌ Error: {ex.Message}");
            }
        }

    }
}
