# Salesforce GRPC .Net/C# Subscribe Example

Disclaimer: I am by no means a gRPC/Protobuf expert, this is my first attempt at using gRPC in any language!

## Prerequisites

- Salesforce DX  
- .Net 6.0

## Background

Motivation for this was to see how easy/hard it is to subscribe ro and publish Salesforce platform events from C# using gRPC.

Prior to gRPC subscribing to Salesforce platform events has been relatively easy in languages such as JavaScript/Nodejs/TypeScript/Java because of good library support but made slightly more complex when using C# due to there only being a few libraries supporting [Bayeaux/CometD](https://developer.salesforce.com/docs/atlas.en-us.api_streaming.meta/api_streaming/BayeauxProtocolAndCometD.htm). I'd always hoped that I could pop in a commercial library such as SignalR and it would all work but alas, no. The solution generally proposed it to make use of web hooks; while being simple this does have some advantages. Enter gRPC as another way of subscribing to Salesforce platform events.

The official [Salesforce gRPC documentation](https://developer.salesforce.com/docs/platform/pub-sub-api/guide/grpc-api.html) is very well detailed but for someone who has not had any expose to gRPC before there is a steep learning curve. While the concepts might be simple to follow, the implimentation is not quite so easy, to help with this Salesforce have created a source code repository with [examples for Go, Java, Python](https://github.com/developerforce/pub-sub-api). Following the Java examples is straight forward, once you appreciate how many elements of the process are being handled by external libraries. 

Library support was one of the issues I encountered, gRPC support for .Net is strong but the gRPC libraries appear to have changed in use between different framework versions and if you do not realise this, it is easy to be confused with different techniques which achieve the same outcome. All in, this simple demonstration code took around two days and a bit days to complete!

## The example
Example contains two project
- SFDX project for Salesforce containing a Platform event named Simple_Event__e
- .Net/C# project that subscribes to the Platform event Simple_Event__e

To use the example
- create a scratch org and push the code; more information is contained in the GrpcSalesforce [readme](./GrpcSalesforce/Readme.md)
- using SFDX obtain an access token, instance url and tenant id; run the solution and generate a platform event; more information is contained in the GrpcSalesforceClinet [readme](./GrpcSalesforceClient/Readme.md)

Note: it is unlikely that this project will be maintained, it is just here to act as an example and purpose of being a short term reference!
