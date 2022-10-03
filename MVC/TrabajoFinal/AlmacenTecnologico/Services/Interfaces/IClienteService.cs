using AlmacenTecnologico.Context;
using AlmacenTecnologico.Models;
using AlmacenTecnologico.Models.ViewModel;

namespace AlmacenTecnologico.Services.Interfaces
{
    public interface IClienteService
    {
        Task<List<ClienteViewModel>> ListarClientes();

        Task<List<ClienteViewModel>> ListarClientesEliminados();

        Task<ClienteViewModel> GetCliente(int id);
        bool AgregarCliente(Cliente request);

        Task<bool> EditarCliente(ClienteViewModel request);

        Task<bool> EliminarClienteLogico(ClienteViewModel request);

        Task<bool> RecuperarCliente(ClienteViewModel request);
    }
}
