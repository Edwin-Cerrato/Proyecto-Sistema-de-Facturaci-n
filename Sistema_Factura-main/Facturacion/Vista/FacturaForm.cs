using Datos;
using Entidades;
using System;
using System.Windows.Forms;

namespace Vista
{
    public partial class FacturaForm : Form
    {
        public FacturaForm()
        {
            InitializeComponent();
        }

        Cliente micliente = null;
        ClienteDB clienteDB = new ClienteDB();

        private void txtIdentidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            //al preionar enter se buscara el cliente

            if (e.KeyChar == (char)Keys.Enter)
            {
                micliente = new Cliente();
                micliente = clienteDB.DevolverClientePorIdentidad(txtIdentidad.Text);
                txtNombreCliente.Text = micliente.Nombre;

            }
            else
            {
                //limpiar
                micliente = null;
                txtNombreCliente.Clear();
            }
        }

        private void btnBuscarCliente_Click(object sender, EventArgs e)
        {

        }
    }
}
