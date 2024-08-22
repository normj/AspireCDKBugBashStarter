using Amazon;
using CustomerFeedback.AppHost;

var builder = DistributedApplication.CreateBuilder(args);

// Update this to the profile and region you want to test with. 
// This can be removed if you want to rely on default environmental profile region setting.
var awsConfig = builder.AddAWSSDKConfig()
                        .WithProfile("default")
                        .WithRegion(RegionEndpoint.USWest2);

// -------------------------------
// The code below shows the 3 different ways you can create AWS resources with our Aspire work.
// Uncomment the version you wish to try. Only one Use method should be uncommented.
// -------------------------------

// UseCloudFormationTemplate();

UseCDKStack();

// UseCDKExtensionMethods();

// This is the functionality we have shipped today. The resources defined in the aws-resources.template
// JSON CloudFormation template.
void UseCloudFormationTemplate()
{
    var awsResources = builder.AddAWSCloudFormationTemplate("AppResourcesCloudFormationTemplate", "aws-resources.template")
                              .WithReference(awsConfig);

    builder.AddProject<Projects.CustomerFeedback_Frontend>("customerfeedback-frontend")
           // CF template's output parameters are added as configuration environment variables
           // adding the prefix AWS::Resources for all output property property names.
           .WithReference(awsResources);

    builder.AddProject<Projects.CustomerFeedback_Processor>("customerfeedback-processor")
           .WithReference(awsResources);

    builder.Build().Run();
}

// This version demostrats creating your resources via a CDK Stack. A subclass of Stack is defined in the AppResourcesStack.cs
// and the resources needed for the application are defined inside this stack. This is the common CDK pattern
// for creating resources using a Stack subclass.
void UseCDKStack()
{
    var awsResources = builder.AddAWSCDKStack("CustomerFeedbackAppResourcesCDKStack", scope => new AppResourcesStack(scope, "AppResources"))
                              .WithReference(awsConfig);

    awsResources.AddOutput("FeedbackQueueUrl", stack => stack.FeedbackQueue.QueueUrl);
    awsResources.AddOutput("FeedbackTopicArn", stack => stack.FeedbackTopic.TopicArn);

    builder.AddProject<Projects.CustomerFeedback_Frontend>("customerfeedback-frontend")
       // The CDK stack's output parameters are added as configuration environment variables
       // adding the prefix AWS::Resources for all output property property names.
       .WithReference(awsResources);

    builder.AddProject<Projects.CustomerFeedback_Processor>("customerfeedback-processor")
           .WithReference(awsResources);

    builder.Build().Run();
}

// This version avoid the need of creating a Stack subclass and uses Aspire extension methods
// on the Aspire resource wrapping the stack to create AWS resources. There are limited extension
// methods now but it would be an areas we could expand to keep making the experience easier.
void UseCDKExtensionMethods()
{
    var awsResources = builder.AddAWSCDKStack("CustomerFeedbackAppResourcesCDKExtensionMethods");
    var topic = awsResources.AddSNSTopic("FeedbackTopic");
    var queue = awsResources.AddSQSQueue("FeedbackQueue");


    topic.AddSubscription(queue);

    builder.AddProject<Projects.CustomerFeedback_Frontend>("customerfeedback-frontend")
           .WithEnvironment("AWS__Resources__FeedbackTopicArn", topic, t => t.TopicArn);

    builder.AddProject<Projects.CustomerFeedback_Processor>("customerfeedback-processor")
           .WithEnvironment("AWS__Resources__FeedbackQueueUrl", queue, q => q.QueueUrl);

    builder.Build().Run();
}
