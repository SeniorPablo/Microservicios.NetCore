using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TiendaServicios.RabbitMQ.Bus.BusRabbit;
using TiendaServicios.RabbitMQ.Bus.EventQueue;

namespace TiendaServicios.Api.Autor.RabbitHandler
{
    public class EmailEventHandler : IEventHandle<EmailEventQueue>
    {
        private readonly ILogger<EmailEventHandler> _logger;

        public EmailEventHandler() {}

        //public EmailEventHandler(ILogger<EmailEventHandler> logger)
        //{
        //    _logger = logger;    
        //}

        public Task Handle(EmailEventQueue @event)
        {
            //_logger.LogInformation($"Este es el valor que consumo desde RabbitMQ {@event.Title}");
            return Task.CompletedTask;
        }
    }
}
