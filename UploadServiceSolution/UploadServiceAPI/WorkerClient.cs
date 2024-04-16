using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Model.ParcelDeliveryTracking;
using UploadServiceAPI.Interface;

namespace UploadServiceAPI
{
    public class WorkerClient : IWorkerClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<WorkerClient> _logger;

        public WorkerClient(HttpClient httpClient, ILogger<WorkerClient> logger)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:5001"); // Angiv basen for din Worker-service URL
            _logger = logger;
        }

        public async Task<IEnumerable<ParcelDeliveryTracking>> GetAllDeliveriesAsync(string date)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/tracking/date/{date}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsAsync<IEnumerable<ParcelDeliveryTracking>>();
                return content;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching deliveries by date");
                throw;
            }
        }

        public async Task<IEnumerable<Delivery>> GetAllDeliveriesByNameAsync(string name)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/tracking/name/{name}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsAsync<IEnumerable<Delivery>>();
                return content;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching deliveries by name");
                throw;
            }
        }

        // Du kan tilføje flere metoder her...
    }
}
