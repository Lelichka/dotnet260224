using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Secret;

namespace GrpcServer.Services;

[Authorize]
public class SecretService : Secret.SecretService.SecretServiceBase 
{
    public override Task<Response> GetSecret(Request request, ServerCallContext context)
    {
        return Task.FromResult(new Response { Secret = "some information" });
    }
}