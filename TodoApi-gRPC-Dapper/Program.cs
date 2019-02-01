using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TodoApi_gRPC_Dapper.Implements;
using TodoApi_gRPC_Dapper.Models.Persistance;
using Grpc;
using Grpc.Core;
using Proto.Todo;

namespace TodoApi_gRPC_Dapper
{
    public class Program
    {
        public static void Main(string[] args)
        {
            const int PORT = 5000;
            var todoImplement = new TodoImplement(new UnitOfWork());
            var server = new Server
            {
                Services =
                {
                    TodoService.BindService(todoImplement)
                },
                Ports = { new ServerPort("0.0.0.0", PORT, ServerCredentials.Insecure) }
            };
            server.Start();

            Console.WriteLine("gRPC Todo server listening on port " + PORT);
            Console.WriteLine("Press any key to stop the server...");
            Console.Read();

            server.ShutdownAsync().Wait();
            // CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
