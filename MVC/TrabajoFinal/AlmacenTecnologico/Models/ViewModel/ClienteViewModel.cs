using System.ComponentModel.DataAnnotations;

namespace AlmacenTecnologico.Models.ViewModel
{
    public class ClienteViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="El nombre del cliente es invalido")]
        public string RazonSocial { get; set; }

        [Required(ErrorMessage = "El CUIT del cliente es invalido")]
        [StringLength(11)]
        public string Cuit { get; set; }

        [Required(ErrorMessage = "El Domicilio del cliente es invalido")]
        public string Domicilio { get; set; }

        [Required(ErrorMessage = "El Mail del cliente es invalido")]
        [DataType(DataType.EmailAddress)]
        public string Mail { get; set; }

        public int EstadoId { get; set; }
        public string Estado { get { return EstadoId != 0 ? "Disponible" : "Eliminado"; } }


    }
}
