namespace Grokeep.ViewModels;

public partial class GroceryStatisticsPageViewModel : BaseViewModel
{
    private readonly IGroceryHistoryService historyService;

    // dictionary is used for preprocessing the data 
    // ignore case sensitivity for key when check if a key exists
    public Dictionary<string, double> TotalExpensesByStore { get; set; } = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);

    // then transfer the data to an observable collection for binding it to the chart
    public ObservableCollection<KeyValuePair<string, double>> Expenses { get; set; } = new ObservableCollection<KeyValuePair<string, double>>();

    // then it will be transfered to a collection
    public GroceryStatisticsPageViewModel(IGroceryHistoryService historyService)
    {
        this.historyService = historyService;
        //InitChart();
    }

    // initialization method for the dicionary
    // USING a dictionary get the total expenses spent at each store
    // store is the key (x) and total expense is value (y)
    public async Task InitChart()
    { 
        TotalExpensesByStore.Clear();
        Expenses.Clear();

        if (IsBusy == true)
            return;

        IsBusy = true;
        var historyProducts = await historyService.RetrieveHistory(App.UserSessionData.AccountID);

        if (historyProducts.Count == 0)
        {
            await Shell.Current.DisplayAlert("Warning", "Cannot initialize chart because there is no data history.", "OK");
            return;
        }
        else
        {
            historyProducts = historyProducts.ToList();
            foreach (var product in historyProducts)
            {
                if (TotalExpensesByStore.ContainsKey(product.Location.ToLower()))
                {
                    TotalExpensesByStore[product.Location] += product.Cost;
                }
                else
                {
                    TotalExpensesByStore.Add(product.Location, product.Cost);
                }
            }
            foreach(var expense in TotalExpensesByStore)
            {
                Expenses.Add(new KeyValuePair<string, double>(expense.Key, expense.Value));
            }
        }
        IsBusy = false;
    }
}

