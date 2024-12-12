using Microsoft.Data.SqlClient;
using System.Data;
using FinalProject.Data.Models;

namespace FinalProject.Data
{
    public class DatabaseConnection
    {
        // Connection string to the database
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=VillageRental;Integrated Security=True";

        // To return a new SQLConnection object (only for reusability on other DB-related methods)
        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        // To validate the employee login details 
        public bool Login(Employee employee)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT Count(*) FROM Employee WHERE employee_ID = @EmployeeID AND password = @EmployeePW";

                SqlCommand cmd = new SqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@EmployeeID", employee.EmployeeID);
                cmd.Parameters.AddWithValue("@EmployeePW", employee.Password);

                // Execute query to check if there will be an employee that would match the login details from DB
                int result = (int)cmd.ExecuteScalar();

                // Returns true if employee count is > 0 and false if 0
                return result > 0;
            }
        }

        // To retrieve all customers from the database
        public List<Customer> GetCustomers()
        {
            List<Customer> customers = new List<Customer>();

            using (SqlConnection connection = GetConnection())
            {
                connection.Open();

                string query = "SELECT * FROM customer";
                SqlCommand cmd = new SqlCommand(query, connection);

                // Read customer data from DB and create Customer object and add to list
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        customers.Add(new Customer
                        {
                            CustomerID = reader.GetInt32(0),
                            LastName = reader.GetString(1),
                            FirstName = reader.GetString(2),
                            PhoneNum = reader.GetString(3),
                            EMail = reader.GetString(4),
                            ClientStatus = reader.GetString(5),
                            Discount = (float)reader.GetDouble(6),
                        });
                    }
                }
            }
            return customers;
        }

        // To add a new client to the database
        public void AddClient(Customer client)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();

                string query = @"INSERT INTO Customer (last_name, first_name, contact_phone, email, customer_status, customer_discount) 
                               VALUES (@LastName, @FirstName, @PhoneNum, @EMail, @ClientStatus, @Discount)";

                SqlCommand cmd = new SqlCommand(query, connection);

                // Add parameters for the query
                cmd.Parameters.AddWithValue("@LastName", client.LastName);
                cmd.Parameters.AddWithValue("@FirstName", client.FirstName);
                cmd.Parameters.AddWithValue("@PhoneNum", client.PhoneNum);
                cmd.Parameters.AddWithValue("@EMail", client.EMail);
                cmd.Parameters.AddWithValue("@ClientStatus", string.IsNullOrEmpty(client.ClientStatus) ? "Active" : client.ClientStatus);
                cmd.Parameters.AddWithValue("@Discount", client.Discount);

                // Execute the query and add the client to the DB
                cmd.ExecuteNonQuery();
            }
        }

        // Updates existing customer information in the database
        public void UpdateClient(Customer client)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();

                string query = @"UPDATE Customer 
                               SET last_name = @LastName, 
                                   first_name = @FirstName, 
                                   contact_phone = @PhoneNum, 
                                   email = @EMail, 
                                   customer_status = @ClientStatus, 
                                   customer_discount = @Discount
                               WHERE customer_id = @CustomerID";

                SqlCommand cmd = new SqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@CustomerID", client.CustomerID);
                cmd.Parameters.AddWithValue("@LastName", client.LastName);
                cmd.Parameters.AddWithValue("@FirstName", client.FirstName);
                cmd.Parameters.AddWithValue("@PhoneNum", client.PhoneNum);
                cmd.Parameters.AddWithValue("@EMail", client.EMail);
                cmd.Parameters.AddWithValue("@ClientStatus", client.ClientStatus);
                cmd.Parameters.AddWithValue("@Discount", client.Discount);

                cmd.ExecuteNonQuery();
            }
        }

        // Deletes a customer from the database
        public void DeleteClient(int clientID)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();

                string query = "DELETE FROM Customer WHERE customer_id = @CustomerID";

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@CustomerID", clientID);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Check if a customer is banned from renting
        /// </summary>
        public bool CheckCustomerBanned(string customerId)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT customer_status FROM customer WHERE customer_id = @CustomerId";

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@CustomerId", customerId);

                var status = cmd.ExecuteScalar() as string;
                return status?.ToLower() == "banned";
            }
        }

        public bool CheckCustomerValid(string customerId)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();

                string query = "SELECT customer_id FROM customer WHERE customer_id = @CustomerId";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@CustomerId", customerId);

                // Use SqlDataReader to check if a record exists
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // Return true if a record is found
                    return reader.HasRows;
                }
            }
        }

        /// <summary>
        /// Check if equipment is available for rental
        /// </summary>
        public bool CheckEquipmentAvailable(string equipmentId)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT status FROM Equipment WHERE equipment_id = @EquipmentId";

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@EquipmentId", equipmentId);

                var status = cmd.ExecuteScalar() as string;
                return status?.ToLower() == "available";
            }
        }

        /// <summary>
        /// Get the daily rental rate for specified equipment
        /// </summary>
        public decimal GetEquipmentDailyRate(string equipmentId)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT daily_rental_cost FROM Equipment WHERE equipment_id = @EquipmentId";

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@EquipmentId", equipmentId);

                var result = cmd.ExecuteScalar();
                return result != null ? Convert.ToDecimal(result) : 0m;
            }
        }

        /// <summary>
        /// Create a new rental record and update equipment status
        /// </summary>
        public bool CreateRentalRecord(string customerId, string equipmentId, DateTime rentalDate, DateTime returnDate, decimal cost)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Create rental record
                        string rentalQuery = @"
                            INSERT INTO Rentals (customer_id, equipment_id, rental_date, return_date, total_cost)
                            VALUES (@CustomerId, @EquipmentId, @RentalDate, @ReturnDate, @Cost);
                            SELECT SCOPE_IDENTITY();";

                        using (SqlCommand cmd = new SqlCommand(rentalQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@CustomerId", customerId);
                            cmd.Parameters.AddWithValue("@EquipmentId", equipmentId);
                            cmd.Parameters.AddWithValue("@RentalDate", rentalDate);
                            cmd.Parameters.AddWithValue("@ReturnDate", returnDate);
                            cmd.Parameters.AddWithValue("@Cost", cost);

                            var rentalId = Convert.ToInt32(cmd.ExecuteScalar());

                            // Update equipment status
                            string updateQuery = "UPDATE Equipment SET status = 'On Rent' WHERE equipment_id = @EquipmentId";
                            using (SqlCommand updateCmd = new SqlCommand(updateQuery, connection, transaction))
                            {
                                updateCmd.Parameters.AddWithValue("@EquipmentId", equipmentId);
                                updateCmd.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// Get all available equipment
        /// </summary>
        public DataTable GetAllEquipment()
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT * FROM Equipment WHERE status = 'available'";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    var dataTable = new DataTable();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                    return dataTable;
                }
            }
        }



        // Get rental report data based on date range
        public List<Models.ReportItem> GetRentalReport(DateTime startDate, DateTime endDate)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                string query = @"
            SELECT 
                r.rental_id,
                r.rental_date,
                r.return_date,
                e.name as equipment_name,
                r.total_cost
            FROM Rentals r
            JOIN Equipment e ON r.equipment_id = e.equipment_id
            WHERE r.rental_date BETWEEN @StartDate AND @EndDate
            ORDER BY r.rental_date DESC";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@StartDate", startDate);
                    cmd.Parameters.AddWithValue("@EndDate", endDate);

                    var results = new List<ReportItem>();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(new ReportItem
                            {
                                RentalId = reader.GetInt32(0),
                                RentalDate = reader.GetDateTime(1),
                                ReturnDate = reader.GetDateTime(2),
                                EquipmentDetails = reader.GetString(3),
                                RentalCost = reader.GetDecimal(4)
                            });
                        }
                    }
                    return results;
                }
            }
        }

        // To calculate total revenue
        public decimal GetTotalRevenue()
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT SUM(total_cost) FROM Rentals";

                SqlCommand cmd = new SqlCommand(query, connection);

                // Execute query to return the totalal revenue
                decimal result = (decimal)cmd.ExecuteScalar();

                // Returns the sum of the total_cost of each rentals
                return result;
            }
        }

        // To get the count of clients
        public int GetTotalClients()
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Customer";

                SqlCommand cmd = new SqlCommand(query, connection);

                // Execute query to return the totalal revenue
                int result = (int)cmd.ExecuteScalar();

                // Returns the sum of the total_cost of each rentals
                return result;
            }
        }

        // To get the count of active rentals
        public int GetActiveRentals()
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Rentals WHERE return_date > GETDATE()";

                SqlCommand cmd = new SqlCommand(query, connection);

                // Execute query to return the totalal revenue
                int result = (int)cmd.ExecuteScalar();

                // Returns the sum of the total_cost of each rentals
                return result;
            }
        }

        // To get the count of avaialble equipments
        public int GetTotalAvailEquip()
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Equipment WHERE status = 'Available'";

                SqlCommand cmd = new SqlCommand(query, connection);

                // Execute query to return the totalal revenue
                int result = (int)cmd.ExecuteScalar();

                // Returns the sum of the total_cost of each rentals
                return result;
            }
        }

        // To get the total sales per day in the last 7 days 
        public List<decimal> GetDailyTotalCost()
        {
            var dailySales = new List<decimal>();

            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                Console.WriteLine("Connection opened.");

                string query = "WITH Last7Days AS (SELECT CAST(DATEADD(DAY, -n, GETDATE()) AS DATE) AS RentalDate " +
                    "FROM (VALUES (0), (1), (2), (3), (4), (5), (6)) AS T(n)) " +
                    "SELECT COALESCE(SUM(r.total_cost), 0) AS TotalCost " +
                    "FROM Last7Days d LEFT JOIN Rentals r ON CAST(r.rental_date AS DATE) = d.RentalDate " +
                    "GROUP BY d.RentalDate " +
                    "ORDER BY d.RentalDate";

                SqlCommand cmd = new SqlCommand(query, connection);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("Query executed. Reading results...");

                    while (reader.Read())
                    {
                        decimal totalCost = reader.GetDecimal(0); 
                        Console.WriteLine($"TotalCost: {totalCost}");

                        dailySales.Add(totalCost);
                    }
                }
            }
            return dailySales;
        }
    }
}