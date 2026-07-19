using Microsoft.Data.SqlClient;

namespace Transferencia.Api.Servicio
{
    public class TransferenciaServicio
    {
        private readonly Transferencia.Api.AccesoDB.AccesoDB accesoDB = new Transferencia.Api.AccesoDB.AccesoDB();

        public int RegistrarTransferencia(string cuentaOrigen, string cuentaDestino, double importe, string codEmp)
        {
            // Consultas SQL
            string sqlLeerCuenta = @"
                SELECT dec_cuensaldo, int_cuencontmov
                FROM cuenta
                WHERE chr_cuencodigo = @cuenta AND vch_cuenestado = 'ACTIVO'";

            string sqlActualizarCuenta = @"
                UPDATE cuenta
                SET dec_cuensaldo = @saldo,
                    int_cuencontmov = @cont
                WHERE chr_cuencodigo = @cuenta AND vch_cuenestado = 'ACTIVO'";

            string sqlInsertarMovimiento = @"
                INSERT INTO movimiento(chr_cuencodigo, int_movinumero, dtt_movifecha, chr_emplcodigo, chr_tipocodigo, dec_moviimporte)
                VALUES(@cuenta, @cont, GETDATE(), @codEmp, @tipoMovimiento, @importe)";

            using (var cn = accesoDB.GetConnection())
            {
                cn.Open();
                SqlTransaction transaction = cn.BeginTransaction(System.Data.IsolationLevel.Serializable);

                try
                {
                    double saldoOrigen, saldoDestino;
                    int contOrigen, contDestino;

                    // Leer saldo y contador de la cuenta origen
                    using (var cmd = new SqlCommand(sqlLeerCuenta, cn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@cuenta", cuentaOrigen);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                return -1; // Cuenta origen no encontrada o no activa
                            }

                            saldoOrigen = Convert.ToDouble(reader["dec_cuensaldo"]);
                            contOrigen = Convert.ToInt32(reader["int_cuencontmov"]);
                        }
                    }

                    // Validar que la cuenta origen tenga suficiente saldo
                    if (saldoOrigen < importe)
                    {
                        return -2; // Saldo insuficiente
                    }

                    // Leer saldo y contador de la cuenta destino
                    using (var cmd = new SqlCommand(sqlLeerCuenta, cn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@cuenta", cuentaDestino);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                return -3; // Cuenta destino no encontrada o no activa
                            }

                            saldoDestino = Convert.ToDouble(reader["dec_cuensaldo"]);
                            contDestino = Convert.ToInt32(reader["int_cuencontmov"]);
                        }
                    }

                    // Actualizar la cuenta origen (retiro)
                    saldoOrigen -= importe;
                    contOrigen++;

                    using (var cmd = new SqlCommand(sqlActualizarCuenta, cn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@saldo", saldoOrigen);
                        cmd.Parameters.AddWithValue("@cont", contOrigen);
                        cmd.Parameters.AddWithValue("@cuenta", cuentaOrigen);
                        cmd.ExecuteNonQuery();
                    }

                    // Actualizar la cuenta destino (depósito)
                    saldoDestino += importe;
                    contDestino++;

                    using (var cmd = new SqlCommand(sqlActualizarCuenta, cn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@saldo", saldoDestino);
                        cmd.Parameters.AddWithValue("@cont", contDestino);
                        cmd.Parameters.AddWithValue("@cuenta", cuentaDestino);
                        cmd.ExecuteNonQuery();
                    }

                    // Registrar el movimiento en la cuenta origen
                    using (var cmd = new SqlCommand(sqlInsertarMovimiento, cn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@cuenta", cuentaOrigen);
                        cmd.Parameters.AddWithValue("@cont", contOrigen);
                        cmd.Parameters.AddWithValue("@codEmp", codEmp);
                        cmd.Parameters.AddWithValue("@tipoMovimiento", "009"); // Código para retiro
                        cmd.Parameters.AddWithValue("@importe", importe);
                        cmd.ExecuteNonQuery();
                    }

                    // Registrar el movimiento en la cuenta destino
                    using (var cmd = new SqlCommand(sqlInsertarMovimiento, cn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@cuenta", cuentaDestino);
                        cmd.Parameters.AddWithValue("@cont", contDestino);
                        cmd.Parameters.AddWithValue("@codEmp", codEmp);
                        cmd.Parameters.AddWithValue("@tipoMovimiento", "008"); // Código para depósito
                        cmd.Parameters.AddWithValue("@importe", importe);
                        cmd.ExecuteNonQuery();
                    }

                    // Confirmar la transacción
                    transaction.Commit();
                    return 1; // Éxito
                }
                catch (Exception ex)
                {
                    // Rollback en caso de error
                    transaction.Rollback();
                    throw new Exception("Error al registrar la transferencia: " + ex.Message, ex);
                }
            }
        }
    }
}
