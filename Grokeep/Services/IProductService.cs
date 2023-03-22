namespace Grokeep.Services;

public interface IProductService
{
    Task<List<Product>> RetrieveProducts(int inventoryID);
    Task<bool> AppendProduct(Product product);
    Task<bool> UpdateProduct(Product product);
    Task<bool> DeleteProduct(Product product);
}

