using System.ComponentModel.DataAnnotations;

namespace AlmacenTecnologico.Models.ViewModel
{
    public class EmpleadoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre no es valido")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido no es valido")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El DNI no es valido")]
        [StringLength(8)]
        public string DNI { get; set; }

        public int Estadoid { get; set; }

        public string Estado
        {
            get
            {
                return Estadoid != 0 ? "Activo" : "Inactivo";
            }
        }
    }
}
