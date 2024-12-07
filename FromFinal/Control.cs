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
    public partial class Control : Form
    {
        private int empleadoId;  // Variable para almacenar el ID del empleado
      

        public Control(int empleadoId)
        {
            InitializeComponent();
            this.empleadoId = empleadoId;  // Guardar el empleadoId que viene de Form1
        }


        private void VeUsuario_Click(object sender, EventArgs e)
        {
           
            // Cadena de conexión a la base de datos
            string connectionString = "Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;";

            // Consulta SQL para obtener los usuarios activos
            string query = "SELECT id_usuario, nombre, correo, telefono, direccion, tipo_usuario, estado " +
                           "FROM Usuarios WHERE estado = 'Activo'";

            // Crear una conexión a la base de datos
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    // Crear el SqlDataAdapter para ejecutar la consulta
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);

                    // Crear un DataTable para almacenar los resultados
                    DataTable dataTable = new DataTable();

                    // Llenar el DataTable con los resultados de la consulta
                    dataAdapter.Fill(dataTable);

                    // Asignar el DataTable al DataGridView para mostrar los usuarios
                    ViewUsuarios.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    // Manejar errores (si los hay)
                    MessageBox.Show("Error al cargar los usuarios: " + ex.Message);
                }
            }
        }

        private void ElUsuario_Click(object sender, EventArgs e)
        {


     
            // Verificar que se ha seleccionado un usuario en el DataGridView
            if (ViewUsuarios.SelectedRows.Count > 0)
            {
                // Obtener el id del usuario seleccionado desde el DataGridView
                int idUsuario = Convert.ToInt32(ViewUsuarios.SelectedRows[0].Cells["id_usuario"].Value);

                // Confirmar con el usuario que desea inactivar el usuario
                var result = MessageBox.Show("¿Está seguro de que desea inactivar este usuario?",
                                             "Confirmación",
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Cadena de conexión a la base de datos
                    string connectionString = "Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;";

                    // Crear la consulta SQL para actualizar el estado del usuario a "Inactivo"
                    string query = "UPDATE Usuarios SET estado = 'Inactivo', updated_by = @updatedBy, updated_at = @updatedAt WHERE id_usuario = @idUsuario";

                    // Establecer la conexión y ejecutar la consulta
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        try
                        {
                            connection.Open();

                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                // Asignar los parámetros a la consulta
                                command.Parameters.AddWithValue("@idUsuario", idUsuario);
                                command.Parameters.AddWithValue("@updatedBy", empleadoId);  // Asignar el ID del empleado
                                command.Parameters.AddWithValue("@updatedAt", DateTime.Now);  // Fecha actual

                                // Ejecutar la consulta de actualización
                                int rowsAffected = command.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Usuario inactivado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    // Recargar los usuarios para reflejar el cambio
                                    VeUsuario_Click(sender, e);
                                }
                                else
                                {
                                    MessageBox.Show("Hubo un error al intentar inactivar el usuario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error: " + ex.Message, "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor seleccione un usuario para inactivar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void AcUsuario_Click(object sender, EventArgs e)
        {
            // Verificar que se ha seleccionado un usuario en el DataGridView
            if (ViewUsuarios.SelectedRows.Count > 0)
            {
                // Obtener el id del usuario seleccionado desde el DataGridView
                int idUsuario = Convert.ToInt32(ViewUsuarios.SelectedRows[0].Cells["id_usuario"].Value);
                string nombre = ViewUsuarios.SelectedRows[0].Cells["nombre"].Value.ToString();
                string correo = ViewUsuarios.SelectedRows[0].Cells["correo"].Value.ToString();
                string telefono = ViewUsuarios.SelectedRows[0].Cells["telefono"].Value.ToString();
                string direccion = ViewUsuarios.SelectedRows[0].Cells["direccion"].Value.ToString();
                string ocupacion = ViewUsuarios.SelectedRows[0].Cells["tipo_usuario"].Value.ToString();

                // Crear el formulario EditUserForm y pasar los datos del usuario seleccionado
                EditUserForm editUserForm = new EditUserForm(idUsuario, nombre, correo, telefono, direccion, ocupacion, empleadoId);

                // Mostrar el formulario de edición
                editUserForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un usuario para actualizar.");
            }
        }   
        

        private void CrUsuarios_Click(object sender, EventArgs e)
        {
            CreateUserForm createUserForm = new CreateUserForm(empleadoId);  // Pasa el id del empleado al constructor
            createUserForm.Show();  // Muestra el formulario para crear usuario
        }

        private void VePrestamos_Click(object sender, EventArgs e)
        {
       
            // Cadena de conexión a la base de datos
            string connectionString = "Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;";

            // Consulta SQL para obtener los préstamos con estado 'Activo'
            string query = "SELECT p.id_prestamo, p.fecha_prestamo, p.fecha_devolucion, " +
                           "u.nombre AS usuario, m.titulo AS material, e.nombre AS empleado, " +
                           "p.estado " +  // Incluimos el campo 'estado' de la tabla prestamos
                           "FROM prestamos p " +
                           "JOIN usuarios u ON p.id_usuario = u.id_usuario " +
                           "JOIN material_bibliografico m ON p.id_material = m.id_material " +
                           "JOIN empleado e ON p.id_empleado = e.id_empleado " +
                           "WHERE p.estado = 'Activo'";  // Condición para solo mostrar los prestamos activos

            // Crear una conexión a la base de datos
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Crear un adaptador para ejecutar la consulta y llenar el DataGridView
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);

                // Crear un DataTable para almacenar los resultados de la consulta
                DataTable dataTable = new DataTable();

                try
                {
                    // Abrir la conexión y llenar el DataTable
                    connection.Open();
                    dataAdapter.Fill(dataTable);

                    // Asignar el DataTable al DataGridView
                    PrestamosView.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    // Manejo de excepciones si ocurre un error al acceder a la base de datos
                    MessageBox.Show("Error al cargar los préstamos: " + ex.Message);
                }
            }
        }

        

        

        private void ElPrestamo_Click(object sender, EventArgs e)
        {
       
            // Obtener el id del préstamo seleccionado en el DataGridView
            int idPrestamo = Convert.ToInt32(PrestamosView.SelectedRows[0].Cells["id_prestamo"].Value);

            // Cadena de conexión a la base de datos
            string connectionString = "Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;";

            // Consulta SQL para actualizar el estado del préstamo a "Inactivo" y asignar el id_empleado que lo está actualizando
            string query = "UPDATE prestamos " +
                           "SET estado = 'Inactivo', id_empleado = @id_empleado " +
                           "WHERE id_prestamo = @id_prestamo";

            // Crear una conexión a la base de datos
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Crear el comando de actualización
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Agregar los parámetros
                    command.Parameters.AddWithValue("@id_empleado", empleadoId);  // empleadoId debería ser la variable global con la ID del empleado actual
                    command.Parameters.AddWithValue("@id_prestamo", idPrestamo);

                    try
                    {
                        // Abrir la conexión
                        connection.Open();

                        // Ejecutar el comando de actualización
                        int rowsAffected = command.ExecuteNonQuery();

                        // Verificar si se actualizó el préstamo
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("El préstamo ha sido marcado como inactivo.");
                        }
                        else
                        {
                            MessageBox.Show("No se encontró el préstamo.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al eliminar préstamo: " + ex.Message);
                    }
                }
            }
        }

        private void AcPrestamos_Click(object sender, EventArgs e)
        {
          
            // Verificar que se ha seleccionado un préstamo en el DataGridView
            if (PrestamosView.SelectedRows.Count > 0)
            {
                // Obtener el id del préstamo seleccionado desde el DataGridView
                int idPrestamo = Convert.ToInt32(PrestamosView.SelectedRows[0].Cells["id_prestamo"].Value);

                // Cadena de conexión a la base de datos
                string connectionString = "Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;";
                string query = "SELECT id_material, fecha_devolucion FROM Prestamos WHERE id_prestamo = @idPrestamo";

                // Obtener los datos del préstamo desde la base de datos
                int idMaterial = 0;
                DateTime fechaDevolucion = DateTime.MinValue;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@idPrestamo", idPrestamo);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            idMaterial = reader.GetInt32(0);  // Obtiene el id_material
                            fechaDevolucion = reader.GetDateTime(1);  // Obtiene la fecha_devolucion
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al obtener los datos del préstamo: " + ex.Message);
                        return;
                    }
                }

                // Crear el formulario EditPrestamoForm y pasar los datos del préstamo seleccionado
                EditPrestamoForm editPrestamoForm = new EditPrestamoForm(idPrestamo, idMaterial, fechaDevolucion, empleadoId);

                // Mostrar el formulario de edición
                editPrestamoForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un préstamo para editar.");
            }
        }


        








        private void btnConfirmar_Click(object sender, EventArgs e)
        {
          
          

            
        }

        private void VeMaterial_Click(object sender, EventArgs e)
        {
         
            // Cadena de conexión a la base de datos
            string connectionString = "Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;";

            // Consulta SQL para obtener los materiales activos
            string query = "SELECT id_material, titulo, autor, año_publicacion, categoria, tipo_material, estado " +
                           "FROM material_bibliografico " +
                           "WHERE estado = 'Activo'";

            // Crear una conexión SQL
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    // Crear un adaptador de datos
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();

                    // Rellenar el DataTable con los datos de la consulta
                    dataAdapter.Fill(dataTable);

                    // Establecer el DataGridView para mostrar los datos
                    DataMaterial.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar los materiales: " + ex.Message);
                }
            }
        }

        private void ElMaterial_Click(object sender, EventArgs e)
        {
         
            // Verificar si se ha seleccionado una fila en el DataGridView
            if (DataMaterial.SelectedRows.Count > 0)
            {
                // Obtener el ID del material seleccionado
                int idMaterial = Convert.ToInt32(DataMaterial.SelectedRows[0].Cells["id_material"].Value);

                // Cadena de conexión a la base de datos
                string connectionString = "Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;";

                // Consulta SQL para actualizar el estado del material a "Inactivo"
                string query = "UPDATE material_bibliografico " +
                               "SET estado = 'Inactivo', updated_by = @updated_by, updated_at = @updated_at " +
                               "WHERE id_material = @id_material";

                // Crear una conexión a la base de datos
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Crear un comando SQL para ejecutar la actualización
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@id_material", idMaterial); // ID del material
                    command.Parameters.AddWithValue("@updated_by", empleadoId); // ID del empleado que está actualizando
                    command.Parameters.AddWithValue("@updated_at", DateTime.Now); // Fecha de actualización

                    try
                    {
                        // Abrir la conexión
                        connection.Open();

                        // Ejecutar el comando de actualización
                        int rowsAffected = command.ExecuteNonQuery();

                        // Verificar si la actualización fue exitosa
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Material marcado como Inactivo correctamente.");

                            // Recargar los materiales activos en el DataGridView
                            VeMaterial_Click(sender, e);
                        }
                        else
                        {
                            MessageBox.Show("No se pudo actualizar el material.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al eliminar material: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un material para eliminar.");
            }
        }

        private void VeProveedores_Click(object sender, EventArgs e)
        {
        
            // Cadena de conexión a la base de datos
            string connectionString = "Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;";

            // Consulta SQL para obtener los proveedores con estado 'Activo'
            string query = "SELECT id_proveedor, nombre, direccion, telefono, estado " +
                           "FROM proveedores WHERE estado = 'Activo'"; // Solo campos existentes en la tabla

            // Crear una conexión a la base de datos
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Crear un adaptador para ejecutar la consulta y llenar el DataGridView
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);

                // Crear un DataTable para almacenar los resultados
                DataTable dataTable = new DataTable();

                try
                {
                    // Llenar el DataTable con los datos de la base de datos
                    dataAdapter.Fill(dataTable);

                    // Asignar el DataTable al DataGridView para mostrar los datos
                    DataProveedores.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    // Mostrar un mensaje de error si ocurre un problema
                    MessageBox.Show("Error al cargar los proveedores: " + ex.Message);
                }
            }
        }

        

        private void ElProveedores_Click(object sender, EventArgs e)
        {
            
            // Verificar si se ha seleccionado un proveedor en el DataGridView
            if (DataProveedores.SelectedRows.Count > 0)
            {
                // Obtener el id del proveedor seleccionado
                int idProveedor = Convert.ToInt32(DataProveedores.SelectedRows[0].Cells["id_proveedor"].Value);

                // Cadena de conexión a la base de datos
                string connectionString = "Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;";

                // Consulta SQL para actualizar el estado del proveedor a 'Inactivo'
                string query = "UPDATE proveedores SET estado = 'Inactivo', updated_by = @updatedBy, updated_at = @updatedAt WHERE id_proveedor = @idProveedor";

                // Obtener la ID del empleado que está realizando la actualización (usamos la variable global empleadoId)
                int empleadoId = 1; // Usar la variable global empleadoId (asegurarte que esta variable esté inicializada con el ID del empleado logueado)
                DateTime fechaActual = DateTime.Now;

                // Crear la conexión y ejecutar la consulta
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        // Crear el comando SQL
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            // Agregar los parámetros para evitar inyecciones SQL
                            command.Parameters.AddWithValue("@idProveedor", idProveedor);
                            command.Parameters.AddWithValue("@updatedBy", empleadoId);  // Empleado que realiza la actualización
                            command.Parameters.AddWithValue("@updatedAt", fechaActual);  // Fecha actual de la actualización

                            // Ejecutar la consulta de actualización
                            int rowsAffected = command.ExecuteNonQuery();

                            // Verificar si se actualizó el estado
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Proveedor desactivado correctamente.");
                                // Actualizar el DataGridView para reflejar los cambios
                                //LoadProveedores();
                            }
                            else
                            {
                                MessageBox.Show("No se pudo desactivar el proveedor.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al desactivar el proveedor: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un proveedor para desactivar.");
            }
        }

        private void VeCompra_Click(object sender, EventArgs e)
        {
            
            // Cadena de conexión a la base de datos
            string connectionString = "Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;";

            // Consulta SQL para obtener las compras con estado 'Activo'
            string query = "SELECT c.id_compra, c.fecha_compra, p.nombre AS proveedor, m.titulo AS material, c.estado " +
                           "FROM compras c " +
                           "JOIN proveedores p ON c.id_proveedor = p.id_proveedor " +
                           "JOIN material_bibliografico m ON c.id_material = m.id_material " +
                           "WHERE c.estado = 'Activo'";

            // Crear una conexión a la base de datos
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Crear un adaptador para ejecutar la consulta y llenar el DataGridView
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();

                try
                {
                    // Llenar el DataTable con los datos de la consulta
                    dataAdapter.Fill(dataTable);

                    // Asignar el DataTable al DataGridView
                    datacompra.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar las compras: " + ex.Message);
                }
            }
        }

        private void ElCompra_Click(object sender, EventArgs e)
        {

            // Verificar si se ha seleccionado una compra en el DataGridView
            if (datacompra.SelectedRows.Count > 0)
            {
                // Obtener el id de la compra seleccionada
                int idCompra = Convert.ToInt32(datacompra.SelectedRows[0].Cells["id_compra"].Value);

                // Cadena de conexión a la base de datos
                string connectionString = "Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;";

                // Consulta SQL para actualizar el estado de la compra a 'Inactivo'
                string query = "UPDATE compras SET estado = 'Inactivo', updated_by = @updatedBy, updated_at = @updatedAt WHERE id_compra = @idCompra";

                // Obtener la ID del empleado que está realizando la actualización (usamos la variable global empleadoId)
                int empleadoId = this.empleadoId; // Suponiendo que la variable global empleadoId está correctamente configurada

                // Obtener la fecha y hora actual
                DateTime currentDate = DateTime.Now;

                // Ejecutar la consulta de actualización
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@idCompra", idCompra);
                        command.Parameters.AddWithValue("@updatedBy", empleadoId);
                        command.Parameters.AddWithValue("@updatedAt", currentDate);

                        // Abrir la conexión y ejecutar la consulta
                        try
                        {
                            connection.Open();
                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("La compra ha sido marcada como inactiva.");
                            }
                            else
                            {
                                MessageBox.Show("No se pudo actualizar el estado de la compra.");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error al actualizar el estado de la compra: " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una compra.");
            }
        }

        private void VeDevoluciones_Click(object sender, EventArgs e)
        {
        
            // Cadena de conexión a la base de datos
            string connectionString = "Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;";

            // Consulta SQL para obtener el historial de devoluciones con estado 'Activo'
            string query = "SELECT hd.id_devolucion, p.fecha_prestamo, hd.fecha_devolucion, " +
                           "u.nombre AS usuario, m.titulo AS material, e.nombre AS empleado, " +
                           "hd.estado " +
                           "FROM historial_devoluciones hd " +
                           "JOIN prestamos p ON hd.id_prestamo = p.id_prestamo " +
                           "JOIN usuarios u ON p.id_usuario = u.id_usuario " +
                           "JOIN material_bibliografico m ON p.id_material = m.id_material " +
                           "JOIN empleado e ON p.id_empleado = e.id_empleado " +
                           "WHERE hd.estado = 'Activo'";  // Filtrar por estado activo

            // Crear una conexión a la base de datos
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Crear un adaptador para ejecutar la consulta y llenar el DataGridView
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();

                // Llenar el DataTable con los resultados de la consulta
                dataAdapter.Fill(dataTable);

                // Asignar el DataTable al DataGridView
                DataDevoluciones.DataSource = dataTable;
            }
        }

        

        private void ElDevoluciones_Click(object sender, EventArgs e)
        {
         
            // Verificar si se ha seleccionado una devolución en el DataGridView
            if (DataDevoluciones.SelectedRows.Count > 0)
            {
                // Obtener el id de la devolución seleccionada
                int idDevolucion = Convert.ToInt32(DataDevoluciones.SelectedRows[0].Cells["id_devolucion"].Value);

                // Cadena de conexión a la base de datos
                string connectionString = "Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;";

                // Consulta SQL para actualizar el estado de la devolución a 'Inactivo'
                string query = "UPDATE historial_devoluciones SET estado = 'Inactivo', updated_by = @updatedBy, updated_at = @updatedAt WHERE id_devolucion = @idDevolucion";

                // Obtener la ID del empleado que está realizando la actualización (usamos la variable global empleadoId)
                int updatedBy = empleadoId;
                DateTime updatedAt = DateTime.Now;

                // Crear una conexión a la base de datos
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Crear un comando para ejecutar la consulta
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Agregar parámetros para la consulta
                        command.Parameters.AddWithValue("@idDevolucion", idDevolucion);
                        command.Parameters.AddWithValue("@updatedBy", updatedBy);
                        command.Parameters.AddWithValue("@updatedAt", updatedAt);

                        try
                        {
                            // Abrir la conexión a la base de datos
                            connection.Open();

                            // Ejecutar la consulta para actualizar el estado
                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("La devolución ha sido marcada como inactiva.");
                                // Recargar el DataGridView con los datos actualizados
                                VeDevoluciones_Click(sender, e); // Volver a cargar los datos de devoluciones
                            }
                            else
                            {
                                MessageBox.Show("No se encontró la devolución para actualizar.");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error al eliminar la devolución: " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una devolución para inactivar.");
            }
        }

        

        private void VeMulta_Click(object sender, EventArgs e)
        {

            // Cadena de conexión a la base de datos
            string connectionString = "Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;";

            // Consulta SQL para obtener las multas activas
            string query = "SELECT m.id_multa, u.nombre AS usuario, m.monto, m.fecha, m.estado " +
                           "FROM multas m " +
                           "JOIN usuarios u ON m.id_usuario = u.id_usuario " +
                           "WHERE m.estado = 'Activo'";  // Solo mostrar multas activas

            // Crear una conexión a la base de datos
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Crear un adaptador para ejecutar la consulta y llenar el DataGridView
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();

                try
                {
                    // Llenar el DataTable con los resultados de la consulta
                    dataAdapter.Fill(dataTable);

                    // Asignar el DataTable al DataGridView para mostrar las multas
                    DataMulta.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar las multas: " + ex.Message);
                }
            }
        }

        private void ElMulta_Click(object sender, EventArgs e)
        {
         
            // Verificar si se ha seleccionado una multa en el DataGridView
            if (DataMulta.SelectedRows.Count > 0)
            {
                // Obtener el id de la multa seleccionada
                int idMulta = Convert.ToInt32(DataMulta.SelectedRows[0].Cells["id_multa"].Value);

                // Cadena de conexión a la base de datos
                string connectionString = "Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;";

                // Consulta SQL para actualizar el estado de la multa a 'Inactivo'
                string query = "UPDATE multas SET estado = 'Inactivo', updated_by = @updatedBy, updated_at = @updatedAt WHERE id_multa = @idMulta";

                // Obtener la ID del empleado que está realizando la actualización (usamos la variable global empleadoId)
                int empleadoId = this.empleadoId;
                // Establecer la fecha y hora de la actualización
                DateTime updatedAt = DateTime.Now;

                // Crear la conexión y el comando
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);

                    // Añadir los parámetros al comando
                    command.Parameters.AddWithValue("@idMulta", idMulta);
                    command.Parameters.AddWithValue("@updatedBy", empleadoId);
                    command.Parameters.AddWithValue("@updatedAt", updatedAt);

                    try
                    {
                        // Abrir la conexión y ejecutar la consulta
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        // Verificar si se actualizó la multa
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("La multa ha sido marcada como inactiva.");
                            // Recargar el DataGridView de multas si es necesario
                            VeMulta_Click(sender, e); // Esto recarga los datos de multas.
                        }
                        else
                        {
                            MessageBox.Show("No se pudo actualizar la multa.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al eliminar la multa: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una multa para eliminar.");
            }
        }

        private void VeSancion_Click(object sender, EventArgs e)
        {
            // Cadena de conexión a la base de datos
            string connectionString = "Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;";

            // Consulta SQL para obtener las sanciones con estado 'Activo'
            string query = "SELECT s.id_sancion, u.nombre AS usuario, s.tipo_sancion, s.descripcion, s.fecha_sancion, e.nombre AS empleado, s.estado " +
                           "FROM sanciones s " +
                           "JOIN usuarios u ON s.id_usuario = u.id_usuario " +
                           "JOIN empleado e ON s.id_empleado = e.id_empleado " +
                           "WHERE s.estado = 'Activo'";  // Solo sanciones activas

            // Crear una conexión a la base de datos
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Crear un adaptador para ejecutar la consulta y llenar el DataGridView
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);

                // Crear un DataTable para almacenar los datos
                DataTable dataTable = new DataTable();

                try
                {
                    // Llenar el DataTable con los resultados de la consulta
                    dataAdapter.Fill(dataTable);

                    // Asignar el DataTable al DataGridView
                    DataSanciones.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    // Mostrar el error si ocurre uno
                    MessageBox.Show("Error al cargar las sanciones: " + ex.Message);
                }
            }
        }

        private void ElSancion_Click(object sender, EventArgs e)
        {
            // Verificar si se ha seleccionado una sanción en el DataGridView
            if (DataSanciones.SelectedRows.Count > 0)
            {
                // Obtener el id de la sanción seleccionada
                int idSancion = Convert.ToInt32(DataSanciones.SelectedRows[0].Cells["id_sancion"].Value);

                // Cadena de conexión a la base de datos
                string connectionString = "Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;";

                // Consulta SQL para actualizar el estado de la sanción a 'Inactivo'
                string query = "UPDATE sanciones SET estado = 'Inactivo', updated_by = @updatedBy, updated_at = @updatedAt WHERE id_sancion = @idSancion";

                // Obtener la ID del empleado que está realizando la actualización (usamos la variable global empleadoId)
                int empleadoId = 123;  // Aquí debes usar la variable global empleadoId

                // Obtener la fecha y hora actuales
                DateTime currentDate = DateTime.Now;

                // Crear una conexión a la base de datos
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Crear un comando para ejecutar la consulta
                    SqlCommand command = new SqlCommand(query, connection);

                    // Agregar los parámetros al comando
                    command.Parameters.AddWithValue("@idSancion", idSancion);
                    command.Parameters.AddWithValue("@updatedBy", empleadoId);
                    command.Parameters.AddWithValue("@updatedAt", currentDate);

                    try
                    {
                        // Abrir la conexión
                        connection.Open();

                        // Ejecutar la consulta de actualización
                        command.ExecuteNonQuery();

                        // Mostrar mensaje de éxito
                        MessageBox.Show("Sanción eliminada correctamente.");

                        // Recargar los datos en el DataGridView
                        VeSancion_Click(sender, e);  // Llamar al método de "ver sanciones" para actualizar la vista

                    }
                    catch (Exception ex)
                    {
                        // Mostrar mensaje de error
                        MessageBox.Show("Error al eliminar sanción: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una sanción para eliminar.");
            }
        }

        private void VeEvento_Click(object sender, EventArgs e)
        {
            
            // Cadena de conexión a la base de datos
            string connectionString = "Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;";

            // Consulta SQL para obtener los eventos con estado 'Activo'
            string query = "SELECT id_evento, nombre_evento, descripcion, fecha_inicio, fecha_fin, estado " +
                           "FROM eventos " +
                           "WHERE estado = 'Activo'";

            // Crear una conexión a la base de datos
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Crear un adaptador para ejecutar la consulta y llenar el DataGridView
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);

                // Crear un DataTable para almacenar los resultados
                DataTable dataTable = new DataTable();

                try
                {
                    // Llenar el DataTable con los resultados de la consulta
                    dataAdapter.Fill(dataTable);

                    // Asignar el DataTable al DataGridView para mostrar los eventos
                    DataEventos.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar los eventos: " + ex.Message);
                }
            }
        }

        private void ElEvento_Click(object sender, EventArgs e)
        {
            // Verificar si se ha seleccionado un evento en el DataGridView
            if (DataEventos.SelectedRows.Count > 0)
            {
                // Obtener el id del evento seleccionado
                int idEvento = Convert.ToInt32(DataEventos.SelectedRows[0].Cells["id_evento"].Value);

                // Cadena de conexión a la base de datos
                string connectionString = "Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;";

                // Consulta SQL para actualizar el estado del evento a 'Inactivo'
                string query = "UPDATE eventos SET estado = 'Inactivo', updated_by = @updatedBy, updated_at = @updatedAt WHERE id_evento = @idEvento";

                // Obtener la ID del empleado que está realizando la actualización (usamos la variable global empleadoId)
                int empleadoId = this.empleadoId; // Suponiendo que empleadoId ya está definida en tu formulario

                // Fecha y hora de la actualización
                DateTime currentDate = DateTime.Now;

                // Crear una conexión a la base de datos
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Crear un comando para ejecutar la consulta
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Agregar los parámetros
                        command.Parameters.AddWithValue("@updatedBy", empleadoId);
                        command.Parameters.AddWithValue("@updatedAt", currentDate);
                        command.Parameters.AddWithValue("@idEvento", idEvento);

                        try
                        {
                            // Abrir la conexión
                            connection.Open();

                            // Ejecutar el comando
                            command.ExecuteNonQuery();

                            // Mensaje de éxito
                            MessageBox.Show("El evento ha sido marcado como inactivo.");
                        }
                        catch (Exception ex)
                        {
                            // En caso de error
                            MessageBox.Show("Error al eliminar evento: " + ex.Message);
                        }
                    }
                }

                // Actualizar el DataGridView
                VeEvento_Click(null, null);
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un evento para eliminar.");
            }
        }

        private void CrPrestamo_Click(object sender, EventArgs e)
        {

            // Supongamos que la variable global empleadoId está definida y contiene el ID del empleado
            CreatePrestamoForm crearPrestamoForm = new CreatePrestamoForm(empleadoId); // Pasa el ID del empleado
            crearPrestamoForm.Show();

        }

        private void CrMaterial_Click(object sender, EventArgs e)
        {
            // Suponiendo que el empleadoId está disponible
            CreateMaterialForm createMaterialForm = new CreateMaterialForm(empleadoId);
            createMaterialForm.Show();
        }

        private void CrProveedores_Click(object sender, EventArgs e)
        {
            CreateProveedorForm crearProveedorForm = new CreateProveedorForm(empleadoId);
            crearProveedorForm.Show();
        }

        private void CrCompra_Click(object sender, EventArgs e)
        {
            CreateCompraForm crearCompraForm = new CreateCompraForm(empleadoId);  // Pasar el ID del empleado
            crearCompraForm.Show();
        }

        private void CrDevoluciones_Click(object sender, EventArgs e)
        {
            // Crear el formulario de creación de devoluciones y pasar el empleadoId
            CreateDevolucionForm crearDevolucionForm = new CreateDevolucionForm(empleadoId);
            crearDevolucionForm.Show();
        }

        private void CrMulta_Click(object sender, EventArgs e)
        {
            // Crear una instancia del formulario CreateMultaForm, pasando el id_empleado global
            CreateMultaForm crearMultaForm = new CreateMultaForm(empleadoId);
            crearMultaForm.Show();
        }

        private void CrSancion_Click(object sender, EventArgs e)
        {
            // Suponiendo que el empleadoId está disponible
            CreateSancionForm crearSancionForm = new CreateSancionForm(empleadoId);
            crearSancionForm.Show();
        }

        private void CrEvento_Click(object sender, EventArgs e)
        {
            CreateEventoForm createEventoForm = new CreateEventoForm(empleadoId);
            createEventoForm.Show();
        }

        private void AcMaterial_Click(object sender, EventArgs e)
        {

            // Verificar que se ha seleccionado un material en el DataGridView
            if (DataMaterial.SelectedRows.Count > 0)
            {
                // Obtener el id del material seleccionado desde el DataGridView
                int idMaterial = Convert.ToInt32(DataMaterial.SelectedRows[0].Cells["id_material"].Value);

                // Cadena de conexión a la base de datos
                string connectionString = "Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;";
                string query = "SELECT titulo, autor, año_publicacion, categoria, tipo_material FROM material_bibliografico WHERE id_material = @idMaterial";

                // Obtener los datos del material desde la base de datos
                string titulo = string.Empty;
                string autor = string.Empty;
                int añoPublicacion = 0;
                string categoria = string.Empty;
                string tipoMaterial = string.Empty;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@idMaterial", idMaterial);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            titulo = reader.GetString(0);
                            autor = reader.GetString(1);
                            añoPublicacion = reader.GetInt32(2);
                            categoria = reader.GetString(3);
                            tipoMaterial = reader.GetString(4);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al obtener los datos del material: " + ex.Message);
                        return;
                    }
                }

                // Crear el formulario EditMaterialForm y pasar los datos del material seleccionado
                EditarMaterialForm editMaterialForm = new EditarMaterialForm(idMaterial, titulo, autor, añoPublicacion, categoria, tipoMaterial, empleadoId);

                // Mostrar el formulario de edición
                editMaterialForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un material para editar.");
            }
        }

        private void AcProveedores_Click(object sender, EventArgs e)
        {
         
            // Verificar que se ha seleccionado un proveedor en el DataGridView
            if (DataProveedores.SelectedRows.Count > 0)
            {
                // Obtener el id del proveedor seleccionado desde el DataGridView
                int idProveedor = Convert.ToInt32(DataProveedores.SelectedRows[0].Cells["id_proveedor"].Value);

                // Cadena de conexión a la base de datos
                string connectionString = "Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;";
                string query = "SELECT nombre, direccion, telefono FROM proveedores WHERE id_proveedor = @idProveedor";

                // Obtener los datos del proveedor desde la base de datos
                string nombre = string.Empty;
                string direccion = string.Empty;
                string telefono = string.Empty;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@idProveedor", idProveedor);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            nombre = reader.GetString(0);
                            direccion = reader.GetString(1);
                            telefono = reader.GetString(2);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al obtener los datos del proveedor: " + ex.Message);
                        return;
                    }
                }

                // Crear el formulario EditProveedorForm y pasar los datos del proveedor seleccionado
                EditProveedorForm editProveedorForm = new EditProveedorForm(idProveedor, nombre, direccion, telefono, empleadoId);

                // Mostrar el formulario de edición
                editProveedorForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un proveedor para editar.");
            }
        }

        private void AcCompra_Click(object sender, EventArgs e)
        {
         
            // Verificar que se ha seleccionado una compra en el DataGridView
            if (datacompra.SelectedRows.Count > 0)
            {
                // Obtener el id de la compra seleccionada desde el DataGridView
                int idCompra = Convert.ToInt32(datacompra.SelectedRows[0].Cells["id_compra"].Value);

                // Cadena de conexión a la base de datos
                string connectionString = "Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;";
                string query = "SELECT fecha_compra, id_proveedor, id_material FROM compras WHERE id_compra = @idCompra";

                // Obtener los datos de la compra desde la base de datos
                DateTime fechaCompra = DateTime.Now;
                int idProveedor = 0;
                int idMaterial = 0;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@idCompra", idCompra);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            fechaCompra = reader.GetDateTime(0);  // Fecha de la compra
                            idProveedor = reader.GetInt32(1);  // ID del proveedor
                            idMaterial = reader.GetInt32(2);  // ID del material
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al obtener los datos de la compra: " + ex.Message);
                        return;
                    }
                }

                // Crear el formulario EditCompraForm y pasar los datos de la compra seleccionada
                EditCompraForm editCompraForm = new EditCompraForm(idCompra, fechaCompra, idProveedor, idMaterial, empleadoId);

                // Mostrar el formulario de edición
                editCompraForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Por favor, selecciona una compra para editar.");
            }
        }

        private void AcDevoluciones_Click(object sender, EventArgs e)
        {
            // Verificar que se ha seleccionado una devolución en el DataGridView
            if (DataDevoluciones.SelectedRows.Count > 0)
            {
                // Obtener el id de la devolución seleccionada desde el DataGridView
                int idDevolucion = Convert.ToInt32(DataDevoluciones.SelectedRows[0].Cells["id_devolucion"].Value);

                // Cadena de conexión a la base de datos
                string connectionString = "Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;";
                string query = @"
        SELECT hd.id_prestamo, hd.fecha_devolucion, p.id_usuario, p.id_material 
        FROM historial_devoluciones hd
        JOIN prestamos p ON hd.id_prestamo = p.id_prestamo
        WHERE hd.id_devolucion = @idDevolucion";

                // Obtener los datos de la devolución desde la base de datos
                DateTime fechaDevolucion = DateTime.Now;
                int idUsuario = 0;
                int idMaterial = 0;
                int idPrestamo = 0;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@idDevolucion", idDevolucion);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            idPrestamo = reader.GetInt32(0);
                            fechaDevolucion = reader.GetDateTime(1);
                            idUsuario = reader.GetInt32(2);
                            idMaterial = reader.GetInt32(3);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al obtener los datos de la devolución: " + ex.Message);
                        return;
                    }
                }

                // Crear el formulario EditDevolucionForm y pasar los datos de la devolución seleccionada
                EditDevolucionForm editDevolucionForm = new EditDevolucionForm(idDevolucion, idPrestamo, fechaDevolucion, idUsuario, idMaterial, empleadoId);

                // Mostrar el formulario de edición
                editDevolucionForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Por favor, selecciona una devolución para editar.");
            }

        }

        private void AcMulta_Click(object sender, EventArgs e)
        {
            // Verificar que se ha seleccionado una multa en el DataGridView
            if (DataMulta.SelectedRows.Count > 0)
            {
                // Obtener el id de la multa seleccionada desde el DataGridView
                int idMulta = Convert.ToInt32(DataMulta.SelectedRows[0].Cells["id_multa"].Value);

                // Cadena de conexión a la base de datos
                string connectionString = "Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;";
                string query = "SELECT id_usuario, monto, fecha FROM multas WHERE id_multa = @idMulta";

                // Obtener los datos de la multa desde la base de datos
                decimal monto = 0;
                DateTime fecha = DateTime.Now;
                int idUsuario = 0;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@idMulta", idMulta);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            idUsuario = reader.GetInt32(0);
                            monto = reader.GetDecimal(1);
                            fecha = reader.GetDateTime(2);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al obtener los datos de la multa: " + ex.Message);
                        return;
                    }
                }

                // Crear el formulario EditMultaForm y pasar los datos de la multa seleccionada
                EditMultaForm editMultaForm = new EditMultaForm(idMulta, idUsuario, monto, fecha, empleadoId);

                // Mostrar el formulario de edición
                editMultaForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Por favor, selecciona una multa para editar.");
            }
        }

        private void AcSancion_Click(object sender, EventArgs e)
        {
          
            // Verificar que se ha seleccionado una sanción en el DataGridView
            if (DataSanciones.SelectedRows.Count > 0)
            {
                // Obtener el ID de la sanción seleccionada desde el DataGridView
                int idSancion = Convert.ToInt32(DataSanciones.SelectedRows[0].Cells["id_sancion"].Value);

                // Cadena de conexión a la base de datos
                string connectionString = "Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;";
                string query = "SELECT tipo_sancion, descripcion, fecha_sancion FROM sanciones WHERE id_sancion = @idSancion";

                // Obtener los datos de la sanción desde la base de datos
                string tipoSancion = "";
                string descripcion = "";
                DateTime fechaSancion = DateTime.Now;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@idSancion", idSancion);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            tipoSancion = reader.GetString(0);
                            descripcion = reader.GetString(1);
                            fechaSancion = reader.GetDateTime(2);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al obtener los datos de la sanción: " + ex.Message);
                        return;
                    }
                }

                // Crear el formulario de edición y pasar los datos de la sanción seleccionada
                EditSancionForm editSancionForm = new EditSancionForm(idSancion, tipoSancion, descripcion, fechaSancion, empleadoId);

                // Mostrar el formulario de edición
                editSancionForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Por favor, selecciona una sanción para editar.");
            }
        }

        private void AcEvento_Click(object sender, EventArgs e)
        {
            
            // Verificar que se ha seleccionado un evento en el DataGridView
            if (DataEventos.SelectedRows.Count > 0)
            {
                // Obtener el id del evento seleccionado desde el DataGridView
                int idEvento = Convert.ToInt32(DataEventos.SelectedRows[0].Cells["id_evento"].Value);

                // Cadena de conexión a la base de datos
                string connectionString = "Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;";
                string query = "SELECT nombre_evento, descripcion, fecha_inicio, fecha_fin FROM eventos WHERE id_evento = @idEvento";

                // Obtener los datos del evento desde la base de datos
                string nombreEvento = string.Empty;
                string descripcionEvento = string.Empty;
                DateTime fechaInicio = DateTime.Now;
                DateTime fechaFin = DateTime.Now;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@idEvento", idEvento);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            nombreEvento = reader.GetString(0);
                            descripcionEvento = reader.GetString(1);
                            fechaInicio = reader.GetDateTime(2);
                            fechaFin = reader.GetDateTime(3);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al obtener los datos del evento: " + ex.Message);
                        return;
                    }
                }

                // Crear el formulario EditEventoForm y pasar los datos del evento seleccionado
                EditEventoForm editEventoForm = new EditEventoForm(idEvento, nombreEvento, descripcionEvento, fechaInicio, fechaFin, empleadoId);

                // Mostrar el formulario de edición
                editEventoForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un evento para editar.");
            }
        }

        private void tabPage9_Click(object sender, EventArgs e)
        {

        }

        private void button29_Click(object sender, EventArgs e)
        {
           
            // Cadena de conexión a la base de datos
            string connectionString = "Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;";

            // Consulta SQL para obtener las solicitudes de donación con estado 'Activo'
            string query = "SELECT s.id_solicitud, s.monto_donacion, s.fecha_solicitud, " +
                           "u.nombre AS usuario, e.nombre AS empleado, s.estado " +  // Incluimos el campo 'estado' de la tabla solicitudes_donaciones
                           "FROM solicitudes_donaciones s " +
                           "JOIN usuarios u ON s.id_usuario = u.id_usuario " +
                           "JOIN empleado e ON s.created_by = e.id_empleado " +
                           "WHERE s.estado = 'Activo'";  // Condición para solo mostrar las solicitudes activas

            // Crear una conexión a la base de datos
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Crear un adaptador para ejecutar la consulta y llenar el DataGridView
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);

                // Crear un DataTable para almacenar los resultados de la consulta
                DataTable dataTable = new DataTable();

                try
                {
                    // Abrir la conexión y llenar el DataTable
                    connection.Open();
                    dataAdapter.Fill(dataTable);

                    // Asignar el DataTable al DataGridView
                    DataSDonacion.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    // Manejo de excepciones si ocurre un error al acceder a la base de datos
                    MessageBox.Show("Error al cargar las solicitudes de donación: " + ex.Message);
                }
            }
        }

        private void button30_Click(object sender, EventArgs e)
        {
           
            // Verificar que se ha seleccionado una solicitud de donación en el DataGridView
            if (DataSDonacion.SelectedRows.Count > 0)
            {
                // Obtener el id de la solicitud de donación seleccionada desde el DataGridView
                int idSolicitud = Convert.ToInt32(DataSDonacion.SelectedRows[0].Cells["id_solicitud"].Value);

                // Cadena de conexión a la base de datos
                string connectionString = "Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;";

                // Consulta SQL para actualizar el estado de la solicitud de donación a "Inactivo" y asignar el id_empleado que lo está actualizando
                string query = "UPDATE solicitudes_donaciones " +
                               "SET estado = 'Inactivo', updated_by = @updated_by " +
                               "WHERE id_solicitud = @id_solicitud";

                // Crear una conexión a la base de datos
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Crear el comando de actualización
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Agregar los parámetros
                        command.Parameters.AddWithValue("@updated_by", empleadoId);  // empleadoId debería ser la variable global con la ID del empleado actual
                        command.Parameters.AddWithValue("@id_solicitud", idSolicitud);

                        try
                        {
                            // Abrir la conexión
                            connection.Open();

                            // Ejecutar el comando de actualización
                            int rowsAffected = command.ExecuteNonQuery();

                            // Verificar si se actualizó la solicitud de donación
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("La solicitud de donación ha sido marcada como inactiva.");
                            }
                            else
                            {
                                MessageBox.Show("No se encontró la solicitud de donación.");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error al eliminar la solicitud de donación: " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona una solicitud de donación para marcar como inactiva.");
            }
        }

        private void button32_Click(object sender, EventArgs e)
        {
          
            // Supongamos que 'empleadoId' es una variable global que tiene el ID del empleado que está logueado
            CrearSolicitudDonacionForm crearSolicitudForm = new CrearSolicitudDonacionForm(empleadoId);
            crearSolicitudForm.ShowDialog(); // Mostrar el formulario de forma modal
        }

        private void button31_Click(object sender, EventArgs e)
        {
         
            // Verificar que se ha seleccionado una solicitud de donación en el DataGridView
            if (DataSDonacion.SelectedRows.Count > 0)
            {
                // Obtener el id de la solicitud de donación seleccionada desde el DataGridView
                int idSolicitud = Convert.ToInt32(DataSDonacion.SelectedRows[0].Cells["id_solicitud"].Value);

                // Cadena de conexión a la base de datos
                string connectionString = "Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;";
                string query = "SELECT id_usuario, monto_donacion, fecha_solicitud FROM solicitudes_donaciones WHERE id_solicitud = @idSolicitud";

                // Obtener los datos de la solicitud de donación desde la base de datos
                int idUsuario = 0;
                decimal montoDonacion = 0;
                DateTime fechaSolicitud = DateTime.Now;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@idSolicitud", idSolicitud);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            idUsuario = reader.GetInt32(0);
                            montoDonacion = reader.GetDecimal(1);
                            fechaSolicitud = reader.GetDateTime(2);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al obtener los datos de la solicitud de donación: " + ex.Message);
                        return;
                    }
                }

                // Crear el formulario EditSolicitudDonacionForm y pasar los datos de la solicitud de donación seleccionada
                EditSolicitudDonacionForm editForm = new EditSolicitudDonacionForm(idSolicitud, montoDonacion, fechaSolicitud, idUsuario, empleadoId);

                // Mostrar el formulario de edición
                editForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Por favor, selecciona una solicitud de donación para editar.");
            }
        }

    }
}












 



























