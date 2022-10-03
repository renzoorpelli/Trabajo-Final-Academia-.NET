using System.ComponentModel.DataAnnotations;

namespace AlmacenTecnologico.Models.ViewModel
{
    public class DetallePedidoViewModel
    {
        public int IdDetalle { get; set; }

        [Required]
        public int PedidoId { get; set; }

        [Required(ErrorMessage ="Debes seleccionar un producto")]
        public int ProductoId { get; set; }

        [Required(ErrorMessage = "Debes seleccionar una cantidad valida")]
        [Range(1, 500, ErrorMessage = "Máximo de unidades disponibles alcanzado")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "Debes indicar un precio")]
        public int PrecioTotal { get; set; }
        public  Pedido Pedido { get; set; }
        public  Producto ProductoObj { get; set; }

    }
}
