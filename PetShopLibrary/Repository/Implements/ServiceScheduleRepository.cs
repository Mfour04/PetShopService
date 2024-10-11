using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetShopLibrary.Models;
using PetShopLibrary.Repository.Interfaces;

namespace PetShopLibrary.Repository.Implements
{
    public class ServiceScheduleRepository : IServiceScheduleRepository
    {
        private readonly PetShopContext _petShopContext;

        public ServiceScheduleRepository(PetShopContext petShopContext)
        {
            _petShopContext = petShopContext;
        }

        public void AddSchedule(ServiceSchedule schedule)
        {
            _petShopContext.ServiceSchedules.Add(schedule);
            _petShopContext.SaveChanges();  
        }
         
        public List<ShopService> GetAllServices()
        {
            return _petShopContext.ShopServices.Where(s => s.IsActive == true).ToList();
        }

        public List<ServiceSchedule> GetSchedulesByDate(DateTime date)
        {
            return _petShopContext.ServiceSchedules
                .Where(s => s.Date.HasValue && s.Date.Value.Date == date.Date).ToList();
        }

        public List<ServiceSchedule> GetAllSchedules()
        {
            return _petShopContext.ServiceSchedules.ToList();
        }

        public List<ServiceSchedule> GetSchedulesByUserId(long userId) 
        {
            return _petShopContext.ServiceSchedules.Where(s => userId == s.UserId).ToList();
        }
    }
}
