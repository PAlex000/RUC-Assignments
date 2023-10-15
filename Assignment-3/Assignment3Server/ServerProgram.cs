using EchoServer;
using System;
using System.Collections.Generic;
using System.Linq;  // Required for using Max()
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;

var port = 5000;
TcpListener server = new TcpListener(IPAddress.Loopback, port);
server.Start();
Console.WriteLine("Server started");

List<Category> categories = new List<Category>
{
    new Category { Id = 1, Name = "Electronics" },
    new Category { Id = 2, Name = "Books" },
};


while (true)
{
    TcpClient client = server.AcceptTcpClient();
    Console.WriteLine("Client connected");

    Task.Run(() => HandleClient(client));
}

void HandleClient(TcpClient client)
{
    string rawRequest = client.MyRead();
    Console.WriteLine($"Received raw request: {rawRequest}");
    Response response = new Response();

    try
    {
        var request = JsonSerializer.Deserialize<Dictionary<string, object>>(rawRequest);
        Console.WriteLine($"Received raw request: {rawRequest}");

        if (request != null)
        {
            request = new Dictionary<string, object>(request, StringComparer.OrdinalIgnoreCase);
        }

        if (!request.ContainsKey("method"))
        {
            response.Status = "4 Missing method";
        }
        else if (!request.ContainsKey("date") || !long.TryParse(request["date"].ToString(), out _))
        {
            Console.WriteLine("Entered 'Missing or illegal date' block");
            response.Status = "4 Missing or illegal date";
        }
        else
        {
            string method = request["method"].ToString();
            if (method == "echo")
            {
                HandleEchoMethod(request, response);
            }
            else if (new[] { "create", "read", "update", "delete" }.Contains(method))
            {
                HandleCrudMethods(method, request, response);
            }
            else
            {
                response.Status = "4 Illegal method";
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Exception caught: {ex.Message}");
        response.Status = "6 Error";
    }
    Console.WriteLine($"Sending response: {JsonSerializer.Serialize(response)}");

    client.MyWrite(JsonSerializer.Serialize(response));
}

void HandleEchoMethod(Dictionary<string, object> request, Response response)
{
    if (!request.ContainsKey("Body"))
    {
        response.Status = "4 Missing Body";
    }
    else
    {
        response.Status = "1 Ok";
        response.Body = request["Body"].ToString();
    }
}

void HandleCrudMethods(string method, Dictionary<string, object> request, Response response)
{
    if (!request.ContainsKey("path"))
    {
        response.Status = "4 Missing resource";
    }
    else if (method == "read" && !request["path"].ToString().StartsWith("/api/"))
    {
        response.Status = "4 Bad Request";
    }

    else if (request["path"].ToString().StartsWith("/api/categories/"))
    {
        string[] pathElements = request["path"].ToString().Split('/');
        if (pathElements.Length >= 4 && !int.TryParse(pathElements[3], out _))
        {
            response.Status = "4 Bad Request";
            return; // Exit the method, as there's no need to continue.
        }
    }

    else if (new[] { "create", "update" }.Contains(method) && !request.ContainsKey("Body"))
    {
        response.Status = "4 Missing Body";
    }
    else
    {
        try
        {
            JsonSerializer.Deserialize<object>(request["Body"].ToString());
            response.Status = "1 Ok";
        }
        catch (JsonException)
        {
            response.Status = "4 Illegal Body";
        }
    }
}

public class Response
{
    public string Status { get; set; } = "6 Error";
    public string? Body { get; set; }
}

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
}