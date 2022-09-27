namespace AlmacenTecnologico.Models.ViewModel
{
    public class ClienteViewModel
    {
        public int Id { get; set; }

        public string RazonSocial { get; set; }
        public string Cuit { get; set; }

        public string Domicilio { get; set; }
        public string Mail { get; set; }

        public int EstadoId { get; set; }
        public string Estado { get { return EstadoId != 0 ? "Disponible" : "Eliminado"; } }


    }
}
