namespace Palm.Controllers
{
    using Domain.Dtos;
    using Domain.Models;
    using Microsoft.AspNetCore.Mvc;
    using Services;

    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _service;

        public ScheduleController(IScheduleService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Schedule>>> GetAll() =>
            Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<Schedule>> GetById(int id)
        {
            var schedule = await _service.GetByIdAsync(id);
            return schedule == null ? NotFound() : Ok(schedule);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(schedualeDto scheduleDto)
        {
            var created = await _service.CreateAsync(scheduleDto);
            return Ok(created);
        }


        [HttpGet("by-customer/{customerId}")]
        public async Task<IActionResult> GetByCustomerId(string customerId)
        {
            var schedules = await _service.GetSchedulesByCustomerIdAsync(customerId);
            return Ok(schedules);
        }

        [HttpGet("by-tenant/{tenantId}")]
        public async Task<IActionResult> GetByTenantId(string tenantId)
        {
            var schedules = await _service.GetSchedulesByTenantIdAsync(tenantId);
            return Ok(schedules);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);

            if (!success)
                return NotFound(new { Message = $"Schedule with ID {id} was not found." });

            return NoContent(); // 204 No Content on successful deletion
        }

    }

}
