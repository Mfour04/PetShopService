using PetShopLibrary.Models;
using PetShopLibrary.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShopLibrary.Service
{
    public class ServiceScheduleService
    {
        private readonly IServiceScheduleRepository _serviceScheduleRepository;

        public ServiceScheduleService(IServiceScheduleRepository serviceScheduleRepository)
        {
            _serviceScheduleRepository = serviceScheduleRepository;
        }

        public async Task<IEnumerable<ServiceSchedule>> GetSchedulesByUserIdAsync(long userId)
        {
            return await _serviceScheduleRepository.GetSchedulesByUserIdAsync(userId);
        }

        public async Task AddScheduleAsync(ServiceSchedule serviceSchedule)
        {
            await _serviceScheduleRepository.AddScheduleAsync(serviceSchedule);
        }
    }
}
