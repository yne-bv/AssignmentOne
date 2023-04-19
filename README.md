# FunctionApp1
## Setup
Setup local databases to run it locally. Can be done with AzureStorageEmulator.
Run Azurite (npm install -g azurite) and when you see the endpoints running you can start the function.

## Changes I would like to make
- More tests
- Split the main function, to have a new one with a blob trigger or table trigger.
- Set all configurable options in local.settings/appsettings
- Inject the settings in the constructor
- Common place to store and use Model (PublicApi)
- Security - setup access for who can access the application.
