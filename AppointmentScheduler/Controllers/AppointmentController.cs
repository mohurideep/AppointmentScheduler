using AppointmentScheduler.Services;
using AppointmentScheduler.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentScheduler.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }
        public IActionResult Index()
        {
            ViewBag.duration = Helper.GetTimeDropDown();
            ViewBag.getDoctorList = _appointmentService.GetDoctorList();
            ViewBag.getPatientList = _appointmentService.GetPatientList();
            return View();
        }
    }
}
