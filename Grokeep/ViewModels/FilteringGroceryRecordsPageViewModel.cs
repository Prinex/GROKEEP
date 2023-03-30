namespace Grokeep.ViewModels;

public partial class FilteringGroceryRecordsPageViewModel : BaseViewModel
{
    // filtering items by dates (ascending descending ) and by price of item (highest - lowest
    // make use of the return type when getting all items of the user : there are some associated functions that you can use
    // after getting all of these items
    private readonly IGroceryHistoryService historyService;

    [ObservableProperty]
    public string itemKeyword;

    [ObservableProperty]
    public string priceOrder;

    [ObservableProperty]
    public string storeLocation;

    [ObservableProperty]
    public string dateOrder;

    public ObservableCollection<GroceryHistory> ProductsHistory { get; set; } = new ObservableCollection<GroceryHistory>();

    public FilteringGroceryRecordsPageViewModel(IGroceryHistoryService historyService)
    {
        this.historyService = historyService;
    }

    public async Task GetProductsHistory()
    {
        ProductsHistory.Clear();
        IsBusy = true;
        var productsHistoryDB = await historyService.RetrieveHistory(App.UserSessionData.AccountID);

        if (productsHistoryDB?.Count > 0)
        {
            productsHistoryDB = productsHistoryDB.OrderBy(i => i.HistoryID).ToList();
            foreach (var product in productsHistoryDB)
            {
                ProductsHistory.Add(product);
            }
        }
        IsBusy = false;
    }

    [RelayCommand]
    public async Task ClearHistory()
    {
        if (IsBusy == true)
            return;

        var deleteInventoryConfirmation = await Shell.Current.DisplayActionSheet($"Are you sure you want to clear the history?", "No", null, "Yes");
        if (deleteInventoryConfirmation == "Yes")
        {
            IsBusy = true;
            var historyProducts = await historyService.RetrieveHistory(App.UserSessionData.AccountID);

            if (historyProducts.Count == 0)
            {
                await Shell.Current.DisplayAlert("Warning", "There is no history to clear out.", "OK");
                return;
            }

            for (int i = 0; i < historyProducts.Count; i++)
            {
                var removeProductResponse = await historyService.RemoveHistory(historyProducts[i]);
                if (removeProductResponse == false)
                {
                    IsBusy = false;
                    await Shell.Current.DisplayAlert("Error", $"Issues occured while trying to clear the history.", "OK");
                    return;
                }
            }
            // update the page history
            IsBusy = false;
            ItemKeyword = "";
            PriceOrder = "";
            DateOrder = "";
            StoreLocation = "";
            await GetProductsHistory();
        }
    }

    [RelayCommand]
    public async Task FilterHistory()
    {
        if (string.IsNullOrEmpty(ItemKeyword) && string.IsNullOrEmpty(StoreLocation) && string.IsNullOrEmpty(PriceOrder) && string.IsNullOrEmpty(DateOrder))
        {
            await Shell.Current.DisplayAlert("Warning", "There are no filtering options selected.", "OK");
            return;
        }
        else
        {
            if (IsBusy == true)
                return;
            
            IsBusy = true;

            // doing filtering with a combination of only if staments 
            ProductsHistory.Clear();

            var productsHistoryDB = await historyService.RetrieveHistory(App.UserSessionData.AccountID);

            // first see if there is any history
            if (productsHistoryDB?.Count < 0)
            {
                await Shell.Current.DisplayAlert("Warning", "There is no history.", "OK");
                return;
            }


            // 1. filter by item keyword
            if (!string.IsNullOrEmpty(ItemKeyword))
            {
                productsHistoryDB = productsHistoryDB.FindAll(k => k.Description.ToLower() == ItemKeyword.ToLower() || k.Description.ToLower().Contains(ItemKeyword.ToLower()) == true).ToList();
                // loop through the history and add all items to the collection view
                foreach (var product in productsHistoryDB)
                {
                    ProductsHistory.Add(product);
                }
                ItemKeyword = "";
            }

            // 2. filter by store: select items bought from a specific store
            if (!string.IsNullOrEmpty(StoreLocation))
            {
                ProductsHistory.Clear();
                productsHistoryDB = productsHistoryDB.FindAll(k => k.Location.ToLower() == StoreLocation.ToLower() || k.Location.ToLower().Contains(StoreLocation.ToLower()) == true).ToList();
                
                foreach (var product in productsHistoryDB)
                {
                    ProductsHistory.Add(product);
                }
                StoreLocation = "";
            }

            // 3.filter by price order(asc - desc)
            if (!string.IsNullOrEmpty(PriceOrder))
            {
                ProductsHistory.Clear();
                if (PriceOrder == "Ascending")
                {
                    productsHistoryDB = productsHistoryDB.OrderBy(p => p.Cost).ToList();
                }
                if (PriceOrder == "Descending")
                {
                    productsHistoryDB = productsHistoryDB.OrderByDescending(p => p.Cost).ToList();
                }
                
                foreach (var product in productsHistoryDB)
                {
                    ProductsHistory.Add(product);
                }
                PriceOrder = "";
            }

            // 4.filter by date order(asc - desc)
            if (!string.IsNullOrEmpty(DateOrder))
            {
                ProductsHistory.Clear();
                if (DateOrder == "Ascending")
                {
                    productsHistoryDB = productsHistoryDB.OrderBy(p => p.DateBought).ToList();
                }
                if (DateOrder == "Descending")
                {
                    productsHistoryDB = productsHistoryDB.OrderByDescending(p => p.DateBought).ToList();
                }

                foreach (var product in productsHistoryDB)
                {
                    ProductsHistory.Add(product);
                }
                DateOrder = "";
            }

            // if there are no matches 
            if (productsHistoryDB.Count == 0)
                await Shell.Current.DisplayAlert("Results", "There are no matching results.", "OK");
            IsBusy = false;
        }
    }
}

