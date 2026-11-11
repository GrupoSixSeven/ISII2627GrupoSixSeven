using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace LogViewer;

public class Subscriber
{
    private readonly IConfiguration _configuration;
    private readonly string _exchangeName;
    private IConnection? _connection;
    private IChannel? _channel;
    private string _queueName = string.Empty;

    public Subscriber(IConfiguration configuration)
    {
        _configuration = configuration;
        _exchangeName = _configuration["RabbitMQ:Exchange"] ?? "logs";
    }

    public async Task InitializeConnection()
    {
        int port = int.TryParse(_configuration["RabbitMQ:Port"], out int p) ? p : 5672;

        var factory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMQ:HostName"] ?? "localhost",
            Port = port,
            UserName = _configuration["RabbitMQ:UserName"] ?? "guest",
            Password = _configuration["RabbitMQ:Password"] ?? "guest"
        };

        // 1. Conexión y canal
        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();

        // 2. Declarar el exchange (mismos parámetros que en la API)
        await _channel.ExchangeDeclareAsync(_exchangeName, ExchangeType.Fanout,
            durable: true, autoDelete: false);

        // 3. Cola efímera: RabbitMQ le pone un nombre aleatorio, es exclusiva
        //    de esta conexión y se borra sola al cerrar el LogViewer
        var tempQueue = await _channel.QueueDeclareAsync(
            queue: string.Empty, durable: false, exclusive: true, autoDelete: true);
        _queueName = tempQueue.QueueName;

        // 4. Enlazar la cola al exchange (en fanout la routing key se ignora)
        await _channel.QueueBindAsync(queue: _queueName, exchange: _exchangeName,
            routingKey: string.Empty);
    }

    public async Task StartConsuming()
    {
        if (_channel == null)
            throw new InvalidOperationException("Hay que llamar antes a InitializeConnection()");

        var consumer = new AsyncEventingBasicConsumer(_channel);

        // Callback: se ejecuta cada vez que llega un log
        consumer.ReceivedAsync += (model, ea) =>
        {
            var message = Encoding.UTF8.GetString(ea.Body.ToArray());
            MostrarLog(message);
            return Task.CompletedTask;
        };

        await _channel.BasicConsumeAsync(queue: _queueName, autoAck: true, consumer: consumer);
    }

    private static void MostrarLog(string json)
    {
        try
        {
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            string fecha     = Campo(root, "Timestamp");
            string nivel     = Campo(root, "LogLevel");
            string categoria = Campo(root, "Category");
            string mensaje   = Campo(root, "Message");
            string excepcion = Campo(root, "Exception");

            Console.ForegroundColor = nivel switch
            {
                "Information" => ConsoleColor.Green,
                "Warning"     => ConsoleColor.Yellow,
                "Error"       => ConsoleColor.Red,
                "Critical"    => ConsoleColor.Magenta,
                _             => ConsoleColor.Gray
            };
            Console.Write($"[{nivel}]");
            Console.ResetColor();
            Console.WriteLine($" {fecha} {categoria}");
            Console.WriteLine($"   {mensaje}");
            if (!string.IsNullOrEmpty(excepcion))
                Console.WriteLine($"   Excepción: {excepcion}");
            Console.WriteLine();
        }
        catch (JsonException)
        {
            Console.WriteLine(json);   // Si no es JSON, se muestra tal cual
        }
    }

    // Devuelve el valor de una propiedad del JSON, o "" si no existe o es null
    private static string Campo(JsonElement root, string nombre)
    {
        if (root.TryGetProperty(nombre, out var valor) && valor.ValueKind != JsonValueKind.Null)
            return valor.ToString();
        return "";
    }

    public async Task Close()
    {
        if (_channel != null) await _channel.CloseAsync();
        if (_connection != null) await _connection.CloseAsync();
    }
}