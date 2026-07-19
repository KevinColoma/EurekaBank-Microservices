using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliEsc_Eurekabank_Rest_G02_.ec.edu.monster.model
{
    public class TransferenciaRequest
    {
        // Asegúrate que los nombres coincidan con los esperados por la API
        public string CuentaOrigen { get; set; }
        public string CuentaDestino { get; set; }
        public double Importe { get; set; }
    }
}
