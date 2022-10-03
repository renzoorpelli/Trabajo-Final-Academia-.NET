using System;
using System.Collections.Generic;

namespace AlmacenTecnologico.Models
{
    public partial class Usuario
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public string Password { get; set; }
        public int? IdRol { get; set; }
        public int? IdPersona { get; set; }
        public int EstadoId { get; set; }

        public virtual Persona IdPersonaNavigation { get; set; }
        public virtual Rol IdRolNavigation { get; set; }
    }
}
