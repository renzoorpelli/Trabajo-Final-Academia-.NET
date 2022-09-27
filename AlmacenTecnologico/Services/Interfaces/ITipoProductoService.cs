using AlmacenTecnologico.Context;
using AlmacenTecnologico.Models.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlmacenTecnologico.Services.Interfaces
{
    public interface ITipoProductoService
    {
        Task<List<TipoProductoViewModel>> ListarTipoProductos(TrabajoFinalContext context);
        Task<List<SelectListItem>> ListarCategorias(TrabajoFinalContext context);
    }
}
