
namespace pe.com.gctelecom.bo
{
    public class VentaBO
    {
        public int VentaId { get; set; }
        public int ClienteId { get; set; }
        public int VendedorId { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public bool EsVisible { get; set; }
    }
}
