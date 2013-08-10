mvc4-many-to-many
=================

A CRUD application developed in ASP.NET MVC4 using Entity Framework Code First.

- Multi tier MVC4 project utilising Entity Framework 5.
- Creates a many to many table structure in our database using code first
- Adds child collections to our objects that lets Users have multiple Courses and Courses have multiple Users
- Uses an Editor Template to render child collections and names the fields in the correct format for the MVC model binder

This isn’t a fully fledged enterprise level application but is a good foundation for a CRUD application. To make the code more robust and complete and have it closer to industry standards we would add unit testing, dependency injection / IoC, error handling, logging, etc.
