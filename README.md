Of course! Here is the translated template for your project's README:

---

# Basic Social Network Project

This project is a basic social network developed with .NET 7 using ASP.NET Core MVC and a SQL Server container using Docker Compose. The application allows users to register, activate their account via an email link, log in, add friends, and create posts with text, images, or videos. Additionally, posts can be commented on and comments can receive replies.

## Features

- User registration with profile picture.
- Account activation via email link.
- User login.
- Add and manage friends.
- Create posts with text, text and video, or text and image.
- Comment on posts.
- Reply to comments.
  
## Images
![signup-socialize](https://github.com/AdrianPolanco/Socialize/assets/116674818/4451c238-840e-4187-8f14-e4463bc78695)
![login-socialize](https://github.com/AdrianPolanco/Socialize/assets/116674818/514cba5f-0007-4e92-87b6-a1bc3ba414a9)
![emailconfirmation-socialize](https://github.com/AdrianPolanco/Socialize/assets/116674818/9b3ec056-39e2-427e-b4b1-9d87af9e476f)
![emailnotconfirmed](https://github.com/AdrianPolanco/Socialize/assets/116674818/be31bf2d-0150-4efb-887b-2813fa7ef622)
![success-socialize](https://github.com/AdrianPolanco/Socialize/assets/116674818/7c3056c5-7476-4efc-ae15-906da8b58d3c)
![Captura de pantalla 2024-07-01 143715](https://github.com/AdrianPolanco/Socialize/assets/116674818/e15ca86c-6b47-4a58-9960-530f14513675)
![resetedpassword-socialize](https://github.com/AdrianPolanco/Socialize/assets/116674818/85a19617-62ff-4971-9dd0-560a21a070d2)
![home-socialize](https://github.com/AdrianPolanco/Socialize/assets/116674818/5c5ede67-f0e9-4d0c-8f78-a76b90c8f4ee)
![friends-socialize](https://github.com/AdrianPolanco/Socialize/assets/116674818/facf847f-a53c-4614-be18-2f284991b608)


## Technologies Used

- .NET 7
- ASP.NET Core MVC
- SQL Server
- Docker Compose

## Installation and Running

### Prerequisites

- [.NET 7 SDK](https://dotnet.microsoft.com/download/dotnet/7.0)
- [Docker](https://www.docker.com/get-started)

### Instructions

1. Clone this repository to your local machine:

   ```sh
   git clone https://github.com/AdrianPolanco/Socialize
   cd Socialize
   ```

2. Build and run the Docker containers:

   ```sh
   docker-compose up --build
   ```

3. Restore the NuGet packages:

   ```sh
   dotnet restore
   ```

4. Apply migrations to set up the database:

   ```sh
   dotnet ef database update
   ```

5. Run the application:

   ```sh
   dotnet run
   ```

### Testing

To run the tests, use the following command:

```sh
dotnet test
```

## Usage

1. Access the application in your browser at `http://localhost:5000`.
2. Register by providing the required information and uploading a profile picture.
3. You will receive an activation link in the registered email. Click the link to activate your account.
4. Log in with your credentials.
5. Add friends by searching for their usernames.
6. Create posts with text, images, or videos.
7. Comment on posts and reply to other users' comments.

