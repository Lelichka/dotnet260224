

using Grpc.Core;
using Grpc.Net.Client;
using Jwt;
using Secret;
using Request = Jwt.Request;

using var channel = GrpcChannel.ForAddress("http://localhost:5098");

var client = new JwtService.JwtServiceClient(channel);

var username = "username";

var serverData = client.GetJwt(new Request{Username = username});

var token = serverData.Token;

if (token is null) throw new Exception("token is null");

var authMetadata = new Metadata { { "Authorization", $"Bearer {token}" } };

var client2 = new SecretService.SecretServiceClient(channel);

var secret = client2.GetSecret(new Secret.Request(), headers: authMetadata);

Console.WriteLine($"secret is {secret.Secret}");