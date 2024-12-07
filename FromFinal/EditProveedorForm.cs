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
    public partial class EditProveedorForm : Form
    {
        private int idProveedor;
        private int empleadoId;  // Variable para almacenar el ID del empleado
        public EditProveedorForm(int idProveedor, string nombre, string direccion, string telefono, int empleadoId)
        {
            InitializeComponent();
            this.idProveedor = idProveedor;  // Guardamos el id del proveedor
            this.empleadoId = empleadoId;  // Guardamos el id del empleado que realiza la actualización

            // Prellenamos los controles con los valores actuales del proveedor
            TxtNombre.Text = nombre;
            TxtDireccion.Text = direccion;
            TxtTelefono.Text = telefono;
        }

        private void EditProveedorForm_Load(object sender, EventArgs e)
        {

        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
          
            // Verificar que los campos no estén vacíos
            if (string.IsNullOrEmpty(TxtNombre.Text) || string.IsNullOrEmpty(TxtDireccion.Text) || string.IsNullOrEmpty(TxtTelefono.Text))
            {
                MessageBox.Show("Todos los campos deben ser llenados.");
                return;
            }

            // Consulta SQL para actualizar el proveedor
            string queryUpdate = "UPDATE proveedores SET nombre = @nombre, direccion = @direccion, telefono = @telefono, " +
                                 "updated_by = @updatedBy, updated_at = @updatedAt WHERE id_proveedor = @idProveedor";

            // Cadena de conexión a la base de datos
            string connectionString = "Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryUpdate, connection);
                command.Parameters.AddWithValue("@idProveedor", idProveedor);
                command.Parameters.AddWithValue("@nombre", TxtNombre.Text);
                command.Parameters.AddWithValue("@direccion", TxtDireccion.Text);
                command.Parameters.AddWithValue("@telefono", TxtTelefono.Text);
                command.Parameters.AddWithValue("@updatedBy", empleadoId);  // ID del empleado que actualiza
                command.Parameters.AddWithValue("@updatedAt", DateTime.Now);  // Fecha y hora de la actualización

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();  // Ejecutar la consulta de actualización
                    MessageBox.Show("Proveedor actualizado correctamente.");

                    // Cerrar el formulario de edición
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al actualizar el proveedor: " + ex.Message);
                }
            }
        }
    }
}
