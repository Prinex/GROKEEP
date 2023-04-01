namespace Grokeep.Services;

public interface IProductService
{
    Task<List<Product>> RetrieveProducts(int inventoryID);
    Task<Product> RetrieveProduct(int inventoryID, int productID);
    Task<bool> AppendProduct(Product product);
    Task<bool> UpdateProduct(Product product);
    Task<bool> DeleteProduct(Product product);
    Task<bool> DeleteProducts(int inventoryID);
}

