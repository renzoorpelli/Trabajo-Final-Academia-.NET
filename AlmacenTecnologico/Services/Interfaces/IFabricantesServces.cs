
using AlmacenTecnologico.Context;
using AlmacenTecnologico.Models.ViewModel;

namespace AlmacenTecnologico.Services.Interfaces
{
    public interface IFabricantesServces
    {
        Task<List<FabricanteViewModel>> ListarFabricantes(TrabajoFinalContext context);

        Task<List<FabricanteViewModel>> ListarFabricantesEliminados(TrabajoFinalContext context);

        Task<FabricanteViewModel> GetFabricante(TrabajoFinalContext context, int id);
        Task<bool> EditarFabricante(TrabajoFinalContext context, FabricanteViewModel request);
    }
}
