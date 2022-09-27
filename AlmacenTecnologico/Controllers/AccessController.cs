using AlmacenTecnologico.Context;
using AlmacenTecnologico.Models;
using AlmacenTecnologico.Models.ViewModel;
using AlmacenTecnologico.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace AlmacenTecnologico.Controllers
{

    //[Authorize()] habilita la vista al usuario que este autenticado sin importar el rol
    //[AllowAnonymous] habilita la vista a cualquier usuario sin importar que este autenticado
    public class AccessController : Controller
    {
        private readonly TrabajoFinalContext _context;
        private readonly IUserService _userService;
        public AccessController(TrabajoFinalContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

       
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Usuario request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            UsuarioViewModel usuarioEncontrado = await _userService.VerificarUsuario(request, _context);
            if (usuarioEncontrado != null)
            {
                await _userService.SetClaims(usuarioEncontrado, HttpContext);
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Message = "Usuario no encontrado";
            return View();

        }

        /// <summary>
        /// permite al usuario registrarse si es que entra por primera vez
        /// </summary>
        /// <returns></returns>
        public IActionResult Registrarme()
        {
            return View();
        }

        /// <summary>
        /// metodo encargado de cerrar la actual sesion
        /// </summary>
        /// <returns>redireccionamiento hacia la pagina de acceso</returns>
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Access");
        }

    }
}
