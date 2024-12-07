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
    public partial class EditUserForm : Form
    {
        private int idUsuario;
        private int empleadoId;

        // Constructor para recibir los datos del usuario seleccionado
        public EditUserForm(int idUsuario, string nombre, string correo, string telefono, string direccion, string ocupacion, int empleadoId)
        {
            InitializeComponent();

            // Guardamos los parámetros para usarlos más tarde
            this.idUsuario = idUsuario;
            this.empleadoId = empleadoId;

            // Llenamos los campos de texto con los datos del usuario
            txtNombre.Text = nombre;
            txtCorreo.Text = correo;
            txtTelefono.Text = telefono;
            txtDireccion.Text = direccion;
            txtOcupacion.Text = ocupacion;
        }

        
        private void btnActualizar_Click_1(object sender, EventArgs e)
        {
           
            // Verificar que los campos no estén vacíos
            if (string.IsNullOrEmpty(txtNombre.Text) || string.IsNullOrEmpty(txtCorreo.Text) || string.IsNullOrEmpty(txtTelefono.Text) || string.IsNullOrEmpty(txtDireccion.Text) || string.IsNullOrEmpty(txtOcupacion.Text))
            {
                MessageBox.Show("Todos los campos deben ser llenados.");
                return;
            }

            // Cadena de conexión a la base de datos
            string connectionString = "Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;";

            // Consulta SQL para actualizar el usuario
            string query = "UPDATE Usuarios SET nombre = @nombre, correo = @correo, telefono = @telefono, direccion = @direccion, tipo_usuario = @ocupacion, updated_by = @updated_by, updated_at = @updated_at " +
                           "WHERE id_usuario = @idUsuario";

            // Crear la conexión y ejecutar la consulta
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@idUsuario", idUsuario);
                command.Parameters.AddWithValue("@nombre", txtNombre.Text);
                command.Parameters.AddWithValue("@correo", txtCorreo.Text);
                command.Parameters.AddWithValue("@telefono", txtTelefono.Text);
                command.Parameters.AddWithValue("@direccion", txtDireccion.Text);
                command.Parameters.AddWithValue("@ocupacion", txtOcupacion.Text);
                command.Parameters.AddWithValue("@updated_by", empleadoId);  // Asignar el id del empleado que actualizó
                command.Parameters.AddWithValue("@updated_at", DateTime.Now); // Registrar la fecha y hora de la actualización

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();  // Ejecutar la consulta de actualización
                    MessageBox.Show("Usuario actualizado correctamente.");

                    // Cerrar el formulario de edición
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al actualizar el usuario: " + ex.Message);
                }
            }
        }

        private void EditUserForm_Load(object sender, EventArgs e)
        {

        }
    }
}



