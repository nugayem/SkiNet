using API.Dtos;
using AutoMapper;
using Core.Entities;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(pd=>pd.ProductBrand, o=>o.MapFrom(p=>p.ProductBrand.Name))
                .ForMember(pd=>pd.ProductType, o=>o.MapFrom(p=>p.ProductType.Name))
                .ForMember(pd=>pd.PictureUrl, o=>o.MapFrom<ProductUrlResolver>());
        }
    }
}