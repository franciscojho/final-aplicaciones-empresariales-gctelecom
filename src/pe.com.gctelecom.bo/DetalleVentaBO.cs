
namespace pe.com.gctelecom.bo
{
    public class DetalleVentaBO
    {
        public int DetalleVentaId { get; set; }
        public int VentaId { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public bool EsVisible { get; set; }
    }
}
