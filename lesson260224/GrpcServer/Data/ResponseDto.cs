namespace GrpcServer.Services;



public class ResponseDto
{
    public HourlyResults Hourly { get; set; }
}

public class HourlyResults
{
    public DateTime[] Time { get; set; }
    public double?[] Temperature_2m { get; set; }
}