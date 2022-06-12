using Core.Dtos;
using Core.models;

using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepo _productRepo;

        public ProductController(IProductRepo productRepo)
        {
            _productRepo = productRepo;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseService<IEnumerable<Product>>>> GetProducts(int typeId, int brandId, int offset = 0, int sort = 0, string search = "")
        {
            var products = await _productRepo.GetProducts(typeId, brandId, offset, sort, search);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _productRepo.GetProduct(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IEnumerable<ProductBrand>>> GetProductsBrands()
        {
            var productsBrands = await _productRepo.GetProductsBrands();
            return Ok(productsBrands);
        }

        [HttpGet("types")]
        public async Task<ActionResult<IEnumerable<ProductType>>> GetProductsTypes()
        {
            var productsTypes = await _productRepo.GetProductsTypes();
            return Ok(productsTypes);
        }

        [HttpPost("brands")]
        public async Task<ActionResult<ProductBrand>> CreateProductBrand(ProductBrand productBrand)
        {
            Console.WriteLine("hello");
            await _productRepo.CreateProductBrand(productBrand);
            return Ok(productBrand);
        }

        [HttpPost("types")]
        public async Task<ActionResult<ProductType>> CreateProductType(ProductType productType)
        {
            await _productRepo.CreateProductType(productType);
            return Ok(productType);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(ProductCreateDto product)
        {
            await _productRepo.CreateProduct(product);
            return Ok(product);
        }


    }
}