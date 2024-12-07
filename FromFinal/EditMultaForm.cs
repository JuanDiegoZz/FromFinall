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


    public partial class EditMultaForm : Form
    {
        private int idMulta;
        private int empleadoId;  // ID del empleado que actualiza
        public EditMultaForm(int idMulta, int idUsuario, decimal monto, DateTime fecha, int empleadoId)
        {
            InitializeComponent();
            this.idMulta = idMulta;
            this.empleadoId = empleadoId;

            // Cargar los datos en los controles del formulario
            txtMonto.Text = monto.ToString();
            dtpFechaMulta.Value = fecha;

            //// Si es necesario, cargar los datos del usuario (por ejemplo, nombre) en un label o combobox
            //lblUsuario.Text = GetUsuarioNombre(idUsuario);
        }
        // Método para obtener el nombre del usuario asociado a la multa
        private string GetUsuarioNombre(int idUsuario)
        {
            string query = "SELECT nombre FROM usuarios WHERE id_usuario = @idUsuario";
            string usuarioNombre = "";

            string connectionString = "Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@idUsuario", idUsuario);

                try
                {
                    connection.Open();
                    usuarioNombre = (string)command.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al obtener el nombre del usuario: " + ex.Message);
                }
            }

            return usuarioNombre;
        }


        private void txtMonto_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {

            // Verificar que el monto no esté vacío
            if (string.IsNullOrWhiteSpace(txtMonto.Text))
            {
                MessageBox.Show("El monto no puede estar vacío.");
                return;
            }

            decimal nuevoMonto;
            if (!decimal.TryParse(txtMonto.Text, out nuevoMonto))
            {
                MessageBox.Show("Monto inválido.");
                return;
            }

            // Consulta SQL para actualizar la multa
            string queryUpdate = "UPDATE multas SET monto = @monto, fecha = @fecha, updated_by = @updatedBy, updated_at = @updatedAt WHERE id_multa = @idMulta";

            string connectionString = "Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryUpdate, connection);
                command.Parameters.AddWithValue("@idMulta", idMulta);
                command.Parameters.AddWithValue("@monto", nuevoMonto);
                command.Parameters.AddWithValue("@fecha", dtpFechaMulta.Value);
                command.Parameters.AddWithValue("@updatedBy", empleadoId);
                command.Parameters.AddWithValue("@updatedAt", DateTime.Now);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Multa actualizada correctamente.");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al actualizar la multa: " + ex.Message);
                }
            }
        }
    }
}

