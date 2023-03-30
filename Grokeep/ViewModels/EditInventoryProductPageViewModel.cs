namespace Grokeep.ViewModels;

[QueryProperty(nameof(ProductInventory), "ProductInventory")]
public partial class EditInventoryProductPageViewModel : BaseViewModel
{
    private readonly IProductService productService;

    [ObservableProperty]
    private Product productInventory = new Product();

    public EditInventoryProductPageViewModel(IProductService productService)
    {
        this.productService = productService;
    }

    [RelayCommand]
    public async Task SaveProduct()
    {
        var currentProductDetail = await productService.RetrieveProduct(ProductInventory.GroceryInventoryID, ProductInventory.ProductID);
        if (currentProductDetail.DoesExist == true)
        {
            if (currentProductDetail.Title == ProductInventory.Title && currentProductDetail.Price == ProductInventory.Price && currentProductDetail.Store == ProductInventory.Store && currentProductDetail.DateBought == ProductInventory.DateBought)
            {
                await Shell.Current.DisplayAlert("Warning", "There are no updates to be made.", "OK");
            }
            else
            {
                IsBusy = true;
                DateTime extractDateBought = DateTime.Parse(ProductInventory.DateBought);
                DateTime onlyDate = extractDateBought.Date;
                ProductInventory.DateBought = onlyDate.ToString("yyy/MM/dd");
                var updateProductResponse = await productService.UpdateProduct(ProductInventory);
                
                if (updateProductResponse == true)
                {
                    IsBusy = false;
                    await Shell.Current.DisplayAlert("Successful operation", $"Current changes for {ProductInventory.Title} product have been made and saved.", "OK");
                }
                else
                {
                    IsBusy = false;
                    await Shell.Current.DisplayAlert("Error", $"Could not make changes for {ProductInventory.Title} due to some tehnical issues.", "OK");
                }
            }
        }
    }
}
