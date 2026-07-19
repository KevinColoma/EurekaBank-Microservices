using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliEsc_Eurekabank_Rest_G02_.ec.edu.monster.model
{
    public class DepositoRequest
    {
        // Asegúrate que los nombres coincidan con los esperados por la API
        public string NumCuenta { get; set; }
        public double Importe { get; set; }
    }
}
