using Core.Dtos;
using Core.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data
{
    public class ProductRepo : IProductRepo
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
 


        public ProductRepo(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

        }


        public async Task<Product> CreateProduct(ProductCreateDto product)
        {
            if (product == null)
            {
                throw new System.ArgumentNullException(nameof(product));
            }

            Product prod = new Product
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ProductBrandId = product.ProductBrandId,
                ProductTypeId = product.ProductTypeId,
                PictureUrl = product.PictureUrl
            };
            var result = _context.Products.Add(prod);
            await _context.SaveChangesAsync();

            if (result == null)
            {
                throw new System.ArgumentNullException(nameof(result));
            }
            return result.Entity;
        }

        public async Task<int> CreateProductBrand(ProductBrand productBrand)
        {
            if (productBrand == null)
            {
                throw new System.ArgumentNullException(nameof(productBrand));
            }

            ProductBrand brand = new ProductBrand
            {
                Name = productBrand.Name

            };
            var result = _context.ProductBrands.Add(brand);
            await _context.SaveChangesAsync();

            if (result == null)
            {
                throw new System.ArgumentNullException(nameof(result));
            }
            Console.WriteLine(result);
            Console.WriteLine(result.Entity);
            return result.Entity.Id;
        }

        public async Task<ProductType> CreateProductType(ProductType productType)
        {
            if (productType == null)
            {
                throw new System.ArgumentNullException(nameof(productType));
            }

            ProductType type = new ProductType
            {
                Name = productType.Name

            };
            var result = _context.ProductTypes.Add(type);
            await _context.SaveChangesAsync();


            if (result == null)
            {
                throw new System.ArgumentNullException(nameof(result));
            }

            return result.Entity;

        }

        public async Task<Product> GetProduct(int id)
        {
            return await _context.Products.Include(p => p.ProductBrand).Include(p => p.ProductBrand).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<ResponseService<IEnumerable<Product>>> GetProducts(int typeId, int brandId, int offset, int sort, string search)
        {



            var productsQuery = _context.Products.Include(p => p.ProductBrand).Include(p => p.ProductType).AsQueryable();

            if (sort == 1)
            {
                productsQuery = productsQuery.OrderBy(p => p.Name);
            }
            else if (sort == 2)
            {
                productsQuery = productsQuery.OrderBy(p => p.Price);
            }
            else if (sort == 3)
            {
                productsQuery = productsQuery.OrderByDescending(p => p.Price);
            }
            else
            {
                productsQuery = productsQuery.OrderByDescending(p => p.Id);
            }

            if (search != "WhatTheHellIsWrongWithYou")
            {
                productsQuery = productsQuery.Where(p => p.Name.Contains(search));
            }

            Console.WriteLine("////////////////");
            Console.WriteLine(string.IsNullOrEmpty(search) == false);
            Console.WriteLine(string.IsNullOrEmpty(search));
            Console.WriteLine(search);
            Console.WriteLine("////////////////");

            if (typeId > 0)
            {
                productsQuery = productsQuery.Where(p => p.ProductTypeId == typeId);
            }
            if (brandId > 0)
            {
                productsQuery = productsQuery.Where(p => p.ProductBrandId == brandId);
            }

            var products = await productsQuery.Skip(offset).Take(6).ToListAsync();
            products.ForEach(p =>
            {
                p.PictureUrl = _configuration.GetSection("ApiUrl").Value + p.PictureUrl;

            });

            var res = new ResponseService<IEnumerable<Product>>();

            res.Data = products;

            res.pagination = new Pagination
            {
                Total = productsQuery.Count(),
                Offset = offset,
                ItemsNumber = 6
            };


            return res;
        }

        public async Task<IEnumerable<ProductBrand>> GetProductsBrands()
        {
            return await _context.ProductBrands.ToListAsync();
        }

        public async Task<IEnumerable<ProductType>> GetProductsTypes()
        {
            return await _context.ProductTypes.ToListAsync();
        }

        enum Sort
        {
            alpha,
            priceAce,
            priceDesc,

            none
        }
    }
}