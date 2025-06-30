# CSharp Middleware Lab

This project demonstrates the implementation of middleware in an ASP.NET Core application. It includes:

- **Logging Middleware** – Logs incoming HTTP requests and outgoing responses.
- **Error Handling Middleware** – Ensures standardized error responses.
- **JWT Authentication Middleware** – Secures endpoints using token-based authentication.

## Getting Started

### Prerequisites

Ensure you have the following installed:
- .NET SDK 6.0+
- Postman or any API testing tool

### Configuration

Before running the application, configure authentication credentials in `appsettings.json`:

```json
"JwtSettings": {
  "AdminLogin": "your_admin_username",
  "AdminPassword": "your_admin_password",
  "SecretKey": "your_secret_key"
}
```

## Running the Application

1. **Clone the repository:**
   ```sh
   git clone https://github.com/lexeci/CSharp_Middleware_Lab.git
   cd CSharp_Middleware_Lab
   ```

2. **Run the application:**
   ```sh
   dotnet run
   ```
   The application will be available at `http://localhost:5000/`.

## Authentication Process

To authenticate, follow these steps:

1. **Make a POST request to obtain a JWT token:**
   ```http
   POST http://localhost:5000/api/auth/login
   Content-Type: application/json

   {
     "name": "your_admin_username",
     "password": "your_admin_password"
   }
   ```
2. **Copy the received token and use it in your requests:**
   - In Postman, go to **Authorization** → Select **Bearer Token** → Paste your token.
   - For cURL:
     ```sh
     curl -X GET "http://localhost:5000/api/users" -H "Authorization: Bearer your_token_here"
     ```

## CRUD Operations

### Get All Users
```http
GET http://localhost:5000/api/users
Authorization: Bearer your_token_here
```

### Get a Specific User
```http
GET http://localhost:5000/api/users/{id}
Authorization: Bearer your_token_here
```

### Create a New User
```http
POST http://localhost:5000/api/users
Content-Type: application/json
Authorization: Bearer your_token_here

{
  "name": "John Doe",
  "age": 25
}
```

### Update a User
```http
PUT http://localhost:5000/api/users/{id}
Content-Type: application/json
Authorization: Bearer your_token_here

{
  "name": "Updated Name",
  "age": 30
}
```

### Delete a User
```http
DELETE http://localhost:5000/api/users/{id}
Authorization: Bearer your_token_here
```

## Middleware Pipeline

The middleware is configured in the following order:
1. **Error Handling Middleware** - Ensures all unhandled exceptions return a JSON response.
2. **Authentication Middleware** - Validates JWT tokens.
3. **Logging Middleware** - Logs HTTP requests and responses.

