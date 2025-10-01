using Amqp;

namespace dotnet_amqp_messaging;
public class AmqpSender
{
    public async Task SendMessageAsync(string messageBody)
    {
        Connection? connection = null;
        try
        {
            Address address = new Address("localhost", 5672, "admin", "admin", scheme: "amqp");
            connection = await Connection.Factory.CreateAsync(address);
            var session = new Session(connection);
            var sender = new SenderLink(session, "demo-sender", "demo-target");

            var message = new Message(messageBody);
            await sender.SendAsync(message);

            Console.WriteLine("Message sent to AMQP topic");

            await sender.CloseAsync();
            await session.CloseAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            if (connection != null)
            {
                await connection.CloseAsync();
            }
        }
    }
}
