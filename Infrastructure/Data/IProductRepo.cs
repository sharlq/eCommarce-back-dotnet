using Core.Dtos;
using Core.models;

namespace Infrastructure.Data
{
    public interface IProductRepo
    {
        Task<Product> GetProduct(int id);
        Task<ResponseService<IEnumerable<Product>>> GetProducts(int typeId, int brandId, int offset, int sort, string search);
        Task<IEnumerable<ProductBrand>> GetProductsBrands();
        Task<IEnumerable<ProductType>> GetProductsTypes();

        Task<Product> CreateProduct(ProductCreateDto product);

        Task<int> CreateProductBrand(ProductBrand productBrand);
        Task<ProductType> CreateProductType(ProductType productType);
    }
}