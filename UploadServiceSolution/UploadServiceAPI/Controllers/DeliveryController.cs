using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Diagnostics;

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
    [HttpGet("readcsv")]
    public IActionResult ReadCsv()
    {
        string filePath = "..\\csvtest.csv"; // Indsæt stien til din CSV-fil her
        List<Delivery> deliveries = new List<Delivery>();

        try
        {
            using (var reader = new StreamReader(filePath, Encoding.UTF8))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    // Antager at din CSV-fil har kolonner for Navn og Alder
                    var delivery = new Delivery
                    {
                        medlemsNavn = values[0],
                        pickupAdresse = values[1],
                        pakkeID = values[2],
                        afleveringsAdresse = values[3]
                    };

                    deliveries.Add(delivery);
                }
            }

            return Ok(deliveries);
        }
        catch (Exception ex)
        {
            // Log any exceptions that occur during processing.
            _logger.LogError(ex, "Error occurred while READING the CSV-file.");

            // Return an appropriate error response.
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpGet("version")]
    public async Task<Dictionary<string, string>> GetVersion()
    {
        var properties = new Dictionary<string, string>();
        var assembly = typeof(Program).Assembly;
        properties.Add("service", "qgt-delivery-service");
        var ver = FileVersionInfo.GetVersionInfo(typeof(Program)
        .Assembly.Location).ProductVersion;
        properties.Add("version", ver!);
        try
        {
            var hostName = System.Net.Dns.GetHostName();
            var ips = await System.Net.Dns.GetHostAddressesAsync(hostName);
            var ipa = ips.First().MapToIPv4().ToString();
            properties.Add("hosted-at-address", ipa);

             _logger.LogInformation("Metoden version er kaldt");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            properties.Add("hosted-at-address", "Could not resolve IP-address");
        }
        
        return properties;
    }
}


