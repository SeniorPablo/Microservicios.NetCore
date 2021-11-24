using System;
using System.Collections.Generic;

namespace TiendaServicios.Api.CarritoCompra.Aplicacion.Dto
{
    public class CarritoDto
    {
        public int CarritoId { get; set; }
        public DateTime? FechaCreacionSesion { get; set; }
        public List<CarritoDetalleDto> ListaProductos { get; set; }
    }
}
