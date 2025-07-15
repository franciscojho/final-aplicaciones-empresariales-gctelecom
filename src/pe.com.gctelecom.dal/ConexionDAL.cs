using Microsoft.Data.SqlClient;

namespace pe.com.gctelecom.dal
{
    public class ConexionDAL
    {
        private string cadenaDeConexion = 
            "Data Source=localhost;" +
            "Initial Catalog=gctelecomdb;" +
            "Integrated Security=SSPI;" +
            "TrustServerCertificate=true;";

        private SqlConnection proveedorSql;

        public SqlConnection? Conectar()
        {
            try
            {
                proveedorSql = new SqlConnection(cadenaDeConexion);
                proveedorSql.Open();
                return proveedorSql;
            }
            catch (SqlException ex)
            {

                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public void CerrarConexion()
        {
            proveedorSql.Close();
            proveedorSql.Dispose();
        }

    }
}
