using Domain.Models;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    public class ProductWithCountSpecifications : BaseSpecifications<Product, int>
    {
        public ProductWithCountSpecifications(ProductSpecificationsParamters specParam) 
            : base(
                  p => (
                        (string.IsNullOrEmpty(specParam.Search) || p.Name.ToLower().Contains(specParam.Search.ToLower()) &&
                        (!specParam.BrandId.HasValue || p.BrandId == specParam.BrandId) &&
                        (!specParam.TypeId.HasValue || p.TypeId == specParam.TypeId)
                    )
                  )
             )
        {
            
        }
    }
}
