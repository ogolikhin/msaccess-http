
using MSAccessHttp.App_Start;
using MSAccessHttp.ExceptionHandling;
using System.Text.Json.Serialization;

namespace MSAccessHttp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ProgramStart.AddServices(builder.Services);

        ProgramStart.RunApp(builder);
    }
}
