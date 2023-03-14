using Datos;
using Entidades;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Vista
{
    public partial class ProductosForm : Syncfusion.Windows.Forms.Office2010Form
    {
        public ProductosForm()
        {
            InitializeComponent();
        }

        //Variable para almacenar que boton dio clic el usuario
        string operacion;

        Producto producto;

        ProductoDB productoDB = new ProductoDB();

        DataTable dt = new DataTable();

        private void btnNuevo_Click(object sender, EventArgs e)
        {

            operacion = "Nuevo";
            HabilitarControles();
            txtCodigo.Focus();
        }

        //Habilitar Controles
        private void HabilitarControles()
        {
            txtCodigo.Enabled = true;
            txtDescripcion.Enabled = true;
            txtExistencia.Enabled = true;
            txtPrecio.Enabled = true;
            cbxEstaActivo.Checked = false;



            btnAjuntarImagen.Enabled = true;
            btnGuardar.Enabled = true;
            btnEliminar.Enabled = true;
            btnCancelar.Enabled = true;
            btnModificar.Enabled = false;

            btnNuevo.Enabled = false;
        }

        private void LimpiarControles()
        {
            txtCodigo.Clear();
            txtDescripcion.Clear();
            txtExistencia.Clear();
            txtPrecio.Clear();
            cbxEstaActivo.Checked = false;
            ImagenPictureBox.Image = null;
            producto = null;

        }


        private void DesHabilitarControles()
        {
            txtCodigo.Enabled = false;
            txtDescripcion.Enabled = false;
            txtExistencia.Enabled = false;
            txtPrecio.Enabled = false;
            btnAjuntarImagen.Enabled = false;
            btnGuardar.Enabled = false;
            btnGuardar.Enabled = false;
            btnEliminar.Enabled = false;
            btnCancelar.Enabled = false;
            btnModificar.Enabled = true;

            btnNuevo.Enabled = true;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DesHabilitarControles();

            LimpiarControles();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            operacion = "Modificar";
            if (dgvProductos.SelectedRows.Count > 0)
            {
                //pasamos los datos a controles(txt)
                txtCodigo.Text = dgvProductos.CurrentRow.Cells["Codigo"].Value.ToString();

                txtDescripcion.Text = dgvProductos.CurrentRow.Cells["Descripcion"].Value.ToString();

                txtExistencia.Text = dgvProductos.CurrentRow.Cells["Existencia"].Value.ToString();

                txtPrecio.Text = dgvProductos.CurrentRow.Cells["Precio"].Value.ToString();

                cbxEstaActivo.Checked = Convert.ToBoolean(dgvProductos.CurrentRow.Cells["EstaActivo"].Value);

                //foto de producto

                byte[] img = productoDB.DevolverFoto(dgvProductos.CurrentRow.Cells["Codigo"].Value.ToString());

                if (img.Length > 0)
                {
                    MemoryStream ms = new MemoryStream(img);
                    ImagenPictureBox.Image = System.Drawing.Bitmap.FromStream(ms);
                }
                HabilitarControles();

                //llave primaria no se puede modificar, //propiedad solo lectura ,para que el usuario no lo cambie
                txtCodigo.ReadOnly = true;



            }
            else
            {
                MessageBox.Show("Seleccione un registro");
            }

        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {

            producto = new Producto();
            //la variable trae almacenado que se hizo,si guardo un producto nuevo o modifico uno ya existente
            if (operacion == "Nuevo")
            {
                //validamos que el usuario ingreso los datos o por lo menos los obligatorios

                //Ttx Codigo
                if (string.IsNullOrEmpty(txtCodigo.Text))
                {
                    errorProvider1.SetError(txtCodigo, "Ingrese un código");
                    txtCodigo.Focus();
                    return;
                }
                errorProvider1.Clear();

                //Ttx Descripción
                if (string.IsNullOrEmpty(txtDescripcion.Text))
                {
                    errorProvider1.SetError(txtDescripcion, "Ingrese una Descripción");
                    txtDescripcion.Focus();
                    return;
                }
                errorProvider1.Clear();

                //Ttx Existencia
                if (string.IsNullOrEmpty(txtExistencia.Text))
                {
                    errorProvider1.SetError(txtExistencia, "Ingrese una Descripción");
                    txtExistencia.Focus();
                    return;
                }
                errorProvider1.Clear();

                //Ttx Precio
                if (string.IsNullOrEmpty(txtPrecio.Text))
                {
                    errorProvider1.SetError(txtPrecio, "Ingrese una Descripción");
                    txtPrecio.Focus();
                    return;
                }
                errorProvider1.Clear();

                producto.Codigo = txtCodigo.Text;
                producto.Descripcion = txtDescripcion.Text;
                producto.Precio = Convert.ToDecimal(txtPrecio.Text);
                producto.Existencia = Convert.ToInt32(txtExistencia.Text);
                producto.EstaActivo = cbxEstaActivo.Checked;

                //codigo de foto

                if (ImagenPictureBox.Image != null)
                {
                    //la clase memorystream sirve para manejar archivos(audios,pdf,iamgenes,etc)
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    ImagenPictureBox.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    producto.Foto = ms.GetBuffer();
                }

                bool inserto = productoDB.Insertar(producto);

                if (inserto)
                {
                    DesHabilitarControles();
                    LimpiarControles();
                    TraerProductos();
                    MessageBox.Show("Registro guardado con exito", "Listo", MessageBoxButtons.OK, MessageBoxIcon.Information);


                }
                else
                {
                    MessageBox.Show("No se pudo guardar el registro", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            }
            else if (operacion == "Modificar")
            {
                producto.Codigo = txtCodigo.Text;
                producto.Descripcion = txtDescripcion.Text;
                producto.Precio = Convert.ToDecimal(txtPrecio.Text);
                producto.Existencia = Convert.ToInt32(txtExistencia.Text);
                producto.EstaActivo = cbxEstaActivo.Checked;

                //codigo de foto

                if (ImagenPictureBox.Image != null)
                {
                    //la clase memorystream sirve para manejar archivos(audios,pdf,iamgenes,etc)
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    ImagenPictureBox.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    producto.Foto = ms.GetBuffer();
                }

                bool modifico = productoDB.Editar(producto);
                if (modifico)
                {
                    //propiedad solo lectura ,para que el usuario no lo cambie
                    txtCodigo.ReadOnly = false;

                    DesHabilitarControles();
                    LimpiarControles();
                    TraerProductos();
                    MessageBox.Show("Registro Actualizado con exito", "Listo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                {
                    MessageBox.Show("No se pudo guardar el registro", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }




            }

        }

        private void txtExistencia_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            //Validamos con evento KeyPress para evitar que el usuario ingrese letras.

            //En la variable e ,viene la tecla/valor que el usuario introdujo.

            //si esto no es un numero
            if (!char.IsNumber(e.KeyChar))
            {
                //omite y no escribe 
                e.Handled = true;
            }
        }

        private void txtPrecio_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && e.KeyChar != '\b')
            {
                e.Handled = true;
            }

            //validamos para que no ingrese más de 2 decimales
            if ((e.KeyChar == '.') && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }


        }

        private void btnAjuntarImagen_Click(object sender, EventArgs e)
        {
            //Abre ventana de archivos de la pc del usuario
            OpenFileDialog Foto = new OpenFileDialog();
            DialogResult resultado = Foto.ShowDialog();

            //ese dialog.ok significa que el usuario si subio la imagen
            if (resultado == DialogResult.OK)
            {
                ImagenPictureBox.Image = Image.FromFile(Foto.FileName);

            }
        }

        private void ProductosForm_Load(object sender, EventArgs e)
        {
            TraerProductos();
        }

        private void TraerProductos()


        {
            dgvProductos.DataSource = productoDB.DevolverProductos();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvProductos.SelectedRows.Count > 0)
            {
                DialogResult resultado = MessageBox.Show("Esta seguro de eliminar registro", "ADVERTENCIA", MessageBoxButtons.YesNo);

                if (resultado == DialogResult.Yes)
                {
                    bool elimino = productoDB.Eliminar(dgvProductos.CurrentRow.Cells["Codigo"].Value.ToString());

                    if (elimino)
                    {
                        LimpiarControles();
                        DesHabilitarControles();
                        TraerProductos();
                        MessageBox.Show("Registro eliminado");
                    }
                    else
                    {
                        MessageBox.Show("No se pudo eliminar el registro");
                    }

                }

            }
            else
            {
                MessageBox.Show("Debe Seleccionar un Registro");
            }
        }
    }
}
