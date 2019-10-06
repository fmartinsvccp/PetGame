# PetGame

This API requires a connection to a SQLServer, please update the connection strings on the **appsettings.json** (or **appsettings.Development.json** if you run it in Development mode). On Startup I added a configuration for the migrations to be executed on the first time the project runs.

The game has 3 pets at the start, a Dog, a Cat and a Bird. 

Classes:
- Action: Used to log of every action a user has towards his/hers pet (Pet and Feed).
- Pet: Object that represents an available pet type that a user can adquire.
- User: Object that represents an user.
- UserPet: Object that represents a pet that was adquired by an user.

It is possible to see all the endpoints on **\swagger** but the 6 endpoints of this API are the following:
- User/CreateUser: Post function, requires a string on the header. Creates a new user.
  - string **name**: Name of the user.
- Pet/GetPetList: Get function with no parameters. Returns the list of all available pets.
- Pet/GetPetForUser: Post function, requires the id of the pet and the user as parameters on the header. Creates a new pet for the user.
  - int **userId**: Id of the user.
  - int **petId**: Id of the pet.
- Pet/GetUserPetList/{userId}: Get function, requires the id of the user as part of the url. Returns the list of pets of a user.
  - int **userId**: Id of the user.
- Actions/FeedAnimal: Post function, requires the id of the userPet as parameters on the header. Feed a pet from a user.
  - int **userPetId**: Id of the userPet.
- Actions/PetAnimal: Post function, requires the id of the userPet as parameters on the header. Pet an pet from a user.
  - int **userPetId**: Id of a userPet.
  
  
I didn't add any endpoint to create new types of pets because I thought it wouldn't be done by the game app. At the moment the only way to do it is with migrations or directly into the database.

There is no maximum of happiness or hunger that a pet can have, just a minimum of 0.

I thought on adding different types of food that would feed every pet with different values (pets eat different food) to add an extra complexity to the game but I tought that it would requires more time than the 5-8 hours of this test. But add this functionality into this game I would require to create a Food class and an intermidiate class with the feeding values bettween each pet and each food type. Than on the UserPetService.cs, on the function FeedPet instead of getting a static value from the Configuration function I would calculate the value using a FoodService by accessing the database (on FoodRepository) with the Pet Id and the Food Id.

And there is a lot of other things we could keep adding into this game like a store for players to buy food, even adding expiring date to the food, and so on.

# Unit Tests

I decided to do all my unit tests on top of the services layer, where all the business logic is. 
