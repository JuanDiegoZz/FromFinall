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
    public partial class CreateCompraForm : Form
    {
        private int empleadoId;  // Guardar el ID del empleado

        // Constructor que recibe el empleadoId
        public CreateCompraForm(int empleadoId)
        {
            InitializeComponent();
            this.empleadoId = empleadoId;
        }

        private void CreateCompraForm_Load(object sender, EventArgs e)
        {
            // Cargar proveedores en el ComboBox
            string queryProveedores = "SELECT id_proveedor, nombre FROM proveedores WHERE estado = 'Activo'";
            using (SqlConnection connection = new SqlConnection("Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;"))
            {
                SqlDataAdapter adapterProveedores = new SqlDataAdapter(queryProveedores, connection);
                DataTable dtProveedores = new DataTable();
                adapterProveedores.Fill(dtProveedores);
                cbProveedor.DisplayMember = "nombre";  // Mostrar nombre del proveedor
                cbProveedor.ValueMember = "id_proveedor";  // Guardar el id del proveedor
                cbProveedor.DataSource = dtProveedores;
            }

            // Cargar materiales en el ComboBox
            string queryMateriales = "SELECT id_material, titulo FROM material_bibliografico WHERE estado = 'Activo'";
            using (SqlConnection connection = new SqlConnection("Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;"))
            {
                SqlDataAdapter adapterMateriales = new SqlDataAdapter(queryMateriales, connection);
                DataTable dtMateriales = new DataTable();
                adapterMateriales.Fill(dtMateriales);
                cbMaterial.DisplayMember = "titulo";  // Mostrar título del material
                cbMaterial.ValueMember = "id_material";  // Guardar el id del material
                cbMaterial.DataSource = dtMateriales;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {

            // Verificar que los campos no estén vacíos
            if (cbProveedor.SelectedIndex == -1 || cbMaterial.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor, seleccione un proveedor y un material.");
                return;
            }

            // Obtener valores de los controles
            int idProveedor = Convert.ToInt32(cbProveedor.SelectedValue);
            int idMaterial = Convert.ToInt32(cbMaterial.SelectedValue);
            DateTime fechaCompra = dateTimePickerCompra.Value;  // Fecha de la compra
            string estado = "Activo";  // Estado de la compra, por defecto "Activo"

            // Consulta SQL para insertar la nueva compra
            string query = "INSERT INTO compras (fecha_compra, id_proveedor, id_material, estado, created_by, updated_by, created_at, updated_at) " +
                           "VALUES (@fechaCompra, @idProveedor, @idMaterial, @estado, @createdBy, @updatedBy, @createdAt, @updatedAt)";

            using (SqlConnection connection = new SqlConnection("Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;"))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@fechaCompra", fechaCompra);
                command.Parameters.AddWithValue("@idProveedor", idProveedor);
                command.Parameters.AddWithValue("@idMaterial", idMaterial);
                command.Parameters.AddWithValue("@estado", estado);
                command.Parameters.AddWithValue("@createdBy", empleadoId);  // Guardar el ID del empleado que crea la compra
                command.Parameters.AddWithValue("@updatedBy", empleadoId);  // El mismo empleado para la actualización
                command.Parameters.AddWithValue("@createdAt", DateTime.Now);  // Fecha y hora de creación
                command.Parameters.AddWithValue("@updatedAt", DateTime.Now);  // Fecha y hora de la última actualización

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Compra creada exitosamente.");
                    this.Close();  // Cerrar el formulario al guardar correctamente
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar la compra: " + ex.Message);
                }
            }
        }
    }
 }

