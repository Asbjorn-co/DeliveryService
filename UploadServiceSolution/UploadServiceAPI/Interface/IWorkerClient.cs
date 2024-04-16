using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Model.ParcelDeliveryTracking;


namespace UploadServiceAPI.Interface
{
    public interface IWorkerClient
    {
        Task<IEnumerable<ParcelDeliveryTracking>> GetAllDeliveriesAsync(string date);
        Task<IEnumerable<Delivery>> GetAllDeliveriesByNameAsync(string name);
    }
}
