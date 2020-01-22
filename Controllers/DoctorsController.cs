using DoctorOffice.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DoctorOffice.Controllers
{
  public class DoctorsController : Controller
  {
    private readonly DoctorOfficeContext _db;

    public DoctorsController(DoctorOfficeContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      return View(_db.Doctors.ToList());
    }

    public ActionResult Details(int id)
    {
      var thisDoctor = _db.Doctors
        .Include(doctor => doctor.Patients)
        .ThenInclude(join => join.Patient)
        .FirstOrDefault(doctor => doctor.DoctorId == id);
      return View(thisDoctor);
    }

    public ActionResult Create()
    {
      return View("Create");
    }

    [HttpPost]
    public ActionResult Create(Doctor doctor)
    {
      _db.Doctors.Add(doctor);
      _db.SaveChanges();
      return RedirectToAction("Index"); 
    }

    public ActionResult Edit(int id)
    { 
      ViewBag.PatientId = new SelectList(_db.Patients, "PatientId", "LastName");

      var thisDoctor = _db.Doctors.Include(doctor => doctor.Patients).ThenInclude(join => join.Patient).FirstOrDefault(doctor => doctor.DoctorId == id);
      return View(thisDoctor);
    }

    [HttpPost]
    public ActionResult Edit(Doctor doctor, int patientId)
    {
      //check if doctor already has patient
      bool match = _db.DoctorPatient.Any(join => join.PatientId == patientId && join.DoctorId == doctor.DoctorId);

      if (patientId != 0 && match == false)
      {
        _db.DoctorPatient.Add(new DoctorPatient { DoctorId = doctor.DoctorId, PatientId = patientId });
      }

      _db.Entry(doctor).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      var thisDoctor = _db.Doctors.FirstOrDefault(doctor => doctor.DoctorId == id);
      return View(thisDoctor);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult ConfrimDelete(int id)
    { 
      var thisDoctor = _db.Doctors.FirstOrDefault(doctor => doctor.DoctorId == id);
      _db.Remove(thisDoctor);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    //TODO remove patient

  }
}