using AlmacenTecnologico.Context;
using AlmacenTecnologico.Models.ViewModel;

namespace AlmacenTecnologico.Services.Interfaces
{
    public interface IClienteService
    {
        Task<List<ClienteViewModel>> ListarClientes(TrabajoFinalContext context);

        Task<List<ClienteViewModel>> ListarClientesEliminados(TrabajoFinalContext context);

        Task<ClienteViewModel> GetCliente(int id, TrabajoFinalContext context);
        Task<bool> EditarCliente(ClienteViewModel request, TrabajoFinalContext context);
    }
}
