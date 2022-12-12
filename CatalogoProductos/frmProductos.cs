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
using Datos;
using ModeloDominio;
using System.Globalization;
using System.IO;

namespace CatalogoProductos
{
    public partial class frmProductos : Form
    {
        private FHelper helper = new FHelper();
        private DatosProducto datos = new DatosProducto();
        public frmProductos()
        {
            InitializeComponent();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            helper.moFiltro(cbxTipo, cbxCriterio, lblTipo, lblCriterio, lblFiltro, lblMensaje, btnFiltro, txtFiltro, true);
            helper.moBoton(btnBuscar, btnMostrar, false, true);
            cbxTipo.Items.Clear();
            cbxTipo.Items.Add("Seleccione un Tipo");
            cbxTipo.Items.Add("Código");
            cbxTipo.Items.Add("Nombre");
            cbxTipo.Items.Add("Categoría");
            cbxTipo.Items.Add("Marca");
            cbxTipo.Items.Add("Precio");
            cbxTipo.SelectedIndex = 0;
        }

        private void btnMostrar_Click(object sender, EventArgs e)
        {
            txtFiltro.Text = "";
            helper.moFiltro(cbxTipo, cbxCriterio, lblTipo, lblCriterio, lblFiltro, lblMensaje, btnFiltro, txtFiltro, false);
            helper.moBoton(btnBuscar, btnMostrar, true, false);
            datos.cargarDGV(dgvProductos);
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAgregar agregar = new frmAgregar();
            agregar.ShowDialog();
            datos.cargarDGV(dgvProductos);
        }

        private void btnDetalles_Click(object sender, EventArgs e)
        {
            if(dgvProductos.CurrentRow != null)
            {
                Producto producto = (Producto)dgvProductos.CurrentRow.DataBoundItem;
                frmDetalles detalles = new frmDetalles(producto);
                detalles.ShowDialog();
                datos.cargarDGV(dgvProductos);
            }  
        }

        private void frmProductos_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 168; i++) //No se si se puede modificar el espacio donde inicia el 
            {                             //textbox de busqueda en un tsm,
                lblPrueba.Text += " ";    //con esto hago que se mueva para que no quede tan
            }                             //pegado a los botones del menu
            datos.cargarDGV(dgvProductos);
        }

        private void dgvProductos_SelectionChanged(object sender, EventArgs e)
        {
            if(dgvProductos.CurrentRow != null)
                cargarImagen();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if(dgvProductos.CurrentRow != null)
            {
                Producto producto = (Producto)dgvProductos.CurrentRow.DataBoundItem;
                frmAgregar modificar = new frmAgregar(producto);
                modificar.ShowDialog();
                datos.cargarDGV(dgvProductos);
            }
            else
                MessageBox.Show("Debe seleccionar un item de la lista", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void cbxTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxTipo.SelectedIndex == 5)
                lblFiltro.Text = "Filtro*";
            else
                lblFiltro.Text = "Filtro";
            if (cbxTipo.SelectedIndex > 0)
                helper.habilitarCbx(cbxTipo, cbxCriterio);
            else
                helper.habilitarCbx(cbxTipo, cbxCriterio);
        }

        private void btnFiltro_Click(object sender, EventArgs e)
        {
            if(helper.validarFiltro(cbxTipo, cbxCriterio, txtFiltro.Text))
            {
                return;
            }
            dgvProductos.DataSource = datos.filtrar(cbxTipo.SelectedItem.ToString(), cbxCriterio.SelectedItem.ToString(), txtFiltro.Text);
            helper.ocultarColumnas(dgvProductos);

        }
        private void cargarImagen()
        {
       
            Producto producto = (Producto)dgvProductos.CurrentRow.DataBoundItem;
            try
            {
                pbxProductos.Load(producto.ImagenUrl);
            }
            catch (Exception)
            {
                pbxProductos.Load("https://i.imgur.com/yzczBvI.png");
            }
        }
        private void cbxCriterio_SelectedIndexChanged(object sender, EventArgs e)
        {
            helper.habilitarTxt(cbxCriterio, txtFiltro);
        }
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            Producto producto = (Producto)dgvProductos.CurrentRow.DataBoundItem;
            DialogResult resultado = MessageBox.Show("Está a punto de eliminar el producto " + producto.Nombre + ", esta acción no se puede revertir, ¿Desea continuar?", "Eliminar Producto", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (resultado == DialogResult.Yes)
                datos.eliminar(producto);
            datos.cargarDGV(dgvProductos);
        }
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            cerrar();
        }
        private void frmProductos_FormClosing(object sender, FormClosingEventArgs e)
        {
                Application.Exit();
        }
        private void cerrar()
        {
            DialogResult cerrar = MessageBox.Show("¿Desea cerrar la aplicación?", "Cerrando aplicación", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (cerrar == DialogResult.OK)
                this.Close();
                
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblFechaHora.Text = DateTime.Now.ToString();
        }

        private void tsbAgregar_Click(object sender, EventArgs e)
        {
            frmAgregar agregar = new frmAgregar();
            agregar.ShowDialog();
            datos.cargarDGV(dgvProductos);
        }

        private void tsbModificar_Click(object sender, EventArgs e)
        {
            Producto producto = (Producto)dgvProductos.CurrentRow.DataBoundItem;
            frmAgregar modificar = new frmAgregar(producto);
            modificar.ShowDialog();
            datos.cargarDGV(dgvProductos);
        }

        private void tsbDetalles_Click(object sender, EventArgs e)
        {
            Producto producto = (Producto)dgvProductos.CurrentRow.DataBoundItem;
            frmDetalles detalles = new frmDetalles(producto);
            detalles.ShowDialog();
        }

        private void tsbEliminar_Click(object sender, EventArgs e)
        {
            Producto producto = (Producto)dgvProductos.CurrentRow.DataBoundItem;
            DialogResult resultado = MessageBox.Show("Está a punto de eliminar el producto " + producto.Nombre + ", esta acción no se puede revertir, ¿Desea continuar?", "Eliminar Producto", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (resultado == DialogResult.Yes)
                datos.eliminar(producto);
            datos.cargarDGV(dgvProductos);
        }

        private void tsbSalir_Click(object sender, EventArgs e)
        {
            cerrar();
        }

        private void txtBusqueda_Enter(object sender, EventArgs e)
        {
            txtBusqueda.Text = "";
        }

        private void txtBusqueda_TextChanged(object sender, EventArgs e)
        {
            DatosProducto datos = new DatosProducto();
            if (txtBusqueda.Text != "Buscar un producto.")
                datos.buscar(ref txtBusqueda, dgvProductos);
        }

        private void txtBusqueda_Leave(object sender, EventArgs e)
        {
            dgvProductos.Select();
            txtBusqueda.Text = "Buscar un producto.";
        }
    }
}
