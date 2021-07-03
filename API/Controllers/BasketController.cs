using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BasketController : BaseApiController
    {
        public readonly IMapper _mapper ;
        private readonly  IBasketRepository _basketRepository;
        public BasketController(IBasketRepository basketRepository, IMapper mapper)
        {
            _basketRepository = basketRepository;
        }
        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasketById(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);
            return Ok(basket ?? new CustomerBasket());
        }
        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basketDto)
        {

            var basketItem = new BasketItem(){ Id=basketDto.Items[0].Id, Type=basketDto.Items[0].Type, Brand=basketDto.Items[0].Brand, ProductName=basketDto.Items[0].ProductName, Price=basketDto.Items[0].Price, PictureUrl= basketDto.Items[0].PictureUrl , Quantity=basketDto.Items[0].Quantity};
            var item =new List<BasketItem>();
            item.Add(basketItem);
            var basket= new CustomerBasket(){Id=basketDto.Id, Items=item}; //_mapper.Map<CustomerBasket>(basketDto);
            var updatedBasket = await _basketRepository.UpdateBasketAsync(basket);
            return Ok(updatedBasket);
        }
        [HttpDelete]
        public async Task DeleteBasket(string id)
        {            
            await _basketRepository.DeleteBasketAsync(id);
        }


    }
}