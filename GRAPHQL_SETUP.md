# GraphQL Client Setup

This document explains how to use the GraphQL client that has been added to the FwkWebApplication1 project.

## Overview

A GraphQL client has been integrated into the ASP.NET MVC 5 application to connect to a GraphQL API running at `http://localhost:4000` and display results in table format.

## Components Added

### 1. **NuGet Packages**
   - `GraphQL.Client` (v6.0.2)
   - `GraphQL.Client.Http` (v6.0.2)

### 2. **Services**
   - **GraphQLClientService** (`Services/GraphQLClientService.cs`)
     - Handles GraphQL API calls
     - Supports both queries and mutations
     - Converts responses to usable data

### 3. **Models**
   - **GraphQLDataModel** (`Models/GraphQLDataModel.cs`)
     - Data model for displaying GraphQL results
     - Handles table-format conversion

### 4. **Controllers**
   - **HomeController** - Enhanced with GraphQL methods:
     - `GraphQL()` - Displays the query executor page
     - `ExecuteGraphQLQuery(string query)` - Executes a GraphQL query and returns results

### 5. **Views**
   - **GraphQL.cshtml** (`Views/Home/GraphQL.cshtml`)
     - Interactive query executor interface
     - Text area for entering GraphQL queries
     - Display results in table format
     - Shows raw JSON response

## How to Use

### 1. **Start the GraphQL API Server**
   Ensure your GraphQL API is running at `http://localhost:4000`

### 2. **Run the ASP.NET Application**
   ```bash
   # In Visual Studio: Press F5
   # Or via command line:
   msbuild FwkSolution1.sln
   ```

### 3. **Navigate to GraphQL Page**
   - Open the application in your browser (https://localhost:44331/)
   - Click on "GraphQL" in the navigation menu
   - Or navigate directly to: `https://localhost:44331/Home/GraphQL`

### 4. **Execute a GraphQL Query**
   - Enter your GraphQL query in the text area
   - Example query:
     ```graphql
     {
       users {
         id
         name
         email
       }
     }
     ```
   - Click "Execute Query" button or press Ctrl+Enter
   - View results in the table below

## Features

### Query Execution
- Real-time GraphQL query execution
- Automatic error handling and display
- Loading spinner during query execution

### Result Display
- **Table View**: Results displayed in a formatted table
  - Automatic column detection
  - Supports nested arrays and objects
  - Long values are truncated for readability

- **Raw JSON View**: Full JSON response displayed for debugging
  - Properly formatted and syntax-highlighted

### Error Handling
- Connection errors to GraphQL endpoint
- Query validation errors
- Empty query prevention

## Code Examples

### Using GraphQLClientService Directly

```csharp
using FwkWebApplication1.Services;

// Create an instance
var graphQLService = new GraphQLClientService("http://localhost:4000");

// Execute a query
var query = @"
{
  users {
    id
    name
    email
  }
}
";

var result = await graphQLService.GetDataAsync(query);

// Execute a typed query
public class User
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}

var typedResult = await graphQLService.QueryAsync<User>(query);
```

### Query Structure

The service expects GraphQL queries in standard format:

```graphql
# Simple query
{
  fieldName {
    subField1
    subField2
  }
}

# Query with variables
query GetUser($id: ID!) {
  user(id: $id) {
    name
    email
  }
}

# Mutation
mutation CreateUser {
  createUser(input: {name: "John", email: "john@example.com"}) {
    id
    name
  }
}
```

## Response Format

The service returns data in a structured format:

```json
{
  "success": true,
  "data": {
    "rows": [
      {"id": "1", "name": "John", "email": "john@example.com"},
      {"id": "2", "name": "Jane", "email": "jane@example.com"}
    ],
    "columns": ["id", "name", "email"]
  },
  "rawData": "{"data":{...}}"
}
```

## Configuration

### Change GraphQL Endpoint

To change the GraphQL API endpoint, modify the HomeController:

```csharp
public HomeController()
{
    _graphQLService = new GraphQLClientService("http://your-endpoint:port");
}
```

Or modify the default in GraphQLClientService:

```csharp
public GraphQLClientService(string graphqlEndpoint = "http://localhost:4000")
```

## Troubleshooting

### Error: "Failed to connect to GraphQL endpoint"
- Ensure your GraphQL server is running on the specified port
- Check firewall settings
- Verify the endpoint URL is correct

### No Results Displayed
- Verify your GraphQL query syntax
- Check the Raw JSON Response section for error details
- Ensure the GraphQL API returns data in the expected format

### Column Names Not Appearing
- The client automatically detects columns from the response
- Ensure your query returns object fields with distinct names
- Nested objects will be converted to JSON strings

## File Structure

```
FwkWebApplication1/
├── Services/
│   └── GraphQLClientService.cs       # Main GraphQL client service
├── Models/
│   └── GraphQLDataModel.cs          # Data models for results
├── Controllers/
│   └── HomeController.cs            # Updated with GraphQL actions
├── Views/
│   ├── Home/
│   │   └── GraphQL.cshtml           # GraphQL query executor UI
│   └── Shared/
│       └── _Layout.cshtml           # Updated with GraphQL link
└── FwkWebApplication1.csproj        # Updated with new references
```

## Browser Support

- Chrome/Edge (latest)
- Firefox (latest)
- Safari (latest)
- IE 11+ (requires jQuery)

## Next Steps

1. Ensure your GraphQL API is running
2. Test with sample queries
3. Integrate GraphQL queries into other pages as needed
4. Customize the table display format as required
