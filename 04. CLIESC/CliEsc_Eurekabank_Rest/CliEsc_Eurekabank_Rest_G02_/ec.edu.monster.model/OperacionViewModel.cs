using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliEsc_Eurekabank_Rest_G02_.ec.edu.monster.model
{
    public class OperacionViewModel
    {
        // Usado para Depósito, Retiro (Origen), Transferencia (Origen)
        public string Cuenta { get; set; }

        public double Importe { get; set; }

        // Solo para Transferencia
        public string CuentaDestino { get; set; }

        // Mensaje de éxito o error para mostrar al usuario
        public string Resultado { get; set; }
    }
}
