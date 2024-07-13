using AutoMapper;
using NET8_Microservices_ProductAPI.Models;
using NET8_Microservices_ProductAPI.Models.DTOs;

namespace NET8_Microservices_ProductAPI.Mappings
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            return new MapperConfiguration(config =>
            {
                //CreateMap returns IMapper. Call this function in Program.cs.
                //Need to register IMapper and AutoMapper in Program.cs
                config.CreateMap<Product, ProductDTO>();
                config.CreateMap<ProductDTO, Product>();
            });
            
            
        }
    }
}
