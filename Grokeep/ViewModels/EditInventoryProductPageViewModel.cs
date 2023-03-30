namespace Grokeep.ViewModels;

[QueryProperty(nameof(ProductInventory), "ProductInventory")]
public partial class EditInventoryProductPageViewModel : BaseViewModel
{
    private readonly IProductService productService;
    private readonly IGroceryHistoryService historyService;

    [ObservableProperty]
    public Product productInventory = new Product();

    public EditInventoryProductPageViewModel(IProductService productService, IGroceryHistoryService groceryHistory)
    {
        this.productService = productService;
        this.historyService = groceryHistory;
    }

    [RelayCommand]
    public async Task SaveProduct()
    {
        var currentProductDetail = await productService.RetrieveProduct(ProductInventory.GroceryInventoryID, ProductInventory.ProductID);
        

        if (currentProductDetail.DoesExist == true)
        {
            if (currentProductDetail.Description == ProductInventory.Description && currentProductDetail.Cost == ProductInventory.Cost && currentProductDetail.Location == ProductInventory.Location && currentProductDetail.DateBought == ProductInventory.DateBought)
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
                var updateProductInHistory = await historyService.UpdateHistory(currentProductDetail.Description, ProductInventory);

                if (updateProductResponse == true && updateProductInHistory == true)
                {
                    IsBusy = false;
                    await Shell.Current.DisplayAlert("Successful operation", $"Current changes for {ProductInventory.Description} product have been made and saved.", "OK");
                }
                else
                {
                    IsBusy = false;
                    await Shell.Current.DisplayAlert("Error", $"Could not make changes for {ProductInventory.Description} due to some tehnical issues.", "OK");
                }
            }
        }
    }
}
