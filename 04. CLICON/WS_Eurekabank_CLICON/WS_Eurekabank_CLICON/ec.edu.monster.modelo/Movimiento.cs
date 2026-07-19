using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WS_Eurekabank_CLICON.ec.edu.monster.modelo
{
    public class Movimiento
    {
        public string cuenta { get; set; }
        public int nromov { get; set; }
        public string fecha { get; set; }
        public string tipo { get; set; }
        public string accion { get; set; }
        public double importe { get; set; }
    }
}
