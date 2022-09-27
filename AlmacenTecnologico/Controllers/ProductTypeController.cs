using AlmacenTecnologico.Context;
using AlmacenTecnologico.Models;
using AlmacenTecnologico.Models.ViewModel;
using AlmacenTecnologico.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlmacenTecnologico.Controllers
{
    [Authorize]
    public class ProductTypeController : Controller
    {
        private readonly TrabajoFinalContext _context;
        private readonly ITipoProductoService _tProductoService;

        public ProductTypeController(TrabajoFinalContext context, ITipoProductoService tProductoService)
        {
            this._context = context;
            this._tProductoService = tProductoService;
        }

        /// <summary>
        /// metodo action encargado de mostrar el dashboard principal con todos los tipos de productos con su categoria correspondiente
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            List<TipoProductoViewModel> lista = await _tProductoService.ListarTipoProductos(_context);
            return View(lista);
        }

        /// <summary>
        /// metodo action encargado de mostrar la vista de formulario de carga de tipos de producto,
        /// la lista de categorias sera utilizada para cargar el dropdownlist que se encuentra en el formulario
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Add()
        {
            ViewData["ListaCategorias"] = await _tProductoService.ListarCategorias(_context);
            return View();
        }

        /// <summary>
        /// metodo action el cual es recibido por el formulario de la vista en metodo POST el cual trae en la request todos los datos 
        /// del nuevo tipo de proucto a agregar a la lista
        /// </summary>
        /// <param name="request">el fabricante que se desea agregar a la vista</param>
        /// <returns>redirecciona al dashboard principal (metodo action index)</returns>
        [HttpPost]
        public IActionResult Add(TipoProducto request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            _context.Add(request);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
