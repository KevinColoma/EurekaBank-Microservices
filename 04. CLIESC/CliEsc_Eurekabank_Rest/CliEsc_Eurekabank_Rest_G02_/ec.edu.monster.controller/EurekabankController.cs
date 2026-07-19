using CliEsc_Eurekabank_Rest_G02_.ec.edu.monster.model;
using CliEsc_Eurekabank_Rest_G02_.ec.edu.monster.service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliEsc_Eurekabank_Rest_G02_.ec.edu.monster.controller
{
    public class EurekabankController
    {
        // Instancia del servicio que habla con la API REST
        private EurekaService _eurekaService = new EurekaService();

        /// <summary>
        /// Consulta los movimientos de una cuenta.
        /// </summary>
        public async Task<CuentaViewModel> ConsultarMovimientosAsync(string cuentaCodigo)
        {
            var model = new CuentaViewModel { CuentaCodigo = cuentaCodigo };
            try
            {
                // Traemos los movimientos
                var movimientos = await _eurekaService.TraerMovimientoAsync(cuentaCodigo);

                model.Movimientos = movimientos ?? new List<Movimiento>();

                // Calculamos el monto actual sumando los movimientos
                // ENTRADA suma, SALIDA resta
                double montoActual = 0;
                if (movimientos != null && movimientos.Count > 0)
                {
                    foreach (var mov in movimientos)
                    {
                        System.Diagnostics.Debug.WriteLine($"Movimiento: {mov.nromov} | Acción: {mov.accion} | Importe: {mov.importe}");

                        if (mov.accion == "ENTRADA")
                        {
                            montoActual += mov.importe;
                        }
                        else if (mov.accion == "SALIDA")
                        {
                            montoActual -= mov.importe;
                        }
                    }
                }
                model.MontoActual = montoActual;

                if (model.Movimientos.Count == 0)
                {
                    model.MensajeError = "No se encontraron movimientos para esta cuenta.";
                }
            }
            catch (Exception ex)
            {
                // Captura errores (500, timeout, etc.)
                model.MensajeError = $"Error al consultar: {ex.InnerException?.Message ?? ex.Message}";
            }
            return model;
        }

        /// <summary>
        /// Realiza un depósito.
        /// </summary>
        public async Task<OperacionViewModel> HacerDepositoAsync(string cuenta, double importe)
        {
            var model = new OperacionViewModel { Cuenta = cuenta, Importe = importe };

            // 1. Mapear al DTO del servicio (el 'Request' que espera la API)
            var request = new DepositoRequest
            {
                NumCuenta = cuenta,
                Importe = importe
            };

            try
            {
                // 2. Llamar al servicio
                string resultado = await _eurekaService.RegDepositoAsync(request);

                // 3. Asignar resultado de éxito
                model.Resultado = resultado;
            }
            catch (Exception ex)
            {
                // Capturamos el error de negocio o conexión
                model.Resultado = $"Error: {ex.InnerException?.Message ?? ex.Message}";
            }
            return model;
        }

        /// <summary>
        /// Realiza un retiro.
        /// </summary>
        public async Task<OperacionViewModel> HacerRetiroAsync(string cuenta, double importe)
        {
            var model = new OperacionViewModel { Cuenta = cuenta, Importe = importe };

            // 1. Mapear al DTO (usa el mismo DTO que depósito)
            var request = new DepositoRequest
            {
                NumCuenta = cuenta,
                Importe = importe
            };

            try
            {
                // 2. Llamar al servicio
                string resultado = await _eurekaService.RegRetiroAsync(request);

                // 3. Asignar resultado de éxito
                model.Resultado = resultado;
            }
            catch (Exception ex)
            {
                // Capturamos el error de negocio o conexión
                model.Resultado = $"Error: {ex.InnerException?.Message ?? ex.Message}";
            }
            return model;
        }

        /// <summary>
        /// Realiza una transferencia.
        /// </summary>
        public async Task<OperacionViewModel> HacerTransferenciaAsync(string cuentaOrigen, string cuentaDestino, double importe)
        {
            var model = new OperacionViewModel { Cuenta = cuentaOrigen, CuentaDestino = cuentaDestino, Importe = importe };

            // 1. Mapear al DTO
            var request = new TransferenciaRequest
            {
                CuentaOrigen = cuentaOrigen,
                CuentaDestino = cuentaDestino,
                Importe = importe
            };

            try
            {
                // 2. Llamar al servicio
                string resultado = await _eurekaService.RegTransferenciaAsync(request);

                // 3. Asignar resultado de éxito
                model.Resultado = resultado;
            }
            catch (Exception ex)
            {
                // Capturamos el error de negocio o conexión
                model.Resultado = $"Error: {ex.InnerException?.Message ?? ex.Message}";
            }
            return model;
        }
    }
}
