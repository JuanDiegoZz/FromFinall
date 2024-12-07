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
    public partial class EditSolicitudDonacionForm : Form
    {

        private int idSolicitud;
        private int empleadoId;  // ID del empleado que actualiza
        public EditSolicitudDonacionForm(int idSolicitud, decimal montoDonacion, DateTime fechaSolicitud, int idUsuario, int empleadoId)
        {
            InitializeComponent();
            this.idSolicitud = idSolicitud;
            this.empleadoId = empleadoId;

            // Cargar los datos en los controles del formulario
            txtMontoDonacion.Text = montoDonacion.ToString();
            dtpFechaSolicitud.Value = fechaSolicitud;
            cbmUsuario.SelectedValue = idUsuario;

            CargarUsuarios();
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

        private void EditSolicitudDonacionForm_Load(object sender, EventArgs e)
        {

        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Verificar que los campos no estén vacíos
            if (cbmUsuario.SelectedItem == null || string.IsNullOrEmpty(txtMontoDonacion.Text))
            {
                MessageBox.Show("Debe seleccionar un usuario y un monto de donación.");
                return;
            }

            decimal montoDonacion = Convert.ToDecimal(txtMontoDonacion.Text);
            DateTime fechaSolicitud = dtpFechaSolicitud.Value;
            int idUsuario = Convert.ToInt32(cbmUsuario.SelectedValue);

            // Consulta SQL para actualizar la solicitud de donación
            string queryUpdate = "UPDATE solicitudes_donaciones SET monto_donacion = @montoDonacion, fecha_solicitud = @fechaSolicitud, updated_by = @updatedBy, updated_at = @updatedAt WHERE id_solicitud = @idSolicitud";

            string connectionString = "Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryUpdate, connection);
                command.Parameters.AddWithValue("@idSolicitud", idSolicitud);
                command.Parameters.AddWithValue("@montoDonacion", montoDonacion);
                command.Parameters.AddWithValue("@fechaSolicitud", fechaSolicitud);
                command.Parameters.AddWithValue("@updatedBy", empleadoId);
                command.Parameters.AddWithValue("@updatedAt", DateTime.Now);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("La solicitud de donación ha sido actualizada correctamente.");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al actualizar la solicitud de donación: " + ex.Message);
                }
            }
        }
    }
}
