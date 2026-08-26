Console.WriteLine("This application can be used to inspect yfinance API responses");
Console.WriteLine("Type 'lookup <symbol>' to test the lookup functionality");
Console.WriteLine("Type 'exit' to quit.");

using var httpClient = new HttpClient();

string command;

while (true)
{
    Console.Write("> ");
    command = Console.ReadLine() ?? "";

    if (command.Equals("exit", StringComparison.OrdinalIgnoreCase))
        break;

    if (command.StartsWith("lookup ", StringComparison.OrdinalIgnoreCase))
    {
        string symbol = command.Substring("lookup ".Length).Trim();

        if (string.IsNullOrWhiteSpace(symbol))
        {
            Console.WriteLine("Please specify a symbol.");
            continue;
        }

        try
        {
            string url =$"http://127.0.0.1:7000/search?q={Uri.EscapeDataString(symbol)}";

            HttpResponseMessage response = await httpClient.GetAsync(url);
            string json = await response.Content.ReadAsStringAsync();
            Console.WriteLine(json);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Could not connect to the API: {ex.Message}");
        }
    }
    else
    {
        Console.WriteLine("Unknown command. Try: lookup <symbol>");
    }
}