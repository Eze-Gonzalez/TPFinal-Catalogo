using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ModeloDominio;
using Datos;
using frmHelper;
using System.IO;
using System.Configuration;

namespace CatalogoProductos
{
    public partial class frmAgregar : Form
    {
        private Producto producto = null;
        private OpenFileDialog imagen = null;
        public frmAgregar()
        {
            InitializeComponent();
        }
        public frmAgregar(Producto producto)
        {
            InitializeComponent();
            this.producto = producto;
            Text = "Modificar Producto";
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            DatosProducto datos = new DatosProducto();
            int cont = 0;
            try
            {
                if (producto == null)
                    producto = new Producto();
                if (validarCampo50(txtCodigo))
                {
                    producto.Codigo = txtCodigo.Text;
                    cont++;
                }
                if (validarCampo50(txtNombre))
                {
                    producto.Nombre = txtNombre.Text;
                    cont++;
                }
                if (validarDescripcion())
                {
                    producto.Descripcion = txtDescripcion.Text;
                    cont++;
                }
                if (txtPrecio.Text.Contains("."))
                    txtPrecio.Text = txtPrecio.Text.Replace(".", ",");
                producto.Precio = decimal.Parse(txtPrecio.Text);
                cont++;
                if (validarCategoria())
                {
                    producto.Categoria = (Categoria)cbxCategoria.SelectedItem;
                    cont++;
                }
                if (validarMarca())
                {
                    producto.Marca = (Marca)cbxMarca.SelectedItem;
                    cont++;
                }
                if(txtImagen.Text.Length <= 1000)
                {
                    cont++;
                    if (imagen != null && !(txtImagen.Text.ToLower().Contains("http")))
                        File.Copy(imagen.FileName, ConfigurationManager.AppSettings["directorio-Imagenes"] + DateTime.Now.ToString("T").Replace(":", "-") + imagen.SafeFileName);
                }
                else
                {
                    txtImagen.ForeColor = Color.Red;
                    txtImagen.Text = "Este enlace o ruta es demasiado largo, intente con uno mas corto.";
                }
                if (producto.Id != 0 && cont == 7)
                {
                    producto.ImagenUrl = txtImagen.Text;
                    datos.modificar(producto);
                    MessageBox.Show("El producto " + producto.Nombre + ", ha sido modificado exitosamente.", "Modificado exitosamente", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
                else if (cont == 7)
                {
                    producto.ImagenUrl = txtImagen.Text;
                    datos.agregar(producto);
                    MessageBox.Show("El producto " + producto.Nombre + ", ha sido agregado exitosamente.", "Agregado exitosamente", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
                else
                    MessageBox.Show("Hay campos con errores o incompletos, revise el formulario e intente nuevamente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (FormatException)
            {
                validarMarca();
                validarCategoria();
                txtPrecio.ForeColor = Color.Red;
                txtPrecio.Text = "Debe completar este campo solo con números, sin símbolos ni separación de mil.";
                MessageBox.Show("Hay campos con errores o incompletos, revise el formulario e intente nuevamente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmAgregar_Load(object sender, EventArgs e)
        {
            DatosMarca marca = new DatosMarca();
            DatosCategoria categoria = new DatosCategoria();
            try
            {
                cbxMarca.DataSource = marca.listar();
                cbxMarca.ValueMember = "Id";
                cbxMarca.DisplayMember = "Descripcion";
                cbxCategoria.DataSource = categoria.listar();
                cbxCategoria.ValueMember = "Id";
                cbxCategoria.DisplayMember = "Descripcion";

                if (producto != null)
                {
                    txtCodigo.Text = producto.Codigo;
                    txtNombre.Text = producto.Nombre;
                    txtDescripcion.Text = producto.Descripcion;
                    txtImagen.Text = producto.ImagenUrl;
                    txtPrecio.Text = producto.Precio.ToString();
                    cbxCategoria.SelectedValue = producto.Categoria.Id;
                    cbxMarca.SelectedValue = producto.Marca.Id;
                }
                else
                {
                    cbxMarca.SelectedIndex = -1;
                    cbxCategoria.SelectedIndex = -1;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void txtImagen_TextChanged(object sender, EventArgs e)
        {
            FHelper helper = new FHelper();
            helper.cargarImagen(pbxProducto, txtImagen.Text);
        }
        private bool validarCampo50(TextBox campo)
        {
            string texto = campo.Text;
            if(texto.Length > 50)
            {
                campo.ForeColor = Color.Red;
                campo.Text = "Se admite un máximo de 50 caracteres en este campo, usted ingresó " + texto.Length + " caracteres.";
                return false;
            }
            else
            {
                if (texto == "" || texto == "Debe completar este campo.")
                {
                    campo.ForeColor = Color.Red;
                    campo.Text = "Debe completar este campo.";
                    return false;
                }
            }
            return true;
        }
        private bool validarDescripcion()
        {
            if(txtDescripcion.Text.Length > 150)
            {
                txtDescripcion.ForeColor = Color.Red;
                txtDescripcion.Text = "Se admite un máximo de 150 caracteres en este campo, usted ingresó " + txtDescripcion.Text.Length + " caracteres.";
                return false;
            }
            if (txtDescripcion.Text == "" || txtDescripcion.Text == "Debe completar este campo.")
            {
                txtDescripcion.ForeColor = Color.Red;
                txtDescripcion.Text = "Debe completar este campo.";
                return false;
            }
            return true;
        }
        private bool validarCategoria()
        {
            if (cbxCategoria.SelectedIndex < 0)
            {
                lblWCategoria.Visible = true;
                return false;
            }
            return true;
        }
        private bool validarMarca()
        {
            if (cbxMarca.SelectedIndex < 0)
            {
                lblWMarca.Visible = true;
                return false;
            }
            return true;
        }

        private void cbxCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblWCategoria.Visible = false;
        }

        private void cbxMarca_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblWMarca.Visible = false;
        }

        private void txtCodigo_Enter(object sender, EventArgs e)
        {
            if (txtCodigo.Text == "Debe completar este campo." || txtCodigo.Text.Contains("Se admite un máximo de 50 caracteres en este campo"))
            {
                txtCodigo.Text = "";
                txtCodigo.ForeColor = Color.Black;
            }
        }

        private void txtNombre_Enter(object sender, EventArgs e)
        {
            if (txtNombre.Text == "Debe completar este campo." || txtNombre.Text.Contains("Se admite un máximo de 50 caracteres en este campo"))
            {
                txtNombre.Text = "";
                txtNombre.ForeColor = Color.Black;
            }
        }

        private void txtDescripcion_Enter(object sender, EventArgs e)
        {
            if (txtDescripcion.Text == "Debe completar este campo." || txtDescripcion.Text.Contains("Se admite un máximo de 150 caracteres en este campo"))
            {
                txtDescripcion.Text = "";
                txtDescripcion.ForeColor = Color.Black;
            }
        }

        private void txtImagen_TextChanged_1(object sender, EventArgs e)
        {
            FHelper helper = new FHelper();
            helper.cargarImagen(pbxProducto, txtImagen.Text);
        }

        private void txtPrecio_Enter(object sender, EventArgs e)
        {
            if (txtPrecio.Text == "Debe completar este campo solo con números, sin símbolos ni separación de mil.")
            {
                txtPrecio.Text = "";
                txtPrecio.ForeColor = Color.Black;
            }
        }
        private void btnImagen_Click(object sender, EventArgs e)
        {
            imagen = new OpenFileDialog();
            FHelper helper = new FHelper();
            imagen.Filter = "jpg|*.jpg;|png|*.png;|bmp|*.bmp";
            if(imagen.ShowDialog() == DialogResult.OK)
            {
                txtImagen.Text = imagen.FileName;
                helper.cargarImagen(pbxProducto, imagen.FileName);
            }
        }
        private bool validarCaracter50(string texto)
        {
            if (texto.Length <= 50)
            {
                return true;
            }
            else
                return false;
        }

        private void txtImagen_Enter(object sender, EventArgs e)
        {
            if(txtImagen.Text == "Este enlace o ruta es demasiado largo, intente con uno mas corto.")
            {
                txtImagen.Text = "";
                txtImagen.ForeColor = Color.Black;
            }
        }
    }
}
