using Microsoft.AspNetCore.Mvc;
using backend.Dependencies;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthifyController : ControllerBase
    {
        private readonly HealthifyDependencies _deps;
        public HealthifyController(HealthifyDependencies deps)
        {
            _deps = deps;
        }

        [HttpGet("doctors-list")]
        public async Task<IActionResult> GetDoctorList()
        {
            var doctors = await _deps.DoctorService.GetAllDoctorsAsync();

            var result = doctors.Select(doctor => new
            {
                doctor.Id,
                doctor.Name,
                doctor.Email,
                doctor.Specialty,
                doctor.Available,
                doctor.Degree,
                doctor.Experience,
                doctor.Fees,
                doctor.About,
                doctor.ImageUrl,
                doctor.AddressLine1,
                doctor.AddressLine2,
                BookedSlots = doctor.BookedSlots
                    .GroupBy(bs => bs.SlotDate)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(bs => bs.SlotTime).ToList()
                    )
            });

            return Ok(new
            {
                success = true,
                doctors = result
            });
        }
    }
}