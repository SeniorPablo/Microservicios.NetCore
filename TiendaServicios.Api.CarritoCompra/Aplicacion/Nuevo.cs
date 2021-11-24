using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.CarritoCompra.Modelo;
using TiendaServicios.Api.CarritoCompra.Persistencia;

namespace TiendaServicios.Api.CarritoCompra.Aplicacion
{
    public class Nuevo
    {
        public class Ejecuta : IRequest
        {
            public DateTime FechaCreacionSesion { get; set; }
            public List<string> ProductoLista { get; set; }
        }

        //public class EjecutaValidacion : AbstractValidator<Ejecuta>
        //{
        //    public EjecutaValidacion()
        //    {
        //        RuleFor(r => r.Nombre).NotEmpty();
        //        RuleFor(r => r.Apellido).NotEmpty();
        //    }
        //}

        public class Manejador : IRequestHandler<Ejecuta>
        {
            public readonly CarritoContexto _contexto;

            public Manejador(CarritoContexto contexto)
            {
                _contexto = contexto;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                CarritoSesion carritoSesion = new CarritoSesion
                {
                    FechaCreacion = request.FechaCreacionSesion
                };

                _contexto.CarritoSesiones.Add(carritoSesion);
                var valor = await _contexto.SaveChangesAsync();

                if (valor == 0)
                {
                    throw new Exception("Errores en la inserción del carrito");
                }

                int id = carritoSesion.CarritoSesionId;

                foreach(var item in request.ProductoLista)
                {
                    CarritoSesionDetalle carritoSesionDetalle = new CarritoSesionDetalle
                    {
                        FechaCreacion = DateTime.Now,
                        CarritoSesionId = id,
                        ProductoSeleccionado = item
                    };

                    _contexto.CarritoSesionDetalles.Add(carritoSesionDetalle);
                }

                valor = await _contexto.SaveChangesAsync();

                if (valor > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo insertar el detaller del carrito de compras");
            }
        }
    }
}
