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
    <div className='max-w-lg flex flex-col gap-2 text-sm'>
      <img className='w-36 rounded' src={userData.image} alt="" />

      {
        isEdit
          ? <input className='bg-gray-50 text-3xl font-medium max-w-60 mt-4' type="text" value={userData.name} onChange={e => setUserData(prev => ({ ...prev, name: e.target.value }))} />
          : <p className='font-medium text-3xl text-neutral-800 mt-4'>{userData.name}</p>
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
          <p>Address:</p>
          {
            isEdit
              ? <p>
                <input type="text" value={userData.address.line1} onChange={e => setUserData(prev => ({ ...prev, address: { ...prev.address, line1: e.target.value } }))} />
                <br />
                <input type="text" value={userData.address.line2} onChange={e => setUserData(prev => ({ ...prev, address: { ...prev.address, line2: e.target.value } }))} />
              </p>

              : <p>
                {userData.address.line1}
                <br />
                {userData.address.line2}
              </p>
          }
        </div>
      </div>
      <div>
        <p>BASIC INFORMATION</p>
        <div>
          <p>Gender:</p>
          {
            isEdit
              ? <select value={userData.gender} onChange={(e) => setUserData(prev => ({ ...prev, gender: e.target.value }))}>
                <option value="Male">Male</option>
                <option value="Female">Female</option>
              </select>
              : <p>{userData.gender}</p>
          }
          <p>Birthday:</p>
          {
            isEdit 
            ? <input type="date" value={userData.dob} onChange={(e) => setUserData(prev => ({ ...prev, dob: e.target.value }))}/>
            : <p>{userData.dob}</p>
          }
        </div>
      </div>
      <div>
          {
            isEdit
            ? <button onClick={()=>setIsEdit(false)} >Save Information</button>
            : <button onClick={()=>setIsEdit(true)} >Edit</button>
          }
      </div>
    </div>
  )
}

export default UserProfile
