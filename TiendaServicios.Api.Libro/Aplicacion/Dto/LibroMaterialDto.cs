using System;

namespace TiendaServicios.Api.Libro.Aplicacion.Dto
{
    public class LibroMaterialDto
    {
        public Guid? LibreriaMaterialId { get; set; }
        public string Titulo { get; set; }
        public DateTime? FechaPublicacion { get; set; }

        public Guid? AutorLibro { get; set; }
    }
}
