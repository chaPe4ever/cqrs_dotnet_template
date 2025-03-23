dotnet new webapi -n UserManagement.Api --no-https
cd UserManagement.Api
dotnet add package MediatR
dotnet add package MediatR.Extensions.Microsoft.DependencyInjection
dotnet add package FluentValidation
dotnet add package FluentValidation.DependencyInjection
dotnet add package Microsoft.EntityFrameworkCore.InMemory
# Root folders
mkdir -p Features/Users
mkdir -p Infrastructure/Persistence
mkdir -p Common/Behaviors
mkdir -p Common/Interfaces

# Feature slices
mkdir -p Features/Users/Create
mkdir -p Features/Users/GetById
mkdir -p Features/Users/GetAll
mkdir -p Features/Users/Update
mkdir -p Features/Users/Delete
