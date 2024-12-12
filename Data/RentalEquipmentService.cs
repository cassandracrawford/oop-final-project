using FinalProject.Data.Models;
using Microsoft.Data.SqlClient;

namespace FinalProject.Data;

public class RentalEquipmentService
{
    private readonly SqlConnection _cnn;
    private readonly CategoryService categoryService;

    public RentalEquipmentService(DatabaseConnection dbConnection, CategoryService categoryService)
    {
        //_cnn = cnn;
        _cnn = dbConnection.GetConnection();
        this.categoryService = categoryService;
    }

    // Adds new equipment to the database
    public bool AddNewEquipment(RentalEquipment re)
    {
        SqlCommand cmd = null;
        try
        {
            _cnn.Open();
            string sql = @"INSERT INTO Equipment 
                        (equipment_id, name, description, daily_rental_cost, category_id, status) 
                        VALUES (@EquipmentId, @Name, @Description, @DailyRate, @CategoryId, @Status)";

            using (cmd = new SqlCommand(sql, _cnn))
            {
                cmd.Parameters.AddWithValue("@EquipmentId", re.EquipmentId);
                cmd.Parameters.AddWithValue("@Name", re.Name);
                cmd.Parameters.AddWithValue("@Description", re.Description);
                cmd.Parameters.AddWithValue("@DailyRate", re.DailyRate);
                cmd.Parameters.AddWithValue("@CategoryId", re.CategoryId);
                cmd.Parameters.AddWithValue("@Status", re.Status);

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        } 
        catch (Exception ex)
        {
            return false;
        }
        finally
        {
            if (cmd != null)
            {
                cmd.Dispose();
            }
            _cnn.Close();
        }
    }

    // Fetches ALL Rental Equipments from the database
    public List<RentalEquipment> GetRentalEquipments()
    {
        List<RentalEquipment> rEquipments = new();
        _cnn.Open();

        SqlCommand sqlCommand;
        String sql = "SELECT * FROM Equipment";

        sqlCommand = new SqlCommand(sql, _cnn);
        SqlDataReader reader = sqlCommand.ExecuteReader();
        while (reader.Read())
        {
            var re = new RentalEquipment
            {
                EquipmentId = reader.GetInt32(0),
                Category = categoryService.GetCategoryById(reader.GetInt32(1)),
                Name = reader.GetString(2),
                Description = reader.GetString(3),
                DailyRate = reader.GetDecimal(4),
                Status = reader.GetString(5),
            };
            rEquipments.Add(re);
        }

        sqlCommand.Dispose();
        _cnn.Close();

        return rEquipments;
    }

    // Removes a Rental Equipment by ID from the database
    public void RemoveRentalEquipmentById(int equipmentId)
    {
        _cnn.Open();
        String sql = @"DELETE FROM Equipment WHERE equipment_id = @equipmentId";

        SqlCommand cmd = new SqlCommand(sql, _cnn);
        cmd.Parameters.AddWithValue("@equipmentId", equipmentId);
        cmd.ExecuteNonQuery();

        _cnn.Close();
    }
}
