# Microservices Implementation for E-commerce

## 1. Architecture Overview

### Microservices Pattern
The system utilizes a microservices architecture, where different services handle distinct responsibilities. In this case, the `ProductController` handles two responsibilities:

1. **Product Data**: Fetching details about a specific product.
2. **Product Inventory Data**: Fetching inventory details across various shops for a given product.

The application is built using **ASP.NET Core** with **Dapper** as the data access layer.

## 2. Endpoints

### A. Get Product Information
- **Endpoint**: `/Product/GetProduct/{p_code}`
- **Description**: Fetches a product’s information based on its product code (`p_code`).
- **Return Type**: `Task<ActionResult<ProductModel>>`
  
  **Code Flow**:
  - Accepts a product code (`p_code`), connects to the database using Dapper, and runs a query to fetch the product from the `Products` table.
  - Returns the product data or logs the error if the product does not exist.

### B. Get Product Inventory Information
- **Endpoint**: `/Product/GetInventory/{p_code}`
- **Description**: Fetches the inventory details for a product across different shops.
- **Return Type**: `Task<ActionResult<List<InventoryDataModelByShops>>>`
  
  **Code Flow**:
  - Queries multiple tables like `ProductItems`, `Products`, `ProductInventory`, `ProductCombinations`, and `Shops` to gather comprehensive product inventory data.
  - Returns inventory data grouped by shop with applicable discounts.

## 3. Tables Involved

1. **Products**: Stores product-level information like name, ID, and code.
2. **ProductItems**: Contains product-specific variants or items.
3. **ProductInventory**: Stores inventory data for the products across shops.
4. **ProductCombinations**: Defines product combinations such as sizes or colors.
5. **Shops**: Stores information about different shops where the product is available.
6. **ProductBasedDiscount**: Stores shop-specific discounts for products.

## 4. Technologies Used

1. **.NET Core (ASP.NET Core)**: For building REST APIs with clean architecture.
2. **Dapper**: A micro-ORM for mapping SQL queries to C# objects.
3. **SQL Server**: The backend database.
4. **IConfiguration & ILogger**: Standard dependency injection services in ASP.NET Core for logging and configuration management.

## 5. Error Handling and Logging

The `ILogger<ProductController>` is injected into the controller to log errors and trace data. For example:
```csharp
_logger.LogError($"Error: {ex.Message + ex.InnerException + ex.StackTrace}");
6. Best Practices
Separation of Concerns: The controller handles only the API logic, while Dapper executes queries to fetch data.
Dependency Injection: The IConfiguration and ILogger are injected via the constructor.
Task-Based Asynchronous Programming: Methods are asynchronous to ensure non-blocking I/O operations.
7. CRUD Operations
Create Product: Add a new product to the Products table.
Update Product: Update the details of an existing product.
Delete Product: Remove a product from the system.
8. Product and Inventory Models
The ProductModel, ProductItemModel, InventoryDataModelByShops, and ProductCombination classes represent the structure of the data returned by the database queries.

9. Sample Usage
To fetch a product with a product code P123:

arduino
Copy code
GET http://localhost/Product/GetProduct/P123
Response:

json
Copy code
{
  "ProductID": 1,
  "ProductName": "Example Product",
  "ProductCode": "P123"
}
To fetch inventory details for a product with code P123:

arduino
Copy code
GET http://localhost/Product/GetInventory/P123
Response:

json
Copy code
[
  {
    "Inv_shopID": 1,
    "Inv_shopName": "Shop 1",
    "InventoryDataModel": [
      {
        "ProductCombinationID": 1001,
        "ProductCombinationName": "Size L",
        "ProductItemModel": [
          {
            "ProductID": 1,
            "ProductItemName": "Product Item 1",
            "Quantity": 50
          }
        ]
      }
    ],
    "DiscountByShop": {
      "ShopID": 1,
      "DiscountValue": 10
    }
  }
]
10. Possible Improvements
Pagination: Implement pagination for large datasets.
Caching: Use caching mechanisms to reduce database load.
Exception Handling: Add more granular error handling.
