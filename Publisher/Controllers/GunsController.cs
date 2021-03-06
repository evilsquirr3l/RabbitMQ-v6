using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ_v6.Models;
using RabbitMQ.Client;

namespace RabbitMQ_v6.Controllers;

[ApiController]
[Route("[controller]")]
public class GunsController : ControllerBase
{
    private readonly RabbitMqConfiguration _configuration;

    public GunsController(IOptions<RabbitMqConfiguration> configuration)
    {
        _configuration = configuration.Value;
    }

    [HttpPost]
    public IActionResult CreateGun(Gun gun)
    {
        ValidateModel(gun);
        PublishMessage(JsonConvert.SerializeObject(gun));
        
        return Ok("Message is sent!");
    }

    private void ValidateModel(Gun gun)
    {
        if (gun.Materials.Contains("wood"))
        {
            throw new ArgumentException("No wood in the gun, please!");
        }

        if (gun.Ttc.ShootingRange < 0)
        {
            throw new ArgumentException("Shooting range must be more than 0!");
        }
    }

    private void PublishMessage(string gun)
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

        var body = Encoding.UTF8.GetBytes(gun);

        channel.BasicPublish(string.Empty, _configuration.QueueName, null, body);
    }
}