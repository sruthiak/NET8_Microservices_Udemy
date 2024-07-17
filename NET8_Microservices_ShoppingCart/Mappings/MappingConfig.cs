using AutoMapper;
using NET8_Microservices_ShoppingCartAPI.Models;
using NET8_Microservices_ShoppingCartAPI.Models.DTOs;

namespace NET8_Microservices_ShoppingCartAPI.Mappings
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CartHeaderDTO, CartHeader>();
                config.CreateMap<CartDetailsDTO, CartDetails>();
            });
            return mapperConfig;
        }
    }
}
