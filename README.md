# Warehouse Management

Warehouse Management is a system which helps people manage their warehouses and inventory.

## Prerequisites

* Node.js - v^18.19.1 or newer
* Angular -v^18.0.0 or newer
* SQL server 2022 (with MS SQL Management STudio)
* .Net 8

## Usage

1. Start the SQL server. 
Make server name = localhost and choose Windows Authentication (should be the default one).

2. Setup the DB
May need to manually create the DB with name WHM.

Go to ```YARA-Project\backend\WarehouseManagement.API\appsettings.json```

And check WarehouseManagementConnectionString.
Server should be localhost as your SQL server name.
Database should be "WHM" or whatever name you chose when creating the DB.

Apply the Migrations with EF core. 
From ```YARA-Project\backend\WarehouseManagement.API``` run

```
dotnet ef database update
```


3. Start the API.
When in project - navigate to YARA-Project/backend/WarehouseManagement.API
Run:
```bash
dotnet run --launch-profile http
```

Open a browser and go to ```http://localhost:<your_port>/swagger/index.html``` (port can be found in ```YARA-Project/backend/WarehouseManagement.API/WarehouseManagement.API.http```) to verify API is running. 

4. Start the UI.

Navigate to ```YARA-Project/UI/warehouse-management```
Run this to install all the UI's dependencies
```bash
npm install
```
After installation, go to ```YARA-Project/UI/warehouse-management/src/proxy.config.json``` and verify that the target equals the API URL
```
"target": "http://localhost:<your_port>
```

When this is ready start the Angular application with
```bash
npm start
```

Open a browser and go to ```http://http://localhost:4200/``` (port 4200 is default for Angular).

5. Now go ahead and manage your warehouses.

Using the API you can create Warehouses. Products can be created in the UI. It would be good in the start to create a few Warehouses and Inventory records/Transactions in the API.

The current UI supports only Import of products which are already added to a WH (there is an existing Inventory Record about it). And transactions are only made when a successful import is made. 

## Known bugs
1. When creating import if any request fail - no rollback mechanism. You'd have to manually delete the data from the DB.
2. When import is successful - the warehouse component is not refreshed (you are viewing old data. Refresh to see it)
3. Create product form sometimes doesn't show the validation errors.
4. When you cancel import creation, the wizard may not reset and may even break.

## Future improvements

1. Create a script to install everything needed in a single line.
2. Better project management - create, edit, delete products from the UI.
3. Better warehouse object management - create, edit, delete warehouses from the UI.
4. Add support for exporting products.
5. Add support for importing free-standing products.
6. Fix bugs.
7. Improve API capability (add missing methods).
8. Improve UI design.
9. Add more UI validations and checks.
10. Improve structure in bigger files both in UI and API.
11. Add RBAC and authentication.
12. Add unit tests.
13. HTTPS!


## Open to feedback
