using APIAuth.Models;
using APIAuth.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace APIAuth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccessController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IUserService userService;

        public AccessController(IConfiguration configuration, IUserService userService)
        {
            this.configuration = configuration;
            this.userService = userService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// metodo action login encargado de verificar si el usuario que le es pasado por parametro
        /// existe, generar el JWT para que posteriormente sea utilizado por el cliente (proyecto MVC)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody]UserLoginViewModel request)
        {
            Usuario usuarioVerificado = await userService.VerificarUsuario(request);
            if (usuarioVerificado is not null)
            {
                //si el usuario existe, crea el token
                string asingnarToken = userService.CreateToken(usuarioVerificado);
                return Ok(asingnarToken);
            }
            return Unauthorized("Credenciales no validas");
        }

        /// <summary>
        /// metodo encargado de crear un usuario en la base de datos, verificando que el
        /// usuario nuevo no exista
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<ActionResult<string>> Register(UserLoginViewModel request)
        {
            bool respuesta = await userService.CrearUsuario(request);
            if (respuesta)
            {
                return Ok();
            }
            return BadRequest("El usuario ya existe");
        } 
    }
}
