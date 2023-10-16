using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Text;
using System.Text.Json.Serialization;

var port = 5000;

var server = new TcpListener(IPAddress.Loopback, port);
server.Start();

Console.WriteLine("Server started");

while (true)
{
    var client = server.AcceptTcpClient();
    Response request = client.ReadResponse();
    client.SendRequest(request.ToJson());
    //client.Close();
}

public class Response
{
    public string Status { get; set; }
    public string Body { get; set; }
}
public class Request
{
    public string Method { get; set; }
    public string Path { get; set; }
    public string Date { get; set; }
    public string Body { get; set; }
}

public class Category
{
    [JsonPropertyName("cid")]
    public int Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
}

public static class Util
{
    public static string ToJson(this object data)
    {
        return JsonSerializer.Serialize(data, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
    }

    public static T FromJson<T>(this string element)
    {
        return JsonSerializer.Deserialize<T>(element, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
    }

    public static void SendRequest(this TcpClient client, string request)
    {
        var msg = Encoding.UTF8.GetBytes(request);
        client.GetStream().Write(msg, 0, msg.Length);
    }

    public static Response ReadResponse(this TcpClient client)
    {
        var strm = client.GetStream();
        //strm.ReadTimeout = 250;
        byte[] resp = new byte[2048];
        var categories = new List<Category>
        {
            new Category{Id = 1, Name = "Beverages"},
            new Category{Id = 2, Name = "Condiments"},
            new Category{Id = 3, Name = "Confections"}
        };
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
            List<string> methods = new List<string>();
            methods.Add("create");
            methods.Add("read");
            methods.Add("update");
            methods.Add("delete");
            methods.Add("echo");

            //Method cases
            if (requestData.Method == null) 
                response.Status += "Missing method, ";
            else if (!methods.Contains(requestData.Method))
                response.Status += "illegal method";
            if (requestData.Path == null && requestData.Method != "echo")
                response.Status += "missing resource, ";
            //Date cases
            if (requestData.Date == null)
                response.Status += "missing date, ";
            else if (requestData.Date.Length != 10)
                response.Status += "illegal date, ";
            //Body cases
            if (requestData.Method == "echo" || requestData.Method == "create" || requestData.Method == "update")
            {
                if (requestData.Body == null)
                    response.Status += "missing body, ";
                try
                {
                    FromJson<String>(response.Body);
                }
                catch (Exception)
                {
                    response.Status += "illegal body, ";
                }
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
                }
                else
                    response.Body = categories.ToJson();
            }
            return response;
        }
    }
    private static string UnixTimestamp()
    {
        return DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
    }
}

