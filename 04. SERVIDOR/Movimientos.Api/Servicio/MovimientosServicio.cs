using Microsoft.Data.SqlClient;
using Movimientos.Api.Model;

namespace Movimientos.Api.Servicio
{
    public class MovimientosServicio
    {
        private readonly Movimientos.Api.AccesoDB.AccesoDB accesoDB = new Movimientos.Api.AccesoDB.AccesoDB();

        public List<Movimiento> LeerMovimientos(string cuenta)
        {
            var lista = new List<Movimiento>();
            string sql = @"
                SELECT
                    m.chr_cuencodigo AS cuenta,
                    m.int_movinumero AS nromov,
                    m.dtt_movifecha AS fecha,
                    t.vch_tipodescripcion AS tipo,
                    t.vch_tipoaccion AS accion,
                    m.dec_moviimporte AS importe
                FROM tipomovimiento t
                INNER JOIN movimiento m ON t.chr_tipocodigo = m.chr_tipocodigo
                WHERE m.chr_cuencodigo = @cuenta
                ORDER BY m.dtt_movifecha DESC";

            using (var cn = accesoDB.GetConnection())
            {
                try
                {
                    cn.Open(); // Abrimos la conexión

                    using (SqlCommand cmd = new SqlCommand(sql, cn))
                    {
                        cmd.Parameters.AddWithValue("@cuenta", cuenta);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var movimiento = new Movimiento
                                {
                                    Cuenta = reader["cuenta"].ToString(),
                                    NroMov = Convert.ToInt32(reader["nromov"]),
                                    Fecha = Convert.ToDateTime(reader["fecha"]),
                                    Tipo = reader["tipo"].ToString(),
                                    Accion = reader["accion"].ToString(),
                                    Importe = Convert.ToDouble(reader["importe"])
                                };
                                lista.Add(movimiento);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al leer movimientos: " + ex.Message, ex);
                }
            }

            return lista;
        }
    }
}
