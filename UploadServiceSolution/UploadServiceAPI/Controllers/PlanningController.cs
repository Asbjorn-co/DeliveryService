using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UploadServiceAPI.Interface;
using Model.ParcelDeliveryTracking;
using UploadServiceAPI;

namespace UploadServiceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlanningController : Controller
    {  
        private readonly IWorkerClient _workerClient;

        public PlanningController(IWorkerClient workerClient)
        {
            _workerClient = workerClient;
        }

        [HttpGet]
        [Route("date/{date}")]
        public async Task<ActionResult<IEnumerable<ParcelDeliveryTracking>>> GetPlanningDeliveries(string date)
        {
            var deliveries = await _workerClient.GetAllDeliveriesAsync(date);
            return Ok(deliveries);
        }

        [HttpGet]
        [Route("name/{name}")]
        public async Task<ActionResult<IEnumerable<Delivery>>> GetAllDeliveriesName(string name)
        {
            var deliveries = await _workerClient.GetAllDeliveriesByNameAsync(name);
            return Ok(deliveries);
        }

        

    }
}
