using System.ComponentModel.DataAnnotations;

namespace AlmacenTecnologico.Models.ViewModel
{
    public class TipoProductoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre no es valido")]
        public string Nombre { get; set; }
        public string NombreCategoria { get; set; }

        [Required(ErrorMessage = "Debes seleccionar una categoria")]
        public int IdCategoria { get; set; }
        public Categorium Categoria { get; set; }
        public int EstadoId { get; set; }
        public string Estado
        {
            get
            {
                return EstadoId != 0 ? "Disponible" : "Inactiva";
            }
        }

    }
}
