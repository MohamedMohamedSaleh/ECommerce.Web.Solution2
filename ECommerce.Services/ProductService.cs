using AutoMapper;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.ProductModule;
using ECommerce.ServiceAbstraction;
using ECommerce.Services.Specification;
using ECommerce.Shared;
using ECommerce.Shared.CommonResult;
using ECommerce.Shared.DTOS.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<BrandDTO>> GetAllBrandsAsync()
        {
            var Brands = await _unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync();
            return _mapper.Map<IEnumerable<BrandDTO>>(Brands);
            // Do not forget to create the mapping profile for Brand to BrandDTO in your Mapping profile.
        }

        public async Task<PaginatedResult<ProductDTO>> GetAllProductsAsync(ProductQueryParams queryParams)
        {
            var Spec = new ProductWithBrandAndTypeSpecification(queryParams);
            var Products = await _unitOfWork.GetRepository<Product, int>().GetAllAsync(Spec);

            var ProductsDTO = _mapper.Map<IEnumerable<ProductDTO>>(Products);
            var PageCount = ProductsDTO.Count();
            var CountSpec = new ProductCountSpecification(queryParams);
            var CountOfProducts = await _unitOfWork.GetRepository<Product, int>().CountAsync(CountSpec);
            return new PaginatedResult<ProductDTO>(queryParams.PageIndex, PageCount, queryParams.PageSize, CountOfProducts, ProductsDTO);
        }

        public async Task<IEnumerable<TypeDTO>> GetAllTypesAsync()
        {
            var Types = await _unitOfWork.GetRepository<ProductType, int>().GetAllAsync();
            return _mapper.Map<IEnumerable<TypeDTO>>(Types);
        }

        public async Task<Result<ProductDTO>> GetProductByIdAsync(int id)
        {
            var Spec = new ProductWithBrandAndTypeSpecification(id);
            var Product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(Spec);
            if (Product is null)
            {
                return Error.NotFound();
            }
            return _mapper.Map<ProductDTO>(Product);
        }
    }
}
