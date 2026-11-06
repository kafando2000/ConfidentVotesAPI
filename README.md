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

## How to install and run ConfidentVoteAPI
 1. Install .NET 10 SDK
 2. clone the repo with: <br>
    for https users: <br>
    `git clone https://github.com/kafando2000/ConfidentVotesAPI.git` <br>
    for ssh users: <br>
    `git clone git@github.com:kafando2000/ConfidentVotesAPI.git` <br>
 3. run the project: <br>
    test with swagger UI (it bypass a custom validator for headers ): <br>
      `dotnet watch run --testwithswaggerui true` <br>
       head to http://localhost:5284/swagger/index.html to test <br>
    test with the custom validator: <br>
      `dotnet watch run --testwithswaggerui false` <br>
       can use ConfidentVotesAPI.http file (locate at the root of the project folder) to test 

## Next
 For the moment the project is using session services to store data, 
 later on connection to real data base will be implemented
    
