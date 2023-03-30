namespace Grokeep.ViewModels;

public partial class GroceryInventoriesPageViewModel : BaseViewModel
{
    private readonly IGroceryInventoryService inventoryService;
    private readonly IProductService productService;

    [ObservableProperty]
    private string inventoryName;

    public ObservableCollection<GroceryInventory> Inventories { get; set; } = new ObservableCollection<GroceryInventory>(); 

    public GroceryInventoriesPageViewModel(IGroceryInventoryService inventoryService, IProductService productService)
    {
        this.inventoryService = inventoryService;
        this.productService = productService;
    }

    public async Task GetInventories()
    {
        Inventories.Clear();
        IsBusy = true;
        var inventoriesDB = await inventoryService.RetrieveInventories(App.UserSessionData.AccountID);

        if (inventoriesDB?.Count > 0)
        {
            inventoriesDB = inventoriesDB.OrderByDescending(i => i.GroceryInventoryID).ToList();
            foreach (var inventory in inventoriesDB)
            {
                Inventories.Add(inventory);
            }
        }
        IsBusy = false;
    }

    [RelayCommand]
    public async Task CreateInventory()
    {
        if (IsBusy == true)
            return;
        
        if (string.IsNullOrEmpty(InventoryName))
        {
            await Shell.Current.DisplayAlert("Warning", "Cannot create an inventory with no name.", "OK");
        }    
        else
        {
            IsBusy = true;
            bool createInventoryResponse = await inventoryService.AppendInventory(new GroceryInventory
            {
                AccountID = App.UserSessionData.AccountID,
                GroceryInventoryName = InventoryName
            });

            if (createInventoryResponse == true)
            {
                IsBusy = false;
                await Shell.Current.DisplayAlert("Operation successfull", $"The {InventoryName} inventory has been created.", "OK");
                InventoryName = "";
                await GetInventories();
            }
            else
            {
                IsBusy = false;
                await Shell.Current.DisplayAlert("Error", $"Something went wrong while trying to create {InventoryName}.", "OK");
                InventoryName = "";
            }
        }
    }

    [RelayCommand]
    public async Task DeleteInventory(GroceryInventory inventory)
    {
        var deleteInventoryConfirmation = await Shell.Current.DisplayActionSheet($"Are you sure you want to delete {inventory.GroceryInventoryName} inventory?", "No", null, "Yes");
        if (deleteInventoryConfirmation == "Yes")
        {
            IsBusy = true;
            var inventoryProducts = await productService.RetrieveProducts(inventory.GroceryInventoryID);

            if (inventoryProducts.Count == 0)
            {
                await Shell.Current.DisplayAlert("Warning", "There is no history to clear out.", "OK");
                return;
            }
            for (int i = 0; i < inventoryProducts.Count; i++)
            {
                var removeProductResponse = await productService.DeleteProduct(inventoryProducts[i]);
                if (removeProductResponse == false)
                {
                    IsBusy = false;
                    await Shell.Current.DisplayAlert("Error", $"Issues occured while trying to delete the {inventory.GroceryInventoryName} inventory of items.", "OK");
                    return;
                }
            }
            var removeInventory = await inventoryService.RemoveInventory(inventory);
            if (removeInventory == true)
            {
                IsBusy = false;
                await GetInventories();
            }
            else
            {
                IsBusy = false;
                await Shell.Current.DisplayAlert("Error", $"Issues occured while trying to delete the {inventory.GroceryInventoryName} inventory of items.", "OK");
            }
        }
    }

    [RelayCommand]
    public async Task EditInventory(GroceryInventory inventory)
    {
        // can use this instead of query property to another page; there is only one field to be edited
        string newInventoryName = await Shell.Current.DisplayPromptAsync($"Edit {inventory.GroceryInventoryName} inventory", "Edit or insert a new name for the inventory.");
        if (string.IsNullOrEmpty(newInventoryName))
        {
            await Shell.Current.DisplayAlert("Warning", $"The {inventory.GroceryInventoryName} cannot be updated to an inventory with no name.", "OK");
        }
        else
        {
            IsBusy = true;
            inventory.GroceryInventoryName = newInventoryName;
            bool updateResponse = await inventoryService.UpdateInventory(inventory);
            IsBusy = false;

            if (updateResponse == true)
            {
                // update the collection view ui
                await GetInventories();
            }
            else
                await Shell.Current.DisplayAlert("Error", $"Issues occured while trying to update the {inventory.GroceryInventoryName} inventory.", "OK");
        }
    }

    [RelayCommand]
    public async Task GoToViewInventoryProductPage(GroceryInventory inventory)
    {
        // navigation with query property to another page another 
        var navigationQueryParameters = new Dictionary<string, object>();
        navigationQueryParameters.Add("Inventory", inventory);
        await Shell.Current.GoToAsync($"/{nameof(ViewInventoryProductsPage)}", navigationQueryParameters);
    }
}
