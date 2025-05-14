import React from 'react'
import { useContext } from 'react'
import { AdminContext } from '../../context/AdminContext'
import { useEffect } from 'react'

const AllAppointments = () => {
  const {aToken, appoinments, getAllAppointments} = useContext(AdminContext)

  useEffect(()=>{
    if (aToken) {
      getAllAppointments()
    }
  }, [aToken])

  return (
    <div>
      <p>All appoinments</p>
      <div>
        <div>
          <p>#</p>
          <p>Patient</p>
          <p>Age</p>
          <p>Date & Time</p>
          <p>Doctor</p>
          <p>Fees</p>
          <p>Actions</p>
        </div>
      </div>
    </div>
  )
}

export default AllAppointments
