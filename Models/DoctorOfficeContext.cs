using Microsoft.EntityFrameworkCore;

namespace DoctorOffice.Models
{
  public class DoctorOfficeContext : DbContext
  {
    public DoctorOfficeContext(DbContextOptions options) : base(options) { }

    public virtual DbSet<Patient> Patients { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<DoctorPatient> DoctorPatient { get; set; }
  }
}
