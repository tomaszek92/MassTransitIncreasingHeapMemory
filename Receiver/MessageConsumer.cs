using Common;
using MassTransit;

namespace Receiver;

public class MessageConsumer : IConsumer<Message>
{
    public Task Consume(ConsumeContext<Message> context)
    {
        Console.WriteLine($"{context.MessageId}, {context.Message.TimeStamp:O}");
        return Task.CompletedTask;
    }
}
