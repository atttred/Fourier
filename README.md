# Fourier Backend Solution

A .NET-based backend solution for Fourier analysis with load balancing capabilities. The solution consists of two projects: main API service (Fourier) and load balancer (Fourier_Balancer).

## Solution Structure

```
Solution 'Fourier'/
├── Fourier/                           # Main API Project
│   ├── Controllers/
│   │   ├── AuthController.cs          # Authentication endpoints
│   │   └── ProblemController.cs       # Problem management endpoints
│   ├── Data/
│   │   └── FourierDbContext.cs        # Database context
│   ├── DTOs/                          # Data Transfer Objects
│   │   ├── AuthResponseDto.cs
│   │   ├── LoginDto.cs
│   │   ├── ProblemDto.cs
│   │   └── RegisterDto.cs
│   ├── Migrations/                    # Database Migrations
│   ├── Models/                        # Domain Models
│   │   ├── JWT/
│   │   ├── CancellationToken.cs
│   │   ├── Problem.cs
│   │   └── User.cs
│   ├── Repositories/                  # Data Access Layer
│   ├── Services/                      # Business Logic Layer
│   │   ├── AuthService.cs
│   │   ├── CancellationTokenService.cs
│   │   ├── LogicService.cs
│   │   ├── ProblemService.cs
│   │   └── UserService.cs
│   ├── appsettings.json              # Configuration
│   ├── Fourier.http                  # HTTP request examples
│   └── Program.cs                    # Application entry point
│
└── Fourier_Balancer/                 # Load Balancer Project
    ├── Controllers/
    ├── CustomLoadPolicy.cs           # Custom load balancing logic
    ├── appsettings.json
    ├── Fourier_Balancer.http
    └── Program.cs
```

## Technologies Used

- ASP.NET Core 7.0
- Entity Framework Core
- YARP (Yet Another Reverse Proxy) for load balancing
- JWT Authentication
- SQL Server

## Prerequisites

- .NET 7.0 SDK
- SQL Server
- Visual Studio 2022 or similar IDE

## Setup and Configuration

1. Clone the repository:
```bash
git clone [repository-url]
```

2. Update the connection string in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "your_connection_string_here"
  }
}
```

3. Apply database migrations:
```bash
dotnet ef database update
```

4. Run the projects:
```bash
# Run main API
dotnet run --project Fourier

# Run load balancer
dotnet run --project Fourier_Balancer
```

## API Endpoints

### Authentication
- POST `/api/auth/register` - User registration
- POST `/api/auth/login` - User login
- POST `/api/auth/logout` - User logout

### Problem Management
- GET `/api/problems` - Get all problems
- POST `/api/problems` - Create new problem
- GET `/api/problems/{id}` - Get specific problem
- PUT `/api/problems/{id}` - Update problem
- DELETE `/api/problems/{id}` - Delete problem

## Load Balancer Configuration

The load balancer is configured to distribute traffic between multiple instances of the main API. Configuration can be modified in `Fourier_Balancer/appsettings.json`.

Example configuration:
```json
{
  "ReverseProxy": {
    "Routes": {
      "route1": {
        "ClusterId": "fourier",
        "Match": {
          "Path": "{**catch-all}"
        }
      }
    },
    "Clusters": {
      "fourier": {
        "Destinations": {
          "destination1": {
            "Address": "http://localhost:5001"
          },
          "destination2": {
            "Address": "http://localhost:5002"
          }
        }
      }
    }
  }
}
```

## Development

1. Add new endpoints in appropriate controllers
2. Create corresponding DTOs for new features
3. Implement business logic in Services
4. Update database context and migrations as needed
5. Test endpoints using provided `.http` files

## Project Features

- JWT-based authentication
- Custom load balancing with YARP
- Database migrations
- DTO pattern implementation
- Service layer architecture
- Repository pattern (optional)

## Error Handling

The API uses a consistent error handling pattern with appropriate HTTP status codes and error messages.

## Testing

To test the API endpoints, use the provided `.http` files in each project:
- `Fourier.http` for main API endpoints
- `Fourier_Balancer.http` for load balancer testing

## Security

- JWT token authentication
- Password hashing
- Token expiration handling
- Cancellation token support

## Build and Deployment

```bash
# Build solution
dotnet build

# Publish projects
dotnet publish -c Release
```
