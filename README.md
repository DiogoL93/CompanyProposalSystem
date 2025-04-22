# Company Proposal System API

This is a .NET 9.0 Web API application for managing company proposals, users, and companies.

## Prerequisites

- .NET 9.0 SDK
- SQL Server (or SQL Server Express)
- Visual Studio 2022 (recommended) or Visual Studio Code

## Getting Started

1. Clone the repository
2. Open the solution in Visual Studio or your preferred IDE
3. Update the connection string in `appsettings.json` to point to your SQL Server instance
4. Build and run the application

## Running the Application

### Using Visual Studio
1. Open `CompanyProposalSystem.sln` in Visual Studio
2. Press F5 or click the "Start" button to run the application

### Using Command Line
```bash
# Navigate to the project directory
cd CompanyProposalAPI

# Build the application
dotnet build

# Run the application
dotnet run
```

The application will start and be available at:
- HTTP: http://localhost:5000
- Swagger UI: http://localhost:5000/swagger

## Testing the Application

### Using Swagger UI
1. Open http://localhost:5000/swagger in your browser
2. You'll see all available API endpoints
3. Click on any endpoint to expand it
4. Click "Try it out" to test the endpoint
5. Fill in the required parameters and click "Execute"

### Using Postman
1. Import the Postman collection from `CompanyProposalAPI.postman_collection.json`
2. The collection includes pre-configured requests for all endpoints
3. Run the requests in sequence to test the full workflow

## API Endpoints

### Managing
- `GET /api/managing/proposals/{itemId}` - View proposals for a specific item
- `POST /api/managing/proposal` - Create a new proposal
- `POST /api/managing/proposal/{proposalId}/counter` - Create a counter proposal
- `PUT /api/managing/proposal/{proposalId}/accept` - Accept a proposal
- `GET /api/managing/items` - Get company items with filtering and sorting options

## Database

The application uses Entity Framework Core with SQL Server. The database will be automatically created and seeded with initial data when the application starts.

## Troubleshooting

1. If you get a database connection error:
   - Verify your SQL Server is running
   - Check the connection string in `appsettings.json`
   - Ensure the database server is accessible

2. If you get HTTPS certificate errors:
   - Run `dotnet dev-certs https --trust` to trust the development certificate
   - Restart the application

3. If you encounter any other issues:
   - Check the application logs
   - Verify all prerequisites are installed
   - Ensure you're using the correct .NET version 