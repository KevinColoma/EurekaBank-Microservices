using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliEsc_Eurekabank_Rest_G02_.ec.edu.monster.model
{
    public class Movimiento
    {
        public string cuenta { get; set; }
        public int nromov { get; set; }
        public DateTime fecha { get; set; }
        public string tipo { get; set; }
        public string accion { get; set; }
        public double importe { get; set; }
    }
}
