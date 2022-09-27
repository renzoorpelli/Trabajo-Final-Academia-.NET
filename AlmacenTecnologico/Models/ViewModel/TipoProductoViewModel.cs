namespace AlmacenTecnologico.Models.ViewModel
{
    public class TipoProductoViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string NombreCategoria { get; set; }
        public Categorium Categoria { get; set; }

    }
}
