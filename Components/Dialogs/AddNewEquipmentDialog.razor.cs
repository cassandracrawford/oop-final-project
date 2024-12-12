using FinalProject.Data.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FinalProject.Components.Dialogs;

public partial class AddNewEquipmentDialog
{

    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; }

    [Parameter]
    public RentalEquipment RentalEquipment { get; set; }

    [Parameter]
    public List<Category> Categories { get; set; }

    [Parameter]
    public EventCallback OnNewCategoryAdded { get; set; }

    private readonly DialogOptions _newCategoryDialogOptions = 
        new() {
            //FullWidth = true,
        };

    private bool _addNewCategory;

    private string _newCategoryName { get; set; }

    // Function that toggles the opening/closing of 'Add New Category' Dialog
    private void ToggleOpen()
    {
        _addNewCategory = !_addNewCategory;
        if (_addNewCategory)
        {
            _newCategoryName = string.Empty;
        }
    }

    // Funtion is triggered on button click.
    // Calls the Service Function to add a new category to the database
    private async void SaveNewCategory()
    {
        Category? newCategory = CategoryService.SaveNewCategory(_newCategoryName);
        if (newCategory != null)
        {
            Snackbar.Add($"'{_newCategoryName}' category added!", Severity.Success);

            // Refresh categories in the parent
            if (OnNewCategoryAdded.HasDelegate)
            {
                await OnNewCategoryAdded.InvokeAsync();
            }
            Categories.Add(newCategory);
            _addNewCategory = false;
        }
        else
        {
            Snackbar.Add($"Failed to add new category", Severity.Error);
        }
    }


    // Funtion is triggered on button click.
    // Calls the Service Function to add a new equipment to the database
    private void SaveEquipment()
    {
        RentalEquipment.Status = "Available";
        bool isAdded = RentalEquipmentService.AddNewEquipment(RentalEquipment);
        if (isAdded)
        {
            Snackbar.Add($"Rental Equipment: {RentalEquipment.Name} successfully added!", Severity.Success);
            MudDialog.Close();
        }
        else
        {
            Snackbar.Add($"Rental Equipment: {RentalEquipment.Name} adding failed.", Severity.Error);
        }
    }

    // Closes the 'Add New Equipment' Dialog
    private void CloseAddEquipment() => MudDialog.Cancel();

    protected override async Task OnInitializedAsync()
    {
    }
}
