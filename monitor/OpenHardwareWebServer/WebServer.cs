/*
 
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
 
	Copyright (C) 2012 Prince Samuel <prince.samuel@gmail.com>
  Copyright (C) 2012-2013 Michael Möller <mmoeller@openhardwaremonitor.org>

*/

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using OpenHardwareMonitor.Hardware;
using System.Collections.Generic;
using SimpleHttpServer;

namespace OpenHardwareServer
{
  public class HttpServer
    {
    private SimpleHttpServer.HttpServer server;
        //private HttpListener listener;
        private int listenerPort, nodeCount;
        private Node root;
    private Thread thread;
    private SimpleHttpServer.Models.Route route;
        public HttpServer(Node node, int port)
        {
            root = node;
            listenerPort = port;

            //JSON node count. 
            nodeCount = 0;
            List<SimpleHttpServer.Models.Route> routes = new List<SimpleHttpServer.Models.Route>();
      SimpleHttpServer.Models.Route route = new SimpleHttpServer.Models.Route();
      route.Name = "Main";
      route.Method = "GET";
      
      route.UrlRegex = "(?s).*";
        route.Callable = (req) => {
          SimpleHttpServer.Models.HttpResponse response = new SimpleHttpServer.Models.HttpResponse();
          response.setContent(GetJSONData());
          response.Headers.Add("Content-Type", "application/json");

          return response;
        };

            routes.Add(route);

            try {
              server = new SimpleHttpServer.HttpServer(port, routes );
            }
            catch (PlatformNotSupportedException)
            {
        server = null;
              Console.WriteLine("SERVER ERROR");
            }
        }
    public void HandleRequests() {
      server.Listen();
    }

        public Boolean StartHTTPListener() {
          thread = new Thread(HandleRequests);
          thread.Start();

      return true;
    }
        public Boolean StopHTTPListener()
        {

            try
            {
        thread.Abort();
            }
            catch (HttpListenerException)
            {
            }
            catch (ThreadAbortException)
            {
            }
            catch (NullReferenceException)
            {
            }
            catch (Exception)
            {
            }
            return true;
        }

    private string GetJSONData()
        {

            string JSON = "{\"id\": 0, \"Text\": \"Sensor\", \"Children\": [";
            nodeCount = 1;
            JSON += GenerateJSON(root);
            JSON += "]";
            JSON += ", \"Min\": \"Min\"";
            JSON += ", \"Value\": \"Value\"";
            JSON += ", \"Max\": \"Max\"";
            JSON += ", \"ImageURL\": \"\"";
            JSON += "}";



      
      return JSON;

        }

        private string GenerateJSON(Node n)
        {
            string JSON = "{\"id\": " + nodeCount + ", \"Text\": \"" + n.Text
              + "\", \"Children\": [";
            nodeCount++;

            foreach (Node child in n.Nodes)
                JSON += GenerateJSON(child) + ", ";
            if (JSON.EndsWith(", "))
                JSON = JSON.Remove(JSON.LastIndexOf(","));
            JSON += "]";

            if (n is SensorNode)
            {
                JSON += ", \"Min\": \"" + ((SensorNode)n).Min + "\"";
                JSON += ", \"Value\": \"" + ((SensorNode)n).Value + "\"";
                JSON += ", \"Max\": \"" + ((SensorNode)n).Max + "\"";
                JSON += ", \"ImageURL\": \"images/transparent.png\"";
            }
            else if (n is HardwareNode)
            {
                JSON += ", \"Min\": \"\"";
                JSON += ", \"Value\": \"\"";
                JSON += ", \"Max\": \"\"";
                JSON += ", \"ImageURL\": \"images_icon/" +
                  GetHardwareImageFile((HardwareNode)n) + "\"";
            }
            else if (n is TypeNode)
            {
                JSON += ", \"Min\": \"\"";
                JSON += ", \"Value\": \"\"";
                JSON += ", \"Max\": \"\"";
                JSON += ", \"ImageURL\": \"images_icon/" +
                  GetTypeImageFile((TypeNode)n) + "\"";
            }
            else
            {
                JSON += ", \"Min\": \"\"";
                JSON += ", \"Value\": \"\"";
                JSON += ", \"Max\": \"\"";
                JSON += ", \"ImageURL\": \"images_icon/computer.png\"";
            }

            JSON += "}";
            return JSON;
        }

        private static void ReturnFile(HttpListenerContext context, string filePath)
        {
            context.Response.ContentType =
              GetcontentType(Path.GetExtension(filePath));
            const int bufferSize = 1024 * 512; //512KB
            var buffer = new byte[bufferSize];
            using (var fs = File.OpenRead(filePath))
            {

                context.Response.ContentLength64 = fs.Length;
                int read;
                while ((read = fs.Read(buffer, 0, buffer.Length)) > 0)
                    context.Response.OutputStream.Write(buffer, 0, read);
            }

            context.Response.OutputStream.Close();
        }

        private static string GetcontentType(string extension)
        {
            switch (extension)
            {
                case ".avi": return "video/x-msvideo";
                case ".css": return "text/css";
                case ".doc": return "application/msword";
                case ".gif": return "image/gif";
                case ".htm":
                case ".html": return "text/html";
                case ".jpg":
                case ".jpeg": return "image/jpeg";
                case ".js": return "application/x-javascript";
                case ".mp3": return "audio/mpeg";
                case ".png": return "image/png";
                case ".pdf": return "application/pdf";
                case ".ppt": return "application/vnd.ms-powerpoint";
                case ".zip": return "application/zip";
                case ".txt": return "text/plain";
                default: return "application/octet-stream";
            }
        }

        private static string GetHardwareImageFile(HardwareNode hn)
        {

            switch (hn.Hardware.HardwareType)
            {
                case HardwareType.CPU:
                    return "cpu.png";
                case HardwareType.GpuNvidia:
                    return "nvidia.png";
                case HardwareType.GpuAti:
                    return "ati.png";
                case HardwareType.HDD:
                    return "hdd.png";
                case HardwareType.Heatmaster:
                    return "bigng.png";
                case HardwareType.Mainboard:
                    return "mainboard.png";
                case HardwareType.SuperIO:
                    return "chip.png";
                case HardwareType.TBalancer:
                    return "bigng.png";
                case HardwareType.RAM:
                    return "ram.png";
                default:
                    return "cpu.png";
            }

        }

        private static string GetTypeImageFile(TypeNode tn)
        {

            switch (tn.SensorType)
            {
                case SensorType.Voltage:
                    return "voltage.png";
                case SensorType.Clock:
                    return "clock.png";
                case SensorType.Load:
                    return "load.png";
                case SensorType.Temperature:
                    return "temperature.png";
                case SensorType.Fan:
                    return "fan.png";
                case SensorType.Flow:
                    return "flow.png";
                case SensorType.Control:
                    return "control.png";
                case SensorType.Level:
                    return "level.png";
                case SensorType.Power:
                    return "power.png";
                default:
                    return "power.png";
            }

        }

        public int ListenerPort
        {
            get { return listenerPort; }
            set { listenerPort = value; }
        }

        ~HttpServer()
        {

            StopHTTPListener();
        }

        public void Quit()
        {
            StopHTTPListener();
    }



}
}
