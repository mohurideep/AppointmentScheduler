using AppointmentScheduler.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentScheduler.Services
{
    public interface IAppointmentService
    {
        public List<DoctorViewModel> GetDoctorList();
        public List<PatientViewModel> GetPatientList();
        public Task<int> AddUpdate(AppointmentViewModel model);
        public List<AppointmentViewModel> DoctorsEventById(string doctorId);
        public List<AppointmentViewModel> PatientsEventById(string patientId);
        public AppointmentViewModel GetAppointmentById(int Id);
        public Task<int> DeleteAppointmentById(int Id);
        public Task<int> ConfirmAppointmentById(int Id);
    }
}
