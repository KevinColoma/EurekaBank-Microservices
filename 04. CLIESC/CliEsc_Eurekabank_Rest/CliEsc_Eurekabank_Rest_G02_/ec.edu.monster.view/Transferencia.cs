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
    public partial class Transferencia : Form
    {
        private EurekabankController _controller;
        public Transferencia()
        {
            InitializeComponent();
            _controller = new EurekabankController();
        }

        private async void btnTransferir_Click(object sender, EventArgs e)
        {
            if (!ValidarEntradas(out string origen, out string destino, out double importe))
                return;

            try
            {
                SetUiEnabled(false); // Deshabilitar UI

                // Llamar al controlador REST
                OperacionViewModel resultado = await _controller.HacerTransferenciaAsync(origen, destino, importe);

                // Mostrar resultado (éxito o error)
                MessageBoxIcon icon = resultado.Resultado.StartsWith("Error") ? MessageBoxIcon.Error : MessageBoxIcon.Information;
                MessageBox.Show(resultado.Resultado, "Resultado de Transferencia", MessageBoxButtons.OK, icon);

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

        private bool ValidarEntradas(out string origen, out string destino, out double importe)
        {
            origen = txtOrigen.Text.Trim();
            destino = txtDestino.Text.Trim();
            string importeStr = txtImporte.Text.Trim();
            importe = 0;

            if (string.IsNullOrEmpty(origen) || string.IsNullOrEmpty(destino) || string.IsNullOrEmpty(importeStr))
            {
                MessageBox.Show("Debe ingresar la cuenta de origen, destino y el importe.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (origen.Equals(destino, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("La cuenta de origen y destino no pueden ser la misma.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            txtOrigen.Text = "";
            txtDestino.Text = "";
            txtImporte.Text = "";
            txtOrigen.Focus();
        }

        private void SetUiEnabled(bool enabled)
        {
            btnTransferir.Enabled = enabled;
            txtOrigen.Enabled = enabled;
            txtDestino.Enabled = enabled;
            txtImporte.Enabled = enabled;
            Cursor = enabled ? Cursors.Default : Cursors.WaitCursor;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Menu menu = new Menu();
            menu.Show();
            this.Hide();
        }
    }
}
