using Amqp;

namespace dotnet_amqp_messaging;
public class AmqpReceiver
{
    public async Task StartListeningAsync(CancellationToken cancellationToken)
    {
        var brokerUrl = "amqp://admin:admin@localhost:5672";
        var addressName = "demo-target";

        Connection? connection = null;
        try
        {
            connection = await Connection.Factory.CreateAsync(new Address(brokerUrl));
            var session = new Session(connection);
            var receiver = new ReceiverLink(session, "demo-receiver", addressName);

            Console.WriteLine($"Receiver attached to address '{addressName}'. Waiting for messages...");

            while (!cancellationToken.IsCancellationRequested)
            {
                var message = await receiver.ReceiveAsync(TimeSpan.FromSeconds(1));

                if (message != null)
                {
                    Console.WriteLine($"Received Message: '{message.Body}'");
                    receiver.Accept(message);
                }
            }

            await receiver.CloseAsync();
            await session.CloseAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        finally
        {
            if (connection != null && !connection.IsClosed)
            {
                await connection.CloseAsync();
                Console.WriteLine("Connection closed.");
            }
        }
    }
}
