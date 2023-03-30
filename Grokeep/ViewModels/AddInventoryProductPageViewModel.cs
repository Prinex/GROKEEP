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
        this.productService = productService;
        this.historyService = historyService;
    }

    [RelayCommand]
    public async Task AddProduct()
    {
        if (string.IsNullOrEmpty(ProductInventory.Title) || ProductInventory.Price <= 0 || string.IsNullOrEmpty(ProductInventory.Store) || string.IsNullOrEmpty(ProductInventory.DateBought))
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
                Title = ProductInventory.Title,
                Price = ProductInventory.Price,
                Store = ProductInventory.Store,
                DateBought = onlyDate.ToString("yyy/MM/dd")
            });

            // Add the product also to history db service for the filtering feature
            var addProductToHistory = await historyService.AppendHistory(new GroceryHistory
            {
                AccountID = App.UserSessionData.AccountID,
                Title = ProductInventory.Title,
                Price = ProductInventory.Price,
                Store = ProductInventory.Store,
                DateBought = onlyDate.ToString("yyy/MM/dd")
            });

            if (addProductQuery == true && addProductToHistory == true)
            {
                IsBusy = false;
                await Shell.Current.DisplayAlert("Successfull operation", $"The {ProductInventory.Title} item has been added to {Inventory.GroceryInventoryName} inventory.", "OK");
            }
            else
            {
                IsBusy = false;
                await Shell.Current.DisplayAlert("Error", "Could not add the item to the inventory due to some technical issues.", "OK");
            }
        }
            
    }
}

