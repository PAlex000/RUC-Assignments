using System;

namespace Server
{
    // Request class represents the structure of incoming client requests
    public class Request
    {
        public string Method { get; set; }      // HTTP method (e.g., create, read, update, delete, echo)
        public string Path { get; set; }        // URL path
        public int DateTime { get; set; }       // Timestamp
        public string Body { get; set; }        // Request body (if applicable)
    }
}
