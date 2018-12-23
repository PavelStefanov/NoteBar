using Grpc.Core;
using Notebar.gRPC;
using System;
using System.Threading.Tasks;
using static Notebar.gRPC.NotebarService;

namespace Notebar.Core.Grpc
{
    public class NotebarGrpcService : NotebarServiceBase
    {
        public Func<uint, string> AddFnc { get; }

        public NotebarGrpcService(Func<uint, string> addFnc)
        {
            AddFnc = addFnc;
        }

        public override Task<AddIndicatorResponse> AddIndicator(AddIndicatorRequest request, ServerCallContext context)
        {
            var result = AddFnc?.Invoke(request.Port);
            return Task.FromResult(new AddIndicatorResponse
            {
                Result = result ?? string.Empty
            });
        }
    }
}
