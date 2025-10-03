using dotnet_amqp_messaging;

Console.WriteLine("Demo started. Press Ctrl+C to exit.");

var sender = new AmqpSender();
await sender.SendMessageAsync("demo message");

var cts = new CancellationTokenSource();

Console.CancelKeyPress += (sender, eventArgs) =>
{
    eventArgs.Cancel = true;
    cts.Cancel();
    Console.WriteLine("Shutting down...");
};

var listener = new AmqpListener();
await listener.StartListeningAsync(cts.Token);

Console.WriteLine("Application has shut down.");