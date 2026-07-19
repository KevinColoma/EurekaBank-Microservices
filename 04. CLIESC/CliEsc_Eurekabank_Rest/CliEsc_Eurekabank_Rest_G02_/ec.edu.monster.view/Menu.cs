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
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void btnRetirar_Click(object sender, EventArgs e)
        {
            Retiro retiro = new Retiro();
            retiro.Show();
            this.Hide();
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            Movimiento movimiento = new Movimiento();
            movimiento.Show();
            this.Hide();
        }

        private void btnDepositar_Click(object sender, EventArgs e)
        {
            Deposito deposito = new Deposito();
            deposito.Show();
            this.Hide();
        }

        private void btnTransferir_Click(object sender, EventArgs e)
        {
            Transferencia transferencia = new Transferencia();
            transferencia.Show();
            this.Hide();
        }
    }
}
