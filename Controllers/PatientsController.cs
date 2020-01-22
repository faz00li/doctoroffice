using DoctorOffice.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DoctorOffice.Controllers
{
  public class PatientsController : Controller
  {
    private readonly DoctorOfficeContext _db;

    public PatientsController(DoctorOfficeContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      return View(_db.Patients.ToList());
    }

    public ActionResult Details(int id)
    {
      Patient thisPatient = _db.Patients//get all patients
        .Include(patients => patients.Doctors)//..and their join tables
        .ThenInclude(join => join.Doctor)//then for every join, get the doctor object
        .FirstOrDefault(patient => patient.PatientId == id);//which db entry used
      return View(thisPatient);
    }

    public ActionResult Create()
    {
      ViewBag.DoctorId = new MultiSelectList(_db.Doctors, "DoctorId", "LastName");
      return View();
    }

    [HttpPost]
    public ActionResult Create(Patient patient, int[] doctorId)
    {
      _db.Patients.Add(patient);

      foreach (var id in doctorId)
      {
        if (id != 0)
        {
          _db.DoctorPatient.Add(new DoctorPatient() { DoctorId = id, PatientId = patient.PatientId });
        }
      }

      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Edit(int id)
    {
      ViewBag.DoctorId = new SelectList(_db.Doctors, "DoctorId", "LastName");
      Patient thisPatient = _db.Patients
        .Include(patients => patients.Doctors)
        .ThenInclude(join => join.Doctor)
        .FirstOrDefault(patient => patient.PatientId == id);
      return View(thisPatient);
    }

    [HttpPost]
    public ActionResult Edit(Patient patient, int doctorId)
    {
      bool match = _db.DoctorPatient.Any(join => join.DoctorId == doctorId && join.PatientId == patient.PatientId);


      if (doctorId != 0 && match == false)
      {
        _db.DoctorPatient.Add(new DoctorPatient() { DoctorId = doctorId, PatientId = patient.PatientId });
      }

      _db.Entry(patient).State = EntityState.Modified;
      _db.SaveChanges();

      return RedirectToAction("Index");
    }

    public ActionResult AddDoctor(int id)
    {
      var thisPatient = _db.Patients.FirstOrDefault(patient => patient.PatientId == id);
      ViewBag.DoctorId = new SelectList(_db.Doctors, "DoctorId", "LastName");
      return View(thisPatient);
    }

    [HttpPost]
    public ActionResult AddDoctor(Patient patient, int doctorId)
    {
      bool match = _db.DoctorPatient.Any(join => join.DoctorId == doctorId && join.PatientId == patient.PatientId);

      if(doctorId != 0 && match == false)
      {
        _db.DoctorPatient.Add(new DoctorPatient { DoctorId = doctorId, PatientId = patient.PatientId });
        _db.SaveChanges();
      }
      
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      var thisPatient = _db.Patients.FirstOrDefault(patient => patient.PatientId == id);

      return View(thisPatient);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      var thisPatient = _db.Patients
        .FirstOrDefault(patient => patient.PatientId == id);
      _db.Remove(thisPatient);
      _db.SaveChanges();

      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult DeleteDoctor(int joinId)
    {
      var joinEntry = _db.DoctorPatient.FirstOrDefault(join => join.DoctorPatientId == joinId);
      _db.DoctorPatient.Remove(joinEntry);//try catch to handle exception
      _db.SaveChanges();

      return RedirectToAction("Index");
    }

  }
}