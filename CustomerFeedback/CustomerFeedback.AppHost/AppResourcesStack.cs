using Amazon.CDK;
using Amazon.CDK.AWS.S3;
using Amazon.CDK.AWS.SNS;
using Amazon.CDK.AWS.SNS.Subscriptions;
using Amazon.CDK.AWS.SQS;
using Constructs;

namespace CustomerFeedback.AppHost;

internal class AppResourcesStack : Stack
{
    internal ITopic FeedbackTopic { get;  }

    internal IQueue FeedbackQueue { get; }

    public AppResourcesStack(Construct scope, string id)
    : base(scope, id)
    {
        FeedbackTopic = new Topic(this, "FeedbackTopic");

        FeedbackQueue = new Queue(this, "FeedbackQueue");

        FeedbackTopic.AddSubscription(new SqsSubscription(FeedbackQueue));
    }
}
