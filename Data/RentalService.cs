using System.Data;
using FinalProject.Data.Models;

namespace FinalProject.Data
{
    /// <summary>
    /// Service class for handling rental operations
    /// This class acts as a bridge between the UI and database operations
    /// </summary>
    public class RentalService
    {
        private readonly DatabaseConnection _dbConnection;

        /// <summary>
        /// Constructor: Inject database connection dependency
        /// </summary>
        public RentalService(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        /// <summary>
        /// Get list of all available equipment from database
        /// </summary>
        public DataTable GetAvailableEquipment()
        {
            return _dbConnection.GetAllEquipment();
        }

        /// <summary>
        /// Get list of all customers from database
        /// </summary>
        public List<Customer> GetAllCustomers()
        {
            return _dbConnection.GetCustomers();
        }

        /// <summary>
        /// Validates if a customer is allowed to rent equipment
        /// </summary>
        /// <param name="customerId">The ID of the customer to check</param>
        /// <returns>true if customer can rent, false if banned or not found</returns>
        public bool CheckCustomerStatus(string customerId)
        {
            // Input validation - return false if customerId is empty
            if (string.IsNullOrEmpty(customerId))
            {
                return false;
            }

            try
            {
                // Check if the customer ID exists in the database
                if (!_dbConnection.CheckCustomerValid(customerId))
                {
                    return false; // Return false if the customer ID does not exist
                }

                // Returns true if customer exists and is not banned
                return !_dbConnection.CheckCustomerBanned(customerId);
            }
            catch (Exception)
            {
                // Return false in case of any database errors
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if specific equipment is available for rental
        /// </summary>
        /// <param name="equipmentId">The ID of the equipment to check</param>
        /// <returns>true if equipment is available, false otherwise</returns>
        public bool CheckEquipmentAvailability(string equipmentId)
        {
            // Input validation - return false if equipmentId is empty
            if (string.IsNullOrEmpty(equipmentId))
            {
                return false;
            }

            try
            {
                return _dbConnection.CheckEquipmentAvailable(equipmentId);
            }
            catch (Exception)
            {
                // Return false in case of any database errors
                return false;
            }
        }

        /// <summary>
        /// Process a new rental request
        /// </summary>
        /// <param name="customerId">Customer ID requesting the rental</param>
        /// <param name="equipmentId">Equipment ID to be rented</param>
        /// <param name="rentalDate">Start date of rental</param>
        /// <param name="returnDate">Expected return date</param>
        /// <returns>true if rental is created successfully, false otherwise</returns>
        public bool CreateRental(string customerId, string equipmentId, DateTime rentalDate, DateTime returnDate)
        {
            // Validate input parameters
            if (string.IsNullOrEmpty(customerId) || string.IsNullOrEmpty(equipmentId))
            {
                return false;
            }

            // Validate dates
            if (rentalDate > returnDate || rentalDate < DateTime.Today)
            {
                return false;
            }

            try
            {
                // Step 1: Verify customer is allowed to rent
                if (!CheckCustomerStatus(customerId))
                {
                    return false;
                }

                // Step 2: Verify equipment is available
                if (!CheckEquipmentAvailability(equipmentId))
                {
                    return false;
                }

                // Step 3: Get the daily rental rate
                decimal dailyRate = _dbConnection.GetEquipmentDailyRate(equipmentId);
                if (dailyRate <= 0)
                {
                    return false;
                }

                // Step 4: Calculate total cost
                var rentalDetail = new RentalDetail
                {
                    CustomerID = customerId,
                    EquipmentID = equipmentId,
                    RentalDate = rentalDate,
                    ReturnDate = returnDate
                };

                decimal totalCost = rentalDetail.CalculateCost(dailyRate);

                // Step 5: Create rental record in database
                bool success = _dbConnection.CreateRentalRecord(
                    customerId,
                    equipmentId,
                    rentalDate,
                    returnDate,
                    totalCost
                );

                return true;
            }
            catch (Exception)
            {
                // Return false for any unexpected errors
                return false;
            }
        }

        /// <summary>
        /// Calculate the total number of days for a rental period
        /// </summary>
        /// <param name="startDate">Start date of rental</param>
        /// <param name="endDate">End date of rental</param>
        /// <returns>Total number of days including start and end dates</returns>
        private int CalculateRentalDays(DateTime startDate, DateTime endDate)
        {
            return (int)(endDate - startDate).TotalDays + 1;
        }
    }
}