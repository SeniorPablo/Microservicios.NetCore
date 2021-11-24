using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.CarritoCompra.Aplicacion.Dto;
using TiendaServicios.Api.CarritoCompra.Modelo;
using TiendaServicios.Api.CarritoCompra.Persistencia;
using TiendaServicios.Api.CarritoCompra.RemoteInterface;

namespace TiendaServicios.Api.CarritoCompra.Aplicacion
{
    public class Consulta
    {
        public class Ejecuta : IRequest<CarritoDto> {
            public int CarritoSesionId { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta, CarritoDto>
        {
            private readonly CarritoContexto _contexto;
            private readonly ILibroService _libroService;

            public Manejador(CarritoContexto contexto, ILibroService libroService)
            {
                _contexto = contexto;
                _libroService = libroService;
            }

            public async Task<CarritoDto> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                CarritoSesion carritoSesion = await _contexto.CarritoSesiones
                                                    .FirstOrDefaultAsync(c => c.CarritoSesionId == request.CarritoSesionId);

                List<CarritoSesionDetalle> carritoSesionDetalle = await _contexto.CarritoSesionDetalles
                                                                        .Where(cs => cs.CarritoSesionId == request.CarritoSesionId)
                                                                        .ToListAsync();

                List<CarritoDetalleDto> listaCarritoDto = new List<CarritoDetalleDto>();

                foreach (var libro in carritoSesionDetalle)
                {
                    var response = await _libroService.GetLibro(new Guid(libro.ProductoSeleccionado));
                    if(response.resultado)
                    {
                        var objetoLibro = response.libro;
                        CarritoDetalleDto carritoDetalle = new CarritoDetalleDto
                        {
                            TituloLibro = objetoLibro.Titulo,
                            FechaPublicacion = objetoLibro.FechaPublicacion,
                            LibroId = objetoLibro.LibreriaMaterialId
                        };

                        listaCarritoDto.Add(carritoDetalle);
                    }
                }

                return new CarritoDto
                {
                    CarritoId = carritoSesion.CarritoSesionId,
                    FechaCreacionSesion = carritoSesion.FechaCreacion,
                    ListaProductos = listaCarritoDto
                };
            }
        }
    }
}
