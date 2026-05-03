# Running Locally
Install the .NET SDK 9

`dotnet tool install --global dotnet-ef`
`dotnet ef migrations add <MigrationName> --project ..\Ember.Infrastructure\Ember.Infrastructure.csproj --startup-project .\Ember.WebServer.csproj`
`dotnet ef database update --project ..\Ember.Infrastructure\Ember.Infrastructure.csproj --startup-project .\Ember.WebServer.csproj`

Run the server by starting debug in VsCode.