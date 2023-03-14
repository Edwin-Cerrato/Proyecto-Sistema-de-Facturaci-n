using Datos;
using Entidades;
using System;
using System.Data;
using System.Windows.Forms;

namespace Vista
{
    public partial class ClientesForm : Form
    {
        public ClientesForm()
        {
            InitializeComponent();
        }

        DataTable dt = new DataTable();
        string TipoOperacion = "";
        ClienteDB clienteDB = new ClienteDB();
        Cliente cliente = new Cliente();





        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DeshabilitarControles();
            LimpiarControles();
        }


        private void HabilitarControles()
        {

            txtIdentidad.Enabled = true;
            txtNombre.Enabled = true;
            txtTelefono.Enabled = true;
            txtCorreo.Enabled = true;
            txtDireccion.Enabled = true;

            FechaDateTimePicker.Enabled = true;



            cbxEstaActivo.Enabled = true;

            //botones
            btnGuardar.Enabled = true;
            btnCancelar.Enabled = true;
            btnModificar.Enabled = false;
        }


        private void DeshabilitarControles()
        {
            txtIdentidad.Enabled = false;
            txtNombre.Enabled = false;
            txtTelefono.Enabled = false;
            txtCorreo.Enabled = false;
            txtDireccion.Enabled = false;

            FechaDateTimePicker.Enabled = false;

            cbxEstaActivo.Enabled = false;


            //botones
            btnGuardar.Enabled = false;
            btnCancelar.Enabled = false;
            btnModificar.Enabled = true;
        }


        private void LimpiarControles()
        {
            txtNombre.Clear();
            txtIdentidad.Clear();
            txtDireccion.Clear();
            txtTelefono.Clear();
            txtCorreo.Clear();
            FechaDateTimePicker.Checked = false;

            //limpiar DateTimePicker


            cbxEstaActivo.Checked = false;


        }


        private void btnNuevo_Click(object sender, EventArgs e)
        {
            txtIdentidad.Focus();
            HabilitarControles();
            TipoOperacion = "Nuevo";
        }


        private void TraerClientes()
        {
            dt = clienteDB.DevolverClientes();
            //cargar usuarios
            dgvCliente.DataSource = dt;
        }


        private void btnGuardar_Click(object sender, EventArgs e)
        {

            if (TipoOperacion == "Nuevo")
            {
                if (string.IsNullOrEmpty(txtIdentidad.Text))
                {
                    errorProvider1.SetError(txtIdentidad, "Ingrese una Identidad");
                    txtIdentidad.Focus();
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



                if (string.IsNullOrEmpty(txtTelefono.Text))
                {
                    errorProvider1.SetError(txtTelefono, "Ingrese la Contraseña");
                    txtTelefono.Focus();
                    return;
                }
                errorProvider1.Clear();
                ;



                cliente.Identidad = txtIdentidad.Text;
                cliente.Nombre = txtNombre.Text;
                cliente.Telefono = txtTelefono.Text;
                cliente.Correo = txtCorreo.Text;
                cliente.Direccion = txtDireccion.Text;
                cliente.FechaNacimiento = FechaDateTimePicker.Value;
                cliente.EstaActivo = cbxEstaActivo.Checked;


                //Insertar en Base de Datos
                bool inserto = clienteDB.Insertar(cliente);

                if (inserto)
                {
                    LimpiarControles();
                    DeshabilitarControles();
                    //TraerUsuarios();
                    MessageBox.Show("Registro Guardado");
                    TraerClientes();
                }
                else
                {
                    MessageBox.Show("No se pudo realizar el registro");
                }

            }
            else if (TipoOperacion == "Modificar")
            {
                cliente.Identidad = txtIdentidad.Text;
                cliente.Nombre = txtNombre.Text;
                cliente.Telefono = txtTelefono.Text;
                cliente.Correo = txtCorreo.Text;
                cliente.Direccion = txtDireccion.Text;
                cliente.FechaNacimiento = FechaDateTimePicker.Value;
                cliente.EstaActivo = cbxEstaActivo.Checked;


                bool modifico = clienteDB.Editar(cliente);
                if (modifico)
                {
                    LimpiarControles();
                    DeshabilitarControles();
                    //TraerClientes();
                    MessageBox.Show("Registro Actualizado Correctamente");
                    TraerClientes();
                }
                else
                {
                    MessageBox.Show("No se pudo Acualizar");
                }
            }
        }


        private void ClientesForm_Load(object sender, EventArgs e)
        {
            DeshabilitarControles();

            TraerClientes();
        }


        private void btnModificar_Click(object sender, EventArgs e)
        {
            TipoOperacion = "Modificar";
            if (dgvCliente.SelectedRows.Count > 0)
            {
                // txt       dataGriedView      Propiedad      nombre BD      propiedad
                txtIdentidad.Text = dgvCliente.CurrentRow.Cells["Identidad"].Value.ToString();

                txtNombre.Text = dgvCliente.CurrentRow.Cells["Nombre"].Value.ToString();

                txtTelefono.Text = dgvCliente.CurrentRow.Cells["Telefono"].Value.ToString();

                txtCorreo.Text = dgvCliente.CurrentRow.Cells["Correo"].Value.ToString();

                txtDireccion.Text = dgvCliente.CurrentRow.Cells["Direccion"].Value.ToString();


                cbxEstaActivo.Checked = Convert.ToBoolean(dgvCliente.CurrentRow.Cells["EstaActivo"].Value);



                HabilitarControles();
            }
            else
            {
                MessageBox.Show("Debe seleccionar un registro");
            }
        }


        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvCliente.SelectedRows.Count > 0)
            {
                DialogResult resultado = MessageBox.Show("Esta seguro de eliminar registro", "ADVERTENCIA", MessageBoxButtons.YesNo);

                if (resultado == DialogResult.Yes)
                {
                    bool elimino = clienteDB.Eliminar(dgvCliente.CurrentRow.Cells["Identidad"].Value.ToString());

                    if (elimino)
                    {
                        LimpiarControles();
                        DeshabilitarControles();
                        TraerClientes();
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
