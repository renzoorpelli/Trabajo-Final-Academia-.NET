using AlmacenTecnologico.Context;
using AlmacenTecnologico.Models;
using AlmacenTecnologico.Models.ViewModel;
using AlmacenTecnologico.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlmacenTecnologico.Services
{
    public class UserService : IUserService
    {
        private readonly TrabajoFinalContext context;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IHttpContextAccessor contextAccessor;

        public UserService(TrabajoFinalContext context, IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor)
        {
            this.context = context;
            this.httpClientFactory = httpClientFactory;
            this.contextAccessor = contextAccessor;
        }


        /// <summary>
        /// Metodo encargado de hacer una lista de usuarios depende la condicion que se le pase por parametor
        /// puede ser ACTIVO =1, PENDIENTE= 2, ELIMINADO = 3, 
        /// solo los usuarios que no sean superadministradores
        /// </summary>
        /// <param name="estado">Enum que guarda constantes con valor numerico</param>
        /// <returns></returns>
        public async Task<List<UsuarioViewModel>> ListarUsuarios(UsuarioViewModel.Estado estado)
        {
            var usuario = await (from u in context.Usuarios
                                 where u.EstadoId == (int)estado
                                 && u.IdRol != (int)UsuarioViewModel.RolId.superadmin
                                 && u.IdRol != (int)UsuarioViewModel.RolId.usuarioNuevo
                                 select new UsuarioViewModel
                                 {
                                     Id = u.Id,
                                     NombreUsuario = u.NombreUsuario,
                                     Password = u.Password,
                                     Rol = u.IdRolNavigation,
                                     IdPersona = u.IdPersona,
                                     Persona = u.IdPersonaNavigation,
                                     EstadoId = (UsuarioViewModel.Estado)u.EstadoId

                                 }).ToListAsync();

            return usuario;
        }

        public async Task<List<UsuarioViewModel>> ListarUsuariosPendientes(UsuarioViewModel.Estado estado)
        {
            var usuario = await (from u in context.Usuarios
                                 where u.EstadoId == (int)estado
                                 && u.IdRol == (int)UsuarioViewModel.RolId.usuarioNuevo
                                 select new UsuarioViewModel
                                 {
                                     Id = u.Id,
                                     NombreUsuario = u.NombreUsuario,
                                     Password = u.Password,
                                     Rol = u.IdRolNavigation,
                                     IdPersona = u.IdPersona,
                                     Persona = u.IdPersonaNavigation,
                                     EstadoId = (UsuarioViewModel.Estado)u.EstadoId

                                 }).ToListAsync();

            return usuario;
        }

        /// <summary>
        /// obtiene un usuario de la base de datos pasandole como parametro el id del usuario que se quiere obtener
        /// </summary>
        /// <param name="id">el id del usuario</param>
        /// <returns></returns>
        public async Task<UsuarioViewModel> GetUsuario(int id)
        {
            var usuario = await (from u in context.Usuarios
                                 where u.Id == id &&
                                 u.EstadoId != 0
                                 select new UsuarioViewModel
                                 {
                                     Id = u.Id,
                                     NombreUsuario = u.NombreUsuario,
                                     Password = u.Password,
                                     Rol = u.IdRolNavigation,
                                     IdPersona = u.IdPersona,
                                     Persona = u.IdPersonaNavigation,
                                     EstadoId = (UsuarioViewModel.Estado)u.EstadoId

                                 }).FirstOrDefaultAsync();

            return usuario;
        }


        /// <summary>
        /// Agrega los roles de usuario a los claim y permite iniciar sesion satifactoriamente al usuario
        /// </summary>
        /// <param name="user">el usuario que inicio sesion</param>
        /// <param name="ControllerBase">HttpContext que permite asignar la cookien en la session del usuario</param>
        /// <returns></returns>
        public async Task SetClaims(UsuarioViewModel user, HttpContext ControllerBase)
        {
            if (user is not null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.NombreUsuario),
                    new Claim(ClaimTypes.Role, user.Rol.Nombre),
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await ControllerBase.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            }
        }

        /// <summary>
        /// metodo encargado de desencriptar la respuesta (jwt) de la api de autenticacion y obtener el usuario del payload
        /// para pasarle lal metodo que setee las cookies de sesion
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="controllerBase"></param>
        /// <returns></returns>
        public async Task<bool> DecryptRequest(string stream, HttpContext controllerBase)
        {
            if (stream is not null)
            {
                //deserelizar respuesta servidor a un tipo que sea valido y se guarde el token
                var handlerToken = new JwtSecurityTokenHandler();
                var token = handlerToken.ReadJwtToken(stream);
                if (token is not null)
                {
                    string usuarioId = token.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
                    //valido que el usuario que viene en el payload del jwt sea un numero valido
                    if (int.TryParse(usuarioId, out int id)){
                        UsuarioViewModel user = await GetUsuario(id);
                        await SetClaims(user, controllerBase);
                        //agrego el jwt a una cookie
                        contextAccessor.HttpContext.Response.Cookies.Append("access_token_generated", stream, new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true
                        });
                        return true;
                    }
                }

            }
            return false;
        }


        /// <summary>
        /// metodo encargado de devolver la lista de Empleados disponibles en una lista tipo SelectListItem utilizada
        /// para los dropdownlist dinamicamente para que cada elemento tome un tipo clave=>valor
        /// solo se mostraran los empleados activos del sistema
        /// </summary>
        /// <returns></returns>
        public async Task<List<SelectListItem>> ListarEmpleados()
        {
            var lista = await (from empleado in context.Personas
                               select new SelectListItem
                               {
                                   Text = empleado.Nombre + " " + empleado.Dni,
                                   Value = empleado.Id.ToString()
                               }).ToListAsync();
            return lista;
        }

        /// <summary>
        /// metodo encargado de devolver la lista de roles disponibles en una lista tipo SelectListItem utilizada
        /// para los dropdownlist dinamicamente para que cada elemento tome un tipo clave=>valor
        /// solo se mostraran los empleados activos del sistema
        /// </summary>
        /// <returns></returns>
        public async Task<List<SelectListItem>> ListarRoles()
        {
            var lista = await (from roles in context.Rols
                               select new SelectListItem
                               {
                                   Text = roles.Nombre,
                                   Value = roles.Id.ToString()
                               }).ToListAsync();
            return lista;
        }

        //crud methods
        /// <summary>
        /// metodo encargado de agregar un cliente a la lista
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool AgregarUsuario(Usuario request)
        {
            if (request is not null)
            {
                if (!context.Usuarios.Any(usuario => usuario.NombreUsuario == request.NombreUsuario)
                    && !context.Usuarios.Any(usuario=> usuario.IdPersona == request.IdPersona))
                {
                    request.EstadoId = 1;
                    this.context.Usuarios.Add(request);
                    this.context.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// metodo encargado de editar un usuario de la base de datos, recibe los datos que se le quieren aplicar al usuario (request)
        /// verifica que el usuario no existan en la base de datos
        /// </summary>
        /// <param name="request">los datos que recibe de la peticion de tipo POST del formulario editar</param>
        /// <returns>retorna true si pudo modificar al usuario</returns>
        public async Task<bool> EditarUsuario(UsuarioViewModel  request)
        {
            if (request is not null)
            {
                Usuario usuarioEditar = await context.Usuarios.FindAsync(request.Id);
                if(usuarioEditar.NombreUsuario == request.NombreUsuario ||
                    usuarioEditar.IdPersona == request.IdPersona)
                {
                    usuarioEditar.NombreUsuario = request.NombreUsuario;
                    usuarioEditar.Password = request.Password;
                    usuarioEditar.IdPersona = request.IdPersona;
                    usuarioEditar.IdRol = (int)request.idRol;
                    await context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    if (!context.Usuarios.Any(usuario => usuario.NombreUsuario == request.NombreUsuario) &&
                    !context.Usuarios.Any(usuario => usuario.IdPersona == request.IdPersona))
                    {
                        usuarioEditar.NombreUsuario = request.NombreUsuario;
                        usuarioEditar.Password = request.Password;
                        usuarioEditar.IdPersona = request.IdPersona;
                        usuarioEditar.IdRol = (int)request.idRol;
                        await context.SaveChangesAsync();
                        return true;
                    }
                }
                
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<bool> EliminarUsuarioLogico(UsuarioViewModel request)
        {
            if (request is not null)
            {
                Usuario usuarioEliminado = await context.Usuarios.FindAsync(request.Id);
                usuarioEliminado.EstadoId = 0;
                usuarioEliminado.IdRol = (int)UsuarioViewModel.RolId.eliminado;
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }


        public async Task<bool> RecuperarUsuario(UsuarioViewModel request)
        {
            if (request is not null)
            {
                Usuario usuarioEliminado = await context.Usuarios.FindAsync(request.Id);
                usuarioEliminado.EstadoId = 1;//recuperado
                usuarioEliminado.IdRol = (int)UsuarioViewModel.RolId.empleado;
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> EliminarDefinitivo(int id)
        {
            Usuario usuarioEliminado = await context.Usuarios.FindAsync(id);
            if(usuarioEliminado is not null && usuarioEliminado.IdRol == (int)UsuarioViewModel.RolId.usuarioNuevo)
            {
                context.Usuarios.Remove(usuarioEliminado);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> IntegrarUsuarioSistema(int id)
        {
            Usuario usuarioNuevo = await context.Usuarios.FindAsync(id);
            if (usuarioNuevo is not null && usuarioNuevo.IdRol == (int)UsuarioViewModel.RolId.usuarioNuevo)
            {
                usuarioNuevo.IdRol = (int)UsuarioViewModel.RolId.empleado;
                usuarioNuevo.EstadoId = (int)UsuarioViewModel.Estado.Activo;
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

    }
}
