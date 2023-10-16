using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Text;
using categoryNS;
using responseNS;
using requestNS;

public class Server {
    public static void Main()
    {
        int port = 5000;
        var server = new TcpListener(IPAddress.Loopback, port);
        server.Start();
        List<Category> categories = new List<Category>
        {
            new Category{Id = 1, Name = "Beverages"},
            new Category{Id = 2, Name = "Condiments"},
            new Category{Id = 3, Name = "Confections"}
        };
        Console.WriteLine("Server started");

        while (true)
        {
            var client = server.AcceptTcpClient();
            Console.WriteLine("Client connected");
            var t = new Thread(() => HandleClientWithThread(client, categories));
            t.Start();
        }
    }
    private static void HandleClientWithThread(object? obj, List<Category> categories)
    {
        var client = obj as TcpClient;

        if (client == null) return;

        HandleClient(client, categories);
    }
    private static void HandleClient(TcpClient client, List<Category> categories)
    {
        Response request = client.ReadResponse(categories);
        client.SendRequest(request.ToJson());
    }
}

public static class Util
{
    public static string ToJson(this object data)
    {
        return JsonSerializer.Serialize(data, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
    }

    public static T FromJson<T>(this string element)
    {
        return JsonSerializer.Deserialize<T>(element);
    }

    public static void SendRequest(this TcpClient client, string request)
    {
        var msg = Encoding.UTF8.GetBytes(request);
        client.GetStream().Write(msg, 0, msg.Length);
    }

    public static Response ReadResponse(this TcpClient client,List<Category> categories)
    {
        var strm = client.GetStream();
        byte[] resp = new byte[2048];
        using (var memStream = new MemoryStream())
        {
            int bytesread = 0;
            Response response = new Response();
            do
            {
                bytesread = strm.Read(resp, 0, resp.Length);
                memStream.Write(resp, 0, bytesread);
            } while (bytesread == 2048);
            string responseData = Encoding.UTF8.GetString(memStream.ToArray());
            var requestData = JsonSerializer.Deserialize<Request>(responseData, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            List<string> methods = createMethodList();
            if (checkMethod(requestData.Method, requestData.Path, methods) != null)
            {
                response.Status += checkMethod(requestData.Method, requestData.Path, methods);
            }
            if (checkDate(requestData.Date) != null)
                response.Status += checkDate(requestData.Date);

            if (requestData.Method == "echo" ||
                requestData.Method == "create" ||
                requestData.Method == "update")
            {
                if (checkBody(requestData.Body) != null)
                    response.Status += (checkBody(requestData.Body));
            }
            //Echo case
            if (requestData.Method == "echo")
                response.Body = requestData.Body;
            //Path cases
            string[] tempPath = new string[0];
            if (requestData.Path != null)
            {
                if (!requestData.Path.StartsWith("/api/categories"))
                    response.Status += "4 Bad Request";
                tempPath = requestData.Path.Split('/'); // "/api/categories/1"
                if (requestData.Method != "create")
                {
                    if (tempPath.Length > 3) // So it's not just /api/categories
                        try
                        {
                            Int16.Parse(tempPath[3]);
                        }
                        catch (Exception)
                        {
                            response.Status += "4 Bad Request";
                        }
                }
                else
                {
                    if (tempPath.Length != 3 && requestData.Body != null)
                        response.Status = "4 Bad Request";
                }
                if (requestData.Method == "update" && requestData.Body != null && requestData.Date.Length == 10)
                    if (tempPath.Length != 4)
                        response.Status = "4 Bad Request";
                if (requestData.Method == "delete")
                    if (tempPath.Length != 4)
                        response.Status = "4 Bad Request";
            }
            if (requestData.Method == "read" && response.Status == null)
            {
                response.Status = "1 Ok";
                if (tempPath.Length == 4)
                {
                    var uniqueID = Int16.Parse(tempPath[3]);
                    foreach (var item in categories)
                    {
                        if (item.Id == uniqueID)
                            response.Body = item.ToJson();
                    }
                    if (response.Body == null)
                        response.Status = "5 Not Found";
                }
                else
                    response.Body = categories.ToJson();
            }
            if (requestData.Method == "update" && response.Status == null)
            {

                var uniqueID = Int16.Parse(tempPath[3]);
                var element = FromJson<Category>(requestData.Body);
                try
                {
                    int index = categories.FindIndex(item => item.Id == uniqueID);
                    categories[index] = element;
                    response.Status = "3 updated";
                }
                catch (Exception)
                {
                    response.Status = "5 Not Found";
                }
            }
            if (requestData.Method == "create" && response.Status == null)
            {
                Category newCat = new Category();
                newCat.Id = categories.Count + 1;
                newCat.Name = FromJson<Category>(requestData.Body).Name;
                categories.Add(newCat);
                response.Body = newCat.ToJson();
                response.Status = "2 Created";
            }
            if (requestData.Method == "delete" && response.Status == null)
            {
                try
                {
                    var uniqueID = Int16.Parse(tempPath[3]);
                    categories.RemoveAt(uniqueID - 1);
                    response.Status = "1 Ok";
                }
                catch (Exception)
                {
                    response.Status = "5 Not Found";
                }
            }
            return response;
        }
    }
    private static List<string> createMethodList()
    {
        return new List<string> {"create","read","update","delete", "echo"};
    }
    private static string checkMethod(string method, string path, List<string> methods)
    {
        string status = null;
        if (method == null)
            status += "Missing method, ";
        else if (!methods.Contains(method))
            status += "illegal method";
        if (path == null && method != "echo")
            status += "missing resource, ";
        return status;
    }
    private static string checkDate(string date)
    {
        string status = null;
        if (date == null)
            status += "missing date, ";
        else if (date.Length != 10)
            status += "illegal date, ";
        return status;
    }
    private static string checkBody(string body)
    {
        string status = null;
        if (body == null)
            status += "missing body, ";
        try
        {
            FromJson<Category>(body);
        }
        catch (Exception)
        {
            status += "illegal body, ";
        }
        return status;
    }
    private static string UnixTimestamp()
    {
        return DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
    }
}

