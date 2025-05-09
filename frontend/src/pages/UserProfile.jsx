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
      <img src={userData.image} alt="" />

      {
        isEdit
          ? <input type="text" value={userData.name} onChange={e => setUserData(prev => ({ ...prev, name: e.target.value }))} />
          : <p>{userData.name}</p>
      }
      <hr/>
      <div>
        <p>CONTACT INFORMATION</p>
        <div>
          <p>Email:</p>
          <p>{userData.email}</p>
          <p>Phone:</p>

          {
            isEdit
              ? <input type="text" value={userData.phone} onChange={e => setUserData(prev => ({ ...prev, phone: e.target.value }))} />
              : <p>{userData.phone}</p>
          }
          
        </div>
      </div>
      
    </div>
  )
}

export default UserProfile
