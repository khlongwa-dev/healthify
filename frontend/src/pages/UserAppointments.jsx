import React, { use, useContext, useEffect, useState } from 'react'
import { AppContext } from '../context/AppContext'
import axios from 'axios'
import { toast } from 'react-toastify'

const UserAppointments = () => {
  const { backendUrl, userToken, fetchDoctors } = useContext(AppContext)
  const [appointments, setAppointments] = useState([])
  const months = ["", "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"]
  
  const slotDateFormat = (slotDate) => {
    const dateArray = slotDate.split('-')

    return dateArray[0] + " " + months[Number(dateArray[1])] + " " + dateArray[2]
  }

  const getUserAppointments = async () => {
    try {
      const {data} = await axios.get(backendUrl + 'api/user/get-appointments', {headers:{Authorization: `Bearer ${userToken}`}})

      if (data.success) {
        setAppointments(data.appointments)
        fetchDoctors()
        console.log(data.appointments)
      }
    } catch (error) {
      console.log(error)
      toast.error(error.message)
    }
  }

  const cancelAppointment = async (appointmentId) => {
    try {
      
      const {data} = await axios.put(backendUrl + 'api/user/cancel-appointment', {appointmentId}, {headers:{Authorization: `Bearer ${userToken}`}})
      
      if(data.success) {
        toast.success(data.message)
        getUserAppointments()
      } else {
        toast.error(data.message)
      }
    } catch (error) {
      console.log(error)
      toast.error(error.message)
    }
  }

  const clearAppointment = async (appointmentId) => {
    try {
      
      const {data} = await axios.post(backendUrl + 'api/user/clear-appointment', {appointmentId}, {headers:{Authorization: `Bearer ${userToken}`}})
      
      if(data.success) {
        toast.success(data.message)
        getUserAppointments()
      } else {
        toast.error(data.message)
      }
    } catch (error) {
      console.log(error)
      toast.error(error.message)
    }
  }

  const payOnline = async () => {
    toast.warn("Online payment currently not available.")
  }

  useEffect(()=>{
    if (userToken) {
      getUserAppointments()
    }
  }, [userToken])


  return (
    <div>
      <p className='pb-3 mt-12 font-medium text-zinc-700 border-b'>My appointments</p>
      <div>
        {appointments.map((item, index)=>(
          <div className='grid grid-cols-[1fr_2fr] gap-4 sm:flex sm:gap-6 py-2 border-b' key={index}>
            <div>
              <img className='w-32 bg-[#fdf6ee]' src={item.doctor.imageUrl} alt="" />
            </div>
            <div className='flex-1 text-sm text-zinc-700'>
              <p className='text-neutral-800 font-semibold'>{item.doctor.name}</p>
              <p>{item.doctor.specialty}</p>
              <p className='text-zinc-700 font-medium mt-1'>Address:</p>
              <p className='text-xs'>{item.doctor.addressLine1}</p>
              <p className='text-xs'>{item.doctor.addressLine2}</p>
              <p className='text-xs mt-1'><span className='text-sm text-neutral-700 font-medium'>Date & Time:</span> {slotDateFormat(item.slotDate)} | {item.slotTime}</p>
            </div>
            <div></div>
            <div className='flex flex-col gap-2 justify-end'>
              {!item.cancelled && !item.isCompleted && <button onClick={()=>payOnline()} className='text-sm text-stone-500 text-center sm:min-w-48 py-2 border rounded hover:bg-primary hover:text-white transition-all duration-300'>Pay Online</button>}
              {!item.cancelled && !item.isCompleted && <button onClick={()=>cancelAppointment(item.id)} className='text-sm text-stone-500 text-center sm:min-w-48 py-2 border rounded hover:bg-red-600 hover:text-white transition-all duration-300'>Cancel appointment</button>}
              
              {item.cancelled && !item.isCompleted && <button className='sm:min-w-48 py-2 border border-red-500 rounded text-red-500'>Appointment cancelled</button>}
              {item.isCompleted && <button className='sm:min-w-48 py-2 border border-green-500 rounded text-green-500'>Appointment completed</button>}
              {item.cancelled  && <button onClick={()=>clearAppointment(item.id)} className='text-sm text-orange-400 text-center sm:min-w-48 py-2 border border-orange-400 rounded hover:bg-orange-400 hover:text-white transition-all duration-300'>Clear appointment</button>}
              {item.isCompleted && <button onClick={()=>clearAppointment(item.id)} className='text-sm text-orange-400 text-center sm:min-w-48 py-2 border border-orange-400 rounded hover:bg-orange-400 hover:text-white transition-all duration-300'>Clear appointment</button>}
            </div>
          </div>
        ))}
      </div>
    </div>
  )
}

export default UserAppointments
