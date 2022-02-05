using MediatR;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaServicios.RabbitMQ.Bus.BusRabbit;
using TiendaServicios.RabbitMQ.Bus.Commands;
using TiendaServicios.RabbitMQ.Bus.Events;

namespace TiendaServicios.RabbitMQ.Bus.Implement
{
    public class RabbitEventBus : IRabbitEventBus
    {
        private readonly IMediator _mediator;
        private readonly Dictionary<string, List<Type>> _handlers;
        private readonly List<Type> _typeEvents;

        public RabbitEventBus(IMediator mediator)
        {
            _mediator = mediator;
            _handlers = new Dictionary<string, List<Type>>();
            _typeEvents = new List<Type>();
        }

        public void Publish<T>(T @event) where T : Event
        {
            var factory = new ConnectionFactory() { HostName = "rabbit-juan-web" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var eventName = @event.GetType().Name;
                channel.QueueDeclare(eventName, false, false, false, null);

                var message = JsonConvert.SerializeObject(@event);
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish("", eventName, null, body);
            }
        }

        public Task SendCommand<T>(T command) where T : Command
        {
            return _mediator.Send(command);
        }

        public void Susbcribe<T, TH>()
            where T : Event
            where TH : IEventHandle<T>
        {
            var eventName = typeof(T).Name;
            var eventTypeHandler = typeof(TH); 

            if(!_typeEvents.Contains(typeof(T)))
            {
                _typeEvents.Add(typeof(T));
            }

            if (!_handlers.ContainsKey(eventName))
            {
                _handlers.Add(eventName, new List<Type>());
            }

            if(_handlers[eventName].Any(e => e.GetType() == eventTypeHandler))
            {
                throw new ArgumentException($"El manejador {eventTypeHandler.Name} fue registrado anteriormente por {eventName}");
            }

            _handlers[eventName].Add(eventTypeHandler);
            var factory = new ConnectionFactory()
            {
                HostName = "rabbit-juan-web",
                DispatchConsumersAsync = true
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(eventName, false, false, false, null);
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += Consumer_Delegate;

            channel.BasicConsume(eventName, true, consumer);
        }

        private async Task Consumer_Delegate(object sender, BasicDeliverEventArgs e)
        {
            var eventName = e.RoutingKey;
            var message = Encoding.UTF8.GetString(e.Body.ToArray());

            try
            {
                 if(_handlers.ContainsKey(eventName))
                {
                    var subscriptions = _handlers[eventName];
                    foreach(var item in subscriptions)
                    {
                        var handler = Activator.CreateInstance(item);
                        if (handler == null) continue;

                        var eventType = _typeEvents.SingleOrDefault(e => e.Name == eventName);
                        var nextEvent = JsonConvert.DeserializeObject(message, eventType);

                        var concretType = typeof(IEventHandle<>).MakeGenericType(eventType);

                        await (Task)concretType.GetMethod("Handle").Invoke(handler, new object[] { nextEvent });
                    }
                }
            }
            catch (Exception ex)
            {
                var error = ex.Message;
            }
        }
    }
}
