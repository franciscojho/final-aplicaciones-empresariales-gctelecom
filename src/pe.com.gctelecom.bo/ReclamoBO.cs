﻿
namespace pe.com.gctelecom.bo
{
    public class ReclamoBO
    {
        public int ReclamoId { get; set; }
        public int VentaId { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; }
        public bool EsVisible { get; set; }
    }
}
