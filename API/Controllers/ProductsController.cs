using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Core.Entities;
using Core.Interfaces;
using AutoMapper;
using API.Dtos;
using Core.Specification;
using API.Error;
using Microsoft.AspNetCore.Http;
using API.Helpers;

namespace API.Controllers
{ 
    public class ProductsController : BaseApiController
    {
        private readonly  IRepository<Product> _productRepository;
        private readonly  IRepository<ProductBrand> _productBrandRepository;
        private readonly  IRepository<ProductType> _productTypeRepository;
        private readonly  IMapper _mapper;

        public ProductsController(
            IRepository<Product> productRepository,
            IRepository<ProductBrand> productBrandRepository,
            IRepository<ProductType> productTypeRepository,
            IMapper mapper
        )
        {
            _productRepository = productRepository;
            _productBrandRepository=productBrandRepository;
            _productTypeRepository=productTypeRepository;
            _mapper=mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<IReadOnlyList<ProductDto>>>> GetProductsAsync(
         [FromQuery] ProductSpecParams specParams)
        {
            System.Console.Write(specParams.BrandId);
            System.Console.Write(specParams.TypeId);
            System.Console.Write(specParams.Sort);
            
            var spec = new ProductsWithTypesAndBrandsSpecification(specParams);
            
            var countSpec = new ProductsWithFiltersForCountSpecification(specParams);
            
            var totlaItems = await _productRepository.CountAsync(countSpec);

            var product= await _productRepository.ListAsync(spec);   
            
            var data= _mapper.Map<IReadOnlyList<Product>,IReadOnlyList<ProductDto>>(product);        
            return Ok(new Pagination<ProductDto>(specParams.PageIndex, specParams.PageSize, totlaItems, data));
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);            
            
            var product =  await _productRepository.GetEntityWithSpec(spec);

            if(product==null) 
                return NotFound( new ApiResponse(404));

            return _mapper.Map<Product,ProductDto>(product);
        }
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrandsAsync()
        {
            var productBrand= await _productBrandRepository.ListAllAsync();          
            return Ok(productBrand);
        }
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductTypesAsync()
        {
            var productType= await _productTypeRepository.ListAllAsync();            
            return Ok(productType);
        }
    }
}