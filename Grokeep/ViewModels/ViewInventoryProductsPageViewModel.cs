using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Grokeep.ViewModels;

[QueryProperty(nameof(Inventory), "Inventory")]
public partial class ViewInventoryProductsPageViewModel : BaseViewModel
{
    private readonly IGroceryInventoryService inventoryService;

    private readonly IProductService productService;

    [ObservableProperty]
    private GroceryInventory inventory = new GroceryInventory();

    public ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>();

    public ViewInventoryProductsPageViewModel(IGroceryInventoryService inventoryService, IProductService productService)
    {
        this.inventoryService = inventoryService;
        this.productService = productService;
    }

    public async Task GetInventoryProducts()
    {
        Products.Clear();
        IsBusy = true;
        var productsDB = await productService.RetrieveProducts(Inventory.GroceryInventoryID);

        if (productsDB?.Count > 0) 
        {
            productsDB = productsDB.OrderByDescending(i => i.ProductID).ToList();
            foreach(var product in productsDB) 
            {
                Products.Add(product);
            }
        }
        IsBusy = false;
    }

    [RelayCommand]
    public async Task GoToAddProductPage()
    {
        var navigationQueryParameters = new Dictionary<string, object>();
        navigationQueryParameters.Add("Inventory", Inventory);
        await Shell.Current.GoToAsync($"/{nameof(AddInventoryProductPage)}", navigationQueryParameters);
    }

    [RelayCommand]
    public async Task GoToEditProduct(Product product)
    {
        var navigationQueryParameters = new Dictionary<string, object>();
        navigationQueryParameters.Add("Product", product);
        await Shell.Current.GoToAsync($"/{nameof(EditInventoryProductPage)}", navigationQueryParameters);
    }

    [RelayCommand]
    public async Task DeleteProduct(Product product)
    {
        var deleteProductConfirmation = await Shell.Current.DisplayActionSheet($"Are you sure you want to delete {product.Title} inventory?", "No", null, "Yes");
        if (deleteProductConfirmation == "Yes")
        {
            IsBusy = true;
            var removeProductQuery = await productService.DeleteProduct(product);
            if (removeProductQuery)
            {
                IsBusy = false;
                await GetInventoryProducts();
            }
            else
            {
                IsBusy = false;
                await Shell.Current.DisplayAlert("Error", $"Issues occured while trying to delete the {product.Title} inventory of items.", "OK");
            }
        }
    }
}
