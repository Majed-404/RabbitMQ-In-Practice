using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

var queueName = "test";

await channel.QueueDeclareAsync(queue: queueName,
                                durable: false,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);

Console.WriteLine("RabbitMQ Producer is running.");

while (true)
{
    Console.Write("Enter message: ");
    string? message = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(message))
    {
        Console.WriteLine("Message cannot be empty. Try again.");
        continue;
    }

    var messageEncoded = Encoding.UTF8.GetBytes(message);

    await channel.BasicPublishAsync(exchange: string.Empty,
                                    routingKey: queueName,
                                    body: messageEncoded);

    Console.WriteLine($"Message Published: {message}");

    Console.Write("Do you want to send another message? (yes to continue, exit to quit): ");
    string? response = Console.ReadLine()?.Trim().ToLower();

    if (response == "exit")
    {
        Console.WriteLine("Exiting application...");
        break;
    }
}