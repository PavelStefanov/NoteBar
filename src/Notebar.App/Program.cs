using CommandLine;
using Grpc.Core;
using Notebar.Core;
using Notebar.gRPC;
using System;
using static Notebar.gRPC.NotebarService;

namespace Notebar.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                    .WithParsed(options =>
                    {
                        Console.WriteLine("Adding indicator...");
                        var channel = new Channel($"127.0.0.1:{Constants.GrpcPort}", ChannelCredentials.Insecure);
                        var client = new NotebarServiceClient(channel);
                        try
                        {
                            var response = client.AddIndicator(new AddIndicatorRequest { Port = options.Port });
                            if (!string.IsNullOrEmpty(response.Result))
                            {
                                Console.WriteLine($"Cannot add indicator. Error: {response.Result}");
                            }
                        }
                        catch (RpcException rpcException)
                        {
                            Console.WriteLine($"Cannot add indicator. Error: {rpcException.Message}");
                        }

                        channel.ShutdownAsync().Wait();
                    })
                    .WithNotParsed(errors =>
                    {
                        foreach (var error in errors)
                        {
                            Console.WriteLine(error);
                        }
                    });
        }
    }
}
