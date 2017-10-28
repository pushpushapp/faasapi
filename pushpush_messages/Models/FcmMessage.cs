using System.Runtime.Serialization;

namespace Function
{
    [DataContract]
    public class FcmMessage
    {
        public FcmMessage()
        {
            Data = new FcmMessageData();
        }

        [DataMember(Name = "registration_ids")]
        public string[] RegistrationIds { get; set; }

        [DataMember(Name = "to")]
        public string To { get; set; }

        [DataMember(Name = "data")]
        public FcmMessageData Data { get; set; }
    }
}