using AutoMapper;
using TiendaServicios.Api.Libro.Aplicacion.Dto;
using TiendaServicios.Api.Libro.Modelo;

namespace TiendaServicios.Api.Libro.Tests
{
    public class MappingTest : Profile
    {
        public MappingTest()
        {
            CreateMap<LibreriaMaterial, LibroMaterialDto>().ReverseMap();
        }
    }
}
