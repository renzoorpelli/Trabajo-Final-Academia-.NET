using APIAuth.Context;
using APIAuth.Models;
using APIAuth.Models.ViewModel;
using APIAuth.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace APIAuth.Services
{
    public class UserService : IUserService
    {
        private readonly TrabajoFinalContext context;
        private readonly IHttpContextAccessor contextAccessor;
        private readonly IConfiguration configuration;

        public UserService(TrabajoFinalContext context, IHttpContextAccessor contextAccessor, IConfiguration configuration)
        {
            this.context = context;
            this.contextAccessor = contextAccessor;
            this.configuration = configuration;
        }

        /// <summary>
        /// metodo encargado de verificar si el usuario existe en la  base de datos, recibe un UserloginViewModel
        /// el cual tendra unicamente los datos de inicio de sesion (username y password)
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<Usuario> VerificarUsuario(UserLoginViewModel user)
        {
            var usuario = await (from u in context.Usuarios
                                 join rol in context.Rols
                                 on u.IdRol equals rol.Id
                                 where u.NombreUsuario == user.Username
                                 && u.Password == user.Password &&
                                 u.EstadoId != 0
                                 select new Usuario
                                 {
                                     Id = u.Id,
                                     NombreUsuario = u.NombreUsuario,
                                     Password = u.Password,
                                     IdRol = u.IdRol,
                                     IdPersona = u.IdPersona,
                                     EstadoId = u.EstadoId,
                                     IdRolNavigation = u.IdRolNavigation,
                                     IdPersonaNavigation = u.IdPersonaNavigation
                                 }
                                 ).FirstOrDefaultAsync();
            return usuario != null ? usuario : null ;
        }


        /// <summary>
        /// metodo encargado de crear un usuario si este no existe en el sistema, si el metodo verificar usuario no existe
        /// creo un usuario y lo agrego a la bbdd
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> CrearUsuario(UserLoginViewModel user)
        {
            Usuario nuevoUsuario = await VerificarUsuario(user);
            if(nuevoUsuario is null)
            {
                nuevoUsuario= new Usuario();
                nuevoUsuario.NombreUsuario = user.Username;
                nuevoUsuario.Password = user.Password;
                //estado 2 = pendiente de aprobacion por parte del superadmin
                nuevoUsuario.EstadoId = 2;
                //rol  4 = UsuarioNuevo
                nuevoUsuario.IdRol = 4;
                await context.Usuarios.AddAsync(nuevoUsuario);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }


        /// <summary>
        /// metodo encargado de generar el JWT al usuario que inicie sesion correctamente
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string CreateToken(Usuario user)
        {
            //seteo las claims las cuales iran en el payload del jwt
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.NombreUsuario),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.IdRolNavigation.Nombre)
            };

            //le paso la firma de verificacion al jwt (appsettings.json)
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:Token").Value));

            //creo payload del jwt
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            //seteo el jwt en una cookie
            contextAccessor?.HttpContext?.Response.Cookies.Append("access_token", jwt, new CookieOptions { HttpOnly = true, Secure = true });

            return jwt;
        }
    }
}
 