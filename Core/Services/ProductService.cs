using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Models;
using Services.Abstractions;
using Services.Specifications;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ProductService(IUnitOfWork unitOfWork, IMapper mapper) : IProductService
    {
        public async Task<PaginationResponse<ProductResultDto>> GetAllProductsAsyns(ProductSpecificationsParamters specParams)
        {
            // Create Specification
            var spec = new ProductWithBrandsAndTypesSpecifications(specParams);
            
            // Get All Products Through ProductRepository
            var products = await unitOfWork.GetRepository<Product,int>().GetAllAsync(spec);

            var totalspec = new ProductWithCountSpecifications(specParams);

            var count = await unitOfWork.GetRepository<Product,int>().CountAsync(totalspec);

            // Mapping IEnumerable<Product> To IEnumerable<ProductResultDto>
            var result = mapper.Map<IEnumerable<ProductResultDto>>(products);
            return new PaginationResponse<ProductResultDto>(specParams.PageIndex, specParams.PageSize, count, result);
        }
        public async Task<ProductResultDto?> GetProductByIdAsync(int id)
        {
            // Create Specification
            var spec = new ProductWithBrandsAndTypesSpecifications(id);

            var product = await unitOfWork.GetRepository<Product, int>().GetAsync(spec);
            if(product == null)
                throw new ProductNotFoundException(id);
            var result = mapper.Map<ProductResultDto>(product);
            return result;
        }
        public async Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync()
        {
            var brand = await unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync();
            var result = mapper.Map<IEnumerable<BrandResultDto>>(brand);
            return result;
        }
        public async Task<IEnumerable<TypeResultDto>> GetAllTypesAsync()
        {
            var type = await unitOfWork.GetRepository<ProductType,int>().GetAllAsync();
            var result = mapper.Map<IEnumerable<TypeResultDto>>(type);
            return result;
        }
    }
}
