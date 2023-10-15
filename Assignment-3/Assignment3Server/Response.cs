using System;

namespace Server
{
    // Response class represents the structure of responses sent to clients
    public class Response
    {
        public string Status { get; internal set; }   // HTTP status code and message
        public string Body { get; set; }              // Response body (if applicable)
    }
}
