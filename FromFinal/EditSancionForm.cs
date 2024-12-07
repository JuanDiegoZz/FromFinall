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
    public partial class EditSancionForm : Form
    {
        private int idSancion;
        private int empleadoId;  // ID del empleado que actualiza
        public EditSancionForm(int idSancion, string tipoSancion, string descripcion, DateTime fechaSancion, int empleadoId)
        {
            InitializeComponent();
            this.idSancion = idSancion;
            this.empleadoId = empleadoId;

            // Cargar los datos en los controles del formulario
            cbmTipoSancion.SelectedItem = tipoSancion;
            txtDescripcion.Text = descripcion;
            dtpFechaSancion.Value = fechaSancion;

            // Cargar opciones en el ComboBox de tipo de sanción
            cbmTipoSancion.Items.Add("Advertencia");
            cbmTipoSancion.Items.Add("Multa");
            cbmTipoSancion.Items.Add("Suspensión");
        }

        private void EditSancionForm_Load(object sender, EventArgs e)
        {

        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Verificar que los campos no estén vacíos
            if (cbmTipoSancion.SelectedItem == null || string.IsNullOrEmpty(txtDescripcion.Text))
            {
                MessageBox.Show("Debe seleccionar un tipo de sanción y una descripción.");
                return;
            }

            // Consulta SQL para actualizar la sanción
            string queryUpdate = "UPDATE sanciones SET tipo_sancion = @tipoSancion, descripcion = @descripcion, fecha_sancion = @fechaSancion, updated_by = @updatedBy, updated_at = @updatedAt WHERE id_sancion = @idSancion";

            string connectionString = "Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryUpdate, connection);
                command.Parameters.AddWithValue("@idSancion", idSancion);
                command.Parameters.AddWithValue("@tipoSancion", cbmTipoSancion.SelectedItem.ToString());
                command.Parameters.AddWithValue("@descripcion", txtDescripcion.Text);
                command.Parameters.AddWithValue("@fechaSancion", dtpFechaSancion.Value);
                command.Parameters.AddWithValue("@updatedBy", empleadoId);
                command.Parameters.AddWithValue("@updatedAt", DateTime.Now);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Sanción actualizada correctamente.");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al actualizar la sanción: " + ex.Message);
                }
            }
        }
    }
}
