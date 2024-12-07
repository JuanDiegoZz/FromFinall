using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FromFinal
{
    public partial class CreateProveedorForm : Form
    {
        private int empleadoId;  // Variable para almacenar el ID del empleado

        // Constructor que recibe el empleadoId
        public CreateProveedorForm(int empleadoId)
        {
            InitializeComponent();
            this.empleadoId = empleadoId;  // Guardamos el empleadoId
        }

        private void CreateProveedorForm_Load(object sender, EventArgs e)
        {

        }

        private void btnGuardarProveedor_Click(object sender, EventArgs e)
        {  // Aquí, cuando guardas el proveedor, puedes usar 'empleadoId'
           // para establecer los campos created_by y created_at en la base de datos

            string nombre = txtNombre.Text;
            string direccion = txtDireccion.Text;
            string telefono = txtTelefono.Text;

            // Creamos la consulta SQL para insertar el proveedor
            string query = "INSERT INTO proveedores (nombre, direccion, telefono, estado, created_by, created_at) " +
                           "VALUES (@nombre, @direccion, @telefono, @estado, @created_by, @created_at)";

            // Cadena de conexión a la base de datos
            string connectionString = "Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@nombre", nombre);
                command.Parameters.AddWithValue("@direccion", direccion);
                command.Parameters.AddWithValue("@telefono", telefono);
                command.Parameters.AddWithValue("@estado", "Activo");  // Estado por defecto
                command.Parameters.AddWithValue("@created_by", empleadoId);  // Usamos el id del empleado
                command.Parameters.AddWithValue("@created_at", DateTime.Now);  // Fecha actual de creación

                connection.Open();
                command.ExecuteNonQuery();  // Ejecutamos la consulta
            }

            // Cerrar el formulario después de guardar
            this.Close();
        }
    }
}

