using AlmacenTecnologico.Context;
using AlmacenTecnologico.Models;
using AlmacenTecnologico.Models.ViewModel;
using AlmacenTecnologico.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AlmacenTecnologico.Services
{
    public class UserService : IUserService
    {
        public async Task<UsuarioViewModel> VerificarUsuario(Usuario user, TrabajoFinalContext context)
        {
            var usuario = await (from u in context.Usuarios
                                 join persona in context.Personas
                                 on u.IdPersona equals persona.Id
                                 where u.NombreUsuario == user.NombreUsuario
                                 && u.Password == user.Password
                                 select new UsuarioViewModel
                                 {
                                     Id = u.Id,
                                     NombreUsuario = u.NombreUsuario,
                                     Password = u.Password,
                                     Rol = u.IdRolNavigation,
                                     Persona = u.IdPersonaNavigation,
                                     EstadoId = u.EstadoId

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
    }
}
