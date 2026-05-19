using Domain.Models;
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
        public ProductWithBrandsAndTypesSpecifications() : base(null)
        {
            AddIncludes();
        }
        private void AddIncludes()
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
        }
    }
}
