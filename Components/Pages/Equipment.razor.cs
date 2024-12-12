using MudBlazor;
using FinalProject.Data;
using FinalProject.Data.Models;
using FinalProject.Components.Dialogs;
using Microsoft.AspNetCore.Components;

namespace FinalProject.Components.Pages;

public partial class Equipment
{
    public string? _searchValue { get; set; } = "";
    private string selectedCategoriesText { get; set; }
    private Category selectedCategory { get; set; }
    private IEnumerable<string> selectedCategories { get; set; } = new HashSet<string>();

    private string selectedStatusText { get; set; }
    private IEnumerable<string> selectedStatuses { get; set; } = new HashSet<string>();

    private List<RentalEquipment> Equipments { get; set; }
    private List<RentalEquipment> SearchedEquipments { get; set; }
    private List<Category> Categories { get; set; }

    private RentalEquipment _newEquipment { get; set; } = new();

    private List<string> _events = new();

    private string[] statuses =
    {
        "Available",
        "On Rent"
    };

    private const string trash_can = @"<svg xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" fill=""currentColor"" class=""bi bi-trash-fill"" viewBox=""0 0 16 16"">
                                        <path d=""M2.5 1a1 1 0 0 0-1 1v1a1 1 0 0 0 1 1H3v9a2 2 0 0 0 2 2h6a2 2 0 0 0 2-2V4h.5a1 1 0 0 0 1-1V2a1 1 0 0 0-1-1H10a1 1 0 0 0-1-1H7a1 1 0 0 0-1 1zm3 4a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 .5-.5M8 5a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7A.5.5 0 0 1 8 5m3 .5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 1 0"" />
                                        </svg>";

    public Equipment()
    {
    }

    // Loads the table data on initialize
    protected override async Task OnInitializedAsync()
    {
        await LoadRentalEquipmentData(true);
    }

    // Async function to open the New Equipment (MudBlazor) Dialog
    private async Task OpenNewEquipmentDialogAsync()
    {
        _newEquipment = new();

        var parameters = new DialogParameters<AddNewEquipmentDialog>
        {
            { x => x.RentalEquipment, _newEquipment },
            { x => x.Categories, Categories },
            { x => x.OnNewCategoryAdded, EventCallback.Factory.Create(this, LoadCategories) }
        };

        var options = new DialogOptions
        {
            NoHeader = true,
            BackdropClick = false,
            //CloseOnEscapeKey = true, 
            //MaxWidth = MaxWidth.ExtraSmall, 
            //FullWidth = true 
        };
        var dialog = await DialogService.ShowAsync<AddNewEquipmentDialog>("Add New Equipment Dialog", parameters, options);
        var result = await dialog.Result;
        if(!result.Canceled)
        {
            await LoadRentalEquipmentData();
        }
    }

    // This function returns the 'Text Value' when selecting one or multiple categories
    private string GetSelectCategoriesMultiSelectionText(List<string> selectedValues)
    {
        if (selectedValues.Count == Categories.Count)
        {
            return "All Categories";
        }
        else if (selectedValues.Count < 2)
        {
            return selectedValues.FirstOrDefault() ?? string.Empty;
        }
        else
        {
            var firstOne = Categories.First(c => selectedValues.Contains(c.Name));
            return $"{firstOne.Name},  +{selectedValues.Count - 1} more";
        }
    }

    // This function returns the 'Text Value' when selecting one or both statuses
    private string GetSelectStatusMultiSelectionText(List<string> selectedValues)
    {
        return (selectedValues.Count == 2) 
            ? "All Statuses" 
            : selectedValues.FirstOrDefault() ?? string.Empty;
    }

    // Removes the equipment from the database if not on rent.
    // Otherwise, returns a Toast/Snackbar warning
    private async Task RemoveRentalEquipment(RentalEquipment re)
    {
        if (re.Status.Equals("Available"))
        {
            RentalEquipmentService.RemoveRentalEquipmentById(re.EquipmentId);

            Snackbar.Add("Rental equipment deleted.", Severity.Success);
            await LoadRentalEquipmentData();
        }
        else
        {
            Snackbar.Add("On rent. Cannot be deleted.", Severity.Warning);
        }

    }

    // This function is called on initialization
    // Calls the Service Function that Loads/Refreshes the data
    private async Task LoadRentalEquipmentData(bool loadCategories = false)
    {
        Equipments = RentalEquipmentService.GetRentalEquipments();
        SearchedEquipments = Equipments;
        if (loadCategories)
        {
            await LoadCategories();
        }
        StateHasChanged();
    }

    // Calls the Service Function that Loads/Refreshes the Categories data
    private async Task LoadCategories()
    {
        Categories = CategoryService.GetCategories();
        StateHasChanged();
    }

    // Bindable function that is triggered whenever search values are modified
    private void OnSelectedValuesChanged(IEnumerable<string> values, bool categorySelect = false)
    {
        if(categorySelect)
        {
            selectedCategories = values;
        }
        else
        {
            selectedStatuses = values;
        }
        SearchEquipments();
    }

    // Actual search function. Filters the table data
    private void SearchEquipments()
    {
        SearchedEquipments = Equipments
            .Where(re => re.Name.ToLower().Contains(_searchValue.ToLower()))
            .Where(re => !selectedCategories.Any() || selectedCategories.Contains(re.Category.Name))
            .Where(re => !selectedStatuses.Any() || selectedStatuses.Contains(re.Status))
            .ToList();
    }

}
