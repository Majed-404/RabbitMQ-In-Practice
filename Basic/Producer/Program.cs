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

var message = $"Hello from producer {DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}";
var messageEncoded = Encoding.UTF8.GetBytes(message);

await channel.BasicPublishAsync(exchange: string.Empty,
                                routingKey: queueName,
                                body: messageEncoded);

Console.WriteLine($"Messaged Published: {message}");
