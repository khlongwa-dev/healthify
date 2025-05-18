import React, { useEffect, useState, useContext } from 'react'
import { DoctorContext } from '../../context/DoctorContext'
import { AppContext } from '../../context/AppContext'
import { toast } from 'react-toastify'
import axios from 'axios'

const DoctorProfile = () => {
  const {doctorToken, profileData, backendUrl, setProfileData, fetchDoctorProfile} = useContext(DoctorContext)
  const [isEdit, setIsEdit] = useState(false)
  const {currencySymbol} = useContext(AppContext)


  const updateDoctorProfile = async () => {
    try {
      const formData = new FormData()

      formData.append('Fees', Number(profileData.fees))
      formData.append('Available', profileData.available)
      formData.append('AddressLine1', profileData.addressLine1)
      formData.append('AddressLine2', profileData.addressLine2)

      const { data } = await axios.post(
        `${backendUrl}api/doctor/update-profile`,
        formData,
        { headers: { Authorization: `Bearer ${doctorToken}` } }
      )
      if (data.success) {
        toast.success(data.message)
        await fetchDoctorProfile()
        setIsEdit(false)
      } else {
        toast.error(data.message)
      }
    } catch (error) {
      console.log(error)
      toast.error(error.message)
    }
  }

  useEffect(()=>
  {
    if(doctorToken)
    {
      fetchDoctorProfile()
    }
  }, [doctorToken])

  return profileData && (
    <div>

      <div className='flex flex-col gap-4 m-5'>
        <div>
          <img className='bg-primary/80 w-full sm:max-w-64 rounded-lg' src={profileData.imageUrl} alt="" />
        </div>

        {/* ------- doc info name, degree, experience -------- */}
        <div className='flex-1 border border-stone-100 rounded-lg p-8 py-7 bg-white'>
          <p className='flex items-center gap-2 text-3xl font-medium text-gray-700'>{profileData.name}</p>
          <div className='flex items-center gap-2 mt-1 text-gray-600'>
            <p>{profileData.degree} - {profileData.specialty}</p>
            <button className='py-0.5 px-2 border text-xs rounded-full'>{profileData.experience}</button>
          </div>

          {/* ----- doctor about --------*/}
          <div>
            <p className='flex items-center gap-1 text-sm font-medium text-neutral-800 mt-3'>About</p>
            <p className='text-sm text-gray-600 max-w-[700px] mt-1'>{profileData.about}</p>
          </div>

          <p className='text-gray-600 font-medium mt-4'>Appointment fee: <span className='text-gray-800'>{currencySymbol} { isEdit ? <input type="number" onChange={(e)=>setDocData(prev => ({...prev, fees: e.target.value}))} value={profileData.fees} /> : profileData.fees}</span></p>

          <div className='flex gap-2 py-2'>
            <p>Address:</p>
            <p className='text-sm'>
              {isEdit? <input type="text" onChange={(e)=>setDocData(prev => ({...prev, addressLine1: e.target.value}))} value={profileData.addressLine1} />:profileData.addressLine1}
              <br />
              {isEdit? <input type="text" onChange={(e)=>setDocData(prev => ({...prev, addressLine2: e.target.value}))} value={profileData.addressLine2} />:profileData.addressLine2}
            </p>
          </div>

          <div className='flex gap-1 pt-2'>
            <input  onChange={()=> isEdit && setProfileData(prev => ({...prev, available: !prev.available}))} checked={profileData.available} type="checkbox" />
            <label htmlFor="">Available</label>
          </div>
            {
              isEdit ? <button onClick={updateDoctorProfile} className='px-4 py-1 border border-primary text-sm rounded-full mt-5 hover:bg-primary hover:text-white transition-all'>Save</button>
              :<button onClick={()=>setIsEdit(true)} className='px-4 py-1 border border-primary text-sm rounded-full mt-5 hover:bg-primary hover:text-white transition-all'>Edit</button>
            }
        </div>
      </div>

    </div>
  )
}

export default DoctorProfile
