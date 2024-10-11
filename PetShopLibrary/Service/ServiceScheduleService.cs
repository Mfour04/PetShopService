using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetShopLibrary.Repository.Interfaces;
using PetShopLibrary.Repository.Implements;
using PetShopLibrary.Models;

namespace PetShopLibrary.Service
{
    public class ServiceScheduleService
    {
        private readonly IServiceScheduleRepository _serviceScheduleRepository;
        public ServiceScheduleService(IServiceScheduleRepository serviceScheduleRepository)
        {
            _serviceScheduleRepository = serviceScheduleRepository;
        }

        public List<ShopService> GetAllServices() => _serviceScheduleRepository.GetAllServices();

        public void AddServiceSchedule(ServiceSchedule serviceSchedule) => _serviceScheduleRepository.AddSchedule(serviceSchedule);
        public List<ServiceSchedule> GetSchedulesByDate(DateTime date) => _serviceScheduleRepository.GetSchedulesByDate(date);

        public List<ServiceSchedule> GetAllSchedules() => _serviceScheduleRepository.GetAllSchedules();
        public List<ServiceSchedule> GetSchedulesByUserId(long userId) => _serviceScheduleRepository.GetSchedulesByUserId(userId);
    }
}
