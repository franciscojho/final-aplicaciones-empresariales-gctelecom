using Microsoft.Data.SqlClient;
using pe.com.gctelecom.bo;

namespace pe.com.gctelecom.dal
{
    public class ClienteDAL
    {
        private ConexionDAL ConexionDAL = new ConexionDAL();

        public List<ClienteBO> ObtenerClientes(bool? EsVisible = null) {
            List<ClienteBO> ListaClientes = new List<ClienteBO>();
            try
            {
                object nuloOBoleano = EsVisible != null ? Convert.ToInt16(EsVisible) : null;
                var ClienteParametros = new List<KeyValuePair<string, object>>()
                {
                    new KeyValuePair<string, object>("@es_visible", nuloOBoleano)
                };

                (SqlCommand?, SqlException?) Respuesta = ConexionDAL.ConectarConParametros("SP_OBTENER_CLIENTES", ClienteParametros);
                if (Respuesta.Item1 != null)
                {
                    SqlCommand ComandoSql = Respuesta.Item1;
                    SqlDataReader RespuestaSql = ComandoSql.ExecuteReader();
                    while (RespuestaSql.Read())
                    {
                        ClienteBO Cliente = new ClienteBO();
                        Cliente.ClienteId = Convert.ToInt32(RespuestaSql["cliente_id"].ToString());
                        Cliente.FuenteId = Convert.ToInt32(RespuestaSql["fuente_id"].ToString());
                        Cliente.Nombre = RespuestaSql["nombre"].ToString();
                        Cliente.Correo = RespuestaSql["correo"].ToString();
                        Cliente.Celular = RespuestaSql["celular"].ToString();
                        Cliente.Direccion = RespuestaSql["direccion"].ToString();
                        Cliente.FechaRegistro = Convert.ToDateTime(RespuestaSql["fecha_registro"].ToString());
                        Cliente.EsVisible = Convert.ToBoolean(RespuestaSql["es_visible"].ToString());
                        ListaClientes.Add(Cliente);
                    }
                }
                else
                {
                    throw Respuesta.Item2;
                }
                return ListaClientes;
            }
            catch(SqlException excepcion)
            {
                Console.WriteLine(excepcion.ToString());
                return ListaClientes;
            }
            finally
            {
                ConexionDAL.CerrarConexion();
            }
        }

        private bool EnviarDatosCliente(string NombreProcedimientoAlmacenado, List<KeyValuePair<string, object>> Parametros)
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

        public bool CrearCliente(ClienteBO Cliente)
        {
            var ClienteParametros = new List<KeyValuePair<string, object>>()
                {
                    new KeyValuePair<string, object>("@fuente_id", Cliente.FuenteId.ToString()),
                    new KeyValuePair<string, object>("@nombre", Cliente.Nombre),
                    new KeyValuePair<string, object>("@correo", Cliente.Correo),
                    new KeyValuePair<string, object>("@celular", Cliente.Celular),
                    new KeyValuePair<string, object>("@direccion", Cliente.Direccion),
                };
            return EnviarDatosCliente("SP_CREAR_CLIENTE", ClienteParametros);
        }

        public bool ActualizarCliente(ClienteBO Cliente)
        {
            var ClienteParametros = new List<KeyValuePair<string, object>>()
                {
                    new KeyValuePair<string, object>("@cliente_id", Cliente.ClienteId),
                    new KeyValuePair<string, object>("@fuente_id", Cliente.FuenteId),
                    new KeyValuePair<string, object>("@nombre", Cliente.Nombre),
                    new KeyValuePair<string, object>("@correo", Cliente.Correo),
                    new KeyValuePair<string, object>("@celular", Cliente.Celular),
                    new KeyValuePair<string, object>("@direccion", Cliente.Direccion),
                };
            return EnviarDatosCliente("SP_ACTUALIZAR_CLIENTE", ClienteParametros);
        }

        public bool ActivarDesactivarCliente(int ClienteId, bool EsVisible)
        {
            var ClienteParametros = new List<KeyValuePair<string, object>>()
                {
                    new KeyValuePair<string, object>("@cliente_id", ClienteId),
                    new KeyValuePair<string, object>("@es_visible", Convert.ToInt16(EsVisible))
                };
            return EnviarDatosCliente("SP_ACTUALIZAR_CLIENTE", ClienteParametros);
        }

        public int ObtenerSiguienteClienteId()
        {
            int SiguienteCodigo = 0;
            try
            {
                (SqlCommand?, SqlException?) Respuesta = ConexionDAL.ConectarSinParametros("SP_OBTENER_SIGUIENTE_CLIENTE_ID");
                if (Respuesta.Item1 != null)
                {
                    SqlCommand ComandoSql = Respuesta.Item1;
                    SqlDataReader RespuestaSql = ComandoSql.ExecuteReader();
                    while (RespuestaSql.Read())
                    {
                        SiguienteCodigo = Convert.ToInt32(RespuestaSql["siguiente_cliente_id"].ToString());
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
