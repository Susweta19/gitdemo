using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Runtime.Serialization.Json;
using System.IO;

namespace TxWebTests.ProxySupport
{
    public class ProxyHandler
    {
        private readonly string _url;

        public ProxyHandler(string url)
        {
            this._url = url;
        }

        public string StartProxy()
        {
            using (WebClient webClient = new WebClient())
            {
                NameValueCollection data = new NameValueCollection();
                byte[] byteResp = webClient.UploadValues(_url, "POST", data);
                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof (ProxyResponse));
                MemoryStream stream = new MemoryStream(byteResp);
                object objResponse = jsonSerializer.ReadObject(stream);
                ProxyResponse jsonResponse = objResponse as ProxyResponse;
                return jsonResponse.Port;
            }
        }

        public void StartMeasure(string measure, string port)
        {
            using (WebClient webClient = new WebClient())
            {
                NameValueCollection data = new NameValueCollection {["initialPageRef"] = measure};
                webClient.UploadValues("http://localhost:9090/proxy/" + port + "/har", "PUT", data);
            }

        }

        public string StopMeasure(string port)
        {
            using (WebClient webClient = new WebClient())
            {
                byte[] byteResp = webClient.DownloadData("http://localhost:9090/proxy/" + port + "/har");
                return Encoding.ASCII.GetString(byteResp);
            }
        }

        public void NewTest(string test)
        {

        }

        public void StopTest()
        {

        }
    }
}
