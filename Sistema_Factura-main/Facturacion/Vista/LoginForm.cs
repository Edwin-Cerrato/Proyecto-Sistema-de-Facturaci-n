using Datos;
using Entidades;
using System;
using System.Windows.Forms;

namespace Vista
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (txtUsuario.Text == string.Empty)
            {
                Alerta.SetError(txtUsuario, "Ingrese un usuario");
                txtUsuario.Clear();
                txtUsuario.Focus();
                return;
            }
            Alerta.Clear();

            if (string.IsNullOrEmpty(txtContrasena.Text))
            {
                Alerta.SetError(txtContrasena, "Ingrese la contraseña");
                txtContrasena.Clear();
                txtContrasena.Focus();
                return;
            }
            //VALIDAR EN BASE DE DATOS

            //crear objeto de clase login

            Login login = new Login(txtUsuario.Text, txtContrasena.Text);
            Usuario usuario = new Usuario();
            UsuarioDB usuarioDB = new UsuarioDB();

            //ejecutar sentencia
            usuario = usuarioDB.Autenticar(login);

            //validamos

            if (usuario != null)
            {

                if (usuario.EstaActivo)
                {
                    //MOSTRAR EL MENU
                    //instanciamos el formulario
                    //Nombre de formulario al que queremos ir,despues el nombre y por ultimo la propiedad
                    Menu menuformulario = new Menu();
                    //comando para ocultar el formulario anterior
                    this.Hide();
                    menuformulario.Show();
                }
                else
                {
                    MessageBox.Show("El usuario no esta activo", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                MessageBox.Show("Datos de Usuario Incorrecto", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }




        }

        private void btnVerContraseña_Click(object sender, EventArgs e)
        {
            ////Mostrar Contraseña

            //if (txtContrasena.PasswordChar == '*')
            //{
            //    bool v = txtContrasena.PasswordChar == '\0';
            //}
            //else
            //{
            //    bool v = txtContrasena.PasswordChar == '*';
            //}

        }
    }
}
