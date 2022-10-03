
using AlmacenTecnologico.Context;
using AlmacenTecnologico.Models;
using AlmacenTecnologico.Models.ViewModel;

namespace AlmacenTecnologico.Services.Interfaces
{
    public interface IFabricantesServces
    {
        Task<List<FabricanteViewModel>> ListarFabricantes();
        Task<List<FabricanteViewModel>> ListarFabricantesEliminados();
        Task<FabricanteViewModel> GetFabricante(int id);
        bool AgregarFabricante(Fabricante request);
        Task<bool> EditarFabricante(FabricanteViewModel request);
        Task<bool> EliminarFabricanteLogico(FabricanteViewModel request);
        Task<bool> RecuperarCliente(FabricanteViewModel request);
    }
}
