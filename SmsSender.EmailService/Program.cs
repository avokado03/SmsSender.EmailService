using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmsSender.Common.RabbitMQ;
using SmsSender.EmailService.DTO;
using System.Reflection;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        var environment = context.HostingEnvironment;

        config.SetBasePath(environment.ContentRootPath)
              .AddJsonFile("appsettings.json", false, true)
              .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", true)
              .AddEnvironmentVariables()
              .AddUserSecrets(Assembly.GetExecutingAssembly());
    });
IHost host = builder.ConfigureServices((context, services) =>
    services.ConfigureRabbit(context.Configuration))
.Build();

RabbitClient client = host.Services.GetRequiredService<RabbitClient>();

client.Subscribe<EmailNotificationMessage>((message, model, ea) =>
{
    Console.WriteLine($"email to: {message.Email} message: {message.Content}");
}, "email_notification");

