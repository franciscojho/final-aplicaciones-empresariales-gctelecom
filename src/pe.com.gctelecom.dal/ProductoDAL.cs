using Microsoft.Data.SqlClient;
using pe.com.gctelecom.bo;

namespace pe.com.gctelecom.dal
{
    public class ProductoDAL
    {
        private ConexionDAL ConexionDAL = new ConexionDAL();

        public List<ProductoBO> ObtenerProductos(bool? EsVisible = null)
        {
            List<ProductoBO> ListaProductos = new List<ProductoBO>();
            try
            {
                object nuloOBoleano = EsVisible != null ? Convert.ToInt16(EsVisible) : null;
                var ProductoParametros = new List<KeyValuePair<string, object>>()
                {
                    new KeyValuePair<string, object>("@es_visible", nuloOBoleano)
                };

                (SqlCommand?, SqlException?) Respuesta = ConexionDAL.ConectarConParametros("SP_OBTENER_PRODUCTOS", ProductoParametros);
                if (Respuesta.Item1 != null)
                {
                    SqlCommand ComandoSql = Respuesta.Item1;
                    SqlDataReader RespuestaSql = ComandoSql.ExecuteReader();
                    while (RespuestaSql.Read())
                    {
                        ProductoBO Producto = new ProductoBO();
                        Producto.ProductoId = Convert.ToInt32(RespuestaSql["producto_id"].ToString());
                        Producto.Nombre = RespuestaSql["nombre"].ToString();
                        Producto.Descripcion = RespuestaSql["descripcion"].ToString();
                        Producto.Precio = Convert.ToDecimal(RespuestaSql["precio"].ToString());
                        Producto.Moneda = RespuestaSql["moneda"].ToString();
                        Producto.EsVisible = Convert.ToBoolean(RespuestaSql["es_visible"].ToString());
                        ListaProductos.Add(Producto);
                    }
                }
                else
                {
                    throw Respuesta.Item2;
                }
                return ListaProductos;
            }
            catch (SqlException excepcion)
            {
                Console.WriteLine(excepcion.ToString());
                return ListaProductos;
            }
            finally
            {
                ConexionDAL.CerrarConexion();
            }
        }

        private bool EnviarDatosProducto(string NombreProcedimientoAlmacenado, List<KeyValuePair<string, object>> Parametros)
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

        public bool CrearProducto(ProductoBO Producto)
        {
            var ProductoParametros = new List<KeyValuePair<string, object>>()
                {
                    new KeyValuePair<string, object>("@nombre", Producto.Nombre),
                    new KeyValuePair<string, object>("@descripcion", Producto.Descripcion),
                    new KeyValuePair<string, object>("@precio", Producto.Precio)
                };
            return EnviarDatosProducto("SP_CREAR_PRODUCTO", ProductoParametros);
        }

        public bool ActualizarProducto(ProductoBO Producto)
        {
            var ProductoParametros = new List<KeyValuePair<string, object>>()
                {
                    new KeyValuePair<string, object>("@producto_id", Producto.ProductoId),
                    new KeyValuePair<string, object>("@nombre", Producto.Nombre),
                    new KeyValuePair<string, object>("@descripcion", Producto.Descripcion),
                    new KeyValuePair<string, object>("@precio", Producto.Precio)
                };
            return EnviarDatosProducto("SP_ACTUALIZAR_PRODUCTO", ProductoParametros);
        }

        public bool ActivarDesactivarProducto(int ProductoId, bool EsVisible)
        {
            var ProductoParametros = new List<KeyValuePair<string, object>>()
                {
                    new KeyValuePair<string, object>("@producto_id", ProductoId),
                    new KeyValuePair<string, object>("@es_visible", Convert.ToInt16(EsVisible))
                };
            return EnviarDatosProducto("SP_ACTIVAR_DESACTIVAR_PRODUCTO", ProductoParametros);
        }

        public int ObtenerSiguienteProductoId()
        {
            int SiguienteCodigo = 0;
            try
            {
                (SqlCommand?, SqlException?) Respuesta = ConexionDAL.ConectarSinParametros("SP_OBTENER_SIGUIENTE_PRODUCTO_ID");
                if (Respuesta.Item1 != null)
                {
                    SqlCommand ComandoSql = Respuesta.Item1;
                    SqlDataReader RespuestaSql = ComandoSql.ExecuteReader();
                    while (RespuestaSql.Read())
                    {
                        SiguienteCodigo = Convert.ToInt32(RespuestaSql["siguiente_producto_id"].ToString());
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
