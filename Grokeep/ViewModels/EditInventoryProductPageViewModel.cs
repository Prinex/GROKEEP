namespace Grokeep.ViewModels;

[QueryProperty(nameof(Product), "Product")]
public partial class EditInventoryProductPageViewModel : BaseViewModel
{
    private readonly IProductService productService;

    [ObservableProperty]
    private Product product = new Product();

    public EditInventoryProductPageViewModel(IProductService productService)
    {
        this.productService = productService;
    }

    [RelayCommand]
    public async Task SaveProduct()
    {
        var currentProductDetail = await productService.RetrieveProduct(Product.GroceryInventoryID, Product.ProductID);
        if (currentProductDetail.DoesExist == true)
        {
            if (currentProductDetail.Title == Product.Title && currentProductDetail.Price == Product.Price && currentProductDetail.Store == Product.Store && currentProductDetail.DateBought == Product.DateBought)
            {
                await Shell.Current.DisplayAlert("Warning", "There are no updates to be made.", "OK");
            }
            else
            {
                IsBusy = true;
                DateTime extractDateBought = DateTime.Parse(Product.DateBought);
                DateTime onlyDate = extractDateBought.Date;
                Product.DateBought = onlyDate.ToString("yyy/MM/dd");
                var updateProductResponse = await productService.UpdateProduct(Product);
                
                if (updateProductResponse == true)
                {
                    IsBusy = false;
                    await Shell.Current.DisplayAlert("Successful operation", $"Current changes for {Product.Title} product have been made and saved.", "OK");
                }
                else
                {
                    IsBusy = false;
                    await Shell.Current.DisplayAlert("Error", $"Could not make changes for {Product.Title} due to some tehnical issues.", "OK");
                }
            }
        }
    }
}
