# SalesforceGrpcClient

## Pre-requisites
- [.Net 6.0](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)  
- IDE or Editor such as [Visual Studio Code](https://code.visualstudio.com/)  
- Access Token, Instance URL and Tenant Id from Salesforce org

## The example
The example makes certain assumptions
- salesforce.proto file in the Protos folder matches the latest Salesforce schemas
  - the gRPC library will automatically create .Net classes based on this definition file
- uses the default well known Salesforce gRPC endpoint
- will subscribe to a Platform event/Topic named Simple_Event__e
- the access token of the user being used to connect to Salesforce has permissions to access the resources

To run the example, open the [Program.cs](./Program.cs) and change the variable placeholders for access token, instance url and tenantId

```C#
var accessToken = "Replace with your access token";
var instanceUrl = "Replace with your instance URL, i.e. https://your-org.my.salesforce.com";
var tenantId = "Replace with your tenantId";
```

Note: all of these values can be obtained using `sfdx force:org:display -u <your org alias>`

Executing `dotnet run` in a command prompt will restore the libraries, build the project and run it. However, you can execute these steps separately with the following commands
```
dotnet restore
dotnet build
dotnet run
```

After building or running you will be able to see the code generated from the proto definition in the obj/Debug/net6.0/Protos folder.

The program is a console app, the program creates a channel, subscribes to a top and wait for a messages ot times out if none have been received. While the program is running and after the console has displayed "Subscribing to topic and waiting for event ...", open Salesforce and publish a platform event. The event information should then be displayed in the console.

## Salesforce gRPC platform event messages
Included in the project folder is an [example of the current message structure](example-payload.json) of a Salesforce platform event when using gRPC. All of the information apart from the event payload is plain text and can be easily read. The payload itself is in binary encoded [Avro format](https://avro.apache.org/) and is described in the [Salesforce documentation](https://developer.salesforce.com/docs/platform/pub-sub-api/guide/event-avro-serialization.html). Therefore if you convert the event payload value to UTF8 string or use ToByteArray and Text encoder you are likely to get unexpected results.

To help with the de-serialisation of the payload property I have referenced a simple to use library to [convert to and from Avro format](https://github.com/AdrianStrugala/AvroConvert); I choose this library as the author has made it very simple to use and supplimented the code with a helpful write up on [c-sharpcorner](https://www.c-sharpcorner.com/article/how-to-work-with-avro-data-type-in-net-environment/). The POCO (Simple_Event__e) used to store the deserialise message was created using this library. 

In order to see the Topic schema and generated POCO (or to generate your own POCOs from your events) open [Program.cs](./Program.cs) and set the following variables to true

```C#
var viewSchema = false;
var viewGeneratedModel = false;
```


