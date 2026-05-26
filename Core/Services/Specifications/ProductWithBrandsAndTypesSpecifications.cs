using Domain.Models;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    public class ProductWithBrandsAndTypesSpecifications : BaseSpecifications<Product, int>
    {
        public ProductWithBrandsAndTypesSpecifications(int id) : base(p => p.Id == id)
        {
            AddIncludes();
        }
        public ProductWithBrandsAndTypesSpecifications(ProductSpecificationsParamters specParam) :
            base(
                    p => (
                        (string.IsNullOrEmpty(specParam.Search) || p.Name.ToLower().Contains(specParam.Search.ToLower()) &&
                        (!specParam.BrandId.HasValue || p.BrandId == specParam.BrandId) &&
                        (!specParam.TypeId.HasValue || p.TypeId == specParam.TypeId)
                    )

               )
            )
        {
            AddIncludes();
            ApplySorting(specParam.Sort);
            ApplyPagination(specParam.PageIndex, specParam.PageSize);
        }
        private void AddIncludes()
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
        }

        protected void ApplySorting(string? sort)
        {
            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort.ToLower())
                {
                    case "namedesc":
                        AddOrderByDescending(p => p.Name);
                        break;
                    case "priceasc":
                        AddOrderBy(p => p.Price);
                        break;
                    case "pricedesc":
                        AddOrderByDescending(p => p.Price);
                        break;
                    default:
                        AddOrderBy(p => p.Name);
                        break;
                }
            }
           else
            {
                AddOrderBy(p => p.Name);
            }
        }
    }
}
