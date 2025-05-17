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


        public async Task<bool> CancelAppointmentAsync(int appointmentId, int? callerId, string callerRole)
        {
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            var userAppointment = await _context.UserAppointments.FindAsync(appointmentId);

            if (appointment == null || userAppointment == null) return false;

            if (callerRole == "User" && appointment.UserId != callerId)
                return false;

            if (callerRole == "Doctor" && appointment.DoctorId != callerId)
                return false;

            if (callerRole == "Admin" && callerId == null)
                return false;

            appointment.Cancelled = true;
            userAppointment.Cancelled = true;


            var slot = await _context.BookedSlots
                .FirstOrDefaultAsync(bs =>
                    bs.DoctorId == appointment.DoctorId &&
                    bs.SlotDate == appointment.SlotDate &&
                    bs.SlotTime == appointment.SlotTime);

            if (slot != null)
                _context.BookedSlots.Remove(slot);

            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> CompleteAppointmentAsync(int appointmentId, int callerId)
        {
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            var userAppointment = await _context.UserAppointments.FindAsync(appointmentId);

            if (appointment == null || appointment.DoctorId != callerId) return false;
            if (userAppointment == null || userAppointment.DoctorId != callerId) return false;

            appointment.IsCompleted = true;
            userAppointment.IsCompleted = true;
            
            var slot = await _context.BookedSlots
                .FirstOrDefaultAsync(bs =>
                    bs.DoctorId == appointment.DoctorId &&
                    bs.SlotDate == appointment.SlotDate &&
                    bs.SlotTime == appointment.SlotTime);

            if (slot != null)
                _context.BookedSlots.Remove(slot);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ClearAppointmentAsync(int appointmentId)
        {
            var appointment = await _context.Appointments.FindAsync(appointmentId);

            if (appointment == null) return false;

            
            _context.Appointments.Remove(appointment);

            await _context.SaveChangesAsync();
            return true;
        }

        
    }
}