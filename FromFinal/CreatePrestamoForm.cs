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
    public partial class CreatePrestamoForm : Form
    {
        private int empleadoId;  // Variable para almacenar el ID del empleado


        public CreatePrestamoForm(int empleadoId)
        {
            InitializeComponent();
            this.empleadoId = empleadoId;  // Guardar el empleadoId que viene de Form1
            CargarUsuarios();
            CargarMateriales();

        }
        private void CargarUsuarios()
        {
            // Consulta para obtener los usuarios activos
            string queryUsuarios = "SELECT id_usuario, nombre FROM usuarios WHERE estado = 'Activo'";
            using (SqlConnection connection = new SqlConnection("Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;"))
            {
                SqlDataAdapter adapterUsuarios = new SqlDataAdapter(queryUsuarios, connection);
                DataTable dtUsuarios = new DataTable();
                adapterUsuarios.Fill(dtUsuarios);

                UsPrestamo.DisplayMember = "nombre";  // Mostrar nombre en el ComboBox
                UsPrestamo.ValueMember = "id_usuario";  // Valor que se usará en el ComboBox (id_usuario)
                UsPrestamo.DataSource = dtUsuarios;
            }
        }
        private void CargarMateriales()
        {
            // Consulta para obtener los materiales activos
            string queryMateriales = "SELECT id_material, titulo FROM material_bibliografico WHERE estado = 'Activo'";
            using (SqlConnection connection = new SqlConnection("Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;"))
            {
                SqlDataAdapter adapterMateriales = new SqlDataAdapter(queryMateriales, connection);
                DataTable dtMateriales = new DataTable();
                adapterMateriales.Fill(dtMateriales);

                MaterialPrestamo.DisplayMember = "titulo";  // Mostrar el título del material
                MaterialPrestamo.ValueMember = "id_material";  // Valor que se usará en el ComboBox (id_material)
                MaterialPrestamo.DataSource = dtMateriales;
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
        
            // Verificar que todos los campos estén llenos
            if (UsPrestamo.SelectedItem == null || MaterialPrestamo.SelectedItem == null || TimeDevolucion.Value == null)
            {
                MessageBox.Show("Por favor, completa todos los campos.");
                return;
            }

            // Obtener los valores de los controles
            int idUsuario = Convert.ToInt32(UsPrestamo.SelectedValue);  // Suponiendo que el ComboBox tiene un ValueMember con el ID
            int idMaterial = Convert.ToInt32(MaterialPrestamo.SelectedValue);  // Igualmente para el material
            DateTime fechaDevolucion = TimeDevolucion.Value;  // Fecha de devolución seleccionada

            // La fecha de préstamo es la fecha actual
            DateTime fechaPrestamo = DateTime.Now;

            // Estado predeterminado es "Activo"
            string estado = "Activo";

            // Cadena de conexión a la base de datos
            string connectionString = "Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;";

            // Consulta SQL para insertar el nuevo préstamo
            string query = "INSERT INTO prestamos (fecha_prestamo, fecha_devolucion, id_usuario, id_material, id_empleado, estado) " +
                           "VALUES (@fechaPrestamo, @fechaDevolucion, @idUsuario, @idMaterial, @idEmpleado, @estado)";

            // Crear la conexión y el comando
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@fechaPrestamo", fechaPrestamo);
                command.Parameters.AddWithValue("@fechaDevolucion", fechaDevolucion);
                command.Parameters.AddWithValue("@idUsuario", idUsuario);
                command.Parameters.AddWithValue("@idMaterial", idMaterial);
                command.Parameters.AddWithValue("@idEmpleado", empleadoId);  // Aquí asignamos el ID del empleado
                command.Parameters.AddWithValue("@estado", estado);

                // Intentar ejecutar la consulta
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Préstamo creado exitosamente.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al crear el préstamo: " + ex.Message);
                }
            }
        }

        private void CreatePrestamoForm_Load(object sender, EventArgs e)
        {

        }
    }
}

