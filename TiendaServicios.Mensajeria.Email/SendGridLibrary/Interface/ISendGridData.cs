using System.Threading.Tasks;
using TiendaServicios.Mensajeria.Email.SendGridLibrary.Models;

namespace TiendaServicios.Mensajeria.Email.SendGridLibrary.Interface
{
    public interface ISendGridData
    {
        Task<(bool result, string errorMessage)> SendEmail(SendGridData data);
    }
}
