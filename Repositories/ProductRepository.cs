using Sandbox_Calc.Model;
using Microsoft.EntityFrameworkCore;

namespace Sandbox_Calc.Repositories
{
    public interface IProductRepository
    {
        Task<Product> AddProductAsync(Product product);
        Task<Product> UpdateProductAsync(Product product);
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<Product?> FindProductByIdAsync(int id);
        Task DeleteProductAsync(Product product);
    }
    public class ProductRepository(AppDbContext context) : IProductRepository
    {
        public async Task<Product> AddProductAsync(Product product)
        {
            context.Products.Add(product);
            await context.SaveChangesAsync();
            return product;  // returning created product, it will automatically fetch `Id`
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            context.Products.Update(product);
            await context.SaveChangesAsync();
            return product;
        }

        public async Task DeleteProductAsync(Product product)
        {
            context.Products.Remove(product);
            await context.SaveChangesAsync();
        }

        public async Task<Product?> FindProductByIdAsync(int id)
        {
            var product = await context.Products.FindAsync(id);
            return product;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await context.Products.ToListAsync();
        }


    }
}