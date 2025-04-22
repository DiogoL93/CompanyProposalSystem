# Company Proposal System API Documentation

## Overview
The Company Proposal System API provides endpoints for managing proposals, items, and company-related operations. This documentation describes how to interact with the API, including request/response structures and examples.

## Base URL
```
http://localhost:5000/api/managing
```

## Authentication
Currently, the API uses a simple user context system where the current user is determined from the request context. The current user is used to track who performs actions like accepting or rejecting proposals. In a production environment, this would be replaced with proper authentication.

## Endpoints

### 1. View Proposals for Item
Retrieves all proposals associated with a specific item.

**Endpoint:** `GET /proposals/{itemId}`

**Path Parameters:**
- `itemId` (integer): The ID of the item to get proposals for

**Response:**
```json
[
    "Pending by John Doe on behalf of Company A",
    "Accepted by Jane Smith on behalf of Company B"
]
```

**Example:**
```bash
curl -X GET http://localhost:5000/api/managing/proposals/1
```

### 2. Create Proposal
Creates a new proposal for an item.

**Endpoint:** `POST /proposal`

**Request Body:**
```json
{
    "description": "New Proposal",
    "itemId": 1,
    "totalValue": 1000.00,
    "valueType": "Amount",
    "currency": "USD"
}
```

**Response:**
```json
{
    "id": 1,
    "description": "New Proposal",
    "itemId": 1,
    "totalValue": 1000.00,
    "valueType": "Amount",
    "currency": "USD",
    "status": "Pending",
    "createdAt": "2024-04-22T10:00:00Z",
    "updatedAt": "2024-04-22T10:00:00Z",
    "createdByUser": {
        "id": 1,
        "firstName": "John",
        "lastName": "Doe",
        "company": {
            "id": 1,
            "name": "Company A"
        }
    }
}
```

**Example:**
```bash
curl -X POST http://localhost:5000/api/managing/proposal \
  -H "Content-Type: application/json" \
  -d '{
    "description": "New Proposal",
    "itemId": 1,
    "totalValue": 1000.00,
    "valueType": "Amount",
    "currency": "USD"
  }'
```

### 3. Create Counter Proposal
Creates a counter proposal in response to an existing proposal.

**Endpoint:** `POST /proposal/{proposalId}/counter`

**Path Parameters:**
- `proposalId` (integer): The ID of the proposal to counter

**Request Body:**
```json
{
    "description": "Counter Proposal",
    "itemId": 1,
    "totalValue": 1200.00,
    "valueType": "Amount",
    "currency": "USD"
}
```

**Response:**
```json
{
    "id": 2,
    "description": "Counter Proposal",
    "itemId": 1,
    "totalValue": 1200.00,
    "valueType": "Amount",
    "currency": "USD",
    "status": "Pending",
    "createdAt": "2024-04-22T10:05:00Z",
    "updatedAt": "2024-04-22T10:05:00Z",
    "createdByUser": {
        "id": 2,
        "firstName": "Jane",
        "lastName": "Smith",
        "company": {
            "id": 2,
            "name": "Company B"
        }
    }
}
```

**Example:**
```bash
curl -X POST http://localhost:5000/api/managing/proposal/1/counter \
  -H "Content-Type: application/json" \
  -d '{
    "description": "Counter Proposal",
    "itemId": 1,
    "totalValue": 1200.00,
    "valueType": "Amount",
    "currency": "USD"
  }'
```

### 4. Accept Proposal
Accepts a proposal, marking it as accepted. The current user will be recorded as the one who accepted the proposal.

**Endpoint:** `PUT /proposal/{proposalId}/accept`

**Path Parameters:**
- `proposalId` (integer): The ID of the proposal to accept

**Response:**
- Status: 204 No Content

**Error Responses:**
- 400 Bad Request: If the current user is not found
- 404 Not Found: If the proposal is not found

**Example:**
```bash
curl -X PUT http://localhost:5000/api/managing/proposal/1/accept
```

### 5. Reject Proposal
Rejects a proposal, marking it as rejected. The current user will be recorded as the one who rejected the proposal.

**Endpoint:** `PUT /proposal/{proposalId}/reject`

**Path Parameters:**
- `proposalId` (integer): The ID of the proposal to reject

**Response:**
- Status: 204 No Content

**Error Responses:**
- 400 Bad Request: If the current user is not found
- 404 Not Found: If the proposal is not found

**Example:**
```bash
curl -X PUT http://localhost:5000/api/managing/proposal/1/reject
```

### 6. Get Company Items
Retrieves items associated with the current user's company, with optional filtering and sorting.

**Endpoint:** `GET /items`

**Query Parameters:**
- `name` (string, optional): Filter items by name
- `createdAfter` (date, optional): Filter items created after this date
- `createdBefore` (date, optional): Filter items created before this date
- `minPrice` (decimal, optional): Filter items with price greater than or equal to this value
- `maxPrice` (decimal, optional): Filter items with price less than or equal to this value
- `sortBy` (string, optional): Field to sort by (name, price, createdAt)
- `sortDescending` (boolean, optional): Sort in descending order

**Response:**
```json
[
    {
        "id": 1,
        "name": "Item 1",
        "description": "Description 1",
        "price": 100.00,
        "createdAt": "2024-04-22T10:00:00Z"
    },
    {
        "id": 2,
        "name": "Item 2",
        "description": "Description 2",
        "price": 200.00,
        "createdAt": "2024-04-22T10:05:00Z"
    }
]
```

**Example:**
```bash
curl -X GET "http://localhost:5000/api/managing/items?name=test&createdAfter=2024-01-01&createdBefore=2024-12-31&minPrice=100&maxPrice=1000&sortBy=name&sortDescending=false"
```

## Error Responses

The API uses standard HTTP status codes to indicate success or failure:

- `200 OK`: Request successful
- `201 Created`: Resource created successfully
- `204 No Content`: Request successful, no content to return
- `400 Bad Request`: Invalid request parameters or body, or current user not found
- `401 Unauthorized`: User not authenticated
- `404 Not Found`: Resource not found
- `500 Internal Server Error`: Server error

Error responses include a message explaining the error:

```json
{
    "statusCode": 400,
    "message": "Current user not found"
}
```

## Best Practices

1. **Error Handling**
   - Always check the response status code
   - Handle error responses appropriately
   - Implement retry logic for transient errors

2. **Rate Limiting**
   - Implement appropriate rate limiting in your client application
   - Handle 429 (Too Many Requests) responses

3. **Data Validation**
   - Validate all input data before sending requests
   - Handle validation errors appropriately

4. **Testing**
   - Use the provided Postman collection for testing
   - Test all error scenarios
   - Verify response formats

## Using the Postman Collection

The API comes with a Postman collection (`CompanyProposalAPI.postman_collection.json`) that includes:
- All available endpoints
- Example request bodies
- Query parameters
- Response examples

To use the collection:
1. Import the collection into Postman
2. Set up environment variables if needed
3. Run the requests in sequence to test the workflow

## Support

For support or questions about the API, please contact the development team. 