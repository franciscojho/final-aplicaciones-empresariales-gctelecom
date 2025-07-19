using Microsoft.Data.SqlClient;
using pe.com.gctelecom.bo;

namespace pe.com.gctelecom.dal
{
    public class ReclamoDAL
    {
        private ConexionDAL ConexionDAL = new ConexionDAL();

        public List<ReclamoBO> ObtenerReclamos(bool? EsVisible = null)
        {
            List<ReclamoBO> ListaReclamos = new List<ReclamoBO>();
            try
            {
                object nuloOBoleano = EsVisible != null ? Convert.ToInt16(EsVisible) : null;
                var ReclamoParametros = new List<KeyValuePair<string, object>>()
                {
                    new KeyValuePair<string, object>("@es_visible", nuloOBoleano)
                };

                (SqlCommand?, SqlException?) Respuesta = ConexionDAL.ConectarConParametros("SP_OBTENER_RECLAMOS", ReclamoParametros);
                if (Respuesta.Item1 != null)
                {
                    SqlCommand ComandoSql = Respuesta.Item1;
                    SqlDataReader RespuestaSql = ComandoSql.ExecuteReader();
                    while (RespuestaSql.Read())
                    {
                        ReclamoBO Reclamo = new ReclamoBO();
                        Reclamo.ReclamoId = Convert.ToInt32(RespuestaSql["reclamo_id"].ToString());
                        Reclamo.ProductoId = Convert.ToInt32(RespuestaSql["producto_id"].ToString());
                        Reclamo.Descripcion = RespuestaSql["descripcion"].ToString();
                        Reclamo.Fecha = Convert.ToDateTime(RespuestaSql["fecha"].ToString());
                        Reclamo.Estado = RespuestaSql["estado"].ToString();
                        Reclamo.EsVisible = Convert.ToBoolean(RespuestaSql["es_visible"].ToString());
                        ListaReclamos.Add(Reclamo);
                    }
                }
                else
                {
                    throw Respuesta.Item2;
                }
                return ListaReclamos;
            }
            catch (SqlException excepcion)
            {
                Console.WriteLine(excepcion.ToString());
                return ListaReclamos;
            }
            finally
            {
                ConexionDAL.CerrarConexion();
            }
        }

        private bool EnviarDatosReclamo(string NombreProcedimientoAlmacenado, List<KeyValuePair<string, object>> Parametros)
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

        public bool CrearReclamo(ReclamoBO Reclamo)
        {
            var ReclamoParametros = new List<KeyValuePair<string, object>>()
                {
                    new KeyValuePair<string, object>("@producto_id", Reclamo.ProductoId),
                    new KeyValuePair<string, object>("@descripcion", Reclamo.Descripcion),
                    new KeyValuePair<string, object>("@fecha", Reclamo.Fecha),
                    new KeyValuePair<string, object>("@estado", Reclamo.Estado)
                };
            return EnviarDatosReclamo("SP_CREAR_RECLAMO", ReclamoParametros);
        }

        public bool ActualizarReclamo(ReclamoBO Reclamo)
        {
            var ReclamoParametros = new List<KeyValuePair<string, object>>()
                {
                    new KeyValuePair<string, object>("@reclamo_id", Reclamo.ReclamoId),
                    new KeyValuePair<string, object>("@producto_id", Reclamo.ProductoId),
                    new KeyValuePair<string, object>("@nombre", Reclamo.Descripcion),
                    new KeyValuePair<string, object>("@correo", Reclamo.Fecha),
                    new KeyValuePair<string, object>("@celular", Reclamo.Estado)
                };
            return EnviarDatosReclamo("SP_ACTUALIZAR_RECLAMO", ReclamoParametros);
        }

        public bool ActivarDesactivarReclamo(int ReclamoId, bool EsVisible)
        {
            var ReclamoParametros = new List<KeyValuePair<string, object>>()
                {
                    new KeyValuePair<string, object>("@reclamo_id", ReclamoId),
                    new KeyValuePair<string, object>("@es_visible", Convert.ToInt16(EsVisible))
                };
            return EnviarDatosReclamo("SP_ACTIVAR_DESACTIVAR_RECLAMO", ReclamoParametros);
        }

        public int ObtenerSiguienteReclamoId()
        {
            int SiguienteCodigo = 0;
            try
            {
                (SqlCommand?, SqlException?) Respuesta = ConexionDAL.ConectarSinParametros("SP_OBTENER_SIGUIENTE_RECLAMO_ID");
                if (Respuesta.Item1 != null)
                {
                    SqlCommand ComandoSql = Respuesta.Item1;
                    SqlDataReader RespuestaSql = ComandoSql.ExecuteReader();
                    while (RespuestaSql.Read())
                    {
                        SiguienteCodigo = Convert.ToInt32(RespuestaSql["siguiente_reclamo_id"].ToString());
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
