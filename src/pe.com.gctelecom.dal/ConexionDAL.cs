using System.Data;
using Microsoft.Data.SqlClient;

namespace pe.com.gctelecom.dal
{
    public class ConexionDAL
    {
        private string cadenaDeConexion =
            "Data Source=DESKTOP-RFDMQII\\SQLEXPRESS;" +
            "Initial Catalog=gctelecomdb;" +
            "Integrated Security=SSPI;" +
            "TrustServerCertificate=true;";

        private SqlConnection? ProveedorSql;

        public (SqlCommand?, SqlException?) ConectarSinParametros(string NombreProcedimientoAlmacenado)
        {
            try
            {
                ProveedorSql = new SqlConnection(cadenaDeConexion);
                ProveedorSql.Open();

                SqlCommand ComandoSql = new SqlCommand();
                ComandoSql.CommandType = CommandType.StoredProcedure;
                ComandoSql.CommandText = NombreProcedimientoAlmacenado;
                ComandoSql.Connection = ProveedorSql;

                return (ComandoSql, null);
            }
            catch (SqlException excepcion)
            {

                Console.WriteLine(excepcion.ToString());
                return (null, excepcion);
            }
        }

        public (SqlCommand?, SqlException?) ConectarConParametros(string NombreProcedimientoAlmacenado, List<KeyValuePair<string, object>> Parametros)
        {
            try
            {
                ProveedorSql = new SqlConnection(cadenaDeConexion);
                ProveedorSql.Open();

                SqlCommand ComandoSql = new SqlCommand();
                ComandoSql.CommandType = CommandType.StoredProcedure;
                ComandoSql.CommandText = NombreProcedimientoAlmacenado;
                ComandoSql.Connection = ProveedorSql;
                Parametros.ForEach(parametro => {
                    ComandoSql.Parameters.AddWithValue(parametro.Key, parametro.Value);
                });

                return (ComandoSql, null);
            }
            catch (SqlException excepcion)
            {

                Console.WriteLine(excepcion.ToString());
                return (null, excepcion);
            }
        }

        public void CerrarConexion()
        {
            if (ProveedorSql != null)
            {
                ProveedorSql.Close();
                ProveedorSql.Dispose();
            }
        }

    }
}
