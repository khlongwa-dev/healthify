import React, { useContext, useEffect } from 'react'
import { AdminContext } from '../../context/AdminContext'

const Dashboard = () => {

  const {aToken, getDashData, cancelAppointment, dashData} = useContext(AdminContext)

  useEffect(()=>{
    if (aToken) {
      getDashData()
    }
  }, [aToken])

  return (
    <div>
      
    </div>
  )
}

export default Dashboard
