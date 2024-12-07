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
    public partial class CreateSancionForm : Form
    {
        private int empleadoId;  // Variable para almacenar el ID del empleado


        // Constructor que recibe el empleadoId
        public CreateSancionForm(int empleadoId)
        {
            InitializeComponent();
            // Agregar las opciones manualmente al ComboBox para tipos de sanción
            cmbTipoSancion.Items.Add("Multa");
            cmbTipoSancion.Items.Add("Suspensión");
            cmbTipoSancion.Items.Add("Advertencia");
            cmbTipoSancion.SelectedIndex = 0;

            this.empleadoId = empleadoId; // Guardar el empleadoId que viene de Form1
            CargarUsuarios();  // Cargar los usuarios activos al iniciar el formulario
        }
        private void CargarUsuarios()
        {
            string connectionString = "Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;";
            string query = "SELECT id_usuario, nombre FROM usuarios WHERE estado = 'Activo'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                DataTable dtUsuarios = new DataTable();
                dataAdapter.Fill(dtUsuarios);

                cmbUsuarios.DataSource = dtUsuarios;
                cmbUsuarios.DisplayMember = "nombre";  // Mostrar el nombre del usuario en el ComboBox
                cmbUsuarios.ValueMember = "id_usuario";  // Establecer el valor del ComboBox como el id del usuario
            }
        }

        private void CreateSancionForm_Load(object sender, EventArgs e)
        {
           
            // Cargar los usuarios activos en el ComboBox
            string queryUsuarios = "SELECT id_usuario, nombre FROM usuarios WHERE estado = 'Activo'";
            using (SqlConnection connection = new SqlConnection("Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;"))
            {
                SqlDataAdapter adapterUsuarios = new SqlDataAdapter(queryUsuarios, connection);
                DataTable dtUsuarios = new DataTable();
                adapterUsuarios.Fill(dtUsuarios);

                cmbUsuarios.DisplayMember = "nombre";  // Mostrar nombre en el ComboBox
                cmbUsuarios.ValueMember = "id_usuario"; // El valor será el id_usuario
                cmbUsuarios.DataSource = dtUsuarios;
            }

         
        }
        private void button1_Click(object sender, EventArgs e)
        {
            // Validar si los campos están completos
            if (cmbUsuarios.SelectedItem == null || string.IsNullOrEmpty(txtDescripcion.Text))
            {
                MessageBox.Show("Por favor complete todos los campos.");
                return;
            }

            // Obtener los valores
            int idUsuario = Convert.ToInt32(cmbUsuarios.SelectedValue);
            string tipoSancion = cmbTipoSancion.SelectedItem.ToString();
            string descripcion = txtDescripcion.Text;
            DateTime fechaSancion = dtpFechaSancion.Value;
            string estado = "Activo";  // El estado siempre será "Activo"
            int createdBy = empleadoId; // El empleado que creó la sanción

            // Insertar los datos en la base de datos
            string query = "INSERT INTO sanciones (id_usuario, tipo_sancion, descripcion, fecha_sancion, id_empleado, estado, created_by, created_at) " +
                           "VALUES (@idUsuario, @tipoSancion, @descripcion, @fechaSancion, @idEmpleado, @estado, @createdBy, @createdAt)";

            using (SqlConnection connection = new SqlConnection("Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;"))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@idUsuario", idUsuario);
                command.Parameters.AddWithValue("@tipoSancion", tipoSancion);
                command.Parameters.AddWithValue("@descripcion", descripcion);
                command.Parameters.AddWithValue("@fechaSancion", fechaSancion);
                command.Parameters.AddWithValue("@idEmpleado", empleadoId);
                command.Parameters.AddWithValue("@estado", estado);
                command.Parameters.AddWithValue("@createdBy", empleadoId);
                command.Parameters.AddWithValue("@createdAt", DateTime.Now);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }

            // Informar al usuario que la sanción se ha guardado
            MessageBox.Show("Sanción creada con éxito.");
            this.Close(); // Cerrar el formulario después de guardar
        }
    }
}

