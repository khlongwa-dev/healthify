import React, { createContext, useState } from 'react'
import { toast } from 'react-toastify'
import axios from 'axios'

export const DoctorContext = createContext()

const DoctorContextProvider = (props) => {
    const [dToken, setDToken] = useState(localStorage.getItem('dToken')?localStorage.getItem('dToken'):'')
    const [appointments, setAppointments] = useState([])
    const [dashData, setDashData] = useState(false)
    const backendUrl = import.meta.env.VITE_BACKEND_URL

    const getDoctorAppointments = async () => {
        try {

            const {data} = await axios.get(backendUrl + 'api/doctor/appointments', {headers:{dToken}})
            if (data.success) {
                setAppointments(data.appointments)
                console.log(data.appointments)
            } else {
                toast.error(data.message)
            }
        } catch (error) {
            console.log("the issue is here dummy")
            console.log(error)
            toast.error(error.message)
        }
    }

    const completeAppointment = async (appointmentId) => {
        try {
            
            const {data} = await axios.post(backendUrl + 'api/doctor/complete-appointment', {appointmentId}, {headers:{dToken}})
            if (data.success) {
                toast.success(data.message)
                getDoctorAppointments()
            } else {
                toast.error(data.message)
            }
        } catch (error) {
            console.log(error)
            toast.error(error.message)
        }
    }

    const cancelAppointment = async (appointmentId) => {
        try {
            
            const {data} = await axios.post(backendUrl + 'api/doctor/cancel-appointment', {appointmentId}, {headers:{dToken}})
            if (data.success) {
                toast.success(data.message)
                getDoctorAppointments()
            } else {
                toast.error(data.message)
            }
        } catch (error) {
            console.log(error)
            toast.error(error.message)
        }
    }

    const getDashboardData = async () => {
        try {
            const {data} = await axios.get(backendUrl + 'api/doctor/dashboard', {headers:{dToken}})

            if (data.success) {
                setDashData(data.dashdata)
                console.log(data.dashdata)
            } else {
                toast.error(data.message)
            }
        } catch (error) {
            console.log(error)
            toast.error(error.message)
        }
    }
    
    const value = {
        dToken, setDToken,
        backendUrl, appointments,
        setAppointments, getDoctorAppointments,
        completeAppointment, cancelAppointment,
        dashData, setDashData,
        getDashboardData
    }

    return (
        <DoctorContext.Provider value= {value}>
            {props.children}
        </DoctorContext.Provider>
    )
}

export default DoctorContextProvider
