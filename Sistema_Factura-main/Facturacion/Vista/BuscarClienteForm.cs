using Datos;
using Entidades;
using System;
using System.Windows.Forms;

namespace Vista
{
    public partial class BuscarClienteForm : Form
    {
        public BuscarClienteForm()
        {
            InitializeComponent();
        }

        ClienteDB clienteDB = new ClienteDB();
        public Cliente cliente = new Cliente();

        private void BuscarClienteForm_Load(object sender, EventArgs e)
        {
            //metodo que permita consultar todos los clientes
            dgvClientes.DataSource = clienteDB.DevolverClientes();

        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (dgvClientes.SelectedRows.Count > 0)
            {
                cliente.Identidad = dgvClientes.CurrentRow.Cells["Identidad"].Value.ToString();

                cliente.Nombre = dgvClientes.CurrentRow.Cells["Nombre"].Value.ToString();

                cliente.Telefono = dgvClientes.CurrentRow.Cells["Telefono"].Value.ToString();

                cliente.Correo = dgvClientes.CurrentRow.Cells["Correo"].Value.ToString();

                cliente.Direccion = dgvClientes.CurrentRow.Cells["Direccion"].Value.ToString();

                cliente.FechaNacimiento = Convert.ToDateTime(dgvClientes.CurrentRow.Cells["FechaNacimiento"].Value);

                cliente.EstaActivo = Convert.ToBoolean(dgvClientes.CurrentRow.Cells["EstaActivo"].Value);

                this.Close();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtNombre_KeyUp(object sender, KeyEventArgs e)
        {
            dgvClientes.DataSource = clienteDB.DevolverClientesPorNombre(txtNombre.Text);
        }
    }
}
