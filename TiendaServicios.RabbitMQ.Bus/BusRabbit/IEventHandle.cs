using System.Threading.Tasks;
using TiendaServicios.RabbitMQ.Bus.Events;

namespace TiendaServicios.RabbitMQ.Bus.BusRabbit
{
    public interface IEventHandle<in TEvent> : IEventHandle where TEvent : Event
    {
        Task Handle(TEvent @event);
    }

    public interface IEventHandle { }
}
