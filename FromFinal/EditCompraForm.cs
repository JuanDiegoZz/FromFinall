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
    public partial class EditCompraForm : Form
    {
        private int idCompra;
        private int empleadoId;  // Variable para almacenar el ID del empleado

        public EditCompraForm(int idCompra, DateTime fechaCompra, int idProveedor, int idMaterial, int empleadoId)
        {
            InitializeComponent();
            this.idCompra = idCompra;
            this.empleadoId = empleadoId;

            // Prellenamos los controles con los valores actuales de la compra
            dtpFechaCompra.Value = fechaCompra;
            cbmProveedor.SelectedValue = idProveedor;
            cbmMaterial.SelectedValue = idMaterial;

            CargarProveedores();
            CargarMateriales();
        }
        // Cargar proveedores activos en el ComboBox
        private void CargarProveedores()
        {
            string queryProveedores = "SELECT id_proveedor, nombre FROM proveedores WHERE estado = 'Activo'";
            using (SqlConnection connection = new SqlConnection("Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;"))
            {
                SqlDataAdapter adapterProveedores = new SqlDataAdapter(queryProveedores, connection);
                DataTable dtProveedores = new DataTable();
                adapterProveedores.Fill(dtProveedores);

                cbmProveedor.DisplayMember = "nombre";  // Mostrar el nombre del proveedor
                cbmProveedor.ValueMember = "id_proveedor";  // Valor que se usará en el ComboBox (id_proveedor)
                cbmProveedor.DataSource = dtProveedores;
            }
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

                cbmMaterial.DisplayMember = "titulo";  // Mostrar el título del material
                cbmMaterial.ValueMember = "id_material";  // Valor que se usará en el ComboBox (id_material)
                cbmMaterial.DataSource = dtMateriales;
            }
        }
        private void EditCompraForm_Load(object sender, EventArgs e)
        {

        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {

            // Verificar que los campos no estén vacíos
            if (cbmProveedor.SelectedItem == null || cbmMaterial.SelectedItem == null)
            {
                MessageBox.Show("Debe seleccionar un proveedor y un material.");
                return;
            }

            // Consulta SQL para actualizar la compra
            string queryUpdate = "UPDATE compras SET fecha_compra = @fechaCompra, id_proveedor = @idProveedor, id_material = @idMaterial, " +
                                 "updated_by = @updatedBy, updated_at = @updatedAt WHERE id_compra = @idCompra";

            string connectionString = "Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryUpdate, connection);
                command.Parameters.AddWithValue("@idCompra", idCompra);
                command.Parameters.AddWithValue("@fechaCompra", dtpFechaCompra.Value);
                command.Parameters.AddWithValue("@idProveedor", cbmProveedor.SelectedValue);
                command.Parameters.AddWithValue("@idMaterial", cbmMaterial.SelectedValue);
                command.Parameters.AddWithValue("@updatedBy", empleadoId);  // ID del empleado que actualiza
                command.Parameters.AddWithValue("@updatedAt", DateTime.Now);  // Fecha y hora de la actualización

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();  // Ejecutar la consulta de actualización
                    MessageBox.Show("Compra actualizada correctamente.");

                    // Cerrar el formulario de edición
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al actualizar la compra: " + ex.Message);
                }
            }
        }
    }
}

