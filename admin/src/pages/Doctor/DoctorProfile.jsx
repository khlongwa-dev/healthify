import React, { useEffect } from 'react'
import { useContext } from 'react'
import { DoctorContext } from '../../context/DoctorContext'


const DoctorProfile = () => {
  const {dToken, loadDoctorProfileData} = useContext(DoctorContext)

  useEffect(()=>{
    if(dToken) {
      loadDoctorProfileData()
    }
  })
  return (
    <div>
      
    </div>
  )
}

export default DoctorProfile
