using Microsoft.Data.SqlClient;
using pe.com.gctelecom.bo;

namespace pe.com.gctelecom.dal
{
    public class EventoDAL
    {
        private ConexionDAL ConexionDAL = new ConexionDAL();

        public List<EventoBO> ObtenerEventos(bool? EsVisible = null)
        {
            List<EventoBO> ListaEventos = new List<EventoBO>();
            try
            {
                object nuloOBoleano = EsVisible != null ? Convert.ToInt16(EsVisible) : null;
                var EventoParametros = new List<KeyValuePair<string, object>>()
                {
                    new KeyValuePair<string, object>("@es_visible", nuloOBoleano)
                };

                (SqlCommand?, SqlException?) Respuesta = ConexionDAL.ConectarConParametros("SP_OBTENER_EVENTOS", EventoParametros);
                if (Respuesta.Item1 != null)
                {
                    SqlCommand ComandoSql = Respuesta.Item1;
                    SqlDataReader RespuestaSql = ComandoSql.ExecuteReader();
                    while (RespuestaSql.Read())
                    {
                        EventoBO Evento = new EventoBO();
                        Evento.EventoId = Convert.ToInt32(RespuestaSql["evento_id"].ToString());
                        Evento.ClienteId = Convert.ToInt32(RespuestaSql["cliente_id"].ToString());
                        Evento.VendedorId = Convert.ToInt32(RespuestaSql["vendedor_id"].ToString());
                        Evento.Tipo = RespuestaSql["tipo"].ToString();
                        Evento.Descripcion = RespuestaSql["descripcion"].ToString();
                        Evento.FechaInicio = Convert.ToDateTime(RespuestaSql["fecha_inicio"].ToString());
                        Evento.Duracion = Convert.ToInt32(RespuestaSql["duracion"].ToString());
                        Evento.EsVisible = Convert.ToBoolean(RespuestaSql["es_visible"].ToString());
                        ListaEventos.Add(Evento);
                    }
                }
                else
                {
                    throw Respuesta.Item2;
                }
                return ListaEventos;
            }
            catch (SqlException excepcion)
            {
                Console.WriteLine(excepcion.ToString());
                return ListaEventos;
            }
            finally
            {
                ConexionDAL.CerrarConexion();
            }
        }

        private bool EnviarDatosEvento(string NombreProcedimientoAlmacenado, List<KeyValuePair<string, object>> Parametros)
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

        public bool CrearEvento(EventoBO Evento)
        {
            var EventoParametros = new List<KeyValuePair<string, object>>()
                {
                    new KeyValuePair<string, object>("@cliente_id", Evento.ClienteId),
                    new KeyValuePair<string, object>("@vendedor_id", Evento.VendedorId),
                    new KeyValuePair<string, object>("@tipo", Evento.Tipo),
                    new KeyValuePair<string, object>("@descripcion", Evento.Descripcion),
                    new KeyValuePair<string, object>("@fecha_inicio", Evento.FechaInicio),
                    new KeyValuePair<string, object>("@duracion", Evento.Duracion)
                };
            return EnviarDatosEvento("SP_CREAR_EVENTO", EventoParametros);
        }

        public bool ActualizarEvento(EventoBO Evento)
        {
            var EventoParametros = new List<KeyValuePair<string, object>>()
                {
                    new KeyValuePair<string, object>("@evento_id", Evento.EventoId),
                    new KeyValuePair<string, object>("@cliente_id", Evento.ClienteId),
                    new KeyValuePair<string, object>("@vendedor_id", Evento.VendedorId),
                    new KeyValuePair<string, object>("@tipo", Evento.Tipo),
                    new KeyValuePair<string, object>("@descripcion", Evento.Descripcion),
                    new KeyValuePair<string, object>("@fecha_inicio", Evento.FechaInicio),
                    new KeyValuePair<string, object>("@duracion", Evento.Duracion)
                };
            return EnviarDatosEvento("SP_ACTUALIZAR_EVENTO", EventoParametros);
        }

        public bool ActivarDesactivarEvento(int EventoId, bool EsVisible)
        {
            var EventoParametros = new List<KeyValuePair<string, object>>()
                {
                    new KeyValuePair<string, object>("@evento_id", EventoId),
                    new KeyValuePair<string, object>("@es_visible", Convert.ToInt16(EsVisible))
                };
            return EnviarDatosEvento("SP_ACTIVAR_DESACTIVAR_EVENTO", EventoParametros);
        }

        public int ObtenerSiguienteEventoId()
        {
            int SiguienteCodigo = 0;
            try
            {
                (SqlCommand?, SqlException?) Respuesta = ConexionDAL.ConectarSinParametros("SP_OBTENER_SIGUIENTE_EVENTO_ID");
                if (Respuesta.Item1 != null)
                {
                    SqlCommand ComandoSql = Respuesta.Item1;
                    SqlDataReader RespuestaSql = ComandoSql.ExecuteReader();
                    while (RespuestaSql.Read())
                    {
                        SiguienteCodigo = Convert.ToInt32(RespuestaSql["siguiente_evento_id"].ToString());
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
