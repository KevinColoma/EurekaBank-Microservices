using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliEsc_Eurekabank_Rest_G02_.ec.edu.monster.model
{
    public class Usuario
    {
        // Los nombres de propiedad deben coincidir con lo que espera
        // el DTO 'AutenticacionRequest' del servidor para que la
        // serialización JSON funcione.
        public string NombreUsuario { get; set; }
        public string Contrasena { get; set; }
    }
}
