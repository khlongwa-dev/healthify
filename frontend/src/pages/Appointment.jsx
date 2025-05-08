import React, { useContext, useEffect, useState } from 'react'
import { useParams } from 'next/navigation'
import { AppContext } from '../context/AppContext'
import RelatedDoctors from '../components/RelatedDoctors'

const Appointment = () => {
  const {doctorId} = useParams()
  const {doctors, currencySymbol} = useContext(AppContext)
  const daysOfWeek = ['SUN', 'MON', 'TUE', 'WED', 'THU', 'FRI', 'SAT']

  const [doctorInfo, setDoctorInfo] = useState(null)
  const [doctorSlots, setDoctorSlots] = useState([])
  const [slotIndex, setSlotIndex] = useState(0)
  const [slotTime, setSlotTime] = useState('')

  const fetchDoctorInfo = async () => {
    const doctorInfo = doctors.find(doc => doc._id === doctorId)
    setDoctorInfo(doctorInfo)
  }

  return (
    <div>
      
    </div>
  )
}

export default Appointment
