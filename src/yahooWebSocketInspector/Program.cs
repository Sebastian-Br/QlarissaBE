using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

const string yahooUrl =
    "wss://streamer.finance.yahoo.com/?version=2";

string[] tickers = [ "BTC-USD", "ETH-USD"];

using var socket = new ClientWebSocket();

Console.WriteLine("Connecting to Yahoo Finance WebSocket...");

await socket.ConnectAsync(
    new Uri(yahooUrl),
    CancellationToken.None);

Console.WriteLine("Connected.");

var subscribe = JsonSerializer.Serialize(new
{
    subscribe = tickers
});

await socket.SendAsync(
    Encoding.UTF8.GetBytes(subscribe),
    WebSocketMessageType.Text,
    true,
    CancellationToken.None);

Console.WriteLine($"Subscribed to {String.Join(", ", tickers)}");
Console.WriteLine("Waiting for messages...\n");

var buffer = new byte[16 * 1024];

while (socket.State == WebSocketState.Open)
{
    using var stream = new MemoryStream();

    WebSocketReceiveResult result;

    do
    {
        result = await socket.ReceiveAsync(
            buffer,
            CancellationToken.None);

        if (result.MessageType == WebSocketMessageType.Close)
        {
            Console.WriteLine("Connection closed.");
            return;
        }

        stream.Write(buffer, 0, result.Count);

    } while (!result.EndOfMessage);

    var text = Encoding.UTF8.GetString(stream.ToArray());

    using var json = JsonDocument.Parse(text);

    if (!json.RootElement.TryGetProperty(
            "message",
            out var messageElement))
        continue;

    var base64 = messageElement.GetString();

    if (string.IsNullOrEmpty(base64))
        continue;

    var bytes = Convert.FromBase64String(base64);

    var data = PricingData.Parser.ParseFrom(bytes);

    Console.WriteLine(
        $"{data.Id,-10} " +
        $"price={data.Price,12:F2} " +
        $"high={data.DayHigh,12:F2} " +
        $"low={data.DayLow,12:F2}");
}