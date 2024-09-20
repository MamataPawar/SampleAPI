using AutoMapper;
using SampleAPI.Entities;
using SampleAPI.Requests;

namespace SampleAPI
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Order, CreateOrderRequest>().ReverseMap();
        }
    }
}
