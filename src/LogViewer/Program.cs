using System.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace LogViewer;

class Program
{
    static async Task Main(string[] args)
    {
        var configuration = BuildConfiguration();
        var subscriber = new Subscriber(configuration);

        try
        {
            await subscriber.InitializeConnection();
            await subscriber.StartConsuming();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"No se pudo conectar a RabbitMQ: {ex.Message}");
            return;
        }

        Console.WriteLine("LogViewer escuchando logs... (pulsa Enter para salir)\n");

        // En Docker sin -it, ReadLine devuelve null al instante: esperamos indefinidamente
        if (Console.ReadLine() == null)
            await Task.Delay(Timeout.Infinite);

        await subscriber.Close();
    }

    private static IConfiguration BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false);

        if (Debugger.IsAttached)
            builder.AddJsonFile("appsettings.Development.json", optional: true);

        return builder.Build();
    }
}