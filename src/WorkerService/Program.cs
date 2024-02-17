using MSAccessHttp.App_Start;

namespace WorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ProgramStart.AddServices(builder.Services);

            builder.Services.AddWindowsService();
            builder.Services.AddHostedService<WorkerService>();

            ProgramStart.RunApp(builder);
        }
    }
}
