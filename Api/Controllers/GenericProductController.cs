using Core.Interfaces;
using Core.models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenericProductController : Controller
    {
        private IGenericRepo<Product> _productRepo;

        private IGenericRepo<ProductBrand> _productBrandRepo;

        private IGenericRepo<ProductType> _productTypeRepo;

        public GenericProductController(IGenericRepo<Product> productRepo, IGenericRepo<ProductBrand> productBrandRepo, IGenericRepo<ProductType> productTypeRepo)
        {
            _productRepo = productRepo;
            _productBrandRepo = productBrandRepo;
            _productTypeRepo = productTypeRepo;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseService<IEnumerable<Product>>>> GetProducts()
        {
            var products = await _productRepo.ListAllAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IEnumerable<ProductBrand>>> GetProductsBrands()
        {
            var productsBrands = await _productBrandRepo.ListAllAsync();
            return Ok(productsBrands);
        }

        [HttpGet("types")]
        public async Task<ActionResult<IEnumerable<ProductType>>> GetProductsTypes()
        {
            var productsTypes = await _productTypeRepo.ListAllAsync();
            return Ok(productsTypes);
        }





    }
}
