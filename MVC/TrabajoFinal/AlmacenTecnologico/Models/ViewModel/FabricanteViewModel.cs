using System.ComponentModel.DataAnnotations;

namespace AlmacenTecnologico.Models.ViewModel
{
    public class FabricanteViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre no es valido")]
        public string Nombre { get; set; }

        public int? EstadoId { get; set; }
        public string Estado {
            get
            {
                return EstadoId != 0 ? "Disponible" : "Eliminado";
            }
        }
    }
}
