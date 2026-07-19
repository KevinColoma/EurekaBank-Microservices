using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliEsc_Eurekabank_Rest_G02_.ec.edu.monster.model
{
    public class LoginViewModel
    {
        public string Usuario { get; set; }
        public string Contrasena { get; set; }

        /// <summary>
        /// Almacenará el mensaje de error si la autenticación falla.
        /// </summary>
        public string MensajeError { get; set; }
    }
}
