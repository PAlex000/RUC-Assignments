using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Linq;

namespace Server
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("The Server");

            // Sample data for categories
            var categories = new List<Category>
            {
                new Category { Cid = 1, Name = "Beverages" },
                new Category { Cid = 2, Name = "Condiments" },
                new Category { Cid = 3, Name = "Confections" }
            };

            var server = new TcpListener(IPAddress.Loopback, 5000);
            server.Start();
            Console.WriteLine("Server started...");

            while (true)
            {
                var client = server.AcceptTcpClient();
                Console.WriteLine("Client connected...");

                try
                {
                    HandleClient(client, categories);
                }
                catch (Exception)
                {
                    Console.WriteLine("Unable to communicate with the client...");
                }
            }
        }

        public static void HandleClient(TcpClient client, List<Category>? categories)
        {
            var stream = client.GetStream();
            var buffer = new byte[1024];
            var rcnt = stream.Read(buffer);
            var requestText = Encoding.UTF8.GetString(buffer, 0, rcnt);
            var request = JsonSerializer.Deserialize<Request>(requestText);

            if (request != null)
            {
                VerifyRequest(stream, request, categories);
            }

            stream.Close();
        }

        public static void SendResponse(NetworkStream stream, Response response)
        {
            var responseText = JsonSerializer.Serialize<Response>(response);
            var responseBuffer = Encoding.UTF8.GetBytes(responseText);
            stream.Write(responseBuffer);
        }

        public static Response CreateResponse(string status, string body = "")
        {
            return new Response
            {
                Status = status,
                Body = body
            };
        }

        public static bool IsValidJson(string json)
        {
            try
            {
                // Attempt to parse the JSON
                _ = JsonSerializer.Deserialize<object>(json);
                return true;
            }
            catch (JsonException)
            {
                // JSON parsing failed, so it's not valid JSON
                return false;
            }
        }

        public static void VerifyRequest(NetworkStream stream, Request request, List<Category> categories)
        {
            if (request.Method == "update")
            {
                // Check if the request body is valid JSON
                if (!IsValidJson(request.Body))
                {
                    Response response = CreateResponse("4 Bad Request", "Illegal body");
                    SendResponse(stream, response);
                    return; // Exit the method to prevent further processing
                }
           
                if (string.IsNullOrEmpty(request.Path) || !request.Path.StartsWith("/api/categories/"))
                {
                    Response response = CreateResponse("4 Bad Request");
                    SendResponse(stream, response);
                    return; // Exit the method to prevent further processing
                }

                // Extract the category ID from the path
                string categoryIdStr = request.Path.Replace("/api/categories/", "");

                // Check if the category ID is a valid integer
                if (!int.TryParse(categoryIdStr, out _))
                {
                    Response response = CreateResponse("4 Bad Request");
                    SendResponse(stream, response);
                    return; // Exit the method to prevent further processing
                }

                if (string.IsNullOrEmpty(request.Path) || !request.Path.StartsWith("/api/categories/"))
                {
                    Response response = CreateResponse("4 Bad Request");
                    SendResponse(stream, response);
                    return; // Exit the method to prevent further processing
                }

                // Extract the category ID from the path
                _ = request.Path.Replace("/api/categories/", "");

                // Check if the category ID is a valid integer
                if (!int.TryParse(categoryIdStr, out _))
                {
                    Response response = CreateResponse("4 Bad Request");
                    SendResponse(stream, response);
                    return; // Exit the method to prevent further processing
                }
                if (string.IsNullOrEmpty(request.Body))
                {
                    Response response = CreateResponse("4 missing body");
                    SendResponse(stream, response);
                    return; // Exit the method to prevent further processing
                }

                // Check if the body is valid JSON
                if (!IsValidJson(request.Body))
                {
                    Response response = CreateResponse("4 illegal body");
                    SendResponse(stream, response);
                    return; // Exit the method to prevent further processing
                }

            }
            if (string.IsNullOrEmpty(request.Method) || string.IsNullOrEmpty(request.Path) || string.IsNullOrEmpty(request.Body) || request.DateTime == 0)
            {
                Response response = CreateResponse("4 Bad Request", "Missing method, path, body, or date");
                SendResponse(stream, response);
                return; // Exit the method to prevent further processing
            }

            if (request.Method != "read" && request.Method != "create" && request.Method != "update" && request.Method != "delete" && request.Method != "echo")
            {
                Response response = CreateResponse("4 Bad Request", "Invalid method");
                SendResponse(stream, response);
                return; // Exit the method to prevent further processing
            }

            if (request.Method == "read" || request.Method == "create" || request.Method == "update" || request.Method == "delete")
            {
                if (string.IsNullOrEmpty(request.Path) || string.IsNullOrEmpty(request.Body))
                {
                    Response response = CreateResponse("4 Bad Request", "Missing resource");
                    SendResponse(stream, response);
                    return; // Exit the method to prevent further processing
                }
            }
        }
    }
}
