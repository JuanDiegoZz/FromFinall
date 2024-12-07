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
    public partial class CreateEventoForm : Form
    {
        private int empleadoId;  // Variable para almacenar el ID del empleado

        // Constructor que recibe el empleadoId
        public CreateEventoForm(int empleadoId)
        {
            InitializeComponent();
            this.empleadoId = empleadoId; // Guardar el empleadoId que viene de Form1
        }

        private void CreateEventoForm_Load(object sender, EventArgs e)
        {

       

        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {

            // Obtener los valores de los controles
            string nombreEvento = txtNombreEvento.Text;
            string descripcion = txtDescripcionEvento.Text;
            DateTime fechaInicio = dateTimePickerInicio.Value;
            DateTime fechaFin = dateTimePickerFin.Value;
            string estado = "Activo";  // El estado siempre es "Activo"
            DateTime createdAt = DateTime.Now; // Fecha actual para la creación
            int createdBy = empleadoId; // El empleado que está creando el evento

            // Validar que los campos no estén vacíos
            if (string.IsNullOrEmpty(nombreEvento) || string.IsNullOrEmpty(descripcion))
            {
                MessageBox.Show("Por favor, complete todos los campos.");
                return;
            }

            // Cadena de conexión a la base de datos
            string connectionString = "Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;";

            // Consulta SQL para insertar un nuevo evento
            string query = "INSERT INTO eventos (nombre_evento, descripcion, fecha_inicio, fecha_fin, estado, created_by, created_at) " +
                           "VALUES (@nombreEvento, @descripcion, @fechaInicio, @fechaFin, @estado, @createdBy, @createdAt)";

            // Ejecutar la consulta de inserción en la base de datos
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@nombreEvento", nombreEvento);
                command.Parameters.AddWithValue("@descripcion", descripcion);
                command.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                command.Parameters.AddWithValue("@fechaFin", fechaFin);
                command.Parameters.AddWithValue("@estado", estado);
                command.Parameters.AddWithValue("@createdBy", createdBy);
                command.Parameters.AddWithValue("@createdAt", createdAt);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Evento creado con éxito.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al crear el evento: " + ex.Message);
                }
            }

            // Cerrar el formulario después de guardar
            this.Close();
        }

    
    }
}
   