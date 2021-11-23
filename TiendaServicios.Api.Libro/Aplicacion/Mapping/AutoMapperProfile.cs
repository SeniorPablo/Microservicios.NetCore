using AutoMapper;
using TiendaServicios.Api.Libro.Aplicacion.Dto;
using TiendaServicios.Api.Libro.Modelo;

namespace TiendaServicios.Api.Libro.Aplicacion.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<LibreriaMaterial, LibroMaterialDto>().ReverseMap();
        }
    }
}
