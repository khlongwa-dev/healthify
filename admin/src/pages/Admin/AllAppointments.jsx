import React from 'react'
import { useContext } from 'react'
import { AdminContext } from '../../context/AdminContext'
import { useEffect } from 'react'
import { AppContext } from '../../context/AppContext'
import { assets } from '../../assets/assets'

const AllAppointments = () => {
  const { adminToken, appointments, fetchAppointments, cancelAppointment, clearAppointment } = useContext(AdminContext)
  const { calculateAge, formatSlotDate, currencySymbol } = useContext(AppContext)

  useEffect(() =>
    {
      if (adminToken)
      {
        fetchAppointments()
      }
    }, [adminToken])

  return (
    <div className='w-full max-w-6xl m-5'>
      <p className='mb-3 text-lg font-medium'>All appoinments</p>
      <div className='bg-white border rounded text-sm max-h-[80vh] min-h-[60vh] overflow-y-scroll'>
        <div className='hidden sm:grid grid-cols-[0.5fr_3fr_1fr_3fr_3fr_1fr_1fr] grid-flow-col py-3 px-6 border-b'>
          <p>#</p>
          <p>Patient</p>
          <p>Age</p>
          <p>Date & Time</p>
          <p>Doctor</p>
          <p>Fees</p>
          <p>Actions</p>
        </div>
        {appointments.map((item, index) => (
          <div className='flex flex-wrap justify-between max-sm:gap-2 sm:grid sm:grid-cols-[0.5fr_3fr_1fr_3fr_3fr_1fr_1fr] items-center text-gray-500 py-3 px-6 border-b hover:bg-gray-50' key={index}>
            <p className='max-sm:hidden'>{index + 1}</p>
            <div className='flex items-center gap-2'>
              <img className='w-8 rounded-full' src={item.user.imageUrl} alt="" /> <p>{item.user.name}</p>
            </div>
            <p className='max-sm:hidden'>{calculateAge(item.user.doB)}</p>
            <p>{formatSlotDate(item.slotDate)}, {item.slotTime}</p>
            <div className='flex items-center gap-2'>
              <img className='w-8 rounded-full bg-gray-200' src={item.doctor.imageUrl} alt="" /> <p>{item.doctor.name}</p>
            </div>
            <p>{currencySymbol}{item.doctorFee}</p>
            {
              item.cancelled || item.isCompleted ? (
                <div className='flex items-center gap-2'>
                  <p className={`text-xs font-medium ${item.cancelled ? 'text-red-400' : 'text-green-500'}`}>
                    {item.cancelled ? 'Cancelled' : 'Completed'}
                  </p>
                  <span
                    className='text-orange-400 text-xs cursor-pointer font-medium'
                    onClick={() => clearAppointment(item.id)}
                  >
                    Clear
                  </span>
                </div>
              ) : (
                <img
                  onClick={() => cancelAppointment(item.id)}
                  className='w-10 cursor-pointer'
                  src={assets.cancel_icon}
                  alt="Cancel"
                />
              )
            }
          </div>
        ))}
      </div>
    </div>
  )
}

export default AllAppointments
