using MassTransit;
using Microsoft.Extensions.Hosting;
using Receiver;

var host = new HostBuilder()
    .ConfigureServices(services =>
    {
        services.AddMassTransit(busRegistrationConfigurator =>
        {
            busRegistrationConfigurator.AddConsumer<MessageConsumer>();

            busRegistrationConfigurator.UsingRabbitMq((context, rabbitMqBusFactoryConfigurator) =>
            {
                rabbitMqBusFactoryConfigurator.Host("localhost", "/", rabbitMqHostConfigurator =>
                {
                    rabbitMqHostConfigurator.Username("guest");
                    rabbitMqHostConfigurator.Password("guest");
                });

                rabbitMqBusFactoryConfigurator.ConfigureEndpoints(context);
            });
        });
    });

var build = host.Build();
build.Run();
