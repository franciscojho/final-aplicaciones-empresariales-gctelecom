using Microsoft.Data.SqlClient;
using pe.com.gctelecom.bo;

namespace pe.com.gctelecom.dal
{
    public class VendedorDAL
    {
        private ConexionDAL ConexionDAL = new ConexionDAL();

        public List<VendedorBO> ObtenerVendedores(bool? EsVisible = null)
        {
            List<VendedorBO> ListaVendedores = new List<VendedorBO>();
            try
            {
                object nuloOBoleano = EsVisible != null ? Convert.ToInt16(EsVisible) : null;
                var VendedorParametros = new List<KeyValuePair<string, object>>()
                {
                    new KeyValuePair<string, object>("@es_visible", nuloOBoleano)
                };

                (SqlCommand?, SqlException?) Respuesta = ConexionDAL.ConectarConParametros("SP_OBTENER_VENDEDORES", VendedorParametros);
                if (Respuesta.Item1 != null)
                {
                    SqlCommand ComandoSql = Respuesta.Item1;
                    SqlDataReader RespuestaSql = ComandoSql.ExecuteReader();
                    while (RespuestaSql.Read())
                    {
                        VendedorBO Vendedor = new VendedorBO();
                        Vendedor.VendedorId = Convert.ToInt32(RespuestaSql["vendedor_id"].ToString());
                        Vendedor.Nombre = RespuestaSql["nombre"].ToString();
                        Vendedor.Correo = RespuestaSql["correo"].ToString();
                        Vendedor.Celular = RespuestaSql["celular"].ToString();
                        Vendedor.Direccion = RespuestaSql["direccion"].ToString();
                        Vendedor.EsVisible = Convert.ToBoolean(RespuestaSql["es_visible"].ToString());
                        ListaVendedores.Add(Vendedor);
                    }
                }
                else
                {
                    throw Respuesta.Item2;
                }
                return ListaVendedores;
            }
            catch (SqlException excepcion)
            {
                Console.WriteLine(excepcion.ToString());
                return ListaVendedores;
            }
            finally
            {
                ConexionDAL.CerrarConexion();
            }
        }

        private bool EnviarDatosVendedor(string NombreProcedimientoAlmacenado, List<KeyValuePair<string, object>> Parametros)
        {
            bool EsExitoso = false;
            try
            {

                (SqlCommand?, SqlException?) Respuesta = ConexionDAL.ConectarConParametros(NombreProcedimientoAlmacenado, Parametros);
                if (Respuesta.Item1 != null)
                {
                    SqlCommand ComandoSql = Respuesta.Item1;
                    EsExitoso = Convert.ToBoolean(ComandoSql.ExecuteNonQuery());

                }
                else
                {
                    throw Respuesta.Item2;
                }
                return EsExitoso;
            }
            catch (SqlException excepcion)
            {
                Console.WriteLine(excepcion.ToString());
                return false;
            }
            finally
            {
                ConexionDAL.CerrarConexion();
            }
        }

        public bool CrearVendedor(VendedorBO Vendedor)
        {
            var VendedorParametros = new List<KeyValuePair<string, object>>()
                {
                    new KeyValuePair<string, object>("@nombre", Vendedor.Nombre),
                    new KeyValuePair<string, object>("@correo", Vendedor.Correo),
                    new KeyValuePair<string, object>("@celular", Vendedor.Celular),
                    new KeyValuePair<string, object>("@direccion", Vendedor.Direccion),
                };
            return EnviarDatosVendedor("SP_CREAR_VENDEDOR", VendedorParametros);
        }

        public bool ActualizarVendedor(VendedorBO Vendedor)
        {
            var VendedorParametros = new List<KeyValuePair<string, object>>()
                {
                    new KeyValuePair<string, object>("@vendedor_id", Vendedor.VendedorId),
                    new KeyValuePair<string, object>("@nombre", Vendedor.Nombre),
                    new KeyValuePair<string, object>("@correo", Vendedor.Correo),
                    new KeyValuePair<string, object>("@celular", Vendedor.Celular),
                    new KeyValuePair<string, object>("@direccion", Vendedor.Direccion),
                };
            return EnviarDatosVendedor("SP_ACTUALIZAR_VENDEDOR", VendedorParametros);
        }

        public bool ActivarDesactivarVendedor(int VendedorId, bool EsVisible)
        {
            var VendedorParametros = new List<KeyValuePair<string, object>>()
                {
                    new KeyValuePair<string, object>("@vendedor_id", VendedorId),
                    new KeyValuePair<string, object>("@es_visible", Convert.ToInt16(EsVisible))
                };
            return EnviarDatosVendedor("SP_ACTIVAR_DESACTIVAR_VENDEDOR", VendedorParametros);
        }

        public int ObtenerSiguienteVendedorId()
        {
            int SiguienteCodigo = 0;
            try
            {
                (SqlCommand?, SqlException?) Respuesta = ConexionDAL.ConectarSinParametros("SP_OBTENER_SIGUIENTE_VENDEDOR_ID");
                if (Respuesta.Item1 != null)
                {
                    SqlCommand ComandoSql = Respuesta.Item1;
                    SqlDataReader RespuestaSql = ComandoSql.ExecuteReader();
                    while (RespuestaSql.Read())
                    {
                        SiguienteCodigo = Convert.ToInt32(RespuestaSql["siguiente_vendedor_id"].ToString());
                    }
                }
                else
                {
                    throw Respuesta.Item2;
                }
                return SiguienteCodigo;
            }
            catch (SqlException excepcion)
            {
                Console.WriteLine(excepcion.ToString());
                return SiguienteCodigo;
            }
            finally
            {
                ConexionDAL.CerrarConexion();
            }
        }
    }
}
