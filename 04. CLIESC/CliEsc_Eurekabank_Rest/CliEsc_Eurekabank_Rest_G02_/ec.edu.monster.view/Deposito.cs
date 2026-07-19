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
    public partial class Deposito : Form
    {
        private EurekabankController _controller;
        public Deposito()
        {
            InitializeComponent();
            _controller = new EurekabankController();
        }

        private async void btnDepositar_Click(object sender, EventArgs e)
        {
            if (!ValidarEntradas(out string cuenta, out double importe))
                return;

            try
            {
                SetUiEnabled(false); // Deshabilitar UI

                // Llamar al controlador REST
                OperacionViewModel resultado = await _controller.HacerDepositoAsync(cuenta, importe);

                // Mostrar resultado (éxito o error)
                MessageBoxIcon icon = resultado.Resultado.StartsWith("Error") ? MessageBoxIcon.Error : MessageBoxIcon.Information;
                MessageBox.Show(resultado.Resultado, "Resultado de Depósito", MessageBoxButtons.OK, icon);

                if (icon == MessageBoxIcon.Information)
                {
                    LimpiarCampos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}", "Error Fatal", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                SetUiEnabled(true); // Rehabilitar UI
            }
        }

        private bool ValidarEntradas(out string cuenta, out double importe)
        {
            cuenta = txtCuenta.Text.Trim();
            string importeStr = txtImporte.Text.Trim();
            importe = 0;

            if (string.IsNullOrEmpty(cuenta) || string.IsNullOrEmpty(importeStr))
            {
                MessageBox.Show("Debe ingresar el código de cuenta y el importe.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!double.TryParse(importeStr, out importe) || importe <= 0)
            {
                MessageBox.Show("El importe debe ser un número positivo.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void LimpiarCampos()
        {
            txtCuenta.Text = "";
            txtImporte.Text = "";
            txtCuenta.Focus();
        }

        private void SetUiEnabled(bool enabled)
        {
            btnDepositar.Enabled = enabled;
            txtCuenta.Enabled = enabled;
            txtImporte.Enabled = enabled;
            Cursor = enabled ? Cursors.Default : Cursors.WaitCursor;
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            Menu menu = new Menu();
            menu.Show();
            this.Hide();
        }
    }
}
