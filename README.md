# PublicApiRepo
This repo has three Azure Functions built with .Net Core 6 and Azure Storage Emulator. It fetches data from https://api.publicapis.org/random?auth=null periodically and stores logs in Azure Table and payload in Blob Storage.

### FetchApiDataTimerFunction.cs
  This file contains Timer Triggered Azure function which fetches data and then stores logs in Azure Table Storage and actual payload in Azure Blob Storage.
 
### GetLogsFunction.cs
  This file contains HTTP Triggered Azure function which allows client to fetch logs based on specific time period (from/to).
  
### GetPayloadFunction.cs
  This file contains another HTTP Triggered Azure function which allows client to get payload of specific log entry.

#### Note: This is not a production ready solution because it's built by using Azure Storage Emulator so it's missing required configurations for cloud deployment.
