import React, { createContext, useState } from 'react'
import axios from 'axios'
import { toast } from 'react-toastify'

export const DoctorContext = createContext()

const DoctorContextProvider = ({ children }) => {
  const [doctorToken, setDoctorToken] = useState(
    localStorage.getItem('doctorToken') || ''
  )
  const [appointments, setAppointments] = useState([])
  const [dashboardStats, setDashboardStats] = useState(null)
  const [profileData, setProfileData] = useState(null)

  const backendUrl = import.meta.env.VITE_BACKEND_URL

  const getDoctorAppointments = async () => {
    try {
      const { data } = await axios.get(`${backendUrl}api/doctor/get-appointments`, {
        headers: { Authorization: `Bearer ${doctorToken}` },
      })

      if (data.success) {
        setAppointments(data.appointments)
      } else {
        toast.error(data.message)
      }
    } catch (error) {
      toast.error(error.message)
    }
  }

  const completeAppointment = async (appointmentId) => {
    try {
      const { data } = await axios.put(
        `${backendUrl}api/doctor/complete-appointment`,
        { appointmentId },
        { headers: { Authorization: `Bearer ${doctorToken}` } }
      )

      if (data.success) {
        toast.success(data.message)
        getDoctorAppointments()
        fetchDashboardStats()
      } else {
        toast.error(data.message)
      }
    } catch (error) {
      toast.error(error.message)
    }
  }

  const cancelAppointment = async (appointmentId) => {
    try {
      const { data } = await axios.put(
        `${backendUrl}api/doctor/cancel-appointment`,
        { appointmentId },
        { headers: { Authorization: `Bearer ${doctorToken}` } }
      )

      if (data.success) {
        toast.success(data.message)
        getDoctorAppointments()
        fetchDashboardStats()
      } else {
        toast.error(data.message)
      }
    } catch (error) {
      toast.error(error.message)
    }
  }

  const fetchDashboardStats = async () => {
    try {
      const { data } = await axios.get(`${backendUrl}api/doctor/dashboard`, {
        headers: { Authorization: `Bearer ${doctorToken}` },
      })

      if (data.success) {
        setDashboardStats(data.dashdata)
      } else {
        toast.error(data.message)
      }
    } catch (error) {
      toast.error(error.message)
    }
  }

  const fetchDoctorProfile = async () => {
    try {
      const { data } = await axios.get(`${backendUrl}api/doctor/get-profile`, {
        headers: { Authorization: `Bearer ${doctorToken}` },
      })

      if (data.success) {
        setProfileData(data.doctor)
      } else {
        toast.error(data.message)
      }
    } catch (error) {
      toast.error(error.message)
    }
  }

  const contextValue = {
    doctorToken,
    setDoctorToken,
    backendUrl,
    appointments,
    setAppointments,
    getDoctorAppointments,
    completeAppointment,
    cancelAppointment,
    dashboardStats,
    setDashboardStats,
    fetchDashboardStats,
    profileData,
    setProfileData,
    fetchDoctorProfile,
  }

  return (
    <DoctorContext.Provider value={contextValue}>
      {children}
    </DoctorContext.Provider>
  )
}

export default DoctorContextProvider
