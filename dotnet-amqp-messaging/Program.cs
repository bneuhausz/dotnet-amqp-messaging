using dotnet_amqp_messaging;

var sender = new AmqpSender();
await sender.SendMessageAsync("demo message");