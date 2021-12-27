using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Subscriber.Models;

namespace Subscriber.Controllers;

[ApiController]
[Route("[controller]")]
public class GunsConsumerController : ControllerBase
{
    private readonly RabbitMqConfiguration _configuration;
    private readonly IDbContextFactory<GunsDbContext> _context;

    public GunsConsumerController(IOptions<RabbitMqConfiguration> configuration, IDbContextFactory<GunsDbContext> context)
    {
        _context = context;
        _configuration = configuration.Value;
    }

    [HttpGet]
    public IActionResult ReadMessage()
    {
        GetMessage();

        return Ok("Message is here!");
    }

    private void GetMessage()
    {
        var factory = new ConnectionFactory
        {
            HostName = _configuration.Hostname,
            UserName = _configuration.UserName,
            Password = _configuration.Password
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(_configuration.QueueName, false, false, false, null);

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += (sender, e) =>
        {
            var body = e.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(message);
            SendToDb(message);
            Console.WriteLine("Send to db!");
        };

        channel.BasicConsume(_configuration.QueueName, true, consumer);

    }
    void SendToDb(string message)
    {
        var dbGun = new Gun()
        {
            Id = Guid.NewGuid().ToString(),
            Description = message
        };

        try
        {
            using var db = _context.CreateDbContext();
            db.Guns.AddAsync(dbGun);

            db.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}