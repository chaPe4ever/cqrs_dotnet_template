## Additional Notes

- This is a minimal API using the latest .NET features
- The project is organized by vertical slices
- Each slice has its own command/query, handler, and validator
- The validation is handled via a pipeline behavior
- The project uses in-memory database for simplicity, but you can replace it with any EF Core provider
- The API uses minimal API endpoints for a cleaner structure

#### This structure provides a great foundation for a maintainable, testable project that allows for easy expansion. Each feature (like Users) is fully self-contained, and adding new features follows the same pattern.
