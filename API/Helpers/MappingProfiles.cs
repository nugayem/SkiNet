using System.Collections.Generic;
using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;
using Core.Entities.OrderAggregate;

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
            CreateMap<Core.Entities.Identity.Address, AddressDto>().ReverseMap();
            CreateMap<CustomerBasketDto, CustomerBasket>() ;
            CreateMap<BasketItemDto, BasketItem>().ReverseMap();
            CreateMap<AddressDto, Core.Entities.OrderAggregate.Address>(); 
            CreateMap<Order, OrderToReturnDto>()
                    .ForMember(d=>d.DeliveryMethod, o=> o.MapFrom(s=>s.DeliveryMethod.ShortName)) 
                    .ForMember(d=>d.ShippingPrice, o=> o.MapFrom(s=>s.DeliveryMethod.Price)); 
            CreateMap<OrderItem, OrderItemDto>()
                    .ForMember(d=>d.ProductId, o=>o.MapFrom(s=>s.ItemOrdered.ProductItemId))
                    .ForMember(d=>d.ProductName, o=>o.MapFrom(s=>s.ItemOrdered.ProductName)) 
                    .ForMember(d=>d.PictureUrl, o=>o.MapFrom(s=>s.ItemOrdered.PictureUrl))         
                    .ForMember(d=>d.PictureUrl, o=>o.MapFrom<OrderItemUrlResolver>()) ;        

        }
    }
}