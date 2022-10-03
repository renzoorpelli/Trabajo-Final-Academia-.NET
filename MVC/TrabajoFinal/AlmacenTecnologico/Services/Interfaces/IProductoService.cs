using AlmacenTecnologico.Context;
using AlmacenTecnologico.Models;
using AlmacenTecnologico.Models.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlmacenTecnologico.Services.Interfaces
{
    public interface IProductoService
    {
        Task<List<ProductoViewModel>> ListarProductos();

        Task<List<ProductoViewModel>> ListarProductosEliminados();

        Task<List<SelectListItem>> ListarTipoProducto();

        Task<List<SelectListItem>> ListarFabricantes();

        Task<ProductoViewModel> GetProducto(int id);

        Task<bool> AgregarProducto(Producto request);
        Task<bool> EditarProducto(ProductoViewModel request);
        Task<bool> EliminarProductoLogico(ProductoViewModel request);

        Task<bool> RecuperarProducto(ProductoViewModel request);

    }
}
