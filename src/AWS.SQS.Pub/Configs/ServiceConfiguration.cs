namespace AWS.SQS.Pub.Configs
{
    public class ServiceConfiguration
    {
        public AWSSQS AWSSQS { get; set; }
    }
    public class AWSSQS
    {
        public string QueueUrl { get; set; }
        public string DLQueueUrl { get; set; }
        public string FifoQueueUrl { get; set; }

    }
}
