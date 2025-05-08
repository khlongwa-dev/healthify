import React, { useContext, useEffect, useState } from 'react'
import { useParams } from 'next/navigation'
import { AppContext } from '../context/AppContext'
import RelatedDoctors from '../components/RelatedDoctors'

const Appointment = () => {
  const { doctorId } = useParams()
  const { doctors, currencySymbol } = useContext(AppContext)
  const daysOfWeek = ['SUN', 'MON', 'TUE', 'WED', 'THU', 'FRI', 'SAT']

  const [doctorInfo, setDoctorInfo] = useState(null)
  const [doctorSlots, setDoctorSlots] = useState([])
  const [slotIndex, setSlotIndex] = useState(0)
  const [slotTime, setSlotTime] = useState('')

  const fetchDoctorInfo = async () => {
    const doctorInfo = doctors.find(doc => doc._id === doctorId)
    setDoctorInfo(doctorInfo)
  }

  const getAvailableSlots = async () => {
    setDoctorSlots([])

    // getting correct date
    let today = new Date()

    for (let i = 0; i < 7; i++) {
      // getting date with index
      let currentDate = new Date(today)
      currentDate.setDate(today.getDate() + i)

      // setting end time of the date with index
      let endTime = new Date()
      endTime.setDate(today.getDate() + i)
      endTime.setHours(21, 0, 0, 0)

      // setting hours
      if (today.getDate() === currentDate.getDate()) {
        currentDate.setHours(currentDate.getHours() > 10 ? currentDate.getHours() + 1 : 10)
        currentDate.setMinutes(currentDate.getMinutes() > 30 ? 30 : 0)
      } else {
        currentDate.setHours(10)
        currentDate.setMinutes(0)
      }

      let timeSlots = []

      while (currentDate < endTime) {
        let formattedTime = currentDate.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })

        // add slot to array
        timeSlots.push({
          datetime: new Date(currentDate),
          time: formattedTime
        })

        // increment current time by 30 minutes
        currentDate.setMinutes(currentDate.getMinutes() + 30)
      }

      setDoctorSlots(prev => ([...prev, timeSlots]))
    }
  }

  useEffect(()=>{
    fetchDoctorInfo()
  }, [doctors, doctorId])

  useEffect(()=>{
    getAvailableSlots()
    
  }, [doctorInfo])

  return (
    <div>

    </div>
  )
}

export default Appointment
