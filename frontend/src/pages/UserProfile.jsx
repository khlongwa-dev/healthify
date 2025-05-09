import React, { useState } from 'react'
import { assets } from '../assets/assets'

const UserProfile = () => {
  const [userData, setUserData] = useState({
    name: "Kusasalakhe Hlongwa",
    image: assets.profile_pic,
    email: 'ayandahlongwa21@gmail.com',
    phone: '+27 63 384 8266',
    address: {
      line1: "31 Acutt Street",
      line2: "Durban, South Africa"
    },
    gender: 'Male',
    dob: '2001-04-03'
  })

  const [isEdit, setIsEdit] = useState(false)
  return (
    <div>
      
    </div>
  )
}

export default UserProfile
