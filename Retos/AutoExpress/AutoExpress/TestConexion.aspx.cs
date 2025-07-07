using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;

namespace AutoExpres  // ← Sin la 's' final
{
    public partial class TestConexion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnProbar_Click(object sender, EventArgs e)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["AutoExpressDB"].ConnectionString;
                lblResultado.Text = "Cadena de conexión: " + connectionString + "<br/><br/>";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    lblResultado.Text += "✅ Conexión exitosa!<br/>";
                    lblResultado.Text += "Servidor: " + connection.DataSource + "<br/>";
                    lblResultado.Text += "Base de datos: " + connection.Database + "<br/>";
                    lblResultado.ForeColor = Color.Green;

                    // Probar consulta
                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Carros", connection))
                    {
                        int count = (int)cmd.ExecuteScalar();
                        lblResultado.Text += "Registros en tabla Carros: " + count;
                    }
                }
            }
            catch (Exception ex)
            {
                lblResultado.Text = "❌ Error de conexión:<br/>" + ex.Message;
                lblResultado.ForeColor = Color.Red;
            }
        }
    }
}
