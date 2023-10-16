﻿using System.Net.Sockets;
using System.Text;

namespace Assignment3Server;

    public static class MyTcpUtil
    {
        public static string MyRead(this TcpClient client)
        {
            var stream = client.GetStream();
            var buffer = new byte[4096];
            var readCnt = stream.Read(buffer);

            return Encoding.UTF8.GetString(buffer, 0, readCnt);
        }

        public static void MyWrite(this TcpClient client, string response)
        {
            var stream = client.GetStream();
            stream.Write(Encoding.UTF8.GetBytes(response));
        }
    }
