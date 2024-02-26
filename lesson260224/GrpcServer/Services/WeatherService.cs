using System.Text.Json;
using Grpc.Core;
using GrpcServer.Data;
using Weather;

namespace GrpcServer.Services;

public class WeatherService : Weather.WeatherService.WeatherServiceBase 
{
    public override async Task WeatherStream(Request request, IServerStreamWriter<Response> responseStream, ServerCallContext context)
    {

        if (WeatherStorage.WeatherList is null)
        {
            using var client = new HttpClient();
            var response = await client.GetAsync(
                "https://archive-api.open-meteo.com/v1/archive?latitude=52.52&longitude=13.41&past_days=92&hourly=temperature_2m&timezone=Europe%2FMoscow");
            var responseString = await response.Content.ReadAsStringAsync();
            var parsedResponse = JsonSerializer.Deserialize<ResponseDto>(responseString,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            WeatherStorage.WeatherList = parsedResponse;
        }
        
        var i = 0;
        while (!context.CancellationToken.IsCancellationRequested && WeatherStorage.WeatherList.Hourly.Time.Length > i) 
        {
            await responseStream.WriteAsync(new Response { Data = $" {DateTime.UtcNow:HH.mm.ss} погода на {WeatherStorage.WeatherList.Hourly.Time[i]:dd.MM.yyyy HH:mm} = {WeatherStorage.WeatherList.Hourly.Temperature_2m[i]}С" });
            i+=2;
            await Task.Delay(TimeSpan.FromSeconds(1));
        }

        Console.WriteLine("the cancellation is requested from the client");
    }
}