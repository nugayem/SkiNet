using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specification;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    { 
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork, IBasketRepository basketRepository)
        {
            this._unitOfWork = unitOfWork;
            this._basketRepository = basketRepository; 
        }

        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address shippingAddress)
        {
            //get basket from the repo--dont'trust price
            var basket = await _basketRepository.GetBasketAsync(basketId);
            //get items price from the product repo
            var items = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);
                var orderItems = new OrderItem(itemOrdered, productItem.Price, item.Quantity);
                items.Add(orderItems);
            }
            //get delivery method from repo
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            //Calc Subtoal
            var subTotal = items.Sum(item => item.Price * item.Quantity);

            // Create Order
            var order = new Order(items, buyerEmail, shippingAddress, deliveryMethod, subTotal);

            //Save to DB

            _unitOfWork.Repository<Order>().Add(order);

            var result=await _unitOfWork.Complete();

            if(result<=0) return null;

            //Delete Basket

            await _basketRepository.DeleteBasketAsync(basketId);

            //Return Order
            return order;



        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            return await _unitOfWork.Repository<DeliveryMethod>().ListAllAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
             var spec=  new OrdersWithItemsAndOrderingSpecification(id, buyerEmail);

             return await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec=  new OrdersWithItemsAndOrderingSpecification(buyerEmail);

            return await _unitOfWork.Repository<Order>().ListAsync(spec);
        }
    }
}