using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Persistance.Repositories
{
    public interface IScheduleRepsitory
    {
        Task<IEnumerable<Schedule>> GetAllAsync();
        Task<Schedule?> GetByIdAsync(int id);
        Task AddAsync(Schedule schedule);
        //Task<bool> DeleteAsync(int id);

        Task<List<Schedule>> GetSchedulesByCustomerIdAsync(string customerId);
        Task<List<Schedule>> GetSchedulesByTenantIdAsync(string tenantId);
        Task<bool> SaveChangesAsync();
        Task<bool> DeleteWithChildrenAsync(Schedule schedule);
    }
}
