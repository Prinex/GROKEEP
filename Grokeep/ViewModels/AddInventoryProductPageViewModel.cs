namespace Grokeep.ViewModels;

[QueryProperty(nameof(Inventory), "Inventory")]
public partial class AddInventoryProductPageViewModel : BaseViewModel
{
    private readonly IProductService productService;

    [ObservableProperty]
    private GroceryInventory inventory = new GroceryInventory();

    [ObservableProperty]
    private Product product = new Product();

    public AddInventoryProductPageViewModel(IProductService productService)
    {
        this.productService = productService;
    }

    [RelayCommand]
    public async Task AddProduct()
    {
        if (string.IsNullOrEmpty(Product.Title) || Product.Price <= 0 || string.IsNullOrEmpty(Product.Store) || string.IsNullOrEmpty(Product.DateBought))
        {
            await Shell.Current.DisplayAlert("Warning", "One or more fields are empty or have an illogical value.", "OK");
            return;
        }
        else
        {
            IsBusy= true;
            DateTime extractDateBought = DateTime.Parse(Product.DateBought);
            DateTime onlyDate = extractDateBought.Date;
            var addProductQuery = await productService.AppendProduct(new Product
            {
                GroceryInventoryID = Inventory.GroceryInventoryID,
                Title = Product.Title,
                Price = Product.Price,
                Store = Product.Store,
                DateBought = onlyDate.ToString("yyy/MM/dd")
            });
            if (addProductQuery == true)
            {
                IsBusy = false;
                await Shell.Current.DisplayAlert("Successfull operation", $"The {Product.Title} item has been added to {Inventory.GroceryInventoryName} inventory.", "OK");
            }
            else
            {
                IsBusy = false;
                await Shell.Current.DisplayAlert("Error", "Could not add the item to the inventory due to some technical issues.", "OK");
            }
        }
            
    }
}

