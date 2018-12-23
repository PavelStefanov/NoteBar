using Grpc.Core;
using Notebar.gRPC;
using System;

namespace Notebar.Core.Grpc
{
    public class GrpcServer
    {
        private Server Server { get; set; }

        public GrpcServer Start(Func<uint, string> addFnc)
        {
            Server = new Server
            {
                Services = { NotebarService.BindService(new NotebarGrpcService(addFnc)) },
                Ports = { new ServerPort("localhost", Constants.GrpcPort, ServerCredentials.Insecure) }
            };
            Server.Start();

            return this;
        }

        public void ShutDown()
        {
            Server.ShutdownAsync().Wait();
        }

        public static GrpcServer Run(Func<uint, string> addFnc)
        {
            return new GrpcServer().Start(addFnc);
        }
    }
}
