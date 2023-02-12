# Salesforce GRPC .Net/C# Subscribe Example

Disclaimer: I am by no means a gRPC/Protobuf expert, this is my first attempt at using gRPC in any language!

## Prerequisites

- Salesforce DX  
- .Net 6.0

## Background

Motivation for this was to see how easy/hard it is to subscribe ro and publish Salesforce platform events from C# using gRPC.

Prior to gRPC subscribing to Salesforce platform events has been relatively easy in languages such as Java/JavaScript/Nodejs/TypeScript because of good library support but made slightly more complex when using C# due to there only being fewer libraries supporting [Bayeaux/CometD](https://developer.salesforce.com/docs/atlas.en-us.api_streaming.meta/api_streaming/BayeauxProtocolAndCometD.htm). I'd always hoped that I could pop in a commercial library such as SignalR, point it to Salesforce and it would  work but alas, no. An alternative solution to subscribing directly to events is to make use of web hooks. Enter gRPC as another way of subscribing to Salesforce platform events.

The official [Salesforce gRPC documentation](https://developer.salesforce.com/docs/platform/pub-sub-api/guide/grpc-api.html) is very well detailed but for someone who has not had any exposure to gRPC before there is a steep learning curve. While the concepts might be simple to follow, the implimentation is not quite so easy, to help with this Salesforce have created a source code repository with [examples for Go, Java, Python](https://github.com/developerforce/pub-sub-api). Following the Java examples is straight forward, once you appreciate how many elements of the process are being handled by external libraries. 

In fact library support was one of the issues I encountered, gRPC support for .Net is strong but the gRPC libraries appear to have changed between different framework versions and if you do not realise this you will end up with a mixture of old and new techniques which do not always work together. All in, this simple demonstration code took around two days and a bit days to complete!

## The example
Example contains two project
- SFDX project for Salesforce containing a Platform event named Simple_Event__e
- .Net/C# project that subscribes to the Platform event Simple_Event__e

To use the example
- create a scratch org and push the code; more information is contained in the GrpcSalesforce [readme](./GrpcSalesforce/Readme.md)
- using SFDX obtain an access token, instance url and tenant id; run the solution and generate a platform event; more information is contained in the GrpcSalesforceClinet [readme](./GrpcSalesforceClient/Readme.md)

Note: it is unlikely that this project will be maintained, it is just here to act as an example and purpose of being a short term reference!

## Learning gRPC with C#
Started out by following the steps at [Create gRPC services and methods](https://learn.microsoft.com/en-us/aspnet/core/grpc/basics?view=aspnetcore-7.0) to create the Greeter service and clients. After following the steps on the article it is worth spending time to work out how it was put together, particularly the code generated from the proto file.

Next step is to download the Salesforce proto file and scan the code that is generated from it.

One of the key differences and why it took a while to complete this example (which was started November 2022 and completed just before end December 2022!) is because Salesforce gRPC requires Authentication with each call. It was clear what metadata was required to authenticate the calls to Salesforce but unclear how to set the metadata for each call! The Authentication for each requet in this example is handled by an interceptor, I guess this is a middleware component that takes a similar approach to how polly or messagehandlers work for http requests.

When you have authenticted with Salesforce it is easy to download a Topic schema although working out how to subscribe to an event took longer than expected; it was helpful seeing other .Net gRPC examples and looking through issues such as [async duplex streaming call hang](https://github.com/shaan1337/async-duplex-streaming-call-hang/blob/main/Program.cs). The final piece of the puzzle was decrypting the event payload, fortunately the heavy lifting is done by a third party library.

[Greeter Github]: (https://github.com/grpc/grpc-dotnet/blob/master/examples/Greeter/Server/Services/GreeterService.cs): "Greeter Github"
