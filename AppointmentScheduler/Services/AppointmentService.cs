using AppointmentScheduler.Models;
using AppointmentScheduler.Models.ViewModels;
using AppointmentScheduler.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentScheduler.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext _dbContext;
        public AppointmentService(ApplicationDbContext dbcontext)
        {
            _dbContext = dbcontext;
        }

        public async Task<int> AddUpdate(AppointmentViewModel model)
        {
            var startDate = DateTime.Parse(model.StartDate);
            var endDate = DateTime.Parse(model.StartDate).AddMinutes(Convert.ToDouble(model.Duration));
            if (model != null && model.Id > 0)
            {
                //update
                return 1;
            }
            else
            {
                //create
                Appointment appointment = new Appointment
                {
                    Title = model.Title,
                    Description = model.Description,
                    StartDate = startDate,
                    EndDate = endDate,
                    Duration=model.Duration,
                    DoctorId = model.DoctorId,
                    PatientId = model.PatientId,
                    IsDoctorApproved = false,
                    AdminId = model.AdminId
                };

                _dbContext.Appointments.Add(appointment);
                await _dbContext.SaveChangesAsync();
                return 2;
            }
        }

        public List<AppointmentViewModel> DoctorsEventById(string doctorId)
        {
            return _dbContext.Appointments.Where(x => x.DoctorId == doctorId).ToList().Select(c => new AppointmentViewModel()
            {
                Id = c.Id,
                Description = c.Description,
                StartDate = c.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate = c.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Title = c.Title,
                Duration = c.Duration,
                IsDoctorApproved=c.IsDoctorApproved
                
            }).ToList();
        }

        public AppointmentViewModel GetAppointmentById(int Id)
        {
            return _dbContext.Appointments.Where(x => x.Id == Id).ToList().Select(c => new AppointmentViewModel()
            {
                Id = c.Id,
                Description = c.Description,
                StartDate = c.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate = c.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Title = c.Title,
                Duration = c.Duration,
                IsDoctorApproved = c.IsDoctorApproved,
                PatientId = c.PatientId,
                DoctorId = c.DoctorId,
                PatientName = _dbContext.Users.Where(x => x.Id == c.PatientId).Select(x => x.Name).FirstOrDefault(),
                DoctorName = _dbContext.Users.Where(x => x.Id == c.DoctorId).Select(x => x.Name).FirstOrDefault()
            }).SingleOrDefault();
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

        public List<AppointmentViewModel> PatientsEventById(string patientId)
        {
            return _dbContext.Appointments.Where(x => x.PatientId == patientId).ToList().Select(c => new AppointmentViewModel()
            {
                Id = c.Id,
                Description = c.Description,
                StartDate = c.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate = c.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Title = c.Title,
                Duration = c.Duration,
                IsDoctorApproved = c.IsDoctorApproved

            }).ToList();
        }
    }
}
