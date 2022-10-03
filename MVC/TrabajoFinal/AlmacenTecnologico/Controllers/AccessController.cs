using AlmacenTecnologico.Context;
using AlmacenTecnologico.Models;
using AlmacenTecnologico.Models.ViewModel;
using AlmacenTecnologico.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;

namespace AlmacenTecnologico.Controllers
{

    
    [AllowAnonymous]
    public class AccessController : Controller
    {
        private readonly IUserService _userService;
        private readonly IHttpClientFactory _httpClientFactory;
        public AccessController(IUserService userService, IHttpClientFactory httpClientFactory)
        {
            _userService = userService;
            _httpClientFactory = httpClientFactory;
        }


        public IActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// metodo action encargado de de hacer funcionar como cliente a el proyecto, manda una peticion de tipo post
        /// con los datos del usuario a la Api de autorizacion
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Index(Usuario request)
        {
            try
            {
                //cliente definido en el program.cs
                var httpClient = _httpClientFactory.CreateClient("ApiAuth");
                //peticion que va a ser enviada a la api, 1) endpoint de la api, 2)objeto anonimo con los datos que necesito para validar
                var httpResponseMessage = await httpClient.PostAsJsonAsync("api/Access/Login", new { username = request.NombreUsuario, Password = request.Password });
               //si la api devuelve un 200 quiere decir que el usuario fue encontrado
                if (httpResponseMessage.IsSuccessStatusCode && httpClient is not null && httpResponseMessage is not null)
                {
                    //leo el json web token de la respuesta de la api y 
                     string contentString = await httpResponseMessage.Content.ReadAsStringAsync();
                     //le paso el jwt el cual estara en el response y lo valido
                     bool decryptJWT = await _userService.DecryptRequest(contentString, HttpContext);
                     return decryptJWT ? RedirectToAction("index", "home") : View();

                }
                ViewBag.Message = "Usuario no encontrado";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Hubo un error al conectar al servidor. {ex.Message}";
                return View();
            }
        }

        /// <summary>
        /// permite al usuario registrarse si es que entra por primera vez
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// metodo action register, registra a un usuario nuevo al sistema y asignandole los roles de nuevo usuario y 
        /// estado de pendiente, este nuevo usuario le aparecerá al usuario super admin el cual podrá aceptarlo o no
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult>Register(Usuario request)
        {
            try
            {
                //creo el cliente httpo
                var httpClient = _httpClientFactory.CreateClient("ApiAuth");
                //mando la peticion al endpoint register de la api de auth
                var httpResponseMessage = await httpClient.PostAsJsonAsync("api/Access/Register", new { username = request.NombreUsuario, Password = request.Password });
                // si la petición devuelve un 200 lo redirijo al index
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();
                    
                    return RedirectToAction("Index");
                }
                ViewBag.Message = "Hubo un problema al registrarte";
                return View();
            }
            catch (Exception ex)
            {
                //si hubo un probla al conectar a la api mostrara el mensaje por pantalla al usuario
                ViewBag.Message = $"Hubo un error al conectar al servidor. {ex.Message}";
                return View();
            }
            
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

        [AllowAnonymous]
        public IActionResult LoginStatus()
        {
            return View();
        }
    }
}
