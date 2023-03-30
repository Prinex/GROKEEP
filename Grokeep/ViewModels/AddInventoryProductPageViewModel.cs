namespace Grokeep.ViewModels;

[QueryProperty(nameof(Inventory), "Inventory")]
public partial class AddInventoryProductPageViewModel : BaseViewModel
{
    private readonly IProductService productService;
    private readonly IGroceryHistoryService historyService;

    [ObservableProperty]
    private GroceryInventory inventory = new GroceryInventory();

    [ObservableProperty]
    private Product productInventory = new Product();

    public AddInventoryProductPageViewModel(IProductService productService, IGroceryHistoryService historyService)
    {
        // prevent empty string issue when binding to Date property from DatePicker
        DateTime today = DateTime.Today;
        ProductInventory.DateBought = today.ToString("yyy-MM-dd");
        this.productService = productService;
        this.historyService = historyService;
    }

    [RelayCommand]
    public async Task AddProduct()
    {
        if (string.IsNullOrEmpty(ProductInventory.Description) || ProductInventory.Cost <= 0 || string.IsNullOrEmpty(ProductInventory.Location) || string.IsNullOrEmpty(ProductInventory.DateBought))
        {
            await Shell.Current.DisplayAlert("Warning", "One or more fields are empty or have an illogical value.", "OK");
            return;
        }
        else
        {
            IsBusy= true;
            DateTime extractDateBought = DateTime.Parse(ProductInventory.DateBought);
            DateTime onlyDate = extractDateBought.Date;
            var addProductQuery = await productService.AppendProduct(new Product
            {
                GroceryInventoryID = Inventory.GroceryInventoryID,
                Description = ProductInventory.Description,
                Cost = ProductInventory.Cost,
                Location = ProductInventory.Location,
                DateBought = onlyDate.ToString("yyy/MM/dd")
            });

            // Add the product also to history db service for the filtering feature
            var addProductToHistory = await historyService.AppendHistory(new GroceryHistory
            {
                AccountID = App.UserSessionData.AccountID,
                Description = ProductInventory.Description,
                Cost = ProductInventory.Cost,
                Location = ProductInventory.Location,
                DateBought = onlyDate.ToString("yyy/MM/dd")
            });

            if (addProductQuery == true && addProductToHistory == true)
            {
                IsBusy = false;
                await Shell.Current.DisplayAlert("Successfull operation", $"The {ProductInventory.Description} item has been added to {Inventory.GroceryInventoryName} inventory.", "OK");
            }
            else
            {
                IsBusy = false;
                await Shell.Current.DisplayAlert("Error", "Could not add the item to the inventory due to some technical issues.", "OK");
            }
        }
            
    }
}

