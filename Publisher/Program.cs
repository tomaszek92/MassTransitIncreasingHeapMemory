using Common;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureServices(services =>
    {
        services.AddMassTransit(busRegistrationConfigurator =>
        {
            busRegistrationConfigurator.UsingRabbitMq((context, rabbitMqBusFactoryConfigurator) =>
            {
                rabbitMqBusFactoryConfigurator.Host("localhost", "/", rabbitMqHostConfigurator =>
                {
                    rabbitMqHostConfigurator.Username("guest");
                    rabbitMqHostConfigurator.Password("guest");
                    rabbitMqHostConfigurator.PublisherConfirmation = false;
                    // rabbitMqHostConfigurator.RequestedChannelMax(1);
                });

                rabbitMqBusFactoryConfigurator.ConfigureEndpoints(context);
            });
        });
    });

Console.WriteLine("Press any key to start");
Console.ReadLine();

var app = host.Build();
var running = true;
var publishEndpoint = app.Services.GetRequiredService<IPublishEndpoint>();

_ = Task.Run(async () =>
{
    while (running)
    {
        var message = new Message(DateTime.Now);
        try
        {
            await publishEndpoint.Publish(message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
});

await Task.Delay(TimeSpan.FromHours(1));
running = false;
