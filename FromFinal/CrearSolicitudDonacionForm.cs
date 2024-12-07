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
    public partial class CrearSolicitudDonacionForm : Form
    {
        private int empleadoId;  // Variable global para almacenar el ID del empleado que inicia sesión
        public CrearSolicitudDonacionForm(int empleadoId)
        {
            InitializeComponent();
            this.empleadoId = empleadoId;
        }
        private void CrearSolicitudDonacionForm_Load(object sender, EventArgs e)
        {
            // Cargar usuarios activos en el ComboBox
            CargarUsuarios();
            // Establecer la fecha de la solicitud con la fecha actual
            dtpFechaSolicitud.Value = DateTime.Now;

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

        private void btnGuardar_Click(object sender, EventArgs e)
        {

            // Verificar que los campos no estén vacíos
            if (cbmUsuario.SelectedItem == null || string.IsNullOrEmpty(txtMontoDonacion.Text))
            {
                MessageBox.Show("Debe seleccionar un usuario y especificar el monto de la donación.");
                return;
            }

            // Obtener los valores de los controles
            int idUsuario = (int)cbmUsuario.SelectedValue;
            decimal montoDonacion = Convert.ToDecimal(txtMontoDonacion.Text);
            DateTime fechaSolicitud = dtpFechaSolicitud.Value;
            string estado = "Activo";  // El estado por defecto es "Activo"

            // Cadena de conexión a la base de datos
            string connectionString = "Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;";

            // Consulta SQL para insertar la solicitud de donación
            string query = "INSERT INTO solicitudes_donaciones (id_usuario, monto_donacion, fecha_solicitud, estado, created_by, created_at) " +
                           "VALUES (@id_usuario, @monto_donacion, @fecha_solicitud, @estado, @created_by, @created_at)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Agregar los parámetros
                    command.Parameters.AddWithValue("@id_usuario", idUsuario);
                    command.Parameters.AddWithValue("@monto_donacion", montoDonacion);
                    command.Parameters.AddWithValue("@fecha_solicitud", fechaSolicitud);
                    command.Parameters.AddWithValue("@estado", estado);
                    command.Parameters.AddWithValue("@created_by", empleadoId);  // ID del empleado que crea la solicitud
                    command.Parameters.AddWithValue("@created_at", DateTime.Now);  // Fecha de creación

                    try
                    {
                        // Abrir la conexión y ejecutar la consulta
                        connection.Open();
                        command.ExecuteNonQuery();
                        MessageBox.Show("Solicitud de donación creada correctamente.");
                        this.Close();  // Cerrar el formulario después de guardar
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al crear la solicitud de donación: " + ex.Message);
                    }
                }
            }
        }
    }
}
