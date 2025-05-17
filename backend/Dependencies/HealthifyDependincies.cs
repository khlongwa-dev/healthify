using backend.Services.Interfaces;
using CloudinaryDotNet;

namespace backend.Dependencies
{
    public class HealthifyDependencies
    {
        public IDoctorService DoctorService { get; }
        public IJwtService JwtService { get; }
        public Cloudinary Cloudinary { get; }
        public IAppointmentService AppointmentService { get; }
        public IAdminService AdminService { get; }
        public IUserService UserService { get; }

        public HealthifyDependencies(
            IDoctorService doctorService,
            IJwtService jwtService,
            Cloudinary cloudinary,
            IAppointmentService appointmentService,
            IAdminService adminService,
            IUserService userService)
        {
            DoctorService = doctorService;
            JwtService = jwtService;
            Cloudinary = cloudinary;
            AppointmentService = appointmentService;
            AdminService = adminService;
            UserService = userService;
        }
    }
}