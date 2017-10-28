using System.Runtime.Serialization;

namespace Function
{
    [DataContract]
    public class FcmMessageData
    {
        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "from_device")]
        public string FromDevice { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }

        [DataMember(Name = "to")]
        public string To { get; set; }

        [DataMember(Name = "task")]
        public string Task { get; set; }

        [DataMember(Name = "task_data")]
        public string TaskData { get; set; }
    }
}