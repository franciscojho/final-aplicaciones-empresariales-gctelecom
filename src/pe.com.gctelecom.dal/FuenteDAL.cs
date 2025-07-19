using Microsoft.Data.SqlClient;
using pe.com.gctelecom.bo;

namespace pe.com.gctelecom.dal
{
    public class FuenteDAL
    {
        private ConexionDAL ConexionDAL = new ConexionDAL();

        public List<FuenteBO> ObtenerFuentes(bool? EsVisible = null)
        {
            List<FuenteBO> ListaFuentes = new List<FuenteBO>();
            try
            {
                object nuloOBoleano = EsVisible != null ? Convert.ToInt16(EsVisible) : null;
                var FuenteParametros = new List<KeyValuePair<string, object>>()
                {
                    new KeyValuePair<string, object>("@es_visible", nuloOBoleano)
                };

                (SqlCommand?, SqlException?) Respuesta = ConexionDAL.ConectarConParametros("SP_OBTENER_FUENTES", FuenteParametros);
                if (Respuesta.Item1 != null)
                {
                    SqlCommand ComandoSql = Respuesta.Item1;
                    SqlDataReader RespuestaSql = ComandoSql.ExecuteReader();
                    while (RespuestaSql.Read())
                    {
                        FuenteBO Fuente = new FuenteBO();
                        Fuente.FuenteId = Convert.ToInt32(RespuestaSql["fuente_id"].ToString());
                        Fuente.Nombre = RespuestaSql["nombre"].ToString();
                        Fuente.EsVisible = Convert.ToBoolean(RespuestaSql["es_visible"].ToString());
                        ListaFuentes.Add(Fuente);
                    }
                }
                else
                {
                    throw Respuesta.Item2;
                }
                return ListaFuentes;
            }
            catch (SqlException excepcion)
            {
                Console.WriteLine(excepcion.ToString());
                return ListaFuentes;
            }
            finally
            {
                ConexionDAL.CerrarConexion();
            }
        }

        private bool EnviarDatosFuente(string NombreProcedimientoAlmacenado, List<KeyValuePair<string, object>> Parametros)
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

        public bool CrearFuente(FuenteBO Fuente)
        {
            var FuenteParametros = new List<KeyValuePair<string, object>>()
                {
                    new KeyValuePair<string, object>("@nombre", Fuente.Nombre),
                };
            return EnviarDatosFuente("SP_CREAR_FUENTE", FuenteParametros);
        }

        public bool ActualizarFuente(FuenteBO Fuente)
        {
            var FuenteParametros = new List<KeyValuePair<string, object>>()
                {
                    new KeyValuePair<string, object>("@fuente_id", Fuente.FuenteId),
                    new KeyValuePair<string, object>("@nombre", Fuente.Nombre),
                };
            return EnviarDatosFuente("SP_ACTUALIZAR_FUENTE", FuenteParametros);
        }

        public bool ActivarDesactivarFuente(int FuenteId, bool EsVisible)
        {
            var FuenteParametros = new List<KeyValuePair<string, object>>()
                {
                    new KeyValuePair<string, object>("@fuente_id", FuenteId),
                    new KeyValuePair<string, object>("@es_visible", Convert.ToInt16(EsVisible))
                };
            return EnviarDatosFuente("SP_ACTIVAR_DESACTIVAR_FUENTE", FuenteParametros);
        }

        public int ObtenerSiguienteFuenteId()
        {
            int SiguienteCodigo = 0;
            try
            {
                (SqlCommand?, SqlException?) Respuesta = ConexionDAL.ConectarSinParametros("SP_OBTENER_SIGUIENTE_FUENTE_ID");
                if (Respuesta.Item1 != null)
                {
                    SqlCommand ComandoSql = Respuesta.Item1;
                    SqlDataReader RespuestaSql = ComandoSql.ExecuteReader();
                    while (RespuestaSql.Read())
                    {
                        SiguienteCodigo = Convert.ToInt32(RespuestaSql["siguiente_fuente_id"].ToString());
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
