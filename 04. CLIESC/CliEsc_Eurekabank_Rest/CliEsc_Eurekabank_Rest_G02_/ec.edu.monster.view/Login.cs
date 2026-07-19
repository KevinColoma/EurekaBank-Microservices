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
    public partial class Login : Form
    {
        private LoginController _loginController;
        public Login()
        {
            InitializeComponent();
            _loginController = new LoginController();
        }

        private async void btnIngresar_Click(object sender, EventArgs e)
        {
            // 1. Crear el ViewModel con los datos de la UI
            var model = new LoginViewModel
            {
                Usuario = txtUsuario.Text.Trim(),
                Contrasena = txtContrasena.Text // Asumiendo txtConstrasena
            };

            // Validar que los campos no estén vacíos
            if (string.IsNullOrEmpty(model.Usuario) || string.IsNullOrEmpty(model.Contrasena))
            {
                MessageBox.Show("Debe ingresar usuario y contraseña.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // 2. Deshabilitar UI mientras se procesa la solicitud
                SetUiEnabled(false);

                // 3. Llamar al controlador (asincrónicamente)
                bool loginExitoso = await _loginController.AutenticarAsync(model);

                // 5. Reaccionar al resultado
                if (loginExitoso)
                {
                    // Si es exitoso, cerramos este formulario y abrimos el principal
                    MessageBox.Show("¡Bienvenido!", "Login Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Aquí deberías abrir tu formulario principal (frmMenu)
                    Menu menu = new Menu();
                    menu.Show();
                    this.Hide();
                }
                else
                {
                    // Si falla, mostramos el error que el controlador nos dio
                    MessageBox.Show(model.MensajeError, "Error de Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                // Captura de cualquier error inesperado
                MessageBox.Show($"Error inesperado: {ex.Message}", "Error Fatal", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                // 4. Habilitar UI, incluso si hubo un error
                SetUiEnabled(true);
            }
        }

        /// <summary>
        /// Habilita o deshabilita los controles durante la espera de la API.
        /// </summary>
        private void SetUiEnabled(bool enabled)
        {
            btnIngresar.Enabled = enabled;
            txtUsuario.Enabled = enabled;
            txtContrasena.Enabled = enabled; // Asumiendo txtConstrasena

            btnIngresar.Text = enabled ? "Ingresar" : "Ingresando...";
            Cursor = enabled ? Cursors.Default : Cursors.WaitCursor;
        }
    }
}
