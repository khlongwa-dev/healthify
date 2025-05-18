import React, { createContext, useState } from 'react'
import axios from 'axios'
import { toast } from 'react-toastify'

export const AdminContext = createContext()

const AdminContextProvider = ({ children }) => {
  const [adminToken, setAdminToken] = useState(localStorage.getItem('adminToken') || '')
  const [doctors, setDoctors] = useState([])
  const [appointments, setAppointments] = useState([])
  const [dashboardStats, setDashboardStats] = useState(null)

  const backendUrl = import.meta.env.VITE_BACKEND_URL

  // Get dashboard statistics
  const fetchDashboardStats = async () => {
    try {
      const { data } = await axios.get(`${backendUrl}api/admin/dashboard`, {
        headers: { Authorization: `Bearer ${adminToken}` }
      })

      if (data.success) {
        setDashboardStats(data.dashdata)
      }
    } catch (err) {
      toast.error(err.message)
    }
  }

  // Fetch all doctors
  const fetchDoctors = async () => {
    try {
      const { data } = await axios.get(
        `${backendUrl}api/admin/doctors-list`,
        {},
        {
          headers: { Authorization: `Bearer ${adminToken}` }
        }
      )

      if (data.success) {
        setDoctors(data.doctors)
      } else {
        toast.error(data.message)
      }
    } catch (err) {
      toast.error(err.message)
    }
  }

  // Toggle doctor availability
  const toggleDoctorAvailability = async (doctorId) => {
    try {
      const { data } = await axios.put(
        `${backendUrl}api/admin/change-availability`,
        { doctorId },
        {
          headers: { Authorization: `Bearer ${adminToken}` }
        }
      )

      if (data.success) {
        toast.success(data.message)
        fetchDoctors()
      } else {
        toast.error(data.message)
      }
    } catch (err) {
      toast.error(err.message)
    }
  }

  // Fetch all appointments
  const fetchAppointments = async () => {
    try {
      const { data } = await axios.get(`${backendUrl}api/admin/appointments-list`, {
        headers: { Authorization: `Bearer ${adminToken}` }
      })

      if (data.success) {
        setAppointments(data.appointments)
      } else {
        toast.error(data.message)
      }
    } catch (err) {
      toast.error(err.message)
    }
  }

  // Cancel appointment
  const cancelAppointment = async (appointmentId) => {
    try {
      const { data } = await axios.put(
        `${backendUrl}api/admin/cancel-appointment`,
        { appointmentId },
        {
          headers: { Authorization: `Bearer ${adminToken}` }
        }
      )

      if (data.success) {
        toast.success(data.message)
        fetchAppointments()
      } else {
        toast.error(data.message)
      }
    } catch (err) {
      toast.error(err.message)
    }
  }

  

  const contextValue = {
    adminToken,
    setAdminToken,
    backendUrl,
    doctors,
    fetchDoctors,
    toggleDoctorAvailability,
    appointments,
    fetchAppointments,
    cancelAppointment,
    dashboardStats,
    fetchDashboardStats
  }

  return (
    <AdminContext.Provider value={contextValue}>
      {children}
    </AdminContext.Provider>
  )
}

export default AdminContextProvider
