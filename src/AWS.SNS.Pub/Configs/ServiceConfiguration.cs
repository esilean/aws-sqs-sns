namespace AWS.SNS.Pub.Configs
{
    public class ServiceConfiguration
    {
        public AWSSNS AWSSNS { get; set; }
    }
    public class AWSSNS
    {
        public string TopicARN { get; set; }

    }
}
