using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Dtos;
using Domain.Models;

namespace Services
{
    public interface IScheduleService
    {
        Task<IEnumerable<schedualeDto>> GetAllAsync();
        Task<schedualeDto?> GetByIdAsync(int id);
        Task<Schedule> CreateAsync(schedualeDto schedule);
        Task<List<Schedule>> GetSchedulesByCustomerIdAsync(string customerId);
        Task<List<Schedule>> GetSchedulesByTenantIdAsync(string tenantId);
        Task<bool> DeleteAsync(int id);
    }
}
