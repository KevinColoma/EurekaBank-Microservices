using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WS_Eurekabank_CLICON.ec.edu.monster.modelo
{
    public class LoginResponse
    {
        public string codigo { get; set; }
        public string usuario { get; set; }
        public string estado { get; set; }
    }
    public class OperacionResponse
    {
        public int estado { get; set; }
        public string mensaje { get; set; }
    }

    public class ErrorResponse
    {
        public string error { get; set; }
    }
}
