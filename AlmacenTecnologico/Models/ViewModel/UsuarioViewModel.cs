namespace AlmacenTecnologico.Models.ViewModel
{
    public class UsuarioViewModel
    {
        public int Id { get; set; }

        public string NombreUsuario { get; set; }
        public string Password { get; set; }
        public Rol Rol { get; set; }

        public Persona Persona { get; set; }

        public int? EstadoId { get; set; }

        public string Estado { 
            
            get
            {
                return EstadoId != 0 ? "Activo": "Inactivo";
            } 
        }
    }
}
