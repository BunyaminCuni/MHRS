using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MHRS.Model
{
    public class AppointmentDbContext : DbContext
    {
        public AppointmentDbContext(DbContextOptions<AppointmentDbContext> options) : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Hospital> Hospitals { get; set; }
        public DbSet<Doctor> Doctors { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Patient - Appointment ilişkisi
            modelBuilder.Entity<Appointment>()
                .HasOne<Patient>()
                .WithMany()
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            // Hospital - City ilişkisi
            modelBuilder.Entity<Hospital>()
                .HasOne(h => h.City)
                .WithMany(c => c.Hospitals)
                .HasForeignKey(h => h.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            // Doctor - Hospital ilişkisi
            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.Hospital)
                .WithMany(h => h.Doctors)
                .HasForeignKey(d => d.HospitalId)
                .OnDelete(DeleteBehavior.Cascade);

            // Appointment - Doctor ilişkisi
            modelBuilder.Entity<Appointment>()
                .HasOne<Doctor>()
                .WithMany()
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.SetNull);

            // Örnek şehirler ekle
            // Türkiye'nin resmi 81 ili (Plaka kodlarına göre)
            modelBuilder.Entity<City>().HasData(
                new City { CityId = 1, CityName = "Adana" },
                new City { CityId = 2, CityName = "Adıyaman" },
                new City { CityId = 3, CityName = "Afyonkarahisar" },
                new City { CityId = 4, CityName = "Ağrı" },
                new City { CityId = 5, CityName = "Amasya" },
                new City { CityId = 6, CityName = "Ankara" },
                new City { CityId = 7, CityName = "Antalya" },
                new City { CityId = 8, CityName = "Artvin" },
                new City { CityId = 9, CityName = "Aydın" },
                new City { CityId = 10, CityName = "Balıkesir" },
                new City { CityId = 11, CityName = "Bilecik" },
                new City { CityId = 12, CityName = "Bingöl" },
                new City { CityId = 13, CityName = "Bitlis" },
                new City { CityId = 14, CityName = "Bolu" },
                new City { CityId = 15, CityName = "Burdur" },
                new City { CityId = 16, CityName = "Bursa" },
                new City { CityId = 17, CityName = "Çanakkale" },
                new City { CityId = 18, CityName = "Çankırı" },
                new City { CityId = 19, CityName = "Çorum" },
                new City { CityId = 20, CityName = "Denizli" },
                new City { CityId = 21, CityName = "Diyarbakır" },
                new City { CityId = 22, CityName = "Edirne" },
                new City { CityId = 23, CityName = "Elazığ" },
                new City { CityId = 24, CityName = "Erzincan" },
                new City { CityId = 25, CityName = "Erzurum" },
                new City { CityId = 26, CityName = "Eskişehir" },
                new City { CityId = 27, CityName = "Gaziantep" },
                new City { CityId = 28, CityName = "Giresun" },
                new City { CityId = 29, CityName = "Gümüşhane" },
                new City { CityId = 30, CityName = "Hakkari" },
                new City { CityId = 31, CityName = "Hatay" },
                new City { CityId = 32, CityName = "Isparta" },
                new City { CityId = 33, CityName = "Mersin" },
                new City { CityId = 34, CityName = "İstanbul" },
                new City { CityId = 35, CityName = "İzmir" },
                new City { CityId = 36, CityName = "Kars" },
                new City { CityId = 37, CityName = "Kastamonu" },
                new City { CityId = 38, CityName = "Kayseri" },
                new City { CityId = 39, CityName = "Kırklareli" },
                new City { CityId = 40, CityName = "Kırşehir" },
                new City { CityId = 41, CityName = "Kocaeli" },
                new City { CityId = 42, CityName = "Konya" },
                new City { CityId = 43, CityName = "Kütahya" },
                new City { CityId = 44, CityName = "Malatya" },
                new City { CityId = 45, CityName = "Manisa" },
                new City { CityId = 46, CityName = "Kahramanmaraş" },
                new City { CityId = 47, CityName = "Mardin" },
                new City { CityId = 48, CityName = "Muğla" },
                new City { CityId = 49, CityName = "Muş" },
                new City { CityId = 50, CityName = "Nevşehir" },
                new City { CityId = 51, CityName = "Niğde" },
                new City { CityId = 52, CityName = "Ordu" },
                new City { CityId = 53, CityName = "Rize" },
                new City { CityId = 54, CityName = "Sakarya" },
                new City { CityId = 55, CityName = "Samsun" },
                new City { CityId = 56, CityName = "Siirt" },
                new City { CityId = 57, CityName = "Sinop" },
                new City { CityId = 58, CityName = "Sivas" },
                new City { CityId = 59, CityName = "Tekirdağ" },
                new City { CityId = 60, CityName = "Tokat" },
                new City { CityId = 61, CityName = "Trabzon" },
                new City { CityId = 62, CityName = "Tunceli" },
                new City { CityId = 63, CityName = "Şanlıurfa" },
                new City { CityId = 64, CityName = "Uşak" },
                new City { CityId = 65, CityName = "Van" },
                new City { CityId = 66, CityName = "Yozgat" },
                new City { CityId = 67, CityName = "Zonguldak" },
                new City { CityId = 68, CityName = "Aksaray" },
                new City { CityId = 69, CityName = "Bayburt" },
                new City { CityId = 70, CityName = "Karaman" },
                new City { CityId = 71, CityName = "Kırıkkale" },
                new City { CityId = 72, CityName = "Batman" },
                new City { CityId = 73, CityName = "Şırnak" },
                new City { CityId = 74, CityName = "Bartın" },
                new City { CityId = 75, CityName = "Ardahan" },
                new City { CityId = 76, CityName = "Iğdır" },
                new City { CityId = 77, CityName = "Yalova" },
                new City { CityId = 78, CityName = "Karabük" },
                new City { CityId = 79, CityName = "Kilis" },
                new City { CityId = 80, CityName = "Osmaniye" },
                new City { CityId = 81, CityName = "Düzce" }
            );


            // Örnek hastaneler ekle
            modelBuilder.Entity<Hospital>().HasData(
                new Hospital
                {
                    HospitalId = 1,
                    HospitalName = "Acibadem Veteriner Hastanesi",
                    CityId = 34,
                    Phone = "0212-555-0001",
                    Address = "İstanbul, Kadıköy",
                    Description = "Modern veteriner hastanesi",
                    DistrictName = "Kadıköy"
                },
                new Hospital
                {
                    HospitalId = 2,
                    HospitalName = "American Hospital Vet",
                    CityId = 34,
                    Phone = "0212-555-0002",
                    Address = "İstanbul, Nişantaşı",
                    Description = "Uluslararası standartlarda hizmet",
                    DistrictName = "Şişli"
                },
                new Hospital
                {
                    HospitalId = 3,
                    HospitalName = "Ankara Veteriner Merkezi",
                    CityId = 6,
                    Phone = "0312-555-0001",
                    Address = "Ankara, Keçiören",
                    Description = "Ankara'nın en iyi veteriner merkezi",
                    DistrictName = "Keçiören"
                },
                new Hospital
                {
                    HospitalId = 4,
                    HospitalName = "İzmir Pet Hospital",
                    CityId = 35,
                    Phone = "0232-555-0001",
                    Address = "İzmir, Alsancak",
                    Description = "Evcil hayvanlar için özel hizmetler",
                    DistrictName = "Konak"
                },
                new Hospital
                {
                    HospitalId = 5,
                    HospitalName = "Tekirdağ Vet Kliniği",
                    CityId = 59,
                    Phone = "0282-555-0001",
                    Address = "Tekirdağ, Merkez",
                    Description = "Tekirdağ'da güvenilir veteriner hizmeti",
                    DistrictName = "Süleymanpaşa"
                }
            );
        }
    }

    [Table("cities")]
    public class City
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("cityId")]
        public int CityId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("cityName")]
        public string CityName { get; set; } = string.Empty;

        // İlişki: Bir şehirde birden fazla hastane olabilir
        public ICollection<Hospital> Hospitals { get; set; } = new List<Hospital>();
    }

    [Table("hospitals")]
    public class Hospital
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("hospitalId")]
        public int HospitalId { get; set; }

        [Required]
        [MaxLength(150)]
        [Column("hospitalName")]
        public string HospitalName { get; set; } = string.Empty;

        [Required]
        [Column("cityId")]
        public int CityId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("phone")]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        [Column("address")]
        public string Address { get; set; } = string.Empty;

        [MaxLength(500)]
        [Column("description")]
        public string Description { get; set; } = string.Empty;

        [MaxLength(100)]
        [Column("districtName")]
        public string DistrictName { get; set; } = string.Empty;

        // Foreign Key ilişkisi
        [ForeignKey("CityId")]
        public City City { get; set; }

        // İlişki: Bir hastanede birden fazla doktor olabilir
        public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
    }

    [Table("doctors")]
    public class Doctor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("doctorId")]
        public int DoctorId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("doctorName")]
        public string DoctorName { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        [Column("phone")]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [Column("hospitalId")]
        public int HospitalId { get; set; }

        // Foreign Key ilişkisi
        [ForeignKey("HospitalId")]
        public Hospital Hospital { get; set; }
    }

    [Table("patient")]
    public class Patient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("patientId")]
        public int PatientId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("patientName")]
        public string PatientName { get; set; } = string.Empty;

        [Required]
        [MaxLength(10)]
        [Column("mobileNo")]
        public string MobileNo { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        [Column("city")]
        public string City { get; set; } = string.Empty;

        [Required]
        [MaxLength(1000)]
        [Column("address")]
        public string Address { get; set; } = string.Empty;

        [MaxLength(50)]
        [Column("email")]
        public string Email { get; set; } = string.Empty;
    }

    [Table("appointments")]
    public class Appointment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("appointmentId")]
        public int AppointmentId { get; set; }

        [Required]
        [Column("patientId")]
        public int PatientId { get; set; }

        [Required]
        [Column("appointmentDate")]
        public DateTime AppointmentDate { get; set; }

        [Required]
        [Column("isDone")]
        public bool IsDone { get; set; } = false;

        [Required]
        [Column("hospitalId")]
        public int HospitalId { get; set; }

        [Column("doctorId")]
        public int? DoctorId { get; set; }
    }

    public class NewAppointmentRequest
    {
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Hasta adı zorunludur")]
        [MaxLength(50)]
        public string PatientName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Telefon numarası zorunludur")]
        [MaxLength(10)]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Geçerli bir telefon numarası giriniz")]
        public string MobileNo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şehir zorunludur")]
        public int CityId { get; set; }

        [Required(ErrorMessage = "İlçe zorunludur")]
        [MaxLength(100)]
        public string DistrictName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Hastane zorunludur")]
        public int HospitalId { get; set; }

        [Required(ErrorMessage = "Doktor zorunludur")]
        public int DoctorId { get; set; }

        [MaxLength(50)]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Randevu tarihi zorunludur")]
        public DateTime AppointmentDate { get; set; }
    }

    public class AppointmentResponseDto
    {
        public string PatientName { get; set; } = string.Empty;
        public string MobileNo { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public int AppointmentId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public bool IsDone { get; set; }
        public string? DoctorName { get; set; }
    }

    public class CreateDoctorRequest
    {
        [Required(ErrorMessage = "Doktor adı zorunludur")]
        [MaxLength(100)]
        public string DoctorName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Telefon zorunludur")]
        [MaxLength(20)]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Hastane zorunludur")]
        public int HospitalId { get; set; }
    }
}