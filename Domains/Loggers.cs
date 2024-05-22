using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CQRSWithDocker_Identity.Domains
{
    public class Loggers
    {
        public Loggers(string endpoint, string request, string response)
        {
            Endpoint = endpoint;
            Request = request;
            Response = response;
        }

        public int Id { get; set; }
        public string Endpoint { get; set; }
        public string Response { get; set; }
        public string Request {  get; set; }


    }
}
