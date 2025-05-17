using backend.Data;
using backend.DTOs;
using backend.Models;
using backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace backend.Services.Implementations
{
    public class AppointmentService : IAppointmentService
    {
        private readonly AppDbContext _context;

        public AppointmentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> BookAppointmentAsync(BookingAppointmentDto dto, int userId)
        {
            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(d => d.Id == dto.DoctorId);

            if (doctor == null || !doctor.Available) return false;

            var bookedSlot = await _context.BookedSlots
                .FirstOrDefaultAsync(bs =>
                    bs.DoctorId == dto.DoctorId &&
                    bs.SlotDate == dto.SlotDate &&
                    bs.SlotTime == dto.SlotTime);

            if (bookedSlot != null) return false;

            var appointment = new Appointment
            {
                UserId = userId,
                DoctorId = dto.DoctorId,
                SlotDate = dto.SlotDate,
                SlotTime = dto.SlotTime,
                DoctorFee = doctor.Fees,
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            var userAppointment = new UserAppointment
            {
                Id = appointment.Id,
                UserId = userId,
                DoctorId = dto.DoctorId,
                SlotDate = dto.SlotDate,
                SlotTime = dto.SlotTime,
                DoctorFee = doctor.Fees,
            };

            var slot = new BookedSlot
            {
                DoctorId = dto.DoctorId,
                SlotDate = dto.SlotDate,
                SlotTime = dto.SlotTime
            };

            _context.UserAppointments.Add(userAppointment);
            _context.BookedSlots.Add(slot);

            await _context.SaveChangesAsync();

            return true;
        }


        
    }
}