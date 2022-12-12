using Datos;
using frmHelper;
using ModeloDominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CatalogoProductos
{
    public partial class frmDetalles : Form
    {
        private Producto producto;
        public frmDetalles()
        {
            InitializeComponent();
        }
        public frmDetalles(Producto producto)
        {
            InitializeComponent();
            this.producto = producto;
        }
        private void timerFechaHora_Tick(object sender, EventArgs e)
        {
            toolStripStatusFechaHora.Text = DateTime.Now.ToString();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            frmAgregar modificar = new frmAgregar(producto);
            modificar.ShowDialog();
            mostrarDetalle();
        }

        private void frmDetalles_Load(object sender, EventArgs e)
        {
            mostrarDetalle();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            DatosProducto datos = new DatosProducto();
            DialogResult resultado = MessageBox.Show("Está a punto de eliminar el producto " + producto.Nombre + ", esta acción no se puede revertir, ¿Desea continuar?", "Eliminar Producto", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (resultado == DialogResult.Yes)
                datos.eliminar(producto);
            Close();
        }
        private void mostrarDetalle()
        { 
            Text = "Detalles de " + producto.Nombre;
            FHelper helper = new FHelper();
            txtCodigo.Text = " " + producto.Codigo;
            txtNombre.Text = " " + producto.Nombre;
            txtDescripcion.Text = " " + producto.Descripcion;
            txtPrecio.Text = " $" + producto.Precio.ToString();
            txtCategoria.Text = " " + producto.Categoria.Descripcion;
            txtMarca.Text = " " + producto.Marca.Descripcion;
            helper.cargarImagen(pbxProducto, producto.ImagenUrl);
            switch (producto.Marca.Descripcion)
            {
                case "Samsung":
                    helper.cargarImagen(pbxMarca, "https://i.imgur.com/M5LXLYu.png");
                    break;
                case "Apple":
                    helper.cargarImagen(pbxMarca, "https://i.imgur.com/U1s0VmS.png");
                    break;
                case "Sony":
                    helper.cargarImagen(pbxMarca, "https://i.imgur.com/cbAzPb4.png");
                    break;
                case "Huawei":
                    helper.cargarImagen(pbxMarca, "https://i.imgur.com/OOrscVu.png");
                    break;
                case "Motorola":
                    helper.cargarImagen(pbxMarca, "https://i.imgur.com/REjZrXB.png");
                    break;
            }
        }
    }
}
