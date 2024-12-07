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
    
    public partial class EditarMaterialForm : Form
    {
        private int idMaterial;
        private int empleadoId;  // Variable para almacenar el ID del empleado
        // Constructor
        public EditarMaterialForm(int idMaterial, string titulo, string autor, int añoPublicacion, string categoria, string tipoMaterial,int empleadoId)
        {
            InitializeComponent();
            this.idMaterial = idMaterial;  // Guardamos el id del material
            this.empleadoId = empleadoId;  // Guardamos el id del empleado que realiza la actualización

            // Prellenamos los controles con los valores actuales del material
            TxtTitulo.Text = titulo;
            TxtAutor.Text = autor;
            TxtAñoPublicacion.Text = añoPublicacion.ToString();
            TxtCategoria.Text = categoria;
            TxtTipoMaterial.Text = tipoMaterial;
        }


        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            // Verificar que los campos no estén vacíos
            if (string.IsNullOrEmpty(TxtTitulo.Text) || string.IsNullOrEmpty(TxtAutor.Text) ||
                string.IsNullOrEmpty(TxtAñoPublicacion.Text) || string.IsNullOrEmpty(TxtCategoria.Text) ||
                string.IsNullOrEmpty(TxtTipoMaterial.Text))
            {
                MessageBox.Show("Todos los campos deben ser llenados.");
                return;
            }

            // Consulta SQL para actualizar el material
            string queryUpdate = "UPDATE material_bibliografico SET titulo = @titulo, autor = @autor, año_publicacion = @añoPublicacion, " +
                                 "categoria = @categoria, tipo_material = @tipoMaterial, updated_by = @updatedBy, updated_at = @updatedAt " +
                                 "WHERE id_material = @idMaterial";

            // Cadena de conexión a la base de datos
            string connectionString = "Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryUpdate, connection);
                command.Parameters.AddWithValue("@idMaterial", idMaterial);
                command.Parameters.AddWithValue("@titulo", TxtTitulo.Text);
                command.Parameters.AddWithValue("@autor", TxtAutor.Text);
                command.Parameters.AddWithValue("@añoPublicacion", Convert.ToInt32(TxtAñoPublicacion.Text));
                command.Parameters.AddWithValue("@categoria", TxtCategoria.Text);
                command.Parameters.AddWithValue("@tipoMaterial", TxtTipoMaterial.Text);
                command.Parameters.AddWithValue("@updatedBy", empleadoId);  // ID del empleado que actualiza
                command.Parameters.AddWithValue("@updatedAt", DateTime.Now);  // Fecha y hora de la actualización

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();  // Ejecutar la consulta de actualización
                    MessageBox.Show("Material actualizado correctamente.");

                    // Cerrar el formulario de edición
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al actualizar el material: " + ex.Message);
                }
            }
        }
           
        

        private void EditarMaterialForm_Load(object sender, EventArgs e)
        {

        }
    }
}
