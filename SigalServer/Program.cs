public class Program
{
    public struct Player
    {
        public string DisplayName { get; set; }
        public int? Length { get; set; }
    }
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddSignalR();

        var app = builder.Build();

        app.MapGet("/", () => "SignalR server is running");
        app.MapHub<GameHub>("/gameHub");

        app.Run();
    }
}