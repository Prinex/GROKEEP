using System.Collections.ObjectModel;

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
    public async Task DeleteInventory()
    {
        
    }

    [RelayCommand]
    public async Task EditInventory(GroceryInventory inventory)
    {
        string newInventoryName = await Shell.Current.DisplayPromptAsync($"Edit {inventory.GroceryInventoryName} inventory", "Edit or insert a new name for the inventory.");

    }

    [RelayCommand]
    public async Task AddInventoryProduct()
    {
        // navigation with query property to another page another 
    }
}
