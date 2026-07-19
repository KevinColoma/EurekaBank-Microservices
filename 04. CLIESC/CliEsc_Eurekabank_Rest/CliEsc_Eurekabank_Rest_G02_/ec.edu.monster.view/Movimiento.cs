using CliEsc_Eurekabank_Rest_G02_.ec.edu.monster.controller;
using CliEsc_Eurekabank_Rest_G02_.ec.edu.monster.model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CliEsc_Eurekabank_Rest_G02_.ec.edu.monster.view
{
    public partial class Movimiento : Form
    {
        private EurekabankController _controller;
        public Movimiento()
        {
            InitializeComponent();
            _controller = new EurekabankController();
        }

        private async void btnConsultar_Click(object sender, EventArgs e)
        {
            string cuentaCodigo = txtCuenta.Text.Trim();

            if (string.IsNullOrEmpty(cuentaCodigo))
            {
                MessageBox.Show("Debe ingresar un código de cuenta.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Limpiar UI
            label4.Text = "---";
            dgvMovimientos.DataSource = null;

            try
            {
                SetUiEnabled(false); // Deshabilitar UI mientras espera

                // Llamar al controlador REST
                CuentaViewModel resultado = await _controller.ConsultarMovimientosAsync(cuentaCodigo);

                // Mostrar resultados
                if (!string.IsNullOrEmpty(resultado.MensajeError))
                {
                    MessageBox.Show(resultado.MensajeError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    label4.Text = "Error en la consulta";
                }
                else
                {
                    // Mostrar el saldo actual con prefijo USD y símbolo $
                    label4.Text = $"USD ${resultado.MontoActual:F2}";

                    // Enlazar los movimientos al DataGridView
                    dgvMovimientos.DataSource = resultado.Movimientos;

                    // Redimensionar columnas automáticamente para mejor legibilidad
                    dgvMovimientos.AutoResizeColumns(System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells);
                }
            }
            catch (Exception ex)
            {
                // Error fatal inesperado
                MessageBox.Show($"Error inesperado: {ex.Message}", "Error Fatal", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                SetUiEnabled(true); // Rehabilitar UI en cualquier caso
            }
        }

        /// <summary>
        /// Habilita o deshabilita los controles durante una operación async.
        /// </summary>
        private void SetUiEnabled(bool enabled)
        {
            btnConsultar.Enabled = enabled;
            txtCuenta.Enabled = enabled;
            // Cambia el cursor a "Espera"
            Cursor = enabled ? Cursors.Default : Cursors.WaitCursor;
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            Menu menu = new Menu();
            menu.Show();
            this.Hide();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
