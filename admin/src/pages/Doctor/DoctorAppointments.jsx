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
      
    </div>
  )
}

export default DoctorAppointments
