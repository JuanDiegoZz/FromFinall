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
    public partial class CreateMaterialForm : Form
    {
        private int empleadoId;  // Variable para almacenar el ID del empleado
        public CreateMaterialForm(int empleadoId)
        {
            InitializeComponent();
            this.empleadoId = empleadoId;  // Guardamos el ID del empleado que lo creó
        }
    

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Verificar que los campos no estén vacíos
            if (string.IsNullOrEmpty(txtTitulo.Text) || string.IsNullOrEmpty(txtAutor.Text) ||
                string.IsNullOrEmpty(txtCategoria.Text) || string.IsNullOrEmpty(txtTipoMaterial.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos.");
                return;
            }

            // Cadena de conexión a la base de datos
            string connectionString = "Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;";

            // Consulta SQL para insertar el nuevo material
            string query = "INSERT INTO material_bibliografico (titulo, autor, año_publicacion, categoria, tipo_material, estado, created_by) " +
                           "VALUES (@titulo, @autor, @año_publicacion, @categoria, @tipo_material, 'Activo', @created_by)";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@titulo", txtTitulo.Text);
                    command.Parameters.AddWithValue("@autor", txtAutor.Text);
                    command.Parameters.AddWithValue("@año_publicacion", Convert.ToInt32(txtAñoPublicacion.Text)); // Asegúrate de que el año sea un número
                    command.Parameters.AddWithValue("@categoria", txtCategoria.Text);
                    command.Parameters.AddWithValue("@tipo_material", txtTipoMaterial.Text);
                    command.Parameters.AddWithValue("@created_by", this.empleadoId);  // Usamos el ID del empleado que está creando el material

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }

                MessageBox.Show("Material bibliográfico creado con éxito.");
                this.Close(); // Cierra el formulario después de guardar
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el material: {ex.Message}");
            }
        }
    }
}
    