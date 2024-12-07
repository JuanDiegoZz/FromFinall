using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace FromFinal
{
    public partial class EditDevolucionForm : Form
    {
        private int idDevolucion;
        private int empleadoId;  // ID del empleado que actualiza
        public EditDevolucionForm(int idDevolucion, int idPrestamo, DateTime fechaDevolucion, int idUsuario, int idMaterial, int empleadoId)
        {
            InitializeComponent();
            this.idDevolucion = idDevolucion;
            this.empleadoId = empleadoId;

            // Cargar los datos en los controles del formulario
            dtpFechaDevolucion.Value = fechaDevolucion;
            dtpFechaPrestamo.Value = GetFechaPrestamo(idPrestamo);
            cbmUsuario.SelectedValue = idUsuario;
            cbmMaterial.SelectedValue = idMaterial;

            CargarUsuarios();
            CargarMateriales();
        }

        // Método para obtener la fecha de préstamo asociada al idPrestamo
        private DateTime GetFechaPrestamo(int idPrestamo)
        {
            string query = "SELECT fecha_prestamo FROM prestamos WHERE id_prestamo = @idPrestamo";
            DateTime fechaPrestamo = DateTime.Now;

            string connectionString = "Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@idPrestamo", idPrestamo);

                try
                {
                    connection.Open();
                    fechaPrestamo = (DateTime)command.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al obtener la fecha del préstamo: " + ex.Message);
                }
            }

            return fechaPrestamo;
        }

        // Cargar usuarios activos en el ComboBox
        private void CargarUsuarios()
        {
            string queryUsuarios = "SELECT id_usuario, nombre FROM usuarios WHERE estado = 'Activo'";
            using (SqlConnection connection = new SqlConnection("Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;"))
            {
                SqlDataAdapter adapterUsuarios = new SqlDataAdapter(queryUsuarios, connection);
                DataTable dtUsuarios = new DataTable();
                adapterUsuarios.Fill(dtUsuarios);

                cbmUsuario.DisplayMember = "nombre";
                cbmUsuario.ValueMember = "id_usuario";
                cbmUsuario.DataSource = dtUsuarios;
            }
        }
        private void EditDevolucionForm_Load(object sender, EventArgs e)
        {

        }

        // Cargar materiales activos en el ComboBox
        private void CargarMateriales()
        {
            string queryMateriales = "SELECT id_material, titulo FROM material_bibliografico WHERE estado = 'Activo'";
            using (SqlConnection connection = new SqlConnection("Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;"))
            {
                SqlDataAdapter adapterMateriales = new SqlDataAdapter(queryMateriales, connection);
                DataTable dtMateriales = new DataTable();
                adapterMateriales.Fill(dtMateriales);

                cbmMaterial.DisplayMember = "titulo";
                cbmMaterial.ValueMember = "id_material";
                cbmMaterial.DataSource = dtMateriales;
            }
        }


        // Método para actualizar la devolución
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Verificar que los campos no estén vacíos
            if (cbmUsuario.SelectedItem == null || cbmMaterial.SelectedItem == null)
            {
                MessageBox.Show("Debe seleccionar un usuario y un material.");
                return;
            }

            // Consulta SQL para actualizar la devolución
            string queryUpdate = "UPDATE historial_devoluciones SET fecha_devolucion = @fechaDevolucion, updated_by = @updatedBy, updated_at = @updatedAt WHERE id_devolucion = @idDevolucion";

            string connectionString = "Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryUpdate, connection);
                command.Parameters.AddWithValue("@idDevolucion", idDevolucion);
                command.Parameters.AddWithValue("@fechaDevolucion", dtpFechaDevolucion.Value);
                command.Parameters.AddWithValue("@updatedBy", empleadoId);
                command.Parameters.AddWithValue("@updatedAt", DateTime.Now);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Devolución actualizada correctamente.");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al actualizar la devolución: " + ex.Message);
                }
            }
        }
    }
}

