using PetShopLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShopLibrary.Repository.Interfaces
{
    public interface IServiceScheduleRepository
    {
        List<ShopService> GetAllServices();
        void AddSchedule(ServiceSchedule schedule);
        List<ServiceSchedule> GetSchedulesByDate(DateTime date);
        List<ServiceSchedule> GetAllSchedules();
        List<ServiceSchedule> GetSchedulesByUserId(long userId);
    }
}
