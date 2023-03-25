namespace Grokeep.ViewModels;

public partial class GroceryStatisticsPageViewModel : BaseViewModel
{
    private readonly IGroceryHistoryService historyService;

    public Dictionary<string, double> totalExpensesByStore { get; set; } = new Dictionary<string, double>();

    public GroceryStatisticsPageViewModel(IGroceryHistoryService historyService)
    {
        this.historyService = historyService;
        // initialization method for the dicionary
    }
    // USING a dictionary get the total expenses spent at each store
    // store is the key (x) and total expense is value (y)
}

