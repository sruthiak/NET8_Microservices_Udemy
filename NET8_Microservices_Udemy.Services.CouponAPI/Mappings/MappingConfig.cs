using AutoMapper;
using NET8_Microservices_Udemy.Services.CouponAPI.Models;
using NET8_Microservices_Udemy.Services.CouponAPI.Models.DTOs;

namespace NET8_Microservices_Udemy.Services.CouponAPI.Mappings
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mapperConfig=new MapperConfiguration(config =>
            {
                //CreateMap returns IMapper. Need to register in Program.cs
                config.CreateMap<CouponDTO, Coupon>();
                config.CreateMap<Coupon,CouponDTO>();
            });
            return mapperConfig;

        }
    }
}
