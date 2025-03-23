#!/bin/bash
set -e  # Exit on error

# Default values
PROJECT_NAME="UserManagement.Api"
OUTPUT_DIR="."

# Parse command line arguments
while [[ $# -gt 0 ]]; do
  case $1 in
    -n|--name)
      PROJECT_NAME="$2"
      shift 2
      ;;
    -o|--output)
      OUTPUT_DIR="$2"
      shift 2
      ;;
    -h|--help)
      echo "Usage: $0 [options]"
      echo "Options:"
      echo "  -n, --name NAME      Project name (default: UserManagement.Api)"
      echo "  -o, --output DIR     Output directory (default: current directory)"
      echo "  -h, --help           Show this help message"
      exit 0
      ;;
    *)
      # For backward compatibility, treat first positional arg as project name
      if [[ "$PROJECT_NAME" == "UserManagement.Api" ]]; then
        PROJECT_NAME="$1"
      fi
      shift
      ;;
  esac
done

# Get the script directory (where the template files are)
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
echo "Template directory: $SCRIPT_DIR"

# Create absolute path for output directory
mkdir -p "$OUTPUT_DIR"
OUTPUT_DIR="$( cd "$OUTPUT_DIR" && pwd )"
echo "Creating project: $PROJECT_NAME"
echo "Output directory: $OUTPUT_DIR"

# Navigate to the output directory and create the project
cd "$OUTPUT_DIR"
dotnet new webapi -n "$PROJECT_NAME" --no-https

# Check if project was created successfully
if [ ! -d "$OUTPUT_DIR/$PROJECT_NAME" ]; then
  echo "Error: Project directory was not created at $OUTPUT_DIR/$PROJECT_NAME"
  exit 1
fi

# Navigate to the project directory
cd "$PROJECT_NAME"
echo "Working in: $(pwd)"

# Add required packages
echo "Adding NuGet packages..."
dotnet add package MediatR
dotnet add package FluentValidation
dotnet add package FluentValidation.AspNetCore
dotnet add package Microsoft.EntityFrameworkCore.InMemory

# Create directory structure
echo "Creating directory structure..."
mkdir -p Features/Users
mkdir -p Features/Users/Create
mkdir -p Features/Users/GetById
mkdir -p Features/Users/GetAll
mkdir -p Features/Users/Update
mkdir -p Features/Users/Delete
mkdir -p Infrastructure/Persistence
mkdir -p Common/Behaviors
mkdir -p Common/Interfaces

# Copy files to their appropriate directories
echo "Copying template files to project..."
# Common interfaces and behaviors
cp "$SCRIPT_DIR/IBaseCommand.cs" Common/Interfaces/ 2>/dev/null || echo "Warning: IBaseCommand.cs not found"
cp "$SCRIPT_DIR/IBaseQuery.cs" Common/Interfaces/ 2>/dev/null || echo "Warning: IBaseQuery.cs not found"
cp "$SCRIPT_DIR/ValidationBehavior.cs" Common/Behaviors/ 2>/dev/null || echo "Warning: ValidationBehavior.cs not found"

# Infrastructure
cp "$SCRIPT_DIR/User.cs" Infrastructure/Persistence/ 2>/dev/null || echo "Warning: User.cs not found"
cp "$SCRIPT_DIR/UserDbContext.cs" Infrastructure/Persistence/ 2>/dev/null || echo "Warning: UserDbContext.cs not found"

# User features - Create
cp "$SCRIPT_DIR/CreateUserCommand.cs" Features/Users/Create/ 2>/dev/null || echo "Warning: CreateUserCommand.cs not found"
cp "$SCRIPT_DIR/CreateUserCommandHandler.cs" Features/Users/Create/ 2>/dev/null || echo "Warning: CreateUserCommandHandler.cs not found"
cp "$SCRIPT_DIR/CreateUserCommandValidator.cs" Features/Users/Create/ 2>/dev/null || echo "Warning: CreateUserCommandValidator.cs not found"

# User features - GetById
cp "$SCRIPT_DIR/GetUserByIdQuery.cs" Features/Users/GetById/ 2>/dev/null || echo "Warning: GetUserByIdQuery.cs not found"
cp "$SCRIPT_DIR/GetUserByIdQueryHandler.cs" Features/Users/GetById/ 2>/dev/null || echo "Warning: GetUserByIdQueryHandler.cs not found"

# User features - GetAll
cp "$SCRIPT_DIR/GetAllUsersQuery.cs" Features/Users/GetAll/ 2>/dev/null || echo "Warning: GetAllUsersQuery.cs not found"
cp "$SCRIPT_DIR/GetAllUsersQueryHandler.cs" Features/Users/GetAll/ 2>/dev/null || echo "Warning: GetAllUsersQueryHandler.cs not found"

# User features - Update
cp "$SCRIPT_DIR/UpdateUserCommand.cs" Features/Users/Update/ 2>/dev/null || echo "Warning: UpdateUserCommand.cs not found"
cp "$SCRIPT_DIR/UpdateUserCommandHandler.cs" Features/Users/Update/ 2>/dev/null || echo "Warning: UpdateUserCommandHandler.cs not found"
cp "$SCRIPT_DIR/UpdateUserCommandValidator.cs" Features/Users/Update/ 2>/dev/null || echo "Warning: UpdateUserCommandValidator.cs not found"

# User features - Delete
cp "$SCRIPT_DIR/DeleteUserCommand.cs" Features/Users/Delete/ 2>/dev/null || echo "Warning: DeleteUserCommand.cs not found"
cp "$SCRIPT_DIR/DeleteUserCommandHandler.cs" Features/Users/Delete/ 2>/dev/null || echo "Warning: DeleteUserCommandHandler.cs not found"

# UserDto - place in Features/Users
cp "$SCRIPT_DIR/UserDto.cs" Features/Users/ 2>/dev/null || echo "Warning: UserDto.cs not found"

# Root files
cp "$SCRIPT_DIR/UserEndpoints.cs" ./ 2>/dev/null || echo "Warning: UserEndpoints.cs not found"
cp "$SCRIPT_DIR/Program.cs" ./ 2>/dev/null || echo "Warning: Program.cs not found" 
cp "$SCRIPT_DIR/README.md" ./ 2>/dev/null || :

# Copy .gitignore from template root to project root
cp "$SCRIPT_DIR/.gitignore" "$OUTPUT_DIR/" 2>/dev/null || echo "Warning: .gitignore not found"

# Fix namespaces in all files
echo "Updating namespaces in files..."
find . -name "*.cs" -type f -exec sed -i '' "s/UserManagement\.Api/$PROJECT_NAME/g" {} \; 2>/dev/null || echo "Warning: namespace replacement failed"

# Remove old Controllers folder and any default files we don't need
echo "Cleaning up default files..."
rm -rf Controllers/ 2>/dev/null || :
rm -f WeatherForecast.cs 2>/dev/null || :

echo "✅ Project $PROJECT_NAME created successfully in $OUTPUT_DIR/$PROJECT_NAME!"
echo "Files have been organized into vertical slice architecture."
echo "To run the project: cd $OUTPUT_DIR/$PROJECT_NAME && dotnet run"
