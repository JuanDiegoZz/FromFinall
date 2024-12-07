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
    public partial class EditEventoForm : Form
    {
        
        private int idEvento;
        private int empleadoId;  // ID del empleado que actualiza
        public EditEventoForm(int idEvento, string nombreEvento, string descripcionEvento, DateTime fechaInicio, DateTime fechaFin, int empleadoId)
        {
            InitializeComponent();
            this.idEvento = idEvento;
            this.empleadoId = empleadoId;

            // Cargar los datos en los controles del formulario
            txtNombreEvento.Text = nombreEvento;
            txtDescripcionEvento.Text = descripcionEvento;
            dtpFechaInicio.Value = fechaInicio;
            dtpFechaFin.Value = fechaFin;
        }



        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Verificar que los campos no estén vacíos
            if (string.IsNullOrEmpty(txtNombreEvento.Text) || string.IsNullOrEmpty(txtDescripcionEvento.Text))
            {
                MessageBox.Show("Debe ingresar un nombre y una descripción para el evento.");
                return;
            }

            // Consulta SQL para actualizar el evento
            string queryUpdate = "UPDATE eventos SET nombre_evento = @nombreEvento, descripcion = @descripcion, fecha_inicio = @fechaInicio, fecha_fin = @fechaFin, updated_by = @updatedBy, updated_at = @updatedAt WHERE id_evento = @idEvento";

            string connectionString = "Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryUpdate, connection);
                command.Parameters.AddWithValue("@idEvento", idEvento);
                command.Parameters.AddWithValue("@nombreEvento", txtNombreEvento.Text);
                command.Parameters.AddWithValue("@descripcion", txtDescripcionEvento.Text);
                command.Parameters.AddWithValue("@fechaInicio", dtpFechaInicio.Value);
                command.Parameters.AddWithValue("@fechaFin", dtpFechaFin.Value);
                command.Parameters.AddWithValue("@updatedBy", empleadoId);
                command.Parameters.AddWithValue("@updatedAt", DateTime.Now);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Evento actualizado correctamente.");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al actualizar el evento: " + ex.Message);
                }
            }
        }
    }
}
