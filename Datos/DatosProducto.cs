using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Database;
using ModeloDominio;

namespace Datos
{
    public class DatosProducto
    {
        private List<Producto> productos;
        public List<Producto> listar()
        {
			List<Producto> lista = new List<Producto>();
			AccesoDatos datos = new AccesoDatos();
			try
			{
				datos.consulta("Select A.Id Id, Codigo, Nombre, A.Descripcion, IdMarca, IdCategoria, ImagenUrl, Precio, C.Descripcion Categoria, M.Descripcion Marca From ARTICULOS A, CATEGORIAS C, MARCAS M where C.Id = IdCategoria and M.Id = IdMarca");
				datos.lectura();
				while (datos.Lector.Read())
				{
					Producto productoL = new Producto();
					productoL.Id = (int)datos.Lector["Id"];
					productoL.Codigo = (string)datos.Lector["Codigo"];
					productoL.Nombre = (string)datos.Lector["Nombre"];
					productoL.Descripcion = (string)datos.Lector["Descripcion"];
					if (!(datos.Lector["ImagenUrl"] is DBNull))
						productoL.ImagenUrl = (string)datos.Lector["ImagenUrl"];
                    productoL.Precio = Math.Round((decimal)datos.Lector["Precio"], 2, MidpointRounding.AwayFromZero);
                    productoL.Categoria = new Categoria();
					productoL.Categoria.Id = (int)datos.Lector["IdCategoria"];
					productoL.Categoria.Descripcion = (string)datos.Lector["Categoria"];
					productoL.Marca = new Marca();
					productoL.Marca.Id = (int)datos.Lector["IdMarca"];
					productoL.Marca.Descripcion = (string)datos.Lector["Marca"];
					lista.Add(productoL);
				}
				return lista;
			}
            catch (InvalidCastException)
            {
                MessageBox.Show("Hay elementos en la base de datos sin información, solicite a administración una actualización de la misma o contacte a su desarrollador.", "Error, elementos sin datos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Hay elementos en la base de datos sin información, solicite a administración una actualización de la misma o contacte a su desarrollador.", "Error, elementos sin datos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
			catch (Exception)
			{
                MessageBox.Show("Hubo un error al procesar la consulta de la base de datos, intente nuevamente, si el problema persiste, comuniquese con su desarrollador.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
			finally
			{
				datos.cerrarConexion();
			}
        }
        public void cargarDGV(DataGridView dgv)
		{
			try
			{
                dgv.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 10);
                dgv.DataSource = listar();
                ocultarColumnas(dgv);
			}
            catch (NullReferenceException)
            {
                MessageBox.Show("Hay elementos en la base de datos sin información, solicite a administracion una actualizacion de la misma o contacte a su desarrollador.");
            }
            catch (Exception)
			{
                MessageBox.Show("Hubo un error al intentar cargar la grilla, intente nuevamente, si el problema persiste, comuniquese con su desarrollador.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void ocultarColumnas(DataGridView dgv)
        {
            dgv.Columns["Id"].Visible = false;
            dgv.Columns["ImagenUrl"].Visible = false;
            dgv.Columns["Descripcion"].Visible = false;
        }
		public void agregar(Producto nuevo)
		{
			AccesoDatos datos = new AccesoDatos();
			try
			{
				datos.consulta("insert into ARTICULOS (Codigo, Nombre, Descripcion, IdMarca, IdCategoria, ImagenUrl, Precio) values (@Codigo, @Nombre, @Descripcion, @IdMarca, @IdCategoria, @Imagen, @Precio)");
				datos.parametros("@Codigo", nuevo.Codigo);
                datos.parametros("@Nombre", nuevo.Nombre);
                datos.parametros("@Descripcion", nuevo.Descripcion);
                datos.parametros("@IdMarca", nuevo.Marca.Id);
                datos.parametros("@IdCategoria", nuevo.Categoria.Id);
                datos.parametros("@Imagen", nuevo.ImagenUrl);
                datos.parametros("@Precio", nuevo.Precio);
				datos.ejecutar();
            }
			catch (Exception)
			{
                MessageBox.Show("Hubo un error al procesar la consulta de la base de datos, intente nuevamente, si el problema persiste, comuniquese con su desarrollador.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
			finally
			{
				datos.cerrarConexion();
			}
		}
		public void modificar(Producto producto)
		{
			AccesoDatos datos = new AccesoDatos();
			try
			{
				datos.consulta("update ARTICULOS set Codigo = @Codigo, Nombre = @Nombre, Descripcion = @Descripcion, IdMarca = @IdMarca, IdCategoria = @IdCategoria, ImagenUrl = @Img, Precio = @Precio where Id = @Id");
				datos.parametros("@Codigo", producto.Codigo);
                datos.parametros("@Nombre", producto.Nombre);
                datos.parametros("@Descripcion", producto.Descripcion);
                datos.parametros("@IdMarca", producto.Marca.Id);
                datos.parametros("@IdCategoria", producto.Categoria.Id);
                datos.parametros("@Img", producto.ImagenUrl);
                datos.parametros("@Precio", producto.Precio);
                datos.parametros("@Id", producto.Id);
				datos.ejecutar();
            }
			catch (Exception)
			{
                MessageBox.Show("Hubo un error al procesar la consulta de la base de datos, intente nuevamente, si el problema persiste, comuniquese con su desarrollador.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
			finally
			{
				datos.cerrarConexion();
			}
		}
		public void eliminar(Producto producto)
		{
			AccesoDatos datos = new AccesoDatos();
			try
			{
				datos.consulta("delete Articulos Where Id = @Id");
				datos.parametros("@Id", producto.Id);
				datos.ejecutar();
			}
			catch (Exception)
			{
				MessageBox.Show("Hubo un error al procesar la consulta de la base de datos, intente nuevamente, si el problema persiste, comuniquese con su desarrollador.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				datos.cerrarConexion();
			}
		}
		public List<Producto> filtrar(string txt1, string txt2, string txt3)
		{
			AccesoDatos datos = new AccesoDatos();
			productos = listar();
			List<Producto> filtrada = new List<Producto>();
            try
			{
				if(txt1 == "Precio")
				{
                    if (txt3.Contains("."))
                        txt3 = txt3.Replace(".", ",");
                    decimal precio = Math.Round(decimal.Parse(txt3), 2, MidpointRounding.AwayFromZero);
                    switch (txt2)
                    {
                        case "Mayor a":
                            filtrada = productos.FindAll(p => p.Precio > precio);
                            break;
                        case "Menor a":
                            filtrada = productos.FindAll(p => p.Precio < precio);
                            break;
                        case "Igual a":
                            filtrada = productos.FindAll(p => p.Precio == precio);
                            break;
                    }
				}
				else
				{
                    string consulta = "Select A.Id Id, Codigo, Nombre, A.Descripcion Descripcion, IdMarca, IdCategoria, ImagenUrl, Precio, C.Descripcion Categoria, M.Descripcion Marca From ARTICULOS A, CATEGORIAS C, MARCAS M where C.Id = IdCategoria and M.Id = IdMarca and ";
                    switch (txt1)
                    {
                        case "Código":
                            switch (txt2)
                            {
                                case "Empieza con":
                                    consulta += "Codigo like '" + txt3 + "%'";
                                    break;
                                case "Termina con":
                                    consulta += "Codigo like '%" + txt3 + "'";
                                    break;
                                case "Contiene":
                                    consulta += "Codigo like '%" + txt3 + "%'";
                                    break;
                            }
                            break;
                        case "Nombre":
                            switch (txt2)
                            {
                                case "Empieza con":
                                    consulta += "Nombre like '" + txt3 + "%'";
                                    break;
                                case "Termina con":
                                    consulta += "Nombre like '%" + txt3 + "'";
                                    break;
                                case "Contiene":
                                    consulta += "Nombre like '%" + txt3 + "%'";
                                    break;
                            }
                            break;
                        case "Categoría":
                            switch (txt2)
                            {
                                case "Empieza con":
                                    consulta += "C.Descripcion like '" + txt3 + "%'";
                                    break;
                                case "Termina con":
                                    consulta += "C.Descripcion like '%" + txt3 + "'";
                                    break;
                                case "Contiene":
                                    consulta += "C.Descripcion like '%" + txt3 + "%'";
                                    break;
                            }
                            break;
                        case "Marca":
                            switch (txt2)
                            {
                                case "Empieza con":
                                    consulta += "M.Descripcion like '" + txt3 + "%'";
                                    break;
                                case "Termina con":
                                    consulta += "M.Descripcion like '%" + txt3 + "'";
                                    break;
                                case "Contiene":
                                    consulta += "M.Descripcion like '%" + txt3 + "%'";
                                    break;
                            }
                            break;
                    }
                    datos.consulta(consulta);
                    datos.lectura();
                    while (datos.Lector.Read())
                    {
                        Producto filtrado = new Producto();
                        filtrado.Id = (int)datos.Lector["Id"];
                        filtrado.Codigo = (string)datos.Lector["Codigo"];
                        filtrado.Nombre = (string)datos.Lector["Nombre"];
                        filtrado.Descripcion = (string)datos.Lector["Descripcion"];
                        if (!(datos.Lector["ImagenUrl"] is DBNull))
                            filtrado.ImagenUrl = (string)datos.Lector["ImagenUrl"];
                        filtrado.Precio = Math.Round((decimal)datos.Lector["Precio"], 2, MidpointRounding.AwayFromZero);
                        filtrado.Categoria = new Categoria();
                        filtrado.Categoria.Id = (int)datos.Lector["IdCategoria"];
                        filtrado.Categoria.Descripcion = (string)datos.Lector["Categoria"];
                        filtrado.Marca = new Marca();
                        filtrado.Marca.Id = (int)datos.Lector["IdMarca"];
                        filtrado.Marca.Descripcion = (string)datos.Lector["Marca"];
                        filtrada.Add(filtrado);
                    }
                }
                if (filtrada.Count == 0)
                {
                    MessageBox.Show("No se encontro ningun articulo para la búsqueda.", "Producto no encontrado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    filtrada = productos;
                }
                return filtrada;
            }
            catch (FormatException)
            {
                MessageBox.Show("Debe introducir solo números sin símbolos en este campo para filtrar por precio.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                filtrada = productos;
                return filtrada;
            }
			catch (Exception)
			{
                MessageBox.Show("Hubo un error al procesar su solicitud. Intente nuevamente, si el problema persiste, contacte a su desarrollador", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return filtrada;
			}
			finally
			{
				datos.cerrarConexion();
			}
		}
        public void buscar(ref ToolStripTextBox txt, DataGridView dgv)
        {
            string filtro = txt.Text.ToLower();
            productos = listar();
            List<Producto> filtrada = new List<Producto>();
            if(filtro != "")
            {
                filtrada = productos.FindAll(p => p.Nombre.ToLower().Contains(filtro) || p.Codigo.ToLower().Contains(filtro) || p.Marca.Descripcion.ToLower().Contains(filtro) || p.Categoria.Descripcion.ToLower().Contains(filtro) || p.Precio.ToString().Contains(filtro));
                if(filtrada.Count == 0)
                {
                    MessageBox.Show("No se encontro el producto", "Producto no encontrado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    filtrada = productos;
                    txt.Text = "";
                }
            }
            else
                filtrada = productos;
            dgv.DataSource = null;
            dgv.DataSource = filtrada;
            ocultarColumnas(dgv);
        }
    }
}
