import React from 'react'
import { useContext } from 'react'
import { DoctorContext } from '../../context/DoctorContext'
import { useEffect } from 'react'

const DoctorAppointments = () => {

  const { dToken, appointments, getDoctorAppointments } = useContext(DoctorContext)

  useEffect (()=>{
    if(dToken) {
      getDoctorAppointments()
    }
  }, [dToken])
  
  return (
    <div>
      <p>All Appointments</p>
      <div>
        <div>
          <p>#</p>
          <p>Patient</p>
          <p>Payment</p>
          <p>Age</p>
          <p>Date & Time</p>
          <p>Fees</p>
          <p>Actions</p>
        </div>
      </div>
    </div>
  )
}

export default DoctorAppointments
