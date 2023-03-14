using Datos;
using Entidades;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Vista
{
    public partial class UsuariosForm : Syncfusion.Windows.Forms.Office2010Form
    {
        public UsuariosForm()
        {
            InitializeComponent();

        }
        //variable que informara que boton presiono el usuario
        string TipoOperacion = "";

        //variable de tabla de usuarios,recibe el metodo devolver Usuarios

        DataTable dt = new DataTable();
        UsuarioDB UsuarioDB = new UsuarioDB();

        Usuario user = new Usuario();



        private void btnEliminar_Click(object sender, System.EventArgs e)
        {
            if (dgvUsuarios.SelectedRows.Count > 0)
            {
                DialogResult resultado = MessageBox.Show("Esta seguro de eliminar registro", "ADVERTENCIA", MessageBoxButtons.YesNo);

                if (resultado == DialogResult.Yes)
                {
                    bool elimino = UsuarioDB.Eliminar(dgvUsuarios.CurrentRow.Cells["CodigoUsuario"].Value.ToString());

                    if (elimino)
                    {
                        LimpiarControles();
                        DeshabilitarControles();
                        TraerUsuarios();
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

        private void btnNuevo_Click(object sender, System.EventArgs e)
        {
            txtCodigo.Focus();
            HabilitarControles();
            TipoOperacion = "Nuevo";
        }

        //procedimiento que habilita los controles
        private void HabilitarControles()
        {
            txtNombre.Enabled = true;
            txtCodigo.Enabled = true;
            txtContrasena.Enabled = true;
            txtCorreo.Enabled = true;
            btnAdjuntarFoto.Enabled = true;
            cbxRol.Enabled = true;
            EstaActivoCheckBox.Enabled = true;

            //botones
            btnGuardar.Enabled = true;
            btnCancelar.Enabled = true;
            btnModificar.Enabled = false;



        }

        private void btnCancelar_Click(object sender, System.EventArgs e)
        {
            DeshabilitarControles();
            LimpiarControles();
        }
        private void DeshabilitarControles()
        {
            txtNombre.Enabled = false;
            txtCodigo.Enabled = false;
            txtContrasena.Enabled = false;
            txtCorreo.Enabled = false;
            btnAdjuntarFoto.Enabled = false;
            cbxRol.Enabled = false;
            EstaActivoCheckBox.Enabled = false;

            //botones
            btnGuardar.Enabled = false;
            btnCancelar.Enabled = false;
            btnModificar.Enabled = true;
        }

        private void LimpiarControles()
        {
            txtNombre.Clear();
            txtCodigo.Clear();
            txtContrasena.Clear();
            txtCorreo.Clear();
            cbxRol.Text = string.Empty;
            EstaActivoCheckBox.Checked = false;
            FotoPictureBox.Image = null;

        }

        private void btnGuardar_Click(object sender, System.EventArgs e)
        {
            if (TipoOperacion == "Nuevo")
            {
                if (string.IsNullOrEmpty(txtCodigo.Text))
                {
                    errorProvider1.SetError(txtCodigo, "Ingrese un Codigo");
                    txtCodigo.Focus();
                    return;
                }
                errorProvider1.Clear();

                if (string.IsNullOrEmpty(txtNombre.Text))
                {
                    errorProvider1.SetError(txtNombre, "Ingrese el Nombre");
                    txtNombre.Focus();
                    return;
                }
                errorProvider1.Clear();

                if (string.IsNullOrEmpty(txtContrasena.Text))
                {
                    errorProvider1.SetError(txtContrasena, "Ingrese la Contraseña");
                    txtContrasena.Focus();
                    return;
                }
                errorProvider1.Clear();
                ;



                user.CodigoUsuario = txtCodigo.Text;
                user.Nombre = txtNombre.Text;
                user.Contrasena = txtContrasena.Text;
                user.Rol = cbxRol.Text;
                user.Correo = txtCorreo.Text;
                user.EstaActivo = EstaActivoCheckBox.Checked;

                //validar que el usuario adjunto la foto correctamente

                if (FotoPictureBox.Image != null)
                {
                    //la clase memorystream sirve para manejar archivos(audios,pdf,iamgenes,etc)
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    FotoPictureBox.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    user.Foto = ms.GetBuffer();
                }




                //Insertar en Base de Datos
                bool inserto = UsuarioDB.Insertar(user);

                if (inserto)
                {
                    LimpiarControles();
                    DeshabilitarControles();
                    TraerUsuarios();
                    MessageBox.Show("Registro Guardado");
                }
                else
                {
                    MessageBox.Show("No se pudo realizar el registro");
                }


            }
            else if (TipoOperacion == "Modificar")
            {
                user.CodigoUsuario = txtCodigo.Text;
                user.Nombre = txtNombre.Text;
                user.Contrasena = txtContrasena.Text;
                user.Rol = cbxRol.Text;
                user.Correo = txtCorreo.Text;
                user.EstaActivo = EstaActivoCheckBox.Checked;

                //validar que el usuario adjunto la foto correctamente

                if (FotoPictureBox.Image != null)
                {
                    //la clase memorystream sirve para manejar archivos(audios,pdf,iamgenes,etc)
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    FotoPictureBox.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    user.Foto = ms.GetBuffer();
                }

                bool modifico = UsuarioDB.Editar(user);
                if (modifico)
                {
                    LimpiarControles();
                    DeshabilitarControles();
                    TraerUsuarios();
                    MessageBox.Show("Registro Actualizado Correctamente");
                }
                else
                {
                    MessageBox.Show("No se pudo Acualizar");
                }
            }

        }

        private void btnModificar_Click(object sender, System.EventArgs e)
        {
            TipoOperacion = "Modificar";
            if (dgvUsuarios.SelectedRows.Count > 0)
            {
                // txt       dataGriedView      Propiedad      nombre BD      propiedad
                txtCodigo.Text = dgvUsuarios.CurrentRow.Cells["CodigoUsuario"].Value.ToString();
                txtNombre.Text = dgvUsuarios.CurrentRow.Cells["Nombre"].Value.ToString();
                txtContrasena.Text = dgvUsuarios.CurrentRow.Cells["Contrasena"].Value.ToString();
                txtCorreo.Text = dgvUsuarios.CurrentRow.Cells["Correo"].Value.ToString();
                cbxRol.Text = dgvUsuarios.CurrentRow.Cells["Rol"].Value.ToString();
                EstaActivoCheckBox.Checked = Convert.ToBoolean(dgvUsuarios.CurrentRow.Cells["EstaActivo"].Value);

                byte[] miFoto = UsuarioDB.DevolverFoto(dgvUsuarios.CurrentRow.Cells["CodigoUsuario"].Value.ToString());

                if (miFoto.Length > 0)
                {
                    MemoryStream ms = new MemoryStream(miFoto);
                    FotoPictureBox.Image = System.Drawing.Bitmap.FromStream(ms);
                }

                HabilitarControles();
            }
            else
            {
                MessageBox.Show("Debe seleccionar un registro");
            }
        }

        private void btnAdjuntarFoto_Click(object sender, System.EventArgs e)
        {
            //Abre ventana de archivos de la pc del usuario
            OpenFileDialog Foto = new OpenFileDialog();
            DialogResult resultado = Foto.ShowDialog();

            //ese dialog.ok significa que el usuario si subio la imagen
            if (resultado == DialogResult.OK)
            {
                FotoPictureBox.Image = Image.FromFile(Foto.FileName);

            }

        }

        private void UsuariosForm_Load(object sender, System.EventArgs e)
        {
            TraerUsuarios();
        }

        private void TraerUsuarios()
        {
            dt = UsuarioDB.DevolverUsuarios();
            //cargar usuarios
            dgvUsuarios.DataSource = dt;
        }
    }
}
