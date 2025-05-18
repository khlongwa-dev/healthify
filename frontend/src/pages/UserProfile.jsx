import React, { useContext, useState } from 'react'
import { AppContext } from '../context/AppContext'
import {assets} from '../assets/assets'
import axios from 'axios'
import { toast } from 'react-toastify'

const UserProfile = () => {
  const {userProfile, setUserProfile, userToken, backendUrl, fetchUserProfile} = useContext(AppContext)

  const [isEdit, setIsEdit] = useState(false)
  const [image, setImage] = useState(false)

  const updateUserProfile = async () => {
    try {
      const formData = new FormData()
      formData.append('Phone', userProfile.phone)
      formData.append('Name', userProfile.name)
      formData.append('AddressLine1', userProfile.addressLine1)
      formData.append('AddressLine2', userProfile.addressLine2)
      formData.append('Gender', userProfile.gender)
      formData.append('DoB', userProfile.doB)
      
      image && formData.append('ImageUrl', image)
      const {data} = await axios.put(backendUrl + 'api/user/update-profile', formData, {headers:{Authorization: `Bearer ${userToken}`}})

      if (data.success) {
        toast.success(data.message)
        setIsEdit(false)
        setImage(false)
        await fetchUserProfile()
        
      } else {
        toast.error(data.message)
      }
    } catch (error) {
      console.log(error)
      toast.error(error.message)
    }
  }

  return userProfile && (
    <div className='max-w-lg flex flex-col gap-2 text-sm'>
      {
        isEdit
        ? <label htmlFor='image'>
            <div className='inline-block relative cursor-pointer'>
              <img className='w-36 rounded opacity-75' src={image ? URL.createObjectURL(image):userProfile.imageUrl} alt="" />
              <img className='w-10 absolute bottom-12 right-12' src={image ? '': assets.upload_icon} alt="" />
            </div>
            <input onChange={(e)=>setImage(e.target.files[0])} type="file" id='image' hidden/>
        </label>
        : <img className='w-36 rounded' src={userProfile.imageUrl} alt="" />
      }
      
      {
        isEdit
          ? <input className='bg-gray-50 text-3xl font-medium max-w-60 mt-4' type="text" value={userProfile.name} onChange={e => setUserProfile(prev => ({ ...prev, name: e.target.value }))} />
          : <p className='font-medium text-3xl text-neutral-800 mt-4'>{userProfile.name}</p>
      }
      <hr className='bg-zinc-400 h-[1px] border-none' />
      <div>
        <p className='text-neutral-500 underline mt-3'>CONTACT INFORMATION</p>
        <div className='grid grid-cols-[1fr_3fr] gap-y-2.5 mt-3 text-neutral-700'>
          <p className='font-medium'>Email:</p>
          <p className='text-blue-500'>{userProfile.email}</p>
          <p className='font-medium'>Phone:</p>

          {
            isEdit
              ? <input className='bg-gray-100 max-w-52' type="text" value={userProfile.phone} onChange={e => setUserProfile(prev => ({ ...prev, phone: e.target.value }))} />
              : <p className='text-blue-400'>{userProfile.phone}</p>
          }
          <p className='font-medium'>Address:</p>
          {
            isEdit
              ? <p>
                <input className='bg-gray-50' type="text" value={userProfile.addressLine1} onChange={e => setUserProfile(prev => ({ ...prev, addressLine1: e.target.value }))} />
                <br />
                <input className='bg-gray-50' type="text" value={userProfile.addressLine2} onChange={e => setUserProfile(prev => ({ ...prev, addressLine2: e.target.value }))} />
              </p>

              : <p className='text-gray-500'>
                {userProfile.addressLine1}
                <br />
                {userProfile.addressLine2}
              </p>
          }
        </div>
      </div>
      <div>
        <p className='text-neutral-500 underline mt-3'>BASIC INFORMATION</p>
        <div className='grid grid-cols-[1fr_3fr] gap-y-2.5 mt-3 text-neutral-700'>
          <p className='font-medium'>Gender:</p>
          {
            isEdit
              ? <select className='max-w-20 bg-gray-100' value={userProfile.gender} onChange={(e) => setUserProfile(prev => ({ ...prev, gender: e.target.value }))}>
                <option value="Male">Male</option>
                <option value="Female">Female</option>
              </select>
              : <p className='text-gray-400'>{userProfile.gender}</p>
          }
          <p className='font-medium'>Birthday:</p>
          {
            isEdit 
            ? <input className='max-w-28 bg-gray-100' type="date" value={userProfile.doB} onChange={(e) => setUserProfile(prev => ({ ...prev, doB: e.target.value }))}/>
            : <p className='text-gray-400'>{userProfile.doB}</p>
          }
        </div>
      </div>
      <div className='mt-10'>
          {
            isEdit
            ? <button className='border border-primary px-8 py-2 rounded-full hover:bg-primary hover:text-white transition-all' onClick={updateUserProfile} >Save Information</button>
            : <button className='border border-primary px-8 py-2 rounded-full hover:bg-primary hover:text-white transition-all' onClick={()=>setIsEdit(true)} >Edit</button>
          }
      </div>
    </div>
  )
}

export default UserProfile
