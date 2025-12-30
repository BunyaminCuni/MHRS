using MHRS.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MHRS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly AppointmentDbContext _context;
        private readonly ILogger<AppointmentController> _logger;

        public AppointmentController(AppointmentDbContext context, ILogger<AppointmentController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Tüm şehirleri getirir
        /// </summary>
        [HttpGet("cities")]
        public async Task<ActionResult<IEnumerable<City>>> GetAllCities()
        {
            try
            {
                var cities = await _context.Cities.OrderBy(c => c.CityName).ToListAsync();
                return Ok(cities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Şehirler getirilirken hata oluştu");
                return StatusCode(500, "Şehirler getirilirken bir hata oluştu");
            }
        }

        /// <summary>
        /// Belirtilen şehirdeki hastaneleri getirir
        /// </summary>
        [HttpGet("hospitals/{cityId}")]
        public async Task<ActionResult<IEnumerable<Hospital>>> GetHospitalsByCity(int cityId)
        {
            try
            {
                var hospitals = await _context.Hospitals
                    .Where(h => h.CityId == cityId)
                    .OrderBy(h => h.HospitalName)
                    .ToListAsync();

                if (!hospitals.Any())
                {
                    return NotFound(new { message = "Bu şehirde hastane bulunamadı" });
                }

                return Ok(hospitals);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Hastaneler getirilirken hata oluştu: {CityId}", cityId);
                return StatusCode(500, "Hastaneler getirilirken bir hata oluştu");
            }
        }
        /// <summary>
        /// Belirtilen şehirdeki ilçeleri getirir
        /// </summary>
        [HttpGet("districts/{cityId}")]
        public async Task<ActionResult<IEnumerable<string>>> GetDistrictsByCity(int cityId)
        {
            try
            {
                var districts = await _context.Hospitals
                    .Where(h => h.CityId == cityId && !string.IsNullOrEmpty(h.DistrictName))
                    .Select(h => h.DistrictName)
                    .Distinct()
                    .OrderBy(d => d)
                    .ToListAsync();

                if (!districts.Any())
                {
                    return NotFound(new { message = "Bu şehirde ilçe bulunamadı" });
                }

                return Ok(districts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "İlçeler getirilirken hata oluştu: {CityId}", cityId);
                return StatusCode(500, "İlçeler getirilirken bir hata oluştu");
            }
        }

        /// <summary>
        /// Belirtilen şehir ve ilçedeki hastaneleri getirir
        /// </summary>
        [HttpGet("hospitals/{cityId}/{districtName}")]
        public async Task<ActionResult<IEnumerable<Hospital>>> GetHospitalsByCityAndDistrict(int cityId, string districtName)
        {
            try
            {
                var hospitals = await _context.Hospitals
                    .Where(h => h.CityId == cityId && h.DistrictName == districtName)
                    .OrderBy(h => h.HospitalName)
                    .ToListAsync();

                if (!hospitals.Any())
                {
                    return NotFound(new { message = "Bu ilçede hastane bulunamadı" });
                }

                return Ok(hospitals);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Hastaneler getirilirken hata oluştu: CityId={CityId}, District={District}", cityId, districtName);
                return StatusCode(500, "Hastaneler getirilirken bir hata oluştu");
            }
        }
        /// <summary>
        /// Tamamlanmış randevuları getirir
        /// </summary>
        [HttpGet("done")]
        public async Task<ActionResult<IEnumerable<AppointmentResponseDto>>> GetDoneAppointments()
        {
            try
            {
                var appointments = await (from appointment in _context.Appointments
                                          where appointment.IsDone == true
                                          join patient in _context.Patients
                                          on appointment.PatientId equals patient.PatientId
                                          select new AppointmentResponseDto
                                          {
                                              PatientName = patient.PatientName,
                                              MobileNo = patient.MobileNo,
                                              City = patient.City,
                                              AppointmentId = appointment.AppointmentId,
                                              AppointmentDate = appointment.AppointmentDate,
                                              IsDone = appointment.IsDone
                                          }).ToListAsync();

                return Ok(appointments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Tamamlanmış randevular getirilirken hata oluştu");
                return StatusCode(500, "Randevular getirilirken bir hata oluştu");
            }
        }

        /// <summary>
        /// Bekleyen randevuları getirir
        /// </summary>
        [HttpGet("pending")]
        public async Task<ActionResult<IEnumerable<AppointmentResponseDto>>> GetPendingAppointments()
        {
            try
            {
                var appointments = await (from appointment in _context.Appointments
                                          where appointment.IsDone == false
                                          join patient in _context.Patients
                                          on appointment.PatientId equals patient.PatientId
                                          select new AppointmentResponseDto
                                          {
                                              PatientName = patient.PatientName,
                                              MobileNo = patient.MobileNo,
                                              City = patient.City,
                                              AppointmentId = appointment.AppointmentId,
                                              AppointmentDate = appointment.AppointmentDate,
                                              IsDone = appointment.IsDone
                                          }).ToListAsync();

                return Ok(appointments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Bekleyen randevular getirilirken hata oluştu");
                return StatusCode(500, "Randevular getirilirken bir hata oluştu");
            }
        }

        /// <summary>
        /// Tüm randevuları getirir
        /// </summary>
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<AppointmentResponseDto>>> GetAllAppointments()
        {
            try
            {
                var appointments = await (from appointment in _context.Appointments
                                          join patient in _context.Patients
                                          on appointment.PatientId equals patient.PatientId
                                          orderby appointment.AppointmentDate descending
                                          select new AppointmentResponseDto
                                          {
                                              PatientName = patient.PatientName,
                                              MobileNo = patient.MobileNo,
                                              City = patient.City,
                                              AppointmentId = appointment.AppointmentId,
                                              AppointmentDate = appointment.AppointmentDate,
                                              IsDone = appointment.IsDone
                                          }).ToListAsync();

                return Ok(appointments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Tüm randevular getirilirken hata oluştu");
                return StatusCode(500, "Randevular getirilirken bir hata oluştu");
            }
        }

        /// <summary>
        /// Randevu durumunu tamamlandı olarak işaretler
        /// </summary>
        [HttpPatch("{appointmentId}/complete")]
        public async Task<IActionResult> CompleteAppointment(int appointmentId)
        {
            try
            {
                var appointment = await _context.Appointments
                    .FirstOrDefaultAsync(m => m.AppointmentId == appointmentId);

                if (appointment == null)
                {
                    return NotFound(new { message = "Randevu bulunamadı" });
                }

                if (appointment.IsDone)
                {
                    return BadRequest(new { message = "Randevu zaten tamamlanmış" });
                }

                appointment.IsDone = true;
                await _context.SaveChangesAsync();

                return Ok(new { message = "Randevu durumu başarıyla güncellendi" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Randevu durumu güncellenirken hata oluştu: {AppointmentId}", appointmentId);
                return StatusCode(500, "Randevu güncellenirken bir hata oluştu");
            }
        }

        /// <summary>
        /// Yeni randevu oluşturur
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateAppointment([FromBody] NewAppointmentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Hastane kontrolü
                var hospital = await _context.Hospitals
                    .FirstOrDefaultAsync(h => h.HospitalId == request.HospitalId);

                if (hospital == null)
                {
                    return NotFound(new { message = "Hastane bulunamadı" });
                }

                // Şehir kontrolü
                if (hospital.CityId != request.CityId)
                {
                    return BadRequest(new { message = "Seçilen hastane seçilen şehirde değil" });
                }

                // Yeni hasta oluştur
                var patient = new Patient
                {
                    PatientName = request.PatientName,
                    MobileNo = request.MobileNo,
                    City = _context.Cities
                        .Where(c => c.CityId == request.CityId)
                        .Select(c => c.CityName)
                        .FirstOrDefault() ?? string.Empty,
                    Address = hospital.Address ?? string.Empty,
                    Email = request.Email
                };

                _context.Patients.Add(patient);
                await _context.SaveChangesAsync();

                // Yeni randevu oluştur
                var appointment = new Appointment
                {
                    PatientId = patient.PatientId,
                    HospitalId = request.HospitalId,
                    AppointmentDate = request.AppointmentDate,
                    IsDone = false
                };

                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();

                return CreatedAtAction(
                    nameof(GetAllAppointments),
                    new { id = appointment.AppointmentId },
                    new
                    {
                        message = "Randevu başarıyla oluşturuldu",
                        appointmentId = appointment.AppointmentId,
                        patientId = patient.PatientId,
                        hospitalId = request.HospitalId
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Randevu oluşturulurken hata oluştu");
                return StatusCode(500, "Randevu oluşturulurken bir hata oluştu");
            }
        }

        /// <summary>
        /// Randevuyu siler
        /// </summary>
        [HttpDelete("{appointmentId}")]
        public async Task<IActionResult> DeleteAppointment(int appointmentId)
        {
            try
            {
                var appointment = await _context.Appointments
                    .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);

                if (appointment == null)
                {
                    return NotFound(new { message = "Randevu bulunamadı" });
                }

                _context.Appointments.Remove(appointment);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Randevu başarıyla silindi" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Randevu silinirken hata oluştu: {AppointmentId}", appointmentId);
                return StatusCode(500, "Randevu silinirken bir hata oluştu");
            }
        }
    }
}