using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace UploadServiceAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class DeliveryController : ControllerBase
{
    private string _sendPath = string.Empty;
    private readonly ILogger<DeliveryController> _logger;
    public DeliveryController(ILogger<DeliveryController> logger,
    IConfiguration configuration)
    {
        _logger = logger;
        _sendPath = configuration["SendPath"] ?? String.Empty;
    }



    [HttpPost("parcel")]
    public IActionResult PostDelivery([FromBody] Delivery delivery)
    {
        try
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "deliveryqueue",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
                //Serializing af JSON-objekt
                var body = JsonSerializer.SerializeToUtf8Bytes(delivery);
                channel.BasicPublish(exchange: "",
                routingKey: "deliveryqueue",
                basicProperties: null,
                body: body);
            }

            // Here you can handle the received delivery object.
            // For example, you can save it to a database, process it further, etc.

            // For now, let's just log the received data.
            _logger.LogInformation("Received a new delivery: {Delivery}", delivery);

            // You can return whatever you like in response to the POST request.
            // For simplicity, let's just return a 200 OK response.
            return Ok();
        }
        catch (Exception ex)
        {
            // Log any exceptions that occur during processing.
            _logger.LogError(ex, "Error occurred while processing the delivery.");

            // Return an appropriate error response.
            return StatusCode(500, "Internal Server Error");
        }
    }


}