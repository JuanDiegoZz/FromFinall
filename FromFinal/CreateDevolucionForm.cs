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
    public partial class CreateDevolucionForm : Form
    {
        private int empleadoId;  // Variable para almacenar el ID del empleado

        // Constructor que recibe el empleadoId
        public CreateDevolucionForm(int empleadoId)
        {
            InitializeComponent();
            this.empleadoId = empleadoId;  // Guardar el empleadoId que viene de Control
        }
        private void CreateDevolucionForm_Load(object sender, EventArgs e)
        {
            // Cargar los préstamos activos en el ComboBox
            string connectionString = "Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;";
            string queryPrestamos = "SELECT id_prestamo, fecha_prestamo FROM prestamos WHERE estado = 'Activo'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(queryPrestamos, connection);
                DataTable dtPrestamos = new DataTable();
                adapter.Fill(dtPrestamos);

                cmbPrestamos.DisplayMember = "fecha_prestamo";  // Mostrar fecha del préstamo en el ComboBox
                cmbPrestamos.ValueMember = "id_prestamo";  // Usar el id_prestamo como valor
                cmbPrestamos.DataSource = dtPrestamos;
            }


        }

        private void btnGuardarDevolucion_Click(object sender, EventArgs e)
        {
            if (cmbPrestamos.SelectedValue != null && dtpFechaDevolucion.Value != null)
            {
                int idPrestamo = Convert.ToInt32(cmbPrestamos.SelectedValue);  // Obtener el id del préstamo seleccionado
                DateTime fechaDevolucion = dtpFechaDevolucion.Value;  // Obtener la fecha de devolución

                // Cadena de conexión a la base de datos
                string connectionString = "Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;";

                // Consulta SQL para insertar la devolución
                string query = "INSERT INTO historial_devoluciones (id_prestamo, fecha_devolucion, estado, created_by, created_at) " +
                               "VALUES (@idPrestamo, @fechaDevolucion, 'Activo', @createdBy, @createdAt)";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@idPrestamo", idPrestamo);
                    cmd.Parameters.AddWithValue("@fechaDevolucion", fechaDevolucion);
                    cmd.Parameters.AddWithValue("@createdBy", empleadoId);  // El ID del empleado que está registrando la devolución
                    cmd.Parameters.AddWithValue("@createdAt", DateTime.Now);  // Fecha y hora actuales

                    connection.Open();
                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Devolución registrada correctamente.");
                        this.Close();  // Cerrar el formulario después de guardar
                    }
                    else
                    {
                        MessageBox.Show("Error al registrar la devolución.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, complete todos los campos.");
            }
        }
    }
}
