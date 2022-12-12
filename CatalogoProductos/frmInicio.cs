using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using frmHelper;

namespace CatalogoProductos
{
    public partial class frmInicio : Form
    {
        public frmInicio()
        {
            InitializeComponent();
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            frmProductos nuevo = new frmProductos();
            nuevo.Owner = this;
            nuevo.Show();
            this.Hide();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            lblFechaHora.Text = DateTime.Now.ToString();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            DialogResult cerrar = MessageBox.Show("¿Desea cerrar la aplicación?", "Cerrar aplicación", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if(cerrar == DialogResult.OK)
                Application.Exit();
        }
    }
}
