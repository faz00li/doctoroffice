using System.Collections.Generic;

namespace DoctorOffice.Models
{
  public class Doctor
  {
    public int DoctorId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public virtual ICollection<DoctorPatient> Patients { get; set; }

    public Doctor()
    {
      this.Patients = new HashSet<DoctorPatient>();
    }
  }
}
