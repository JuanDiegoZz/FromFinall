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
    public partial class CreateMultaForm : Form
    {
        private int empleadoId;  // Guardar el ID del empleado

        // Constructor que recibe el empleadoId
        public CreateMultaForm(int empleadoId)
        {
            InitializeComponent();
            this.empleadoId = empleadoId;
        }

        private void CreateMultaForm_Load(object sender, EventArgs e)
        {   
            // Cargar los usuarios activos en el ComboBox
            string queryUsuarios = "SELECT id_usuario, nombre FROM usuarios WHERE estado = 'Activo'";

            using (SqlConnection connection = new SqlConnection("Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;"))
            {
                SqlDataAdapter adapterUsuarios = new SqlDataAdapter(queryUsuarios, connection);
                DataTable dtUsuarios = new DataTable();
                adapterUsuarios.Fill(dtUsuarios);

                // Configurar el ComboBox para mostrar los usuarios activos
                cmbUsuarios.DisplayMember = "nombre";
                cmbUsuarios.ValueMember = "id_usuario";
                cmbUsuarios.DataSource = dtUsuarios;
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {

            // Obtener los datos del formulario
            int usuarioId = Convert.ToInt32(cmbUsuarios.SelectedValue);
            decimal monto = Convert.ToDecimal(txtMonto.Text);
            DateTime fechaMulta = dtpFecha.Value;

            // Obtener el empleado que crea la multa (empleadoId ya está en el formulario)
            int createdBy = empleadoId;

            // Estado "Activo" por defecto
            string estado = "Activo";

            // Fecha de creación
            DateTime createdAt = DateTime.Now;

            // Consulta SQL para insertar la multa
            string queryInsert = "INSERT INTO multas (id_usuario, monto, fecha, estado, created_by, created_at) " +
                                 "VALUES (@id_usuario, @monto, @fecha, @estado, @created_by, @created_at)";

            using (SqlConnection connection = new SqlConnection("Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;"))
            {
                SqlCommand command = new SqlCommand(queryInsert, connection);
                command.Parameters.AddWithValue("@id_usuario", usuarioId);
                command.Parameters.AddWithValue("@monto", monto);
                command.Parameters.AddWithValue("@fecha", fechaMulta);
                command.Parameters.AddWithValue("@estado", estado);
                command.Parameters.AddWithValue("@created_by", createdBy);
                command.Parameters.AddWithValue("@created_at", createdAt);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }

            MessageBox.Show("Multa registrada correctamente.");
        }
    }
}

