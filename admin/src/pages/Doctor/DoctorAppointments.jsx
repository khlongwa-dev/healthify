import React from 'react'
import { useContext } from 'react'
import { DoctorContext } from '../../context/DoctorContext'
import { useEffect } from 'react'
import { AppContext } from '../../context/AppContext'

const DoctorAppointments = () => {

  const { dToken, appointments, getDoctorAppointments } = useContext(DoctorContext)
  const { calculateAge } = useContext(AppContext)

  useEffect (()=>{
    if(dToken) {
      getDoctorAppointments()
    }
  }, [dToken])
  
  return (
    <div className='w-full max-w-6xl m-5'>
      <p className='mb-3 text-lg font-medium'>All Appointments</p>
      <div className='bg-white border rounded text-sm max-h-[80vh] min-h-[50vh] overflow-y-scroll'>
        <div className='max-sm:hidden grid grid-cols-[0.5fr_2fr_1fr_1fr_3fr_1fr_1fr] gap-1 py-2 px-6 border-b'>
          <p>#</p>
          <p>Patient</p>
          <p>Payment</p>
          <p>Age</p>
          <p>Date & Time</p>
          <p>Fees</p>
          <p>Actions</p>
        </div>
        {
          appointments.map((item, index)=>(
            <div key={index}>
              <p>{index+1}</p>
              <div>
                <img src={item.user.imageUrl} alt="" /> <p>{item.user.name}</p>
              </div>
              <div>
                <p>
                  {item.paid ? 'Online' : 'CASH'}
                </p>
              </div>
              <p>{calculateAge(item.user.doB)}</p>
            </div>
          ))
        }
      </div>
    </div>
  )
}

export default DoctorAppointments
