using System.Runtime.Serialization;

namespace Function
{
    [DataContract]
    public class AuthKey
    {
        [DataMember(Name = "auth_key", IsRequired = true)]
        public string Key { get; set; }
    }
}