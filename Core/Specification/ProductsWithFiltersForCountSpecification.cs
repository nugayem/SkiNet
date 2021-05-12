using Core.Entities;

namespace Core.Specification
{
    public class ProductsWithFiltersForCountSpecification:BaseSpecification<Product>
    {
        public ProductsWithFiltersForCountSpecification(ProductSpecParams specParams)
          :base(x=>
            (!specParams.BrandId.HasValue || x.ProductBrandId==specParams.BrandId)&&
            !specParams.TypeId.HasValue || x.ProductTypeId==specParams.TypeId)
        {
            
        }
    }
}