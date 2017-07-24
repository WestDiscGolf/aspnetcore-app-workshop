# aspnetcore-app-workshop
Work based of running through https://github.com/jongalloway/aspnetcore-app-workshop but with preview 2 of aspnetcore

## Problem #1
First running through the steps and going to the latest download of aspnet core I got preview 2 (version 006497) and not the specified preview 1 version 005977 build. Due to this the the entity framework core section did not work. This was due to the DbContextFactoryOptions being added to return an object, then changed to be a string (at the point of the workshop) - https://github.com/aspnet/EntityFramework/commit/0ab838a7a3efe4a1d482040893a636069d9dbca6#diff-b63405a30f0a5367d65d9758915c5137 - and then changed back - https://github.com/aspnet/EntityFramework/commit/8e3337905e70876411f89d44d43313f39dc6d12e#diff-b63405a30f0a5367d65d9758915c5137. On investigation it the final bit to get it working came down to - https://github.com/aspnet/EntityFramework/issues/8499 - and the namespace moving.


## Problem #2
Configuring EF core migrations I had to navigate to the folder in the BackEnd project which has the startup.cs file in and run - dotnet add package Microsoft.EntityFrameworkCore.Tools.DotNet - as per the ef core issue - https://github.com/aspnet/EntityFramework/issues/7838. In addition to running the previous package add command the tooling references in the csproj file needed updating to use the preview2 final versions:

  <ItemGroup>
    <DotNetCliToolReference Include="Microsoft.VisualStudio.Web.CodeGeneration.Tools" Version="2.0.0-preview2-final" />
    <DotNetCliToolReference Include="Microsoft.EntityFrameworkCore.Tools.DotNet" Version="2.0.0-preview2-final" />
  </ItemGroup>


## Problem #3
On installing through the command line - dotnet add package Swashbuckle.AspNetCore - there were issues with the dotnet and nuget resolving the dependancy graph to install the package. However going into the Nuget package manager UI in Visual Studio and installing it through there it worked as expected.


## Problem #4
A number of class references could not be resolved / found in the preview so I had to specify their fully qualified name for the solution to build once adding the additional model classes. However on further investigation this was due to the fact the walk through did not specify adding Tag and Track data classe. After looking at the 2a save point - https://github.com/jongalloway/aspnetcore-app-workshop/tree/master/save-points/2a-Refactor-to-ConferenceDTO/ConferencePlanner/BackEnd/Data - it was all cleared up, FQN removed and the missing data classes were added in.


## Problem #5
After the refactor and the additional data structures additional ef migration commands needed to be run. After running:

dotnet ef migrations add Refactor
dotnet ef database update

The output from the powershell window indicated the Build failure.

On running dotnet run the following error was presented to me:

C:\Users\Adam\Documents\Visual Studio 2017\Projects\Workshop\BackEnd\BackEnd.csproj : error NU1003: PackageTargetFallback and AssetTargetFallback cannot be used together. Remove PackageTargetFallback(deprecated) references from the project environment

This got my head scratching for a bit and I turned to google and found - https://github.com/dotnet/cli/issues/6952 - and after a helpful response I was able to get past the issue. Essentially the issue came down to the way, as far as I am aware, that target frameworks or TFMs(?) are resolved from the project. On replacing PackageTargetFallback with AssetTargetFallback the error turned to an expected warning and I was able to continue.


## Problem #6
Not an issue with the frameworks but found that while I was adding the additional controllers specified in the walk through there were additional missing classes defined for model classes as well as additional helper extension mapping methods. On finding the files in the walk through save point I was able to add them in with minimal issue.


## Problem #7
When running the initial seeding of the NDC session data I got the following error:

The entity type 'SessionAttendee' requires a primary key to be defined.

Go into the the ApplicationDbContext OnModelCreating and update the modelBuilder mappings which can be found - https://github.com/jongalloway/aspnetcore-app-workshop/blob/master/src/BackEnd/Data/ApplicationDbContext.cs


## Problem #8
Add auth features does not explicitly specify which site it needs adding to - its the frontend - also it doesn't specify where in the Startup methods with regards to AddMvc - it's after as well as after the httpclient setup dependency setup.


## Problem #9
The signature of IAuthorizationService.AuthorizeAsync does not return a bool anymore but it returns a AuthorizationResult. Need to update the IsAdmin usage as well as the AuthzTagHelper usage to use:

var result = await _authorizationService.AuthorizeAsync(User, "Admin");
IsAdmin = result.Succeeded;


## Problem #10
The tag helper assumes there is an action on the account controller called "Login" which there is not so the href of the anchor needs updating to point to the login page. While I was trying to debug the tag helper I found that the break point would not hit and "log in" and "log out" links would display all of the time. To resolve this I made sure I had the correct version of the Microsoft.AspNetCore.Razor.Runtime package installed - 2.0.0-preview2-final - and also made sure that the project tag helper reference was added to /Pages/_ViewImports.cshtml

@namespace FrontEnd.Pages
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
@addTagHelper *, FrontEnd

## Outstanding issues
* Unable to get Twitter authentication to work however Google works fine
