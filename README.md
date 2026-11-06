# REST API for votes
## A fully working REST API built with ASP.NET Core, integrating custom middleware for request validation and response duration tracking,   and tested with Swagger UI. It also generated an API client in C# using the NSwag library.

In this project, the following functionalities have been implemented:
  * Users service as Singleton to manage CRUD operations on user data
  * Votes service as Singleton to manage CRUD operations on vote data
  * Controllers for routing endpoints and CRUD operations for both Users and Votes
  * Custom exceptions for custom  headers in the request; for example, the "API-SIGNATURE" key was used
  * GlobalValidator middleware for custom exception handling
  * Swagger UI test using OpenAPI standard
  * Built-in authentication and authorization middleware
  * API client generation in C# code

![Swagger UI of the API](assets/swagger.png)