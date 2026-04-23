using Microsoft.AspNetCore.SignalR;

public class GameHub : Hub
{
    public async Task SendInput(int value)
    {
        Console.WriteLine($"Сервер получил: {value}");

        // отправляем всем клиентам это число обратно
        await Clients.All.SendAsync("ReceiveInput", value);
    }
}