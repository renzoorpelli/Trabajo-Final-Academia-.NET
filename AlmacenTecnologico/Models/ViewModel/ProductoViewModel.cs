namespace AlmacenTecnologico.Models.ViewModel
{
    public class ProductoViewModel
    {
        public int Id { get; set; }

        public string Modelo { get; set; }

        public double Precio { get; set; }

        public string UrlImagen { get; set; }

        public int EstadoId { get; set; }

        public string Estado
        {
            get
            {
                return EstadoId != 0 ? "Activo" : "Fuera de Stock";
            }
        }

        public int? Cantidad { get; set; }
        
        public int? UnidadesDisponibles { get; set; }

        public int IdFabricante { get; set; } 

        public int IdTipoProducto { get; set; }

        public Fabricante Fabricante { get; set; }

        public TipoProducto TipoProducto { get; set; }
    
    }
}
