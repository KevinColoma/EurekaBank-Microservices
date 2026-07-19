using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliEsc_Eurekabank_Rest_G02_.ec.edu.monster.model
{
    public class CuentaViewModel
    {
        public string CuentaCodigo { get; set; }
        public double MontoActual { get; set; }
        public List<Movimiento> Movimientos { get; set; }
        public string MensajeError { get; set; }

        public CuentaViewModel()
        {
            // Inicializamos la lista para evitar errores nulos
            Movimientos = new List<Movimiento>();
        }
    }
}
