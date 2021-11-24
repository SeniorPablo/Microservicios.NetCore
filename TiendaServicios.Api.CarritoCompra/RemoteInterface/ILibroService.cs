using System;
using System.Threading.Tasks;
using TiendaServicios.Api.CarritoCompra.RemoteModel;

namespace TiendaServicios.Api.CarritoCompra.RemoteInterface
{
    public interface ILibroService
    {
        Task<(bool resultado, LibroRemote libro, string errorMessage)> GetLibro(Guid LibroId);
    }
}
