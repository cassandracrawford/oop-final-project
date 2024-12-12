# Final Project for CPRG211
## Project Overview
The Village Rental Project is a comprehensive equipment rental application designed to simplify the process of managing customers, equipment, and rental records. This application provides an efficient and user-friendly interface for administrators and staff to manage a rental business. It leverages the power of .NET MAUI Blazor Hybrid for cross-platform functionality and integrates with a SQL Server Management Studio database for reliable and scalable data management.
### Functionalities
The application includes the following features:
1. Login System
   - Secure login functionality to authenticate users.
3. Customer Management
   - Add Customer: Allows staff to add new customers to the database.
   - Update Customer: Enables editing of customer information.
   - Delete Customer: Permits the removal of customer records.
   - Search Customer: Quickly locate a customer by name or ID.
3. Equipment Management
   - Add Equipment: Add new equipment to the inventory, specifying details like name, category, description, and daily rate.
   - Search Equipment: Locate equipment in the database by name, ID, or category.
   - Delete Equipment: Remove equipment from the inventory.
   - Add Category: Create new categories for organizing equipment.
4. Rental Process
   - Check Customer Ban Status: Prevents renting equipment to banned customers.
   - Create Rental Record: Logs rental transactions, including customer details, equipment rented, and associated costs.
   - Update Equipment Status: Automatically updates the equipment status to 'On Rent' in the database
   - Calculate Rental Cost: Automatically computes the rental cost based on predefined rates and rental duration.
5. Reports Page
   - View Rental Reports: Displays a list of rental transactions, including details such as Rental ID, Rental Date, Return Date, Equipment Details, and Rental Cost.
   - Filter by Date: Allows users to filter reports by selecting a specific date range.
6. Database Integration
   - Fully integrated with SQL Server Management Studio for efficient data handling.
   - Ensures data consistency and reliability.
### Problem It Solves
Managing an equipment rental business often involves handling multiple processes and data sources. This project addresses the following challenges:
1. Inefficient Manual Processes
   - Eliminates the need for paper records and manual data entry.
   - Speeds up the rental process with automated workflows.
2. Data Management
   - Centralizes all customer, equipment, and rental records in a single, secure database.
   - Ensures data accuracy and reduces redundancy.
3. Customer Validation
   - Automatically flags banned customers, reducing the risk of renting to problematic individuals.
4. Rental Cost Calculation
   - Automates the calculation of rental costs minimizing errors.
5. Tracking Rental Transactions
   - The Reports Page provides a summary of all rental transactions, making it easy to track and review past rentals.
