 # 🏗 Clean Architecture Project

# 📌 Overview

This project follows the Clean Architecture principles, ensuring a scalable, maintainable, and testable application structure. The application is built as a REST API and leverages modern design patterns and best practices.

# 🚀 Technologies & Patterns Used

# 🔹 1. Serilog

Provides structured and configurable logging.

Supports various sinks (e.g., file, console, database, cloud services).

Ensures detailed tracking of application behavior and errors.

# 🔹 2. Generic Repository & Unit of Work

Implements Generic Repository Pattern to manage data access layer efficiently.

Unit of Work ensures atomic transactions across multiple repositories.

Eliminates redundant data access logic by providing a unified interface for database operations.

# 🔹 3. FluentValidation

Handles input validation centrally using FluentValidation.

Validations are enforced via Pipeline Behaviors in CQRS, ensuring clean and reusable validation logic.

Supports complex validation rules with custom error messages.

# 🔹 4. CQRS (Command Query Responsibility Segregation)

Separates Commands (write operations) and Queries (read operations) for better scalability.

Utilizes MediatR for decoupling application logic.

Provides a structured and maintainable approach to handling business logic.

# 🔹 5. Smart Enum

Enhances traditional enums with object-oriented capabilities.

Allows richer functionality and extensibility for enumerations.

📂 ##Project Structure
```
📦 ProjectRoot
 ┣ 📂 Application
 ┃ ┣ 📂 Services         # Business logic services
 ┃ ┣ 📂 CQRS            # Command, Query Handlers
 ┃ ┃ ┣ 📂 Commands      # Command Handlers
 ┃ ┃ ┣ 📂 Handler       # Handlers
 ┃ ┃ ┣ 📂 Queries       # Query Handlers
 ┃ ┣ 📂 Validators      # FluentValidation rules
 ┣ 📂 Infrastructure    # Data access, repositories, logging, etc.
 ┣ 📂 Domain            # Entities and business rules
 ┣ 📂 API               # Entry point, controllers, dependency injection
 ┗ 📂 Tests             # Unit & integration tests
```
🗄 Database & ORM

Uses EF Core with Generic Repository Pattern.

Models are dynamically registered in the DbContext without requiring explicit DbSet<T> definitions.

Automatic filtering is applied to queries using IsActive and IsDeleted fields.

Each model requires proper configuration and an entity class for persistence.

# ✅ Validation Pipeline

Uses MediatR Pipeline Behaviors to validate incoming requests automatically.

Ensures all requests pass through FluentValidation before execution.

Provides a centralized validation mechanism for cleaner code.

# ⚙ Configuration

Configurations are structured for maintainability and scalability.

Supports appsettings.json, environment variables, and dependency injection.

# 📊 Logging & Monitoring

Logs are structured using Serilog.

Supports logging into different outputs (console, file, database, etc.).

Tracks all API requests, responses, and errors for debugging and analytics.

# 🌍 API Implementation

Built using ASP.NET Core Web API.

Follows RESTful principles.

Implements JWT authentication and role-based access control (RBAC).

# 📦 Deployment

Designed for cloud-native environments.

Supports Docker and CI/CD pipelines.

Scalable for microservices architecture.

# 🚀 How to Run

Clone the repository:
```
git clone https://github.com/azureDevOpsTeam/CleanArchitecture.git
cd CleanArchitecture
```
# Configure the application:

Update appsettings.json with the necessary configurations.

Run the database migrations:
```
dotnet ef database update
```
Start the application:
```
dotnet run
```
Access the API via: http://localhost:5000/swagger

# 🤝 Contributing

Fork the repository and create a new branch.

Follow the project’s coding standards.

Submit a pull request with detailed explanations.

# 📜 License

This project is licensed under the MIT License.

# 🚀 Happy Coding! 🎉
