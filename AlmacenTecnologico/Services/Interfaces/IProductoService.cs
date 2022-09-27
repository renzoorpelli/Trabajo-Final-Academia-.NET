using AlmacenTecnologico.Context;
using AlmacenTecnologico.Models.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlmacenTecnologico.Services.Interfaces
{
    public interface IProductoService
    {
        Task<List<ProductoViewModel>> ListarProductos(TrabajoFinalContext context);

        Task<List<ProductoViewModel>> ListarProductosEliminados(TrabajoFinalContext context);

        Task<List<SelectListItem>> ListarTipoProducto(TrabajoFinalContext context);

        Task<List<SelectListItem>> ListarFabricantes(TrabajoFinalContext context);

        Task<ProductoViewModel> GetProducto(int id, TrabajoFinalContext context);

        Task<bool> EditarProducto(ProductoViewModel request, TrabajoFinalContext context);

    }
}
