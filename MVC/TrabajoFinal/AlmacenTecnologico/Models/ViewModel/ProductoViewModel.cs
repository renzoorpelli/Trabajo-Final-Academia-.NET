using System.ComponentModel.DataAnnotations;

namespace AlmacenTecnologico.Models.ViewModel
{
    public class ProductoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El Modelo no es válido")]
        public string Modelo { get; set; }

        [Required(ErrorMessage = "El precio no es válido")]
        [Range(1, double.MaxValue, ErrorMessage ="Ingrese un valor válido")]
        public double Precio { get; set; }

        [Required(ErrorMessage = "La URL de la imágen no es válido")]
        [DataType(DataType.ImageUrl)]
        public string UrlImagen { get; set; }

        public int EstadoId { get; set; }

        public string Estado
        {
            get
            {
                return EstadoId != 0 ? "Activo" : "Fuera de Stock";
            }
        }

        [Required(ErrorMessage = "La cantidad indicada no es válida")]
        [Range(1, 500, ErrorMessage ="Ingrese una cantidad válida")]
        public int? Cantidad { get; set; }
        
        public int? UnidadesDisponibles { get; set; }

        public int IdFabricante { get; set; } 

        public int IdTipoProducto { get; set; }

        public Fabricante Fabricante { get; set; }

        public TipoProducto TipoProducto { get; set; }
    
    }
}
