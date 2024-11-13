using Microsoft.EntityFrameworkCore;
using PetShopLibrary.Models;
using PetShopLibrary.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShopLibrary.Repository.Implements
{
    public class ServiceScheduleRepository : IServiceScheduleRepository
    {
        private readonly PetShopContext _context;

        public ServiceScheduleRepository(PetShopContext context)
        {
            _context = context;
        }

        public async Task AddScheduleAsync(ServiceSchedule schedule)
        {
            await _context.ServiceSchedules.AddAsync(schedule);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ServiceSchedule>> GetSchedulesByUserIdAsync(long userId)
        {
            return await _context.ServiceSchedules
                .Include(s => s.Service)
                .Where(s => s.UserId == userId)
                .ToListAsync();
        }
    }
}
