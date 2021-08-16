using AppointmentScheduler.Models;
using AppointmentScheduler.Models.ViewModels;
using AppointmentScheduler.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentScheduler.Services
{
    public class AppointmentSerive : IAppointmentService
    {
        private readonly ApplicationDbContext _dbContext;
        public AppointmentSerive(ApplicationDbContext dbcontext)
        {
            _dbContext = dbcontext;
        }
        public List<DoctorViewModel> GetDoctorList()
        {
            var doctor = (from users in _dbContext.Users
                          join userRoles in _dbContext.UserRoles on users.Id equals userRoles.UserId
                          join roles in _dbContext.Roles.Where(x=> x.Name == Helper.Doctor) on userRoles.RoleId equals roles.Id
                          select new DoctorViewModel
                          {
                              ID = users.Id,
                              Name=users.Name
                          }).ToList();
            return doctor;
        }

        public List<PatientViewModel> GetPatientList()
        {
            var patient = (from users in _dbContext.Users
                           join userRoles in _dbContext.UserRoles on users.Id equals userRoles.UserId
                           join roles in _dbContext.Roles.Where(x => x.Name == Helper.Patient) on userRoles.RoleId equals roles.Id
                           select new PatientViewModel
                           {
                               ID = users.Id,
                               Name = users.Name
                           }).ToList();

            return patient;
        }
    }
}
