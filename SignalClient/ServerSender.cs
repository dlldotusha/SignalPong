using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

public static class ServerSender
{
    private static HubConnection? _connection;

    public static async Task StartAsync(string url = "http://localhost:5202/gameHub")
    {
        if (_connection != null && _connection.State == HubConnectionState.Connected)
            return;

        _connection = new HubConnectionBuilder()
            .WithUrl(url)
            .WithAutomaticReconnect()
            .Build();

        await _connection.StartAsync();
        Console.WriteLine("Подключено к серверу");
    }

    public static async Task SendAsync(string methodName, object? data)
    {
        if (_connection == null)
            throw new Exception("Сначала вызови ServerSender.StartAsync()");

        if (_connection.State != HubConnectionState.Connected)
            await _connection.StartAsync();

        await _connection.InvokeAsync(methodName, data);
    }

    public static async Task SendAsync(string methodName, object? data1, object? data2)
    {
        if (_connection == null)
            throw new Exception("Сначала вызови ServerSender.StartAsync()");

        if (_connection.State != HubConnectionState.Connected)
            await _connection.StartAsync();

        await _connection.InvokeAsync(methodName, data1, data2);
    }

    public static async Task StopAsync()
    {
        if (_connection != null)
        {
            await _connection.DisposeAsync();
            _connection = null;
            Console.WriteLine("Отключено от сервера");
        }
    }

    public static void On<T>(string methodName, Action<T> action)
    {
        if (_connection == null)
            throw new Exception("Сначала вызови ServerSender.StartAsync()");

        _connection.On(methodName, action);
    }
}