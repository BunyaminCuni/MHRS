using MHRS.Services;
using Microsoft.AspNetCore.Mvc;

namespace MHRS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly EmailService _emailService;
        private readonly ILogger<AuthController> _logger;

        // Memory'de geçici OTP depolaması (üretim ortamında database kullan)
        private static Dictionary<string, (string otp, DateTime expiry)> _otpStore = new();

        public AuthController(EmailService emailService, ILogger<AuthController> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        /// <summary>
        /// Kayıt için OTP gönder
        /// </summary>
        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp([FromBody] SendOtpRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Email))
                {
                    return BadRequest(new { message = "Email adresi zorunludur" });
                }

                // 6 haneli OTP oluştur
                string otp = new Random().Next(100000, 999999).ToString();

                // OTP'yi memory'de depola (15 dakika geçerli)
                _otpStore[request.Email] = (otp, DateTime.UtcNow.AddMinutes(15));

                // Email gönder
                bool emailSent = await _emailService.SendOtpEmailAsync(request.Email, otp);

                if (!emailSent)
                {
                    return StatusCode(500, new { message = "Email gönderilemedi" });
                }

                _logger.LogInformation($"OTP gönderildi: {request.Email}");

                return Ok(new { message = "OTP emailinize gönderilmiştir. Lütfen kontrol ediniz." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "OTP gönderme hatası");
                return StatusCode(500, new { message = "Bir hata oluştu" });
            }
        }

        /// <summary>
        /// OTP doğrula ve kullanıcı kaydet
        /// </summary>
        [HttpPost("verify-otp")]
        public IActionResult VerifyOtp([FromBody] VerifyOtpRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Otp))
                {
                    return BadRequest(new { message = "Email ve OTP gereklidir" });
                }

                // OTP'yi kontrol et
                if (!_otpStore.ContainsKey(request.Email))
                {
                    return BadRequest(new { message = "OTP bulunamadı. Lütfen yeniden gönderin." });
                }

                var (storedOtp, expiry) = _otpStore[request.Email];

                // Geçerliliği kontrol et
                if (DateTime.UtcNow > expiry)
                {
                    _otpStore.Remove(request.Email);
                    return BadRequest(new { message = "OTP süresi dolmuştur. Lütfen yeniden gönderin." });
                }

                // OTP doğru mu kontrol et
                if (storedOtp != request.Otp)
                {
                    return BadRequest(new { message = "OTP hatalıdır" });
                }

                // OTP'yi sil
                _otpStore.Remove(request.Email);

                // Kullanıcıyı localStorage'a kaydet (frontend'de yapılır)
                _logger.LogInformation($"OTP doğrulandi: {request.Email}");

                return Ok(new
                {
                    message = "Email başarıyla doğrulandı!",
                    verified = true
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "OTP doğrulama hatası");
                return StatusCode(500, new { message = "Bir hata oluştu" });
            }
        }

        /// <summary>
        /// OTP'yi test etmek için (geliştirme ortamında)
        /// </summary>
        [HttpGet("debug-otp/{email}")]
        public IActionResult GetDebugOtp(string email)
        {
            if (_otpStore.ContainsKey(email))
            {
                var (otp, _) = _otpStore[email];
                return Ok(new { email, otp, message = "⚠️ Sadece test için! Üretimde kaldır!" });
            }

            return NotFound(new { message = "OTP bulunamadı" });
        }
    }

    public class SendOtpRequest
    {
        public string Email { get; set; }
    }

    public class VerifyOtpRequest
    {
        public string Email { get; set; }
        public string Otp { get; set; }
    }
}