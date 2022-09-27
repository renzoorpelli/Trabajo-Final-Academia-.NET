using AlmacenTecnologico.Context;
using AlmacenTecnologico.Models;
using AlmacenTecnologico.Models.ViewModel;

namespace AlmacenTecnologico.Services.Interfaces
{
    public interface IPersonaService
    {
        Task<List<EmpleadoViewModel>> ListarEmpleados(TrabajoFinalContext context);

        Task<List<EmpleadoViewModel>> ListarEmpleadosEliminados(TrabajoFinalContext context);

        Task<EmpleadoViewModel> GetEmpleado(int id, TrabajoFinalContext context);

        Task<bool> EditarEmpleado(TrabajoFinalContext context, EmpleadoViewModel request);

        Task<bool> ValidarDatosEmpleado(Persona request, TrabajoFinalContext context, bool editando = false);

        Task<bool> ValidarNombreCompleto(string nombre);

        Task<bool> ValidarDNI(string dni, TrabajoFinalContext context, bool editando = false);
    }
}
