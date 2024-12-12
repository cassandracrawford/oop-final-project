using FinalProject.Data.Models;
using Microsoft.Data.SqlClient;

namespace FinalProject.Data;

public class CategoryService
{
    SqlConnection _cnn;
    public CategoryService(DatabaseConnection dbConnection)
    {
        _cnn = dbConnection.GetConnection();
    }

    // Saves the new Category to the database
    public Category? SaveNewCategory(string categoryName)
    {
        SqlCommand? cmd = null;
        try
        {
            _cnn.Open();
            String sql = @"INSERT INTO Category (name) 
                        OUTPUT INSERTED.category_id 
                        VALUES (@CategoryName)";

            cmd = new SqlCommand(sql, _cnn);
            cmd.Parameters.AddWithValue("@CategoryName", categoryName);
            int newCategoryId = (int)cmd.ExecuteScalar();

            return new Category
            {
                CategoryId = newCategoryId,
                Name = categoryName
            }; 
        }
        catch
        {
            // If error occurs, return null
            throw;
        }
        finally
        {
            cmd?.Dispose();
            _cnn.Close();
        }

    }

    // Fetches Category by ID from database
    public Category GetCategoryById(int categoryId)
    {

        Category category = new();
        _cnn.Open();

        SqlCommand sqlCommand;
        SqlDataAdapter adapter = new SqlDataAdapter();
        String sql = "SELECT * FROM category WHERE category_id = @CategoryId";

        sqlCommand = new SqlCommand(sql, _cnn);
        sqlCommand.Parameters.AddWithValue("@CategoryId", categoryId);
        SqlDataReader reader = sqlCommand.ExecuteReader();
        while (reader.Read())
        {
            category.CategoryId = reader.GetInt32(0);
            category.Name = reader.GetString(1);
        }

        sqlCommand.Dispose();
        _cnn.Close();

        return category;
    }

    // Fetches Category Name by ID from database
    public string GetCategoryNameById(int categoryId)
    {
        string categoryName = "";
        _cnn.Open();
        SqlCommand sqlCommand;
        String sql = @"SELECT [name] FROM Category 
                        WHERE category_id = @categoryId";
        sqlCommand = new SqlCommand(sql, _cnn);
        sqlCommand.Parameters.AddWithValue("@categoryId", categoryId);
        categoryName= (string) sqlCommand.ExecuteScalar();

        sqlCommand.Dispose();
        _cnn.Close();
        return categoryName;
    }

    // Fetches all Categories from the database
    public List<Category> GetCategories()
    {
        List<Category> categories = new();
        _cnn.Open();

        SqlCommand sqlCommand;
        SqlDataAdapter adapter = new SqlDataAdapter();
        String sql = "SELECT * FROM category";

        sqlCommand = new SqlCommand(sql, _cnn);
        SqlDataReader reader = sqlCommand.ExecuteReader();
        while (reader.Read()) {
            var category = new Category
            {
                CategoryId = reader.GetInt32(0),
                Name = reader.GetString(1)
            };
            categories.Add(category);
        }

        sqlCommand.Dispose();
        _cnn.Close();

        return categories;
    }
}
