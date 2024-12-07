using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace FromFinal
{
    public partial class Form1 : Form
    {
        private int empleadoId = -1;
        public Form1()
        {
            InitializeComponent();
        }

        // Método para encriptar la contraseña con SHA256
        private string GetSHA256Hash(string input)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Calcular el hash de la contraseña
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Convertir el resultado a un formato hexadecimal
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        // Método para verificar el inicio de sesión
        private bool Login(string username, string passwordHash)
        {
            string connectionString = "Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT id_empleado FROM Empleado WHERE nombre = @Username AND contraseña = @PasswordHash AND estado = 'activo'";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@PasswordHash", passwordHash);

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();  // Leer el primer (y único) resultado
                        empleadoId = reader.GetInt32(0);  // Guardar el id_empleado
                        return true;  // Login exitoso
                    }
                    else
                    {
                        MessageBox.Show("Usuario o contraseña incorrectos.");
                        return false;  // Login fallido
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al conectarse a la base de datos: " + ex.Message);
                    return false;
                }
            }
        }

        // Evento del botón de inicio de sesión
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            // Encriptar la contraseña con SHA256
            string passwordHash = GetSHA256Hash(password);

            // Llamar al método que valida el inicio de sesión
            if (Login(username, passwordHash))
            {
                // Si el login es exitoso, mostramos el ID del empleado en un mensaje (puedes usarlo más tarde en los CRUDs)
                MessageBox.Show("Login exitoso. ID del empleado: " + empleadoId);

                Control controlForm = new Control(empleadoId);  // Pasamos empleadoId al constructor de Control
                controlForm.Show();
                this.Hide();  // Ocultamos el formulario de inicio de sesión
            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrectos.");
            }
        }

        // Evento del botón de crear cuenta (aún no implementado)
        private void btnCreateAccount_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string passwordHash = GetSHA256Hash(password);

            // Aquí agregarías el código para insertar un nuevo empleado en la base de datos
            string connectionString = "Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;";
            string query = "INSERT INTO Empleado (nombre, contraseña, estado) VALUES (@username, @passwordHash, 'activo')";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@passwordHash", passwordHash);

                connection.Open();
                command.ExecuteNonQuery();
                MessageBox.Show("Cuenta creada con éxito");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
