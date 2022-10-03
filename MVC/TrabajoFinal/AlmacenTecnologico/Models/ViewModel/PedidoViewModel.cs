using System.ComponentModel.DataAnnotations;

namespace AlmacenTecnologico.Models.ViewModel
{
    public class PedidoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Debes seleccionar a un empleado")]
        public int EmpleadoId { get; set; }

        [Required(ErrorMessage = "Debes seleccionar a un cliente")]
        public int ClienteId { get; set; }

        public DateTime FechaPedido { get; set; }

        public int EstadoId { get; set; }
        
        public string Estado
        {
            get
            {
                return EstadoId != 0 ? $"Efectuado" : "Eliminado";
            }
        }
        public  Cliente Cliente { get; set; }
        public  Persona Empleado { get; set; } 
        public  ICollection<DetallePedido> DetallePedido { get; set; }
    }
}
