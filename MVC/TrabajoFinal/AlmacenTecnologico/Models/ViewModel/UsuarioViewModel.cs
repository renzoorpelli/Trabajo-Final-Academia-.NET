using System.ComponentModel.DataAnnotations;

namespace AlmacenTecnologico.Models.ViewModel
{
    public class UsuarioViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El Username no es válido")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "El nombre no es valido")]
        [DataType(DataType.Password, ErrorMessage ="Ingrese un password válido")]
        public string Password { get; set; }

        public RolId idRol { get; set; }
        public Rol Rol { get; set; }

        public int? IdPersona { get; set; }
        public Persona Persona { get; set; }


        public Estado? EstadoId { get; set; }

        public string EstadoString { 
            
            get
            {
                return EstadoId.ToString();
            } 
        }

        public enum Estado { Activo=1, Pendiente, Eliminado=0}
        public enum RolId { admin=1, empleado, superadmin, usuarioNuevo, eliminado}
    }
}
