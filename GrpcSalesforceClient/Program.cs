using Grpc.Core;
using Grpc.Net.Client;
using GrpcSalesforceClient;
using SolTechnology.Avro;

var salesForceGrpcAddress = "https://api.pubsub.salesforce.com:7443";
var topicName = "/event/Simple_Event__e";

var viewSchema = false;
var viewGeneratedModel = false;

// Information obtained from SFDX
var accessToken = ""; // <== Replace with your access token
var instanceUrl = ""; // <== Replace with your instance URL, i.e. https://your-org.my.salesforce.com
var tenantId = ""; // <== Replace with your tenantId

if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(instanceUrl) || string.IsNullOrEmpty(tenantId))
{
    throw new StringNullOrEmptyException("Salesforce access token, instance url or tenant is null or empty");
}

Console.WriteLine("Start");

var credentials = CallCredentials.FromInterceptor((context, metadata) =>
    {
        metadata.Add("accesstoken", accessToken);
        metadata.Add("instanceurl", instanceUrl);
        metadata.Add("tenantid", tenantId);
        return Task.CompletedTask;
    });

var channel = GrpcChannel.ForAddress(salesForceGrpcAddress, new GrpcChannelOptions
{
    Credentials = ChannelCredentials.Create(new SslCredentials(), credentials)
});

var client = new GrpcSalesforceClient.PubSub.PubSubClient(channel);
Console.WriteLine($"Created grpc channel [{channel.Target}] ({channel.State})");

Console.WriteLine($"Requesting topic [{topicName}]");
var topicRequest = new TopicRequest { TopicName = topicName };
var topicInfo = client.GetTopic(topicRequest);
if (topicInfo.CanSubscribe)
{
    Console.WriteLine($"Can subscribe to [{topicInfo.TopicName}]");
}
else
{
    Console.WriteLine($"Cannot subscribe to [{topicInfo.TopicName}]");
    return;
}

var schemaRequest = new SchemaRequest { SchemaId = topicInfo.SchemaId };
var schemaInfo = client.GetSchema(schemaRequest);
if (viewSchema)
{
    Console.WriteLine(schemaInfo.SchemaJson);
}

if (viewGeneratedModel)
{
    var model = AvroConvert.GenerateModel(schemaInfo.SchemaJson);
    Console.WriteLine(model);
}

Console.WriteLine("Subscribing to topic and waiting for event ...");
var subscription = client.Subscribe();
var request = subscription.RequestStream;
var response = subscription.ResponseStream;
var requestMessage = new FetchRequest { TopicName = topicInfo.TopicName, NumRequested = 1 };
await request.WriteAsync(requestMessage);
var res = await response.MoveNext();

foreach (ConsumerEvent ce in response.Current.Events)
{
    Console.WriteLine($"Received event with schema id of [{ce.Event.SchemaId}]");
    Console.WriteLine($"Avro encrypted event payload [{ce.Event.Payload.ToStringUtf8()}]");

    var converted = AvroConvert.DeserializeHeadless<Simple_Event__e>(ce.Event.Payload.ToByteArray(), schemaInfo.SchemaJson);
    Console.WriteLine($"Avro decrypted event payload");
    Console.WriteLine($"Created on [{converted.localCreatedDate()}]");
    Console.WriteLine($"Created by user [{converted.CreatedById}]");
    Console.WriteLine($"Converted has action [{converted.Action__c}]");
}

Console.WriteLine("End");

// Model generated from AvroConvert
public class Simple_Event__e
{
    /// <summary>
    /// CreatedDate:DateTime
    /// </summary>
    public long CreatedDate { get; set; }
    /// <summary>
    /// CreatedBy:EntityId
    /// </summary>
    public string? CreatedById { get; set; }
    /// <summary>
    /// Data:Text:00N3G00000FCIa6
    /// </summary>
    public string? Action__c { get; set; }

    public DateTime localCreatedDate()
    {
        DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        DateTime localDateTime = epoch.AddMilliseconds(CreatedDate).ToLocalTime();
        return localDateTime;
    }
}

public class StringNullOrEmptyException : Exception
{
    public StringNullOrEmptyException()
    {
    }

    public StringNullOrEmptyException(string message)
        : base(message)
    {
    }

    public StringNullOrEmptyException(string message, Exception inner)
        : base(message, inner)
    {
    }
}