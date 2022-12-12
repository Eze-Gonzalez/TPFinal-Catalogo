using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace frmHelper
{
    public class FHelper
    {
        public void moBoton(Button btn1, Button btn2, bool visible1, bool visible2)
        {
            btn1.Visible = visible1;
            btn2.Visible = visible2;
        }
        public void moFiltro(ComboBox cbx1, ComboBox cbx2, Label lbl1, Label lbl2, Label lbl3, Label lbl4, Button btn, TextBox txt, bool visible)
        {
            cbx1.Visible = visible;
            cbx2.Visible = visible;
            lbl1.Visible = visible;
            lbl2.Visible = visible;
            lbl3.Visible = visible;
            lbl4.Visible = visible;
            btn.Visible = visible;
            txt.Visible = visible;
        }
        public void habilitarCbx(ComboBox cbxTipo, ComboBox cbxCriterio)
        {
            if(cbxTipo.SelectedIndex <= 0)
            {
                cbxCriterio.SelectedIndex = -1;
                cbxCriterio.Enabled = false;
            }
            if(cbxTipo.SelectedIndex > 0)
            {
                cbxCriterio.Enabled = true;
                string opcion = cbxTipo.SelectedItem.ToString();
                if (opcion != "Seleccione un Tipo")
                {
                    if (opcion == "Precio")
                    {
                        cbxCriterio.Items.Clear();
                        cbxCriterio.Items.Add("Seleccione un Criterio");
                        cbxCriterio.Items.Add("Menor a");
                        cbxCriterio.Items.Add("Mayor a");
                        cbxCriterio.Items.Add("Igual a");
                    }
                    else
                    {
                        cbxCriterio.Items.Clear();
                        cbxCriterio.Items.Add("Seleccione un Criterio");
                        cbxCriterio.Items.Add("Empieza con");
                        cbxCriterio.Items.Add("Termina con");
                        cbxCriterio.Items.Add("Contiene");
                    }
                }
                cbxCriterio.SelectedIndex = 0;
            }
        }
        public void cargarCriterio(ComboBox cbxTipo, ComboBox cbxCriterio)
        {

        }
        public void habilitarTxt(ComboBox cbx, TextBox txt)
        {
            if(cbx.SelectedIndex > 0)
            {
                txt.Enabled = true;
            }
            else
                txt.Enabled = false;
        }
        public bool validarFiltro(ComboBox cbxTipo, ComboBox cbxCriterio, string filtro)
        {
            if (cbxTipo.SelectedIndex < 1) 
            {
                MessageBox.Show("Debe seleccionar un Tipo para filtrar.", "Seleccione tipo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return true;
            }
            if (cbxCriterio.SelectedIndex < 1)
            {
                MessageBox.Show("Debe seleccionar un Criterio para filtrar.", "Seleccione criterio", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return true;
            }
            return false;
        }
        public void cargarImagen(PictureBox pbx, string imagen)
        {
            try
            {
                pbx.Load(imagen);
            }
            catch (Exception)
            {
                pbx.Load("https://i.imgur.com/yzczBvI.png");
            }
        }
        public void ocultarColumnas(DataGridView dgv)
        {
            dgv.Columns["ImagenUrl"].Visible = false;
            dgv.Columns["Descripcion"].Visible = false;
            dgv.Columns["Id"].Visible = false;
        }
        public void ocultarInicio(Button btn1, Button btn2, PictureBox pbx)
        {
            btn1.Visible = false;
            btn2.Visible = false;
            pbx.Visible = false;
        }
    }
}
