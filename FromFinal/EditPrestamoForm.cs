using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace FromFinal
{
    public partial class EditPrestamoForm : Form
    {
        private int idPrestamo;
        private int empleadoId;  // Variable para almacenar el ID del empleado

        // Constructor para inicializar los valores del préstamo
        public EditPrestamoForm(int idPrestamo, int idMaterial, DateTime fechaDevolucion, int empleadoId)
        {
            InitializeComponent();
            this.idPrestamo = idPrestamo;  // Guardamos el id del préstamo
            this.empleadoId = empleadoId;  // Guardar el empleadoId que viene de Form1

            // Cargar materiales en el ComboBox
            CargarMateriales();

            // Prellenamos los controles con los valores actuales del préstamo
            cbmMaterial.SelectedValue = idMaterial;  // Seleccionar el material actual
            dtpFechaDevolucion.Value = fechaDevolucion;  // Prellenar la fecha de devolución
        }

        // Método para cargar los materiales activos en el ComboBox
        private void CargarMateriales()
        {
            // Consulta para obtener los materiales activos
            string queryMateriales = "SELECT id_material, titulo FROM material_bibliografico WHERE estado = 'Activo'";
            using (SqlConnection connection = new SqlConnection("Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;"))
            {
                SqlDataAdapter adapterMateriales = new SqlDataAdapter(queryMateriales, connection);
                DataTable dtMateriales = new DataTable();
                adapterMateriales.Fill(dtMateriales);

                cbmMaterial.DisplayMember = "titulo";  // Mostrar el título del material
                cbmMaterial.ValueMember = "id_material";  // Valor que se usará en el ComboBox (id_material)
                cbmMaterial.DataSource = dtMateriales;
            }
        }

        // Método que se ejecuta cuando se hace clic en el botón "Guardar"
        private void button1_Click(object sender, EventArgs e)
        {
            // Verificar que se haya seleccionado un material y una fecha de devolución
            if (cbmMaterial.SelectedIndex == -1 || dtpFechaDevolucion.Value == null)
            {
                MessageBox.Show("Todos los campos deben ser llenados.");
                return;
            }

            
            // Consulta SQL para actualizar el préstamo
            string queryUpdate = "UPDATE Prestamos SET id_material = @idMaterial, fecha_devolucion = @fechaDevolucion WHERE id_prestamo = @idPrestamo";




            // Cadena de conexión a la base de datos
            string connectionString = "Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryUpdate, connection);
                command.Parameters.AddWithValue("@idPrestamo", idPrestamo);
                command.Parameters.AddWithValue("@idMaterial", cbmMaterial.SelectedValue);  // El material seleccionado
                command.Parameters.AddWithValue("@fechaDevolucion", dtpFechaDevolucion.Value);  // Nueva fecha de devolución


                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();  // Ejecutar la consulta de actualización
                    MessageBox.Show("Préstamo actualizado correctamente.");

                    // Cerrar el formulario de edición
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al actualizar el préstamo: " + ex.Message);
                }
            }
        }
    }
}

