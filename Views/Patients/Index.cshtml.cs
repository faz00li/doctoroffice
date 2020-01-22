using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DoctorOffice.Models;

namespace DoctorOffice.Views.Patients
{
    public class IndexModel : PageModel
    {
        private readonly DoctorOffice.Models.DoctorOfficeContext _context;

        public IndexModel(DoctorOffice.Models.DoctorOfficeContext context)
        {
            _context = context;
        }

        public IList<Patient> Patient { get;set; }

        public async Task OnGetAsync()
        {
            Patient = await _context.Patients.ToListAsync();
        }
    }
}
