using CliEsc_Eurekabank_Rest_G02_.ec.edu.monster.controller;
using CliEsc_Eurekabank_Rest_G02_.ec.edu.monster.model;
using CliEsc_Eurekabank_Rest_G02_.ec.edu.monster.service;
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
        private TextBox _txtIp;

        public Login()
        {
            InitializeComponent();
            _loginController = new LoginController();
            ConfigurarControlesServidor();
        }

        /// <summary>
        /// Agrega un campo para la IP/host del servidor en la interfaz de login,
        /// de modo que se pueda cambiar sin recompilar. Se precarga con la IP guardada.
        /// </summary>
        private void ConfigurarControlesServidor()
        {
            var lblServidor = new Label
            {
                Text = "Servidor (IP):",
                AutoSize = true,
                ForeColor = System.Drawing.Color.FromArgb(38, 97, 169),
                Font = new System.Drawing.Font("Noto Sans", 10F, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(63, 495)
            };

            _txtIp = new TextBox
            {
                Name = "txtIp",
                BorderStyle = BorderStyle.FixedSingle,
                Font = new System.Drawing.Font("Noto Sans", 10F),
                Location = new System.Drawing.Point(200, 492),
                Size = new System.Drawing.Size(383, 27),
                Text = Config.Ip
            };

            this.Controls.Add(lblServidor);
            this.Controls.Add(_txtIp);
        }

        private async void btnIngresar_Click(object sender, EventArgs e)
        {
            // 0. Tomar la IP/host del servidor desde la interfaz y guardarla.
            string ip = _txtIp.Text.Trim();
            if (string.IsNullOrEmpty(ip))
            {
                MessageBox.Show("Debe ingresar la IP o host del servidor.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Config.Ip = ip;

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
