
namespace pe.com.gctelecom.bo
{
    public class ClienteBO
    {
        public int ClienteId { get; set; }
        public string FuenteId { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Celular { get; set; }
        public string Direccion { get; set; }        
        public DateTime FechaRegistro { get; }
        public bool EsVisible { get; set; }
    }
}
