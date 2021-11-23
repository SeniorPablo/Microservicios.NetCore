using AutoMapper;
using TiendaServicios.Api.Autor.Aplicacion.Dto;
using TiendaServicios.Api.Autor.Modelo;

namespace TiendaServicios.Api.Autor.Aplicacion.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AutorLibro, AutorDto>().ReverseMap();
        }
    }
}
