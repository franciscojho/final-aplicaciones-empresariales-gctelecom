using pe.com.gctelecom.bo;
using pe.com.gctelecom.dal;

namespace pe.com.gctelecom.ui
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ClienteDAL clienteDAL = new ClienteDAL();
            ClienteBO body = new ClienteBO()
            {
                FuenteId = 1,
                Nombre = "Pepe",
                Correo = "test@hotmail.com",
                Celular = "909282333",
                Direccion = "Av. Los Cazadores 122"
            };

            List<ClienteBO>? clientes = clienteDAL.ObtenerClientes(true);
            // bool respuesta = clienteDAL.CrearCliente(body);
            int cod = clienteDAL.ObtenerSiguienteClienteId();

            Console.WriteLine(cod.ToString());
            Console.WriteLine(clientes.ToString());
            // Console.WriteLine(respuesta.ToString());
        }
    }
}
