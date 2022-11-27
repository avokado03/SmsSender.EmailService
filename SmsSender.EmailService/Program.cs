using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmsSender.Common.RabbitMQ;
using SmsSender.Common.RabbitMQ.Interfaces;
using SmsSender.EmailService.DTO;
using System.Reflection;

var services = new ServiceCollection();
var configurationBuilder = new ConfigurationBuilder();
var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;

configurationBuilder.SetBasePath(path)
      .AddJsonFile("appsettings.json", false, true)
      .AddJsonFile($"appsettings.{environmentName}.json", true)
      .AddEnvironmentVariables()
      .AddUserSecrets(Assembly.GetExecutingAssembly());

var configuration = configurationBuilder.Build();

services.ConfigureRabbit(configuration);

var serviceProvider = services.BuildServiceProvider();

IRabbitClient client = serviceProvider.GetRequiredService<IRabbitClient>();

client.Subscribe<EmailNotificationMessage>((message, model, ea) =>
{
    Console.WriteLine($"email to: {message.Email} message: {message.Content} {Environment.NewLine}");
}, "email_notification");

var resetEvent = new ManualResetEventSlim();
resetEvent.Wait();
