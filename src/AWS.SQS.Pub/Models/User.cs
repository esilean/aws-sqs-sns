using Newtonsoft.Json;
using System;

namespace AWS.SQS.Pub.Models
{
    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string EmailId { get; set; }
    }
    public class UserDetail : User
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
    public class AllMessage
    {
        public AllMessage()
        {
            UserDetail = new UserDetail();
        }
        public string MessageId { get; set; }
        public string ReceiptHandle { get; set; }
        public UserDetail UserDetail { get; set; }
    }

    public class SNSMessage
    {
        public string Message { get; set; }
    }

    public class DeleteMessage
    {
        public string ReceiptHandle { get; set; }
    }
}
