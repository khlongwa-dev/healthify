import React, { createContext, useState } from 'react'
import { toast } from 'react-toastify'

export const DoctorContext = createContext()

const DoctorContextProvider = (props) => {
    const [dToken, setDToken] = useState(localStorage.getItem('dToken')?localStorage.getItem('dToken'):'')
    const [appointments, setAppointments] = useState([])
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
            console.log(error)
            toast.error(error.message)
        }
    }
    
    const value = {
        dToken, setDToken,
        backendUrl, appointments,
        setAppointments, getDoctorAppointments
    }

    return (
        <DoctorContext.Provider value= {value}>
            {props.children}
        </DoctorContext.Provider>
    )
}

export default DoctorContextProvider
