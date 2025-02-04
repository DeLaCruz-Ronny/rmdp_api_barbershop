using api_barber.Models;
using api_barber.Models.DTOs;
using AutoMapper;

namespace api_barber.Mapeo
{
    public class ModelsProfile : Profile
    {
        public ModelsProfile()
        {
            CreateMap<Servicio, ServicioDTO>().ReverseMap();
        }
    }
}
