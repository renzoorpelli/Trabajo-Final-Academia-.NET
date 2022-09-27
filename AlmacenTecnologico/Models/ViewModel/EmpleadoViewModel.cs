namespace AlmacenTecnologico.Models.ViewModel
{
    public class EmpleadoViewModel
    {
        public int Id { get; set; }

        public string Nombre { get; set; }
        public string Apellido { get; set; }

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
