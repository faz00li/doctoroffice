using System.Collections.Generic;

namespace DoctorOffice.Models
{
  public class Patient
  {
    public int PatientId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public ICollection<DoctorPatient> Doctors { get; }

    public Patient()
    {
      this.Doctors = new HashSet<DoctorPatient>();					 
    }
  }
}
