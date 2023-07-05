using System.Runtime.Serialization;

namespace TxWebTests.ProxySupport
{
    [DataContract]
    public class ProxyResponse
    {
        [DataMember(Name = "port")]
        public string Port { get; set; }
    }
}
