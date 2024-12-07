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
    public partial class CreateUserForm : Form
    {
        private int empleadoId;

        public CreateUserForm(int empleadoId)  // Recibimos el empleadoId
        {
            InitializeComponent();
            this.empleadoId = empleadoId;  // Asignamos el valor de empleadoId
        }
        private void btnCrear_Click(object sender, EventArgs e)
        {

            // Verificar que los campos no estén vacíos
            if (string.IsNullOrEmpty(txtNombre.Text) || string.IsNullOrEmpty(txtCorreo.Text) ||
                string.IsNullOrEmpty(txtTelefono.Text) || string.IsNullOrEmpty(txtDireccion.Text) ||
                string.IsNullOrEmpty(txtOcupacion.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos.");
                return;
            }

            // Obtener los valores de los TextBox
            string nombre = txtNombre.Text;
            string correo = txtCorreo.Text;
            string telefono = txtTelefono.Text;
            string direccion = txtDireccion.Text;
            string tipoUsuario = txtOcupacion.Text;

            // Crear la conexión a la base de datos
            string connectionString = "Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;";

            // Instrucción SQL para insertar un nuevo usuario
            string query = "INSERT INTO Usuarios (nombre, correo, telefono, direccion, tipo_usuario, estado, id_empleado, created_by, updated_by, created_at, updated_at) " +
                           "VALUES (@nombre, @correo, @telefono, @direccion, @tipo_usuario, 'Activo', @id_empleado, @created_by, @updated_by, GETDATE(), GETDATE())";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Asignar los parámetros
                        command.Parameters.AddWithValue("@nombre", nombre);
                        command.Parameters.AddWithValue("@correo", correo);
                        command.Parameters.AddWithValue("@telefono", telefono);
                        command.Parameters.AddWithValue("@direccion", direccion);
                        command.Parameters.AddWithValue("@tipo_usuario", tipoUsuario);
                        command.Parameters.AddWithValue("@id_empleado", empleadoId);  // Usamos la variable empleadoId
                        command.Parameters.AddWithValue("@created_by", empleadoId);  // El mismo empleado que crea
                        command.Parameters.AddWithValue("@updated_by", empleadoId);  // El mismo empleado que actualiza

                        // Abrir la conexión
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        // Verificar si se insertó correctamente
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Usuario creado exitosamente.");
                        }
                        else
                        {
                            MessageBox.Show("Error al crear el usuario.");
                        }
                    }
                }

                // Cerrar el formulario después de crear el usuario
                this.Close();  // Esto cerrará el formulario actual
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

    }
}




