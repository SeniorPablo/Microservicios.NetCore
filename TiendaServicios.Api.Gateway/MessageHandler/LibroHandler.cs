using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Gateway.InterfaceRemote;
using TiendaServicios.Api.Gateway.LibroRemote;

namespace TiendaServicios.Api.Gateway.MessageHandler
{
    public class LibroHandler : DelegatingHandler
    {
        private readonly ILogger<LibroHandler> _logger;
        private readonly IAutorRemote _autorRemote;

        public LibroHandler(ILogger<LibroHandler> logger, IAutorRemote autorRemote)
        {
            _logger = logger;
            _autorRemote = autorRemote;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Stopwatch tiempo = Stopwatch.StartNew();
            _logger.LogInformation("Inicia el request");

            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            if(response.IsSuccessStatusCode)
            {
                string contenido = await response.Content.ReadAsStringAsync();
                JsonSerializerOptions options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                LibroModeloRemote resultado = JsonSerializer.Deserialize<LibroModeloRemote>(contenido, options);
                var responseAutor = await _autorRemote.GetAutor(resultado.AutorLibro ?? Guid.Empty);
                if(responseAutor.resultado)
                {
                    AutorModeloRemote objetoAutor = responseAutor.autor;
                    resultado.AutorData = objetoAutor;
                    string resultadoStr = JsonSerializer.Serialize(resultado);
                    response.Content = new StringContent(resultadoStr, System.Text.Encoding.UTF8, "application/json");
                }
            }

            _logger.LogInformation($"Este proceso se hizo en {tiempo.ElapsedMilliseconds}ms");

            return response;
        }
    }
}
