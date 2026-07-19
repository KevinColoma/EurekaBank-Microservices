using Microsoft.Data.SqlClient;

namespace Operaciones.Api.Servicio
{
    public class OperacionesServicio
    {
        private readonly Operaciones.Api.AccesoDB.AccesoDB accesoDB = new Operaciones.Api.AccesoDB.AccesoDB();

        public int RegistrarDeposito(string cuenta, double importe, string codEmp)
        {
            string sql1 = @"
                SELECT dec_cuensaldo, int_cuencontmov
                FROM cuenta
                WHERE chr_cuencodigo = @cuenta AND vch_cuenestado = 'ACTIVO'";

            string sql2 = @"
                UPDATE cuenta
                SET dec_cuensaldo = @saldo,
                    int_cuencontmov = @cont
                WHERE chr_cuencodigo = @cuenta AND vch_cuenestado = 'ACTIVO'";

            string sql3 = @"
                INSERT INTO movimiento(chr_cuencodigo, int_movinumero, dtt_movifecha, chr_emplcodigo, chr_tipocodigo, dec_moviimporte)
                VALUES(@cuenta, @cont, GETDATE(), @codEmp, '003', @importe)";

            using (var cn = accesoDB.GetConnection())
            {
                cn.Open();
                SqlTransaction transaction = cn.BeginTransaction(System.Data.IsolationLevel.Serializable);

                try
                {
                    double saldo;
                    int cont;

                    // Paso 1: Leer datos de la cuenta con un bloqueo compartido
                    using (SqlCommand cmd = new SqlCommand(sql1, cn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@cuenta", cuenta);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                return -1; // La cuenta no existe o no está activa
                            }

                            saldo = Convert.ToDouble(reader["dec_cuensaldo"]);
                            cont = Convert.ToInt32(reader["int_cuencontmov"]);
                        }
                    }

                    // Paso 2: Actualizar la cuenta
                    saldo += importe;
                    cont++;

                    using (SqlCommand cmd = new SqlCommand(sql2, cn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@saldo", saldo);
                        cmd.Parameters.AddWithValue("@cont", cont);
                        cmd.Parameters.AddWithValue("@cuenta", cuenta);

                        cmd.ExecuteNonQuery();
                    }

                    // Paso 3: Registrar el movimiento
                    using (SqlCommand cmd = new SqlCommand(sql3, cn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@cuenta", cuenta);
                        cmd.Parameters.AddWithValue("@cont", cont);
                        cmd.Parameters.AddWithValue("@codEmp", codEmp);
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
                    throw new Exception("Error al registrar el depósito: " + ex.Message, ex);
                }
            }
        }

        public int RegistrarRetiro(string cuenta, double importe, string codEmp)
        {
            string sql1 = @"
                SELECT dec_cuensaldo, int_cuencontmov
                FROM cuenta
                WHERE chr_cuencodigo = @cuenta AND vch_cuenestado = 'ACTIVO'";

            string sql2 = @"
                UPDATE cuenta
                SET dec_cuensaldo = @saldo,
                    int_cuencontmov = @cont
                WHERE chr_cuencodigo = @cuenta AND vch_cuenestado = 'ACTIVO'";

            string sql3 = @"
                INSERT INTO movimiento(chr_cuencodigo, int_movinumero, dtt_movifecha, chr_emplcodigo, chr_tipocodigo, dec_moviimporte)
                VALUES(@cuenta, @cont, GETDATE(), @codEmp, '004', @importe)";

            using (var cn = accesoDB.GetConnection())
            {
                cn.Open();
                SqlTransaction transaction = cn.BeginTransaction(System.Data.IsolationLevel.Serializable);

                try
                {
                    double saldo;
                    int cont;

                    // Paso 1: Leer datos de la cuenta con un bloqueo compartido
                    using (SqlCommand cmd = new SqlCommand(sql1, cn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@cuenta", cuenta);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                return -1; // La cuenta no existe o no está activa
                            }

                            saldo = Convert.ToDouble(reader["dec_cuensaldo"]);
                            cont = Convert.ToInt32(reader["int_cuencontmov"]);
                        }
                    }

                    // Validar que el saldo sea suficiente para el retiro
                    if (saldo < importe)
                    {
                        return -2; // Saldo insuficiente
                    }

                    // Paso 2: Actualizar la cuenta
                    saldo -= importe;
                    cont++;

                    using (SqlCommand cmd = new SqlCommand(sql2, cn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@saldo", saldo);
                        cmd.Parameters.AddWithValue("@cont", cont);
                        cmd.Parameters.AddWithValue("@cuenta", cuenta);

                        cmd.ExecuteNonQuery();
                    }

                    // Paso 3: Registrar el movimiento
                    using (SqlCommand cmd = new SqlCommand(sql3, cn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@cuenta", cuenta);
                        cmd.Parameters.AddWithValue("@cont", cont);
                        cmd.Parameters.AddWithValue("@codEmp", codEmp);
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
                    throw new Exception("Error al registrar el depósito: " + ex.Message, ex);
                }
            }
        }
    }
}
