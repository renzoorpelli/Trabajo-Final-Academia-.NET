using AlmacenTecnologico.Context;
using AlmacenTecnologico.Models;
using AlmacenTecnologico.Models.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace AlmacenTecnologico.Services.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// metodo encargado de revisar si el usario ingresado por el AccessController se encuentra
        /// en la base de datos
        /// </summary>
        /// <param name = "user" > el usuario que se intentara loguear</param>
        /// <returns>retornara true si se encuentra, false de lo contrario</returns>
        Task<UsuarioViewModel> VerificarUsuario(Usuario user, TrabajoFinalContext context);


        Task SetClaims(UsuarioViewModel user, HttpContext ControllerBase);

    }
}
