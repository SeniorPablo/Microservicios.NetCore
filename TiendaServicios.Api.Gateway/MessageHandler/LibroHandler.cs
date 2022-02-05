using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace TiendaServicios.Api.Gateway.MessageHandler
{
    public class LibroHandler : DelegatingHandler
    {
        private readonly ILogger<LibroHandler> _logger;

        public LibroHandler(ILogger<LibroHandler> logger)
        {
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Stopwatch tiempo = Stopwatch.StartNew();
            _logger.LogInformation("Inicia el request");

            var response = await base.SendAsync(request, cancellationToken);

            _logger.LogInformation($"Este proceso se hizo en {tiempo.ElapsedMilliseconds}ms");

            return response;
        }
    }
}
