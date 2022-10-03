using AlmacenTecnologico.Context;
using AlmacenTecnologico.Models;
using AlmacenTecnologico.Models.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlmacenTecnologico.Services.Interfaces
{
    public interface ITipoProductoService
    {
        Task<List<TipoProductoViewModel>> ListarTipoProductos();
        Task<List<TipoProductoViewModel>> ListarTipoProductosEliminados();
        Task<List<SelectListItem>> ListarCategorias();
        Task<TipoProductoViewModel> GetTipoProducto(int id);
        bool AgregarTipoProducto(TipoProducto request);
        Task<bool> EditarTipoProducto(TipoProductoViewModel request);
        Task<bool> EliminarTipoProductoLogico(TipoProductoViewModel request);
        Task<bool> RecuperarTipoProducto(TipoProductoViewModel request);
    }
}
