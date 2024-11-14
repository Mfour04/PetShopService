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
        Task<IEnumerable<ServiceSchedule>> GetAllSchedules();
        Task<IEnumerable<ServiceSchedule>> GetSchedulesByUserIdAsync(long userId);
        Task AddScheduleAsync(ServiceSchedule schedule);
        Task UpdateScheduleStatusAsync(long scheduleId, long status);
        Task<ServiceSchedule> GetScheduleByIdAsync(long scheduleId);
    }
}
